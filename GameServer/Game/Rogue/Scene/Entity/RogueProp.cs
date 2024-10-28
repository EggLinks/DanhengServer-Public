using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;

public class RogueProp(SceneInstance scene, MazePropExcel excel, GroupInfo group, PropInfo prop)
    : EntityProp(scene, excel, group, prop)
{
    public RogueProp(EntityProp prop) : this(prop.Scene, prop.Excel, prop.Group, prop.PropInfo)
    {
    }

    public int NextRoomID { get; set; }
    public int NextSiteID { get; set; }
    public int ChestCanUseTimes { get; set; }
    public int CustomPropID { get; set; }

    public bool IsChessRogue { get; set; } = false;
    public bool IsLastRoom { get; set; } = false;

    public override SceneEntityInfo ToProto()
    {
        var proto = base.ToProto();

        if (NextRoomID != 0 || NextSiteID != 0 || ChestCanUseTimes != 0) // do not set if all are 0
            proto.Prop.ExtraInfo = new PropExtraInfo
            {
                RogueInfo = new PropRogueInfo
                {
                    RoomId = (uint)NextRoomID,
                    SiteId = (uint)NextSiteID
                }
            };

        if (IsChessRogue)
            proto.Prop.ExtraInfo = new PropExtraInfo
            {
                ChessRogueInfo = new PropChessRogueInfo
                {
                    EnterNextCell = !IsLastRoom
                }
            };

        if (CustomPropID != 0) proto.Prop.PropId = (uint)CustomPropID;

        return proto;
    }
}