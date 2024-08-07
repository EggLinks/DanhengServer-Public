using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ContentPackage;

public class PacketContentPackageSyncDataScNotify : BasePacket
{
    public PacketContentPackageSyncDataScNotify() : base(CmdIds.ContentPackageSyncDataScNotify)
    {
        var proto = new ContentPackageSyncDataScNotify
        {
            Data = new ContentPackageData
            {
                ContentPackageList =
                {
                    GameData.ContentPackageConfigData.Select(x => new ContentPackageInfo
                    {
                        ContentId = (uint)x.Key,
                        Status = ContentPackageStatus.Finished
                    })
                }
            }
        };

        SetData(proto);
    }
}