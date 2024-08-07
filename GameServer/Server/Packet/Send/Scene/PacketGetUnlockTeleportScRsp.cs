using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketGetUnlockTeleportScRsp : BasePacket
{
    public PacketGetUnlockTeleportScRsp(GetUnlockTeleportCsReq req) : base(CmdIds.GetUnlockTeleportScRsp)
    {
        var rsp = new GetUnlockTeleportScRsp();
        foreach (var entranceId in req.EntryIdList)
        {
            GameData.MapEntranceData.TryGetValue((int)entranceId, out var excel);
            if (excel == null) continue;

            GameData.GetFloorInfo(excel.PlaneID, excel.FloorID, out var floorInfo);
            if (floorInfo == null) continue;

            foreach (var teleport in floorInfo.CachedTeleports)
                rsp.UnlockTeleportList.Add((uint)teleport.Value.MappingInfoID);
        }

        SetData(rsp);
    }
}