using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Message;

public class PacketGetNpcMessageGroupScRsp : BasePacket
{
    public PacketGetNpcMessageGroupScRsp(IEnumerable<uint> contactIdList, PlayerInstance instance) : base(
        CmdIds.GetNpcMessageGroupScRsp)
    {
        var proto = new GetNpcMessageGroupScRsp();

        foreach (var contactId in contactIdList)
        {
            var contact = instance.MessageManager!.GetMessageGroup((int)contactId);

            proto.MessageGroupList.AddRange(contact);
        }

        SetData(proto);
    }
}