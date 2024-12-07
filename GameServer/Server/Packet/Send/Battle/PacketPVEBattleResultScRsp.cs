using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Battle;

public class PacketPVEBattleResultScRsp : BasePacket
{
    public PacketPVEBattleResultScRsp() : base(CmdIds.PVEBattleResultScRsp)
    {
        var proto = new PVEBattleResultScRsp
        {
            Retcode = 1
        };

        SetData(proto);
    }

    public PacketPVEBattleResultScRsp(PVEBattleResultCsReq req, PlayerInstance player, BattleInstance battle) : base(
        CmdIds.PVEBattleResultScRsp)
    {
        var proto = new PVEBattleResultScRsp
        {
            DropData = battle.GetDropItemList(),
            ResVersion = req.ResVersion.ToString(),
            BinVersion = "",
            StageId = req.StageId,
            BattleId = req.BattleId,
            EndStatus = req.EndStatus,
            CheckIdentical = true,
            Unk1 = new ItemList(),
            Unk2 = new ItemList(),
            MultipleDropData = new ItemList(),
            EventId = (uint)battle.EventId
        };

        SetData(proto);
    }
}