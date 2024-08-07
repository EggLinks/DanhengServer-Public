using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid;

public class PacketGetAllSaveRaidScRsp : BasePacket
{
    public PacketGetAllSaveRaidScRsp(PlayerInstance player) : base(CmdIds.GetAllSaveRaidScRsp)
    {
        var proto = new GetAllSaveRaidScRsp();

        foreach (var dict in player.RaidManager!.RaidData.RaidRecordDatas.Values)
        foreach (var record in dict.Values)
            proto.RaidDataList.Add(new RaidData
            {
                RaidId = (uint)record.RaidId,
                WorldLevel = (uint)record.WorldLevel
            });

        SetData(proto);
    }
}