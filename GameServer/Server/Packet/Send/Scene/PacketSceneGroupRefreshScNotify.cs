using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketSceneGroupRefreshScNotify : BasePacket
{
    public PacketSceneGroupRefreshScNotify(List<IGameEntity>? addEntity = null, List<IGameEntity>? removeEntity = null)
        : base(CmdIds.SceneGroupRefreshScNotify)
    {
        var proto = new SceneGroupRefreshScNotify();
        Dictionary<int, GroupRefreshInfo> refreshInfo = [];

        foreach (var e in removeEntity ?? [])
        {
            var group = new GroupRefreshInfo
            {
                GroupId = (uint)e.GroupID,
                RefreshType = SceneGroupRefreshType.Loaded
            };
            group.RefreshEntity.Add(new SceneEntityRefreshInfo
            {
                DeleteEntity = (uint)e.EntityID
            });

            if (refreshInfo.TryGetValue(e.GroupID, out var value))
                value.RefreshEntity.AddRange(group.RefreshEntity);
            else
                refreshInfo[e.GroupID] = group;
        }

        foreach (var e in addEntity ?? [])
        {
            var group = new GroupRefreshInfo
            {
                GroupId = (uint)e.GroupID,
                RefreshType = SceneGroupRefreshType.Loaded
            };
            group.RefreshEntity.Add(new SceneEntityRefreshInfo
            {
                AddEntity = e.ToProto()
            });

            if (refreshInfo.TryGetValue(e.GroupID, out var value))
                value.RefreshEntity.AddRange(group.RefreshEntity);
            else
                refreshInfo[e.GroupID] = group;
        }

        proto.GroupRefreshList.AddRange(refreshInfo.Values);

        SetData(proto);
    }

    public PacketSceneGroupRefreshScNotify(IGameEntity? addEntity = null, IGameEntity? removeEntity = null) :
        this(addEntity == null ? [] : [addEntity], removeEntity == null ? [] : [removeEntity])
    {
    }
}