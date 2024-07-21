using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial
{
    public class PacketGetHeartDialInfoScRsp : BasePacket
    {
        public PacketGetHeartDialInfoScRsp(PlayerInstance player) : base(CmdIds.GetHeartDialInfoScRsp)
        {
            var proto = new GetHeartDialInfoScRsp
            {
                UnlockStatus = HeartDialUnlockStatus.Lock
            };

            if (player.MissionManager?.GetSubMissionStatus(103040103) == Enums.MissionPhaseEnum.Finish)
            {
                proto.UnlockStatus = HeartDialUnlockStatus.UnlockSingle;
            }

            if (player.MissionManager?.GetSubMissionStatus(103040104) == Enums.MissionPhaseEnum.Finish)
            {
                proto.UnlockStatus = HeartDialUnlockStatus.UnlockAll;
            }

            var heartDialData = player.HeartDialData!;

            foreach (var script in GameData.HeartDialScriptData.Values)
            {
                if (heartDialData.DialList.TryGetValue(script.ScriptID, out var info))
                {
                    proto.ScriptInfoList.Add(new HeartDialScriptInfo()
                    {
                        ScriptId = (uint)script.ScriptID,
                        CurEmotionType = (HeartDialEmotionType)info.EmoType,
                        Step = (HeartDialStepType)info.StepType
                    });
                } 
                else
                {
                    proto.ScriptInfoList.Add(new HeartDialScriptInfo()
                    {
                        ScriptId = (uint)script.ScriptID,
                        CurEmotionType = (HeartDialEmotionType)script.DefaultEmoType,
                        Step = (HeartDialStepType)script.StepList.First()
                    });
                }

            }
            foreach (var id in GameData.HeartDialDialogueData.Keys)
            {
                proto.DialogueInfoList.Add(new HeartDialDialogueInfo()
                {
                    DialogueId = (uint)id,
                });
            }

            SetData(proto);
        }
    }
}
