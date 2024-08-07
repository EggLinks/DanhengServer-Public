using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketSceneEntityMoveScNotify : BasePacket
{
    public PacketSceneEntityMoveScNotify(PlayerInstance player) : base(CmdIds.SceneEntityMoveScNotify)
    {
        var proto = new SceneEntityMoveScNotify
        {
            EntryId = (uint)player.Data.EntryId,
            Motion = new MotionInfo
            {
                Pos = player.Data.Pos!.ToProto(),
                Rot = player.Data.Rot!.ToProto()
            }
        };

        SetData(proto);
    }
}