using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
    public class PacketSyncEntityBuffChangeListScNotify : BasePacket
    {
        public PacketSyncEntityBuffChangeListScNotify(IGameEntity entity, SceneBuff buff) : base(CmdIds.SyncEntityBuffChangeListScNotify)
        {
            var proto = new SyncEntityBuffChangeListScNotify();
            var change = new EntityBuffChange()
            {
                EntityId = (uint)entity.EntityID,
                BuffInfo = buff.ToProto(),
            };
            proto.EntityBuffChangeList.Add(change);

            SetData(proto);
        }

        public PacketSyncEntityBuffChangeListScNotify(IGameEntity entity, List<SceneBuff> buffs) : base(CmdIds.SyncEntityBuffChangeListScNotify)
        {
            var proto = new SyncEntityBuffChangeListScNotify();

            foreach (var buff in buffs)
            {
                buff.Duration = 0;
                var change = new EntityBuffChange()
                {
                    EntityId = (uint)entity.EntityID,
                    BuffInfo = buff.ToProto(),
                };
                proto.EntityBuffChangeList.Add(change);
            }

            SetData(proto);
        }
    }
}
