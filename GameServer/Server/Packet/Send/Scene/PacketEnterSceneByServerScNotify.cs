using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketEnterSceneByServerScNotify : BasePacket
{
    public PacketEnterSceneByServerScNotify(SceneInstance scene) : base(CmdIds.EnterSceneByServerScNotify)
    {
        var sceneInfo = scene.ToProto();
        var notify = new EnterSceneByServerScNotify
        {
            Scene = sceneInfo,
            Lineup = scene.Player.LineupManager!.GetCurLineup()!.ToProto()
        };

        SetData(notify);
    }
}