using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.PamSkin;

public class PacketGetPamSkinDataScRsp : BasePacket
{
    public PacketGetPamSkinDataScRsp(PlayerInstance player) : base(CmdIds.GetPamSkinDataScRsp)
    {
        var proto = new GetPamSkinDataScRsp
        {
            CurPamSkinId = (uint)player.Data.CurrentPamSkin,
            UnlockedPamSkinId = { GameData.PamSkinConfigData.Select(x => (uint)x.Key) }
        };

        SetData(proto);
    }
}