using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene
{
    public class PacketContentPackageGetDataScRsp : BasePacket
    {
        public PacketContentPackageGetDataScRsp(uint id) : base(CmdIds.ContentPackageGetDataScRsp)
        {
            var proto = new ContentPackageGetDataScRsp
            {
                Data = new ContentPackageData { ContentInfoList = { new ContentInfo { ContentId = id, Status = ContentPackageStatus.Finished } } }
            };

            SetData(proto);
        }
    }
}
