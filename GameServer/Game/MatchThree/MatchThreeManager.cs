using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.MatchThree;

public class MatchThreeManager(PlayerInstance player) : BasePlayerManager(player)
{
    //public MatchThreeRoomInstance? RoomInstance { get; set; }

    public MatchThreeData ToProto()
    {
        var proto = new MatchThreeData
        {
            FinishedLevels =
            {
                GameData.MatchThreeLevelData.Values.Where(x => x.LevelID <= 1500).Select(x => new MatchThreeFinishedLevelInfos
                {
                    LevelId = (uint)x.LevelID,
                    ModeId = (uint)x.Mode
                })
            },
            BirdRecordInfos =
            {
                GameData.MatchThreeBirdData.Values.Where(x => x.BirdID != 310).Select(x => new MatchThreeBirdInfo
                {
                    BirdId = (uint)x.BirdID
                })
            }
        };

        return proto;
    }
}