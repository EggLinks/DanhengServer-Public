using EggLink.DanhengServer.Database.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;

public class PacketHeartDialScriptChangeScNotify : BasePacket
{
    public PacketHeartDialScriptChangeScNotify(HeartDialUnlockStatus status, HeartDialInfo? changedInfo = null) : base(
        CmdIds.HeartDialScriptChangeScNotify)
    {
        var proto = new HeartDialScriptChangeScNotify
        {
            UnlockStatus = status
        };

        if (changedInfo != null)
            proto.ChangedScriptInfoList.Add(new HeartDialScriptInfo
            {
                ScriptId = (uint)changedInfo.ScriptId,
                CurEmotionType = (HeartDialEmotionType)changedInfo.EmoType,
                Step = (HeartDialStepType)changedInfo.StepType
            });

        SetData(proto);
    }
}