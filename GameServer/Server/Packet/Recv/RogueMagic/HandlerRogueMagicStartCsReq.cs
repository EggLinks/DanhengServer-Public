using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueMagic;

[Opcode(CmdIds.RogueMagicStartCsReq)]
public class HandlerRogueMagicStartCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueMagicStartCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;

        var areaId = (int)req.AreaId;
        var styleType = (int)req.SelectStyleType;
        var difficulty = req.StartDifficultyIdList.Select(x => (int)x).ToList();
        var avatars = req.BaseAvatarIdList.Select(x => (int)x).ToList();

        var rsp = await player.RogueMagicManager!.StartRogueMagic(avatars, areaId, styleType, difficulty);
        await connection.SendPacket(new PacketRogueMagicStartScRsp(rsp.Item1, rsp.Item2));
    }
}