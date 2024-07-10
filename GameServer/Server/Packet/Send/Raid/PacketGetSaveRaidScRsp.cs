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
    public class PacketGetSaveRaidScRsp : BasePacket
    {
        public PacketGetSaveRaidScRsp(PlayerInstance player, int raidId, int worldLevel) : base(CmdIds.GetSaveRaidScRsp)
        {
            var proto = new GetSaveRaidScRsp();

            if (player.RaidManager!.RaidData.RaidRecordDatas.TryGetValue(raidId, out var dict))
            {
                if (dict.TryGetValue(worldLevel, out var record))
                {
                    proto.RaidId = (uint)record.RaidId;
                    proto.WorldLevel = (uint)record.WorldLevel;
                    proto.IsSave = record.Status != RaidStatus.Finish && record.Status != RaidStatus.None;
                }
                else
                {
                    proto.Retcode = (uint)Retcode.RetRaidNoSave;
                }
            }
            else
            {
                proto.Retcode = (uint)Retcode.RetRaidNoSave;
            }

            SetData(proto);
        }
    }
}
