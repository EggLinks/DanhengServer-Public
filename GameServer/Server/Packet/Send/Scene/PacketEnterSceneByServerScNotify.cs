using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
    public class PacketEnterSceneByServerScNotify : BasePacket
    {
        public PacketEnterSceneByServerScNotify(SceneInstance scene, ChangeStoryLineAction storyLineAction = ChangeStoryLineAction.None) : base(CmdIds.EnterSceneByServerScNotify)
        {
            var sceneInfo = scene.ToProto();
            var notify = new EnterSceneByServerScNotify()
            {
                Scene = sceneInfo,
                Lineup = scene.Player.LineupManager!.GetCurLineup()!.ToProto(),
            };

            notify.Scene.BONACBOIIBE = (uint) storyLineAction;

            SetData(notify);
        }
    }
}
