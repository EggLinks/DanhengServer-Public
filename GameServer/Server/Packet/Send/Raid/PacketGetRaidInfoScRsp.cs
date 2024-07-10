using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid
{
    public class PacketGetRaidInfoScRsp : BasePacket
    {
        public PacketGetRaidInfoScRsp(PlayerInstance player) : base(CmdIds.GetRaidInfoScRsp)
        {
            var proto = new GetRaidInfoScRsp()
            {
                ChallengeRaidList = { },
                ChallengeTakenRewardIdListFieldNumber = { },
                FinishedRaidInfoList = { },
            };

            foreach (var recordDict in player.RaidManager!.RaidData.RaidRecordDatas)
            {
                foreach (var record in recordDict.Value)
                {
                    if (record.Value.Status == RaidStatus.Finish)
                    {
                        proto.FinishedRaidInfoList.Add(new RaidInfo()
                        {
                            RaidId = (uint)record.Value.RaidId,
                            WorldLevel = (uint)record.Value.WorldLevel,
                        });
                    }
                }
            }

            SetData(proto);
        }
    }
}
