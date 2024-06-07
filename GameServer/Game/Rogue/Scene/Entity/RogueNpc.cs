using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Game.Rogue.Event;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Scene.Entity
{
    public class RogueNpc(SceneInstance scene, GroupInfo group, NpcInfo npcInfo) : EntityNpc(scene, group, npcInfo)
    {
        public int RogueNpcId { get; set; }
        public int UniqueId { get; set; }
        private bool IsFinish { get; set; } = false;

        public RogueEventInstance? RogueEvent { get; set; }

        public void FinishDialogue()
        {
            IsFinish = true;
            Scene.Player.SendPacket(new PacketSceneGroupRefreshScNotify(this));
        }

        public override SceneEntityInfo ToProto()
        {
            var proto = base.ToProto();

            if (RogueNpcId > 0 && RogueEvent != null)
            {
                proto.Npc.ExtraInfo = new()
                {
                    RogueInfo = new()
                    {
                        EventId = (uint)RogueNpcId,
                        EventUniqueId = (uint)UniqueId,
                        //FinishDialogue = IsFinish,
                        //DialogueGroupId = (uint)GroupID
                    }
                };

                foreach (var param in RogueEvent.Options)
                {
                    proto.Npc.ExtraInfo.RogueInfo.DialogueEventParamList.Add(param.ToNpcProto());
                }
            }

            return proto;
        }
    }
}
