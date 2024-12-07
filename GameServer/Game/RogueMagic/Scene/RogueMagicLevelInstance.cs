using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.RogueMagic;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueMagic.Scene;

public class RogueMagicLevelInstance
{
    public RogueMagicLevelInstance(int levelIndex, int layerId, int roomCount)
    {
        LevelIndex = levelIndex;
        LayerId = layerId;
        EntranceId = GameData.RogueMagicRoomGenData.Where(x => x.RoomType != RogueMagicRoomTypeEnum.Adventure)
            .Select(x => x.EntranceId).ToHashSet().ToList()
            .RandomElement();

        foreach (var index in Enumerable.Range(1, roomCount))
            Rooms.Add(new RogueMagicRoomInstance(index, this));
    }

    public List<RogueMagicRoomInstance> Rooms { get; set; } = [];
    public int LayerId { get; set; }
    public int CurRoomIndex { get; set; }
    public int LevelIndex { get; set; }
    public RogueMagicLayerStatus LevelStatus { get; set; } = RogueMagicLayerStatus.Processing;

    public RogueMagicRoomInstance? CurRoom => Rooms.FirstOrDefault(x => x.RoomIndex == CurRoomIndex);

    public int EntranceId { get; set; }

    public RogueMagicLayerInfo ToProto(List<int>? updateRoomIndexList = null)
    {
        var proto = new RogueMagicLayerInfo
        {
            Status = LevelStatus,
            CurRoomIndex = (uint)CurRoomIndex,
            LayerId = (uint)LayerId,
            LevelIndex = (uint)LevelIndex
        };

        proto.TournRoomList.AddRange(updateRoomIndexList != null
            ? Rooms.Where(x => updateRoomIndexList.Contains(x.RoomIndex)).Select(x => x.ToProto())
            : Rooms.Select(x => x.ToProto()));

        return proto;
    }
}