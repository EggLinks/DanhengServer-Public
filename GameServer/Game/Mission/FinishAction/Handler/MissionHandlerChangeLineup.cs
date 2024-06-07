using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Game.Mission.FinishAction.Handler
{
    [MissionFinishAction(FinishActionTypeEnum.ChangeLineup)]
    public class MissionHandlerChangeLineup : MissionFinishActionHandler
    {

        public override void OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
        {
            Player.LineupManager!.GetCurLineup()!.BaseAvatars!.Clear();
            var count = 0;
            var avatarCount = Params.Count(value => value != 0) - 1;
            foreach (var avatarId in Params)
            {
                if (count++ >= 4) break;
                GameData.SpecialAvatarData.TryGetValue(avatarId * 10 + Player.Data.WorldLevel, out var specialAvatar);
                if (specialAvatar == null)
                {
                    GameData.AvatarConfigData.TryGetValue(avatarId, out var avatar);
                    if (avatar == null) continue;
                    var ava = Player.AvatarManager!.GetAvatar(avatarId);
                    if (ava == null) Player.AvatarManager!.AddAvatar(avatarId);
                    Player.LineupManager!.AddAvatarToCurTeam(avatarId, count == avatarCount);
                }
                else
                {
                    Player.LineupManager!.AddSpecialAvatarToCurTeam(avatarId * 10 + Player.Data.WorldLevel, count == avatarCount);
                }
            }
            GameData.SpecialAvatarData.TryGetValue(Params[4] * 10 + Player.Data.WorldLevel, out var leaderAvatar);
            if (leaderAvatar == null)
            {
                Player.LineupManager!.GetCurLineup()!.LeaderAvatarId = Params[4];
            }
            else
            {
                Player.LineupManager!.GetCurLineup()!.LeaderAvatarId = leaderAvatar.AvatarID;
            }
            DatabaseHelper.Instance!.UpdateInstance(Player.LineupManager!.LineupData);
            Player.SceneInstance!.SyncLineup();
        }
    }
}
