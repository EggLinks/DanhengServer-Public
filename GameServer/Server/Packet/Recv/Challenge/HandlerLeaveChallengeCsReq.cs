using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using static EggLink.DanhengServer.GameServer.Plugin.Event.PluginEvent;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Challenge;

[Opcode(CmdIds.LeaveChallengeCsReq)]
public class HandlerLeaveChallengeCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var player = connection.Player!;

        // TODO: check for plane type
        if (player.SceneInstance != null)
        {
            // As of 1.5.0, the server now has to handle the player leaving battle too
            await player.ForceQuitBattle();

            // Reset lineup
            player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupChallenge, []);
            player.LineupManager.SetExtraLineup(ExtraLineupType.LineupChallenge2, []);

            InvokeOnPlayerQuitChallenge(player, player.ChallengeManager!.ChallengeInstance);

            player.ChallengeManager!.ChallengeInstance = null;
            player.ChallengeManager!.ClearInstance();

            // Leave scene
            await player.LineupManager.SetCurLineup(0);
            // Heal avatars (temproary solution)
            foreach (var avatar in player.LineupManager.GetCurLineup()!.AvatarData!.Avatars) avatar.CurrentHp = 10000;

            var leaveEntryId = GameConstants.CHALLENGE_ENTRANCE;
            if (player.SceneInstance.LeaveEntryId != 0) leaveEntryId = player.SceneInstance.LeaveEntryId;
            await player.EnterScene(leaveEntryId, 0, true);
        }

        await connection.SendPacket(CmdIds.LeaveChallengeScRsp);
    }
}