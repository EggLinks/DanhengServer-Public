using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketSyncEntityBuffChangeListScNotify : BasePacket
{
    public PacketSyncEntityBuffChangeListScNotify(IGameEntity entity, SceneBuff buff) : base(
        CmdIds.SyncEntityBuffChangeListScNotify)
    {
        var proto = new SyncEntityBuffChangeListScNotify();
        var change = new EntityBuffChangeInfo
        {
            EntityId = (uint)entity.EntityID,
            BuffChangeInfo = buff.ToProto()
        };
        proto.EntityBuffChangeList.Add(change);

        SetData(proto);
    }

    public PacketSyncEntityBuffChangeListScNotify(IGameEntity entity, List<SceneBuff> buffs) : base(
        CmdIds.SyncEntityBuffChangeListScNotify)
    {
        var proto = new SyncEntityBuffChangeListScNotify();

        foreach (var buff in buffs)
        {
            var change = new EntityBuffChangeInfo
            {
                EntityId = (uint)entity.EntityID,
                RemoveBuffId = (uint)buff.BuffId
            };
            proto.EntityBuffChangeList.Add(change);
        }

        SetData(proto);
    }
}