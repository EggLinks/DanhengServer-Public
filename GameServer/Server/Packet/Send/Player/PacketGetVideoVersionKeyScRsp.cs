using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

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
                    VideoKey = activity.VideoKey
                })
            },
            VideoKeyInfoList =
            {
                GameData.VideoKeysConfig.VideoKeyInfoData.Select(video => new VideoKeyInfo
                {
                    Id = (uint)video.Id,
                    VideoKey = video.VideoKey
                })
            }
        };

        SetData(proto);
    }
}