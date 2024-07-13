using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Game.Avatar
{
    public class AvatarManager : BasePlayerManager
    {
        public AvatarData AvatarData { get; private set; }

        public AvatarManager(PlayerInstance player) : base(player) 
        {
            AvatarData = DatabaseHelper.Instance!.GetInstanceOrCreateNew<AvatarData>(player.Uid);
            foreach (var avatar in AvatarData.Avatars)
            {
                avatar.PlayerData = player.Data;
                avatar.Excel = GameData.AvatarConfigData[avatar.AvatarId];
            }
        }

        public AvatarConfigExcel? AddAvatar(int avatarId, bool sync = true, bool notify = true, bool isGacha = false)
        {
            GameData.AvatarConfigData.TryGetValue(avatarId, out AvatarConfigExcel? avatarExcel);
            if (avatarExcel == null)
            {
                return null;
            }

            if (AvatarData.Avatars.Find(x => x.AvatarId == avatarId) != null) return null;

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
                if (GetHero() != null) return null;  // Only one hero
                avatar.HeroId = avatarId;
            }

            avatar.PlayerData = Player.Data;
            AvatarData.Avatars.Add(avatar);

            if (sync)
                Player.SendPacket(new PacketPlayerSyncScNotify(avatar));

            if (notify)
            {
                Player.SendPacket(new PacketAddAvatarScNotify(avatar.GetBaseAvatarId(), isGacha));
            }

            return avatarExcel;
        }

        public AvatarInfo? GetAvatar(int baseAvatarId)
        {
            if (baseAvatarId > 8000) baseAvatarId = 8001;
            return AvatarData.Avatars.Find(avatar => avatar.AvatarId == baseAvatarId);
        }

        public AvatarInfo? GetHero()
        {
            return AvatarData.Avatars.Find(avatar => avatar.HeroId > 0);
        }
    }
}
