using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using System.Numerics;
using System.Linq;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player
{
    public class PacketGetVideoVersionKeyScRsp : BasePacket
    {
        public PacketGetVideoVersionKeyScRsp() : base(CmdIds.GetVideoVersionKeyScRsp)
        {
            var proto = new GetVideoVersionKeyScRsp
            {
                ActivityVideoKeyInfoList =
                {
                    GameData.VideoKeysConfig.ActivityVideoKeyData.Select(activity => new VideoKeyInfo
                    {
                        Id = (uint)activity.Id,
                        VideoKey = (ulong)activity.VideoKey
                    })
                },
                VideoKeyInfoList =
                {
                    GameData.VideoKeysConfig.VideoKeyInfoData.Select(video => new VideoKeyInfo
                    {
                        Id = (uint)video.Id,
                        VideoKey = (ulong)video.VideoKey
                    })
                }
            };

            SetData(proto);
        }
    }
}
