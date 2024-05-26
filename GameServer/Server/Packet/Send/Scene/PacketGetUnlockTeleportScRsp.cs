using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
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
                {
                    rsp.UnlockTeleportList.Add((uint)teleport.Value.MappingInfoID);
                }
            }

            SetData(rsp);
        }
    }
}
