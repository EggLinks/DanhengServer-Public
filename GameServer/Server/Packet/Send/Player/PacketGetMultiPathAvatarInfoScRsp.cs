using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketGetMultiPathAvatarInfoScRsp : BasePacket
{
    public PacketGetMultiPathAvatarInfoScRsp(PlayerInstance player) : base(CmdIds.GetMultiPathAvatarInfoScRsp)
    {
        var proto = new GetMultiPathAvatarInfoScRsp();

        foreach (var multiPathAvatar in GameData.MultiplePathAvatarConfigData.Values)
            if (!proto.CurAvatarPath.ContainsKey((uint)multiPathAvatar.BaseAvatarID))
            {
                var avatar = player.AvatarManager!.GetAvatar(multiPathAvatar.BaseAvatarID);
                if (avatar != null)
                {
                    if (avatar.AvatarId == 8001) // only add main character
                        proto.BasicTypeIdList.Add((uint)avatar.PathId);
                    var pathId = avatar.PathId > 0 ? avatar.PathId : avatar.AvatarId;
                    if (pathId == 8001)
                        if (player.Data.CurrentGender != Gender.Man)
                            pathId++;
                    proto.CurAvatarPath.Add((uint)avatar.AvatarId, (MultiPathAvatarType)pathId);
                    if (avatar.AvatarId == multiPathAvatar.BaseAvatarID)
                        proto.MultiPathAvatarInfoList.Add(avatar.ToAvatarPathProto());
                }
            }

        SetData(proto);
    }
}