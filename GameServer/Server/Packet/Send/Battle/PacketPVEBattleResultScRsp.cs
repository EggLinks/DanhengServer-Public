using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Battle
{
    public class PacketPVEBattleResultScRsp : BasePacket
    {
        public PacketPVEBattleResultScRsp() : base(CmdIds.PVEBattleResultScRsp)
        {
            var proto = new PVEBattleResultScRsp()
            {
                Retcode = 1,
            };

            SetData(proto);
        }

        public PacketPVEBattleResultScRsp(PVEBattleResultCsReq req, PlayerInstance player, BattleInstance battle) : base(CmdIds.PVEBattleResultScRsp)
        {
            var proto = new PVEBattleResultScRsp()
            {
                DropData = battle.GetDropItemList(),
                ResVersion = req.ResVersion.ToString(),
                BinVersion = "",
                StageId = req.StageId,
                BattleId = req.BattleId,
                EndStatus = req.EndStatus,
                CheckIdentical = true,
                Unk1 = new(),
                Unk2 = new(),
                Unk3 = new(),
                EventId = (uint)battle.EventId,
            };

            SetData(proto);
        }
    }
}
