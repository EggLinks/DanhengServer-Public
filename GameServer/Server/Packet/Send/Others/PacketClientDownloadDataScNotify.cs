
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;

namespace EggLink.DanhengServer.Server.Packet.Send.Others
{
    public class PacketClientDownloadDataScNotify : BasePacket
    {
        public PacketClientDownloadDataScNotify(byte[] data) : base(CmdIds.ClientDownloadDataScNotify)
        {
            var downloadData = new ClientDownloadData
            {
                Data = Google.Protobuf.ByteString.CopyFrom(data),
                Version = 81,
                Time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds
            };
            var notify = new ClientDownloadDataScNotify
            {
                DownloadData = downloadData
            };
            SetData(notify);
        }
    }
}
