using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Avatar;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Avatar;

public class AvatarManager : BasePlayerManager
{
    public AvatarManager(PlayerInstance player) : base(player)
    {
        AvatarData = DatabaseHelper.Instance!.GetInstanceOrCreateNew<AvatarData>(player.Uid);
        foreach (var avatar in AvatarData.Avatars)
        {
            avatar.PlayerData = player.Data;
            avatar.Excel = GameData.AvatarConfigData[avatar.AvatarId];
        }
    }

    public AvatarData AvatarData { get; }

    public async ValueTask<AvatarConfigExcel?> AddAvatar(int avatarId, bool sync = true, bool notify = true,
        bool isGacha = false)
    {
        GameData.AvatarConfigData.TryGetValue(avatarId, out var avatarExcel);
        if (avatarExcel == null) return null;

        GameData.MultiplePathAvatarConfigData.TryGetValue(avatarId, out var multiPathAvatar);
        if (multiPathAvatar != null && multiPathAvatar.BaseAvatarID != avatarId)
        {
            // Is path
            foreach (var avatarData in AvatarData.Avatars)
                if (avatarData.AvatarId == multiPathAvatar.BaseAvatarID)
                {
                    // Add path for the character
                    avatarData.PathInfoes.Add(avatarId, new PathInfo(avatarId));
                    break;
                }

            return null;
        }

        var avatar = new AvatarInfo(avatarExcel)
        {
            AvatarId = avatarId >= 8001 ? 8001 : avatarId,
            Level = 1,
            Timestamp = Extensions.GetUnixSec(),
            CurrentHp = 10000,
            CurrentSp = 0
        };

        if (avatarId >= 8001)
        {
            if (GetHero() != null) return null; // Only one hero
            avatar.PathId = avatarId;
        }

        avatar.PlayerData = Player.Data;
        AvatarData.Avatars.Add(avatar);

        if (sync)
            await Player.SendPacket(new PacketPlayerSyncScNotify(avatar));

        if (notify) await Player.SendPacket(new PacketAddAvatarScNotify(avatar.GetBaseAvatarId(), isGacha));

        return avatarExcel;
    }

    public AvatarInfo? GetAvatar(int avatarId)
    {
        if (avatarId > 8000) avatarId = 8001;
        if (GameData.MultiplePathAvatarConfigData.ContainsKey(avatarId))
            avatarId = GameData.MultiplePathAvatarConfigData[avatarId].BaseAvatarID;
        return AvatarData.Avatars.Find(avatar => avatar.AvatarId == avatarId);
    }

    public AvatarInfo? GetHero()
    {
        return AvatarData.Avatars.Find(avatar => avatar.AvatarId == 8001);
    }
}