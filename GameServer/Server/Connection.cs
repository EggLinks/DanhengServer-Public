using System.Buffers;
using System.Net;
using System.Reflection;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Kcp.KcpSharp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace EggLink.DanhengServer.GameServer.Server;

public class Connection(KcpConversation conversation, IPEndPoint remote) : DanhengConnection(conversation, remote)
{
    private static readonly Logger Logger = new("GameServer");

    public PlayerInstance? Player { get; set; }

    public override async void Start()
    {
        Logger.Info($"New connection from {RemoteEndPoint}.");
        State = SessionStateEnum.WAITING_FOR_TOKEN;
        await ReceiveLoop();
    }

    public override void Stop()
    {
        Player?.OnLogoutAsync();
        DanhengListener.UnregisterConnection(this);
        base.Stop();
    }

    protected async Task ReceiveLoop()
    {
        while (!CancelToken.IsCancellationRequested)
        {
            // WaitToReceiveAsync call completes when there is at least one message is received or the transport is closed.
            var result = await Conversation.WaitToReceiveAsync(CancelToken.Token);
            if (result.TransportClosed)
            {
                Logger.Debug("Connection was closed");
                break;
            }

            if (result.BytesReceived > MAX_MSG_SIZE)
            {
                // The message is too large.
                Logger.Error("Packet too large");
                Conversation.SetTransportClosed();
                break;
            }

            var buffer = ArrayPool<byte>.Shared.Rent(result.BytesReceived);
            try
            {
                // TryReceive should not return false here, unless the transport is closed.
                // So we don't need to check for result.TransportClosed.
                if (!Conversation.TryReceive(buffer, out result))
                {
                    Logger.Error("Failed to receive packet");
                    break;
                }

                await ProcessMessageAsync(buffer.AsMemory(0, result.BytesReceived));
            }
            catch (Exception ex)
            {
                Logger.Error("Packet parse error", ex);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        Stop();
    }

    // DO THE PROCESSING OF THE GAME PACKET
    private async Task ProcessMessageAsync(Memory<byte> data)
    {
        var gamePacket = data.ToArray();
        if (ConfigManager.Config.GameServer.UsePacketEncryption)
            Crypto.Xor(gamePacket, XorKey!);

        await using MemoryStream ms = new(gamePacket);
        using BinaryReader br = new(ms);

        // Handle
        try
        {
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                // Length
                if (br.BaseStream.Length - br.BaseStream.Position < 12) return;
                // Packet sanity check
                var magic1 = br.ReadUInt32BE();
                if (magic1 != 0x9D74C714)
                {
                    Logger.Error($"Bad Data Package Received: got 0x{magic1:X}, expect 0x9D74C714");
                    return; // Bad packet
                }

                // Data
                var opcode = br.ReadUInt16BE();
                var headerLength = br.ReadUInt16BE();
                var payloadLength = br.ReadUInt32BE();
                var header = br.ReadBytes(headerLength);
                var payload = br.ReadBytes((int)payloadLength);
                LogPacket("Recv", opcode, payload);
                await HandlePacket(opcode, header, payload);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e.Message, e);
        }
    }

    private async Task HandlePacket(ushort opcode, byte[] header, byte[] payload)
    {
        // Find the Handler for this opcode
        var handler = HandlerManager.GetHandler(opcode);
        if (handler != null)
        {
            // Handle
            // Make sure session is ready for packets
            var state = State;
            switch (opcode)
            {
                case CmdIds.PlayerGetTokenCsReq:
                {
                    if (state != SessionStateEnum.WAITING_FOR_TOKEN) return;
                    goto default;
                }
                case CmdIds.PlayerLoginCsReq:
                {
                    if (state != SessionStateEnum.WAITING_FOR_LOGIN) return;
                    goto default;
                }
                default:
                    break;
            }

            try
            {
                await handler.OnHandle(this, header, payload);
            }
            catch
            {
                // get the packet rsp and set retCode to Retcode.RetFail
                var curPacket = LogMap.GetValueOrDefault(opcode);
                if (curPacket == null) return;

                var rspName = curPacket.Replace("Cs", "Sc").Replace("Req", "Rsp"); // Get the response packet name
                if (rspName == curPacket) return; // do not send rsp when resp name = recv name
                var rspOpcode = LogMap.FirstOrDefault(x => x.Value == rspName).Key; // Get the response opcode

                // get proto class
                var typ = AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => assembly.GetName().Name == "DanhengProto")!.GetTypes()
                    .First(t => t.Name == rspName); //get the type using the packet name
                var curTyp = AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => assembly.GetName().Name == "DanhengProto")!.GetTypes()
                    .First(t => t.Name == curPacket); //get the type using the packet name

                // create the response packet
                if (Activator.CreateInstance(typ) is not IMessage rsp) return;

                // set the retCode to Retcode.RetFail
                var retCode = typ.GetProperty("Retcode");
                retCode?.SetValue(rsp, (uint)Retcode.RetFail);

                // get the same field in req and rsp
                var descriptor =
                    curTyp.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static)?.GetValue(
                        null, null) as MessageDescriptor; // get the static property Descriptor
                var reqPacket = descriptor?.Parser.ParseFrom(payload);

                foreach (var propertyInfo in curTyp.GetProperties())
                {
                    var prop = typ.GetProperty(propertyInfo.Name);
                    if (prop != null && prop.CanWrite)
                    {
                        var value = propertyInfo.GetValue(reqPacket);
                        if (value != null)
                            prop.SetValue(rsp, value);
                    }
                }

                // send the response packet
                var packet = new BasePacket((ushort)rspOpcode);
                packet.SetData(rsp);
                await SendPacket(packet);
            }

            return;
        }

        // No handler found
        // get the packet name
        var packetName = LogMap.GetValueOrDefault(opcode);
        if (packetName == null) return;

        var respName = packetName.Replace("Cs", "Sc").Replace("Req", "Rsp"); // Get the response packet name
        if (respName == packetName) return; // do not send rsp when resp name = recv name
        var respOpcode = LogMap.FirstOrDefault(x => x.Value == respName).Key; // Get the response opcode

        // Send Rsp
        await SendPacket(respOpcode);
    }
}