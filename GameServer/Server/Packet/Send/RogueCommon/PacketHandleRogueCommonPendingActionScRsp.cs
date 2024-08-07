using EggLink.DanhengServer.GameServer.Game.Rogue.Buff;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketHandleRogueCommonPendingActionScRsp : BasePacket
{
    public PacketHandleRogueCommonPendingActionScRsp(int queuePosition, bool selectBuff = false,
        bool selectMiracle = false, bool selectBonus = false, RogueBuffSelectMenu? menu = null) : base(
        CmdIds.HandleRogueCommonPendingActionScRsp)
    {
        var proto = new HandleRogueCommonPendingActionScRsp
        {
            QueueLocation = (uint)queuePosition,
            QueuePosition = (uint)queuePosition
        };

        if (selectBuff) proto.BuffSelectCallback = new RogueBuffSelectCallback();

        if (selectMiracle) proto.MiracleSelectCallback = new RogueMiracleSelectCallback();

        if (selectBonus) proto.BonusSelectCallback = new RogueBonusSelectCallback();

        if (menu != null)
            proto.BuffRerollCallback = new RogueBuffRerollCallback
            {
                BuffSelectInfo = menu.ToProto()
            };

        SetData(proto);
    }
}