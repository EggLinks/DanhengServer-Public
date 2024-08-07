using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Rogue;

[Opcode(CmdIds.StartRogueCsReq)]
public class HandlerStartRogueCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = StartRogueCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        var disableAeonIdList = req.DisableAeonIdList.Select(x => (int)x).ToList();
        var avatarIds = req.BaseAvatarIdList.Select(x => (int)x).ToList();

        await player.RogueManager!.StartRogue((int)req.AreaId, (int)req.AeonId, disableAeonIdList, avatarIds);
    }
}