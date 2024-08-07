using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Archive;

public class PacketGetArchiveDataScRsp : BasePacket
{
    public PacketGetArchiveDataScRsp(PlayerInstance player) : base(CmdIds.GetArchiveDataScRsp)
    {
        var proto = new GetArchiveDataScRsp();

        var info = new ArchiveData();

        GameData.MonsterConfigData.Values.ToList().ForEach(monster =>
        {
            info.KillMonsterList.Add(new MonsterList
            {
                MonsterId = (uint)monster.GetId(),
                Num = 1
            });
        });

        info.ArchiveAvatarIdList.Add(23027);

        foreach (var equipment in player.InventoryManager!.Data.EquipmentItems)
            if (!info.ArchiveEquipmentIdList.Contains((uint)equipment.ItemId))
                info.ArchiveEquipmentIdList.Add((uint)equipment.ItemId);
        ;

        foreach (var item in player.InventoryManager!.Data.RelicItems)
        {
            GameData.RelicConfigData.TryGetValue(item.ItemId, out var relic);
            if (relic != null)
                info.RelicList.Add(new RelicList
                {
                    SetId = (uint)relic.SetID,
                    Type = (uint)relic.Type
                });
        }

        ;

        proto.ArchiveData = info;

        SetData(proto);
    }
}