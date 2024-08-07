using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketGetEnteredSceneScRsp : BasePacket
{
    public PacketGetEnteredSceneScRsp() : base(CmdIds.GetEnteredSceneScRsp)
    {
        var proto = new GetEnteredSceneScRsp();

        foreach (var excel in GameData.MapEntranceData.Values)
        {
            // Skip these
            if (excel.FinishMainMissionList.Count == 0 && excel.FinishMainMissionList.Count == 0) continue;

            // Add info
            var info = new EnteredSceneInfo
            {
                FloorId = (uint)excel.FloorID,
                PlaneId = (uint)excel.PlaneID
            };

            proto.EnteredSceneInfoList.Add(info);
        }

        SetData(proto);
    }
}