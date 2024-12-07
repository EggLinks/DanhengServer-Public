using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Enums.TournRogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;

public class RogueTournRoomInstance(int roomIndex, RogueTournLevelInstance levelInstance)
{
    public int RoomId { get; set; }
    public int RoomIndex { get; set; } = roomIndex;
    public RogueTournRoomStatus Status { get; set; } = RogueTournRoomStatus.None;
    public RogueTournLevelInstance LevelInstance { get; set; } = levelInstance;
    public RogueTournRoomTypeEnum RoomType { get; set; }

    public RogueTournRoomConfig? Config { get; set; }

    public RogueTournRoomList ToProto()
    {
        return new RogueTournRoomList
        {
            RoomId = (uint)RoomId,
            RoomIndex = (uint)RoomIndex,
            Status = Status
        };
    }

    public void Init(RogueTournRoomTypeEnum type)
    {
        if (Status == RogueTournRoomStatus.Processing || Status == RogueTournRoomStatus.Finish)
            return; // already initialized

        RoomType = type;
        Status = RogueTournRoomStatus.Processing;
        // get config
        Config = RoomType == RogueTournRoomTypeEnum.Adventure
            ? GameData.RogueTournRoomGenData.Where(x => x.RoomType == RoomType).ToList().RandomElement()
            : GameData.RogueTournRoomGenData
                .Where(x => x.EntranceId == LevelInstance.EntranceId && x.RoomType == RoomType).ToList()
                .RandomElement();

        if (Config == null)
        {
            Status = RogueTournRoomStatus.Finish;
            return;
        }

        RoomId = GameData.RogueTournRoomData.Where(x => x.Value.RogueRoomType == RoomType).Select(x => x.Key).ToList()
            .RandomElement();
    }

    public List<int> GetLoadGroupList()
    {
        var groupList = new List<int>();
        groupList.AddRange(Config!.DefaultLoadBasicGroup);
        groupList.AddRange(Config.DefaultLoadGroup);

        //if (RoomIndex == 1)  // first room
        groupList.AddRange(Config.SubMonsterGroup);

        return groupList;
    }
}