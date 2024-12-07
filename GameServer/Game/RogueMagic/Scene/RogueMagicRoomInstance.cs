using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Enums.RogueMagic;
using EggLink.DanhengServer.GameServer.Game.RogueMagic.Adventure;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueMagic.Scene;

public class RogueMagicRoomInstance(int roomIndex, RogueMagicLevelInstance levelInstance)
{
    public int RoomId { get; set; }
    public int RoomIndex { get; set; } = roomIndex;
    public RogueMagicRoomStatus Status { get; set; } = RogueMagicRoomStatus.None;
    public RogueMagicLevelInstance LevelInstance { get; set; } = levelInstance;
    public RogueMagicRoomTypeEnum RoomType { get; set; }

    public RogueMagicRoomConfig? Config { get; set; }

    public RogueMagicAdventureInstance? AdventureInstance { get; set; }

    public RogueMagicRoomInfo ToProto()
    {
        return new RogueMagicRoomInfo
        {
            RoomId = (uint)RoomId,
            RoomIndex = (uint)RoomIndex,
            Status = Status
        };
    }

    public void Init(RogueMagicRoomTypeEnum type)
    {
        if (Status == RogueMagicRoomStatus.Processing || Status == RogueMagicRoomStatus.Finish)
            return; // already initialized

        RoomType = type;
        Status = RogueMagicRoomStatus.Processing;
        // get config
        Config = RoomType == RogueMagicRoomTypeEnum.Adventure
            ? GameData.RogueMagicRoomGenData.Where(x => x.RoomType == RoomType).ToList().RandomElement()
            : GameData.RogueMagicRoomGenData
                .Where(x => x.EntranceId == LevelInstance.EntranceId && x.RoomType == RoomType).ToList()
                .RandomElement();

        if (Config == null)
        {
            Status = RogueMagicRoomStatus.Finish;
            return;
        }

        RoomId = GameData.RogueMagicRoomData.Where(x => x.Value.RogueRoomType == RoomType).Select(x => x.Key).ToList()
            .RandomElement();

        if (RoomType == RogueMagicRoomTypeEnum.Adventure)
        {
            AdventureInstance = new RogueMagicAdventureInstance(GameData.RogueMagicAdventureRoomData.Values
                .Where(x => x.AdventureType == Config.AdventureType).ToList().RandomElement());

            RoomId = AdventureInstance.Excel.RoomID;
        }
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