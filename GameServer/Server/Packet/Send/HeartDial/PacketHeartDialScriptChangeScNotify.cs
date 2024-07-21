using EggLink.DanhengServer.Database.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial
{
    public class PacketHeartDialScriptChangeScNotify : BasePacket
    {
        public PacketHeartDialScriptChangeScNotify(HeartDialUnlockStatus status, HeartDialInfo? changedInfo = null) : base(CmdIds.HeartDialScriptChangeScNotify)
        {
            var proto = new HeartDialScriptChangeScNotify
            {
                UnlockStatus = status,
            };

            if (changedInfo != null)
            {
                proto.ChangedScriptInfoList.Add(new HeartDialScriptInfo
                {
                    ScriptId = (uint)changedInfo.ScriptId,
                    CurEmotionType = (HeartDialEmotionType)changedInfo.EmoType,
                    Step = (HeartDialStepType)changedInfo.StepType,
                });
            }

            SetData(proto);
        }
    }
}
