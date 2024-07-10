using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Scene.Entity
{
    public class RogueProp(SceneInstance scene, MazePropExcel excel, GroupInfo group, PropInfo prop) : EntityProp(scene, excel, group, prop)
    {
        public int NextRoomID { get; set; }
        public int NextSiteID { get; set; }
        public int ChestCanUseTimes { get; set; }
        public int CustomPropID { get; set; }

        public bool IsChessRogue { get; set; } = false;
        public bool IsLastRoom { get; set; } = false;

        public RogueProp(EntityProp prop) : this(prop.Scene, prop.Excel, prop.Group, prop.PropInfo)
        {
        }

        public override SceneEntityInfo ToProto()
        {
            var proto = base.ToProto();

            if (NextRoomID != 0 || NextSiteID != 0 || ChestCanUseTimes != 0)  // do not set if all are 0
            {
                proto.Prop.ExtraInfo = new()
                {
                    RogueInfo = new()
                    {
                        RoomId = (uint)NextRoomID,
                        SiteId = (uint)NextSiteID,
                    }
                };
            }

            if (IsChessRogue)
            {
                proto.Prop.ExtraInfo = new()
                {
                    ChessRogueInfo = new()
                    {
                        EnterNextCell = !IsLastRoom,
                    }
                };
            }

            if (CustomPropID != 0)
            {
                proto.Prop.PropId = (uint)CustomPropID;
            }

            return proto;
        }
    }
}
