using EggLink.DanhengServer.GameServer.Game.Rogue.Buff;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketHandleRogueCommonPendingActionScRsp : BasePacket
{
    public PacketHandleRogueCommonPendingActionScRsp(int queuePosition, int queueLocation, bool selectBuff = false,
        bool selectMiracle = false, bool selectBonus = false, bool selectFormula = false,
        bool reforgeBuff = false, bool selectMagicUnit = false, bool selectScepter = false,
        RogueBuffSelectMenu? menu = null) : base(
        CmdIds.HandleRogueCommonPendingActionScRsp)
    {
        var proto = new HandleRogueCommonPendingActionScRsp
        {
            QueueLocation = (uint)queueLocation,
            QueuePosition = (uint)queuePosition
        };

        if (selectBuff) proto.BuffSelectCallback = new RogueBuffSelectCallback();

        if (selectMiracle) proto.MiracleSelectCallback = new RogueMiracleSelectCallback();

        if (selectBonus) proto.BonusSelectCallback = new RogueBonusSelectCallback();

        if (selectFormula) proto.RogueTournFormulaCallback = new RogueTournFormulaCallback();

        if (reforgeBuff) proto.BuffReforgeSelectCallback = new RogueBuffReforgeSelectCallback();

        if (selectMagicUnit) proto.MagicUnitSelectCallback = new RogueMagicUnitSelectCallback();

        if (selectScepter) proto.ScepterSelectCallback = new RogueMagicScepterSelectCallback();

        if (menu != null)
            proto.BuffRerollCallback = new RogueBuffRerollCallback
            {
                BuffSelectInfo = menu.ToProto()
            };

        SetData(proto);
    }
}