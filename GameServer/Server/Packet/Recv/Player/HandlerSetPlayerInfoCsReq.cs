using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Player;

[Opcode(CmdIds.SetPlayerInfoCsReq)]
public class HandlerSetPlayerInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;
        var req = SetPlayerInfoCsReq.Parser.ParseFrom(data);
        if (req == null) return;
        player.Data.Name = req.Nickname;
        if (req.Gender == Gender.None)
        {
            await connection.SendPacket(new PacketSetPlayerInfoScRsp(player, req.IsModify));
            return;
        }

        if (req.Gender == Gender.Woman)
            player.Data.CurrentGender = Gender.Woman;
        else
            player.Data.CurrentGender = Gender.Man;
        player.Data.IsGenderSet = true;
        await player.ChangeAvatarPathType(8001, MultiPathAvatarTypeEnum.Warrior);

        await player.LineupManager!.AddAvatarToCurTeam(8001);
        await player.LineupManager!.AddAvatarToCurTeam(1001);
        await player.MissionManager!.FinishSubMission(100010134);

        await connection.SendPacket(new PacketSetPlayerInfoScRsp(player, req.IsModify));
        await connection.SendPacket(new PacketPlayerSyncScNotify(player.ToProto()));
    }
}