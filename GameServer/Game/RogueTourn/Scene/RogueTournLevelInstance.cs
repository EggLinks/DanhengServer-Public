using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.TournRogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;

public class RogueTournLevelInstance
{
    public RogueTournLevelInstance(int levelIndex)
    {
        LevelIndex = levelIndex;
        LayerId = levelIndex * 100 + 1001; // temp
        EntranceId = GameData.RogueTournRoomGenData.Where(x => x.RoomType != RogueTournRoomTypeEnum.Adventure)
            .Select(x => x.EntranceId).ToHashSet().ToList()
            .RandomElement();
        if (levelIndex == 2)
            foreach (var index in Enumerable.Range(1, 5))
                Rooms.Add(new RogueTournRoomInstance(index, this));
        else
            foreach (var index in Enumerable.Range(1, 4))
                Rooms.Add(new RogueTournRoomInstance(index, this));
    }

    public List<RogueTournRoomInstance> Rooms { get; set; } = [];
    public int LayerId { get; set; }
    public int CurRoomIndex { get; set; }
    public int LevelIndex { get; set; }
    public RogueTournLayerStatus LevelStatus { get; set; } = RogueTournLayerStatus.Processing;

    public RogueTournRoomInstance? CurRoom => Rooms.FirstOrDefault(x => x.RoomIndex == CurRoomIndex);

    public int EntranceId { get; set; }

    public RogueTournLevel ToProto()
    {
        var proto = new RogueTournLevel
        {
            Status = LevelStatus,
            CurRoomIndex = (uint)CurRoomIndex,
            LayerId = (uint)LayerId,
            LevelIndex = (uint)LevelIndex,
            TournRoomList = { Rooms.Select(x => x.ToProto()) }
        };

        return proto;
    }
}