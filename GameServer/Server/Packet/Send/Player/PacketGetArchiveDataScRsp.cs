using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketGetArchiveDataScRsp : BasePacket
    {
        public PacketGetArchiveDataScRsp() : base(CmdIds.GetArchiveDataScRsp)
        {
            var proto = new GetArchiveDataScRsp();

            var info = new ArchiveData();

            GameData.MonsterConfigData.Values.ToList().ForEach(monster =>
            {
                info.ArchiveMonsterIdList.Add(new ArchiveMonsterId()
                {
                    MonsterId = (uint)monster.GetId(),
                    Num = 1
                });
            });

            info.ArchiveAvatarIdList.Add(23027);

            GameData.EquipmentConfigData.Values.ToList().ForEach(equipment =>
            {
                info.ArchiveEquipmentIdList.Add((uint)equipment.GetId());
            });

            GameData.RelicConfigData.Values.ToList().ForEach(relic =>
            {
                info.RelicList.Add(new RelicList()
                {
                    SetId = (uint)relic.ID,
                    Type = (uint)relic.Type,
                });
            });

            proto.ArchiveInfo = info;

            SetData(proto);
        }
    }
}
