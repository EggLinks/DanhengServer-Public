using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Scene;

public class RogueRoomInstance
{
    public RogueRoomInstance(RogueMapExcel excel)
    {
        SiteId = excel.SiteID;
        NextSiteIds = excel.NextSiteIDList;

        GameData.RogueMapGenData.TryGetValue(excel.SiteID, out var genData);
        if (genData != null) RoomId = genData.RandomElement();
        Excel = GameData.RogueRoomData[RoomId];
    }

    public int RoomId { get; set; }
    public int SiteId { get; set; }
    public RogueRoomStatus Status { get; set; } = RogueRoomStatus.Lock;
    public List<int> NextSiteIds { get; set; }
    public RogueRoomExcel Excel { get; set; }

    public RogueRoom ToProto()
    {
        return new RogueRoom
        {
            RoomId = (uint)RoomId,
            SiteId = (uint)SiteId,
            CurStatus = Status
        };
    }
}