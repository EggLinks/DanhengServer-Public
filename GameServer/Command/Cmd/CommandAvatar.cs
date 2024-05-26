using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("avatar", "Game.Command.Avatar.Desc", "Game.Command.Avatar.Usage")]
    public class CommandAvatar : ICommand
    {
        [CommandMethod("talent")]
        public void SetTalent(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            if (arg.BasicArgs.Count < 2)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }
            var Player = arg.Target.Player!;
            // change basic type
            var avatarId = arg.GetInt(0);
            var level = arg.GetInt(1);
            if (level < 0 || level > 10)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.InvalidLevel", I18nManager.Translate("Word.Talent")));
                return;
            }
            var player = arg.Target.Player!;
            if (avatarId == -1)
            {
                player.AvatarManager!.AvatarData.Avatars.ForEach(avatar =>
                {
                    if (avatar.HeroId > 0)
                    {
                        avatar.SkillTreeExtra.TryGetValue(avatar.HeroId, out var hero);
                        hero ??= [];
                        var excel = GameData.AvatarConfigData[avatar.HeroId];
                        excel.SkillTree.ForEach(talent =>
                        {
                            hero[talent.PointID] = Math.Min(level, talent.MaxLevel);
                        });
                    } else
                    {
                        avatar.Excel?.SkillTree.ForEach(talent =>
                        {
                            avatar.SkillTree![talent.PointID] = Math.Min(level, talent.MaxLevel);
                        });
                    }
                });
                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AllAvatarsLevelSet", I18nManager.Translate("Word.Talent"), level.ToString()));

                // sync
                player.SendPacket(new PacketPlayerSyncScNotify(player.AvatarManager.AvatarData.Avatars));

                return;
            }
            var avatar = player.AvatarManager!.GetAvatar(avatarId);
            if (avatar == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AvatarNotFound"));
                return;
            }
            avatar.Excel?.SkillTree.ForEach(talent =>
            {
                avatar.SkillTree![talent.PointID] = Math.Min(level, talent.MaxLevel);
            });

            // sync
            player.SendPacket(new PacketPlayerSyncScNotify(avatar));

            arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AvatarLevelSet", avatar.Excel?.Name ?? avatarId.ToString(), I18nManager.Translate("Word.Talent"), level.ToString()));
        }

        [CommandMethod("get")]
        public void GetAvatar(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            if (arg.BasicArgs.Count < 1)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
            }

            var id = arg.GetInt(0);
            var excel = arg.Target.Player!.AvatarManager!.AddAvatar(id);

            if (excel == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AvatarFailedGet", id.ToString()));
                return;
            }
            arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AvatarGet", excel.Name ?? id.ToString()));
        }

        [CommandMethod("rank")]
        public void SetRank(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            if (arg.BasicArgs.Count < 2)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
            }

            var id = arg.GetInt(0);
            var rank = arg.GetInt(1);
            if (rank < 0 || rank > 6)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.InvalidLevel", I18nManager.Translate("Word.Rank")));
                return;
            }
            if (id == -1)
            {
                arg.Target.Player!.AvatarManager!.AvatarData.Avatars.ForEach(avatar =>
                {
                    avatar.Rank = Math.Min(rank, 6);
                });
                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AllAvatarsLevelSet", I18nManager.Translate("Word.Rank"), rank.ToString()));

                // sync
                arg.Target.SendPacket(new PacketPlayerSyncScNotify(arg.Target.Player!.AvatarManager.AvatarData.Avatars));
            }
            else
            {
                var avatar = arg.Target.Player!.AvatarManager!.GetAvatar(id);
                if (avatar == null)
                {
                    arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AvatarNotFound"));
                    return;
                }
                avatar.Rank = Math.Min(rank, 6);

                // sync
                arg.Target.SendPacket(new PacketPlayerSyncScNotify(avatar));

                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AvatarLevelSet", avatar.Excel?.Name ?? id.ToString(), I18nManager.Translate("Word.Rank"), rank.ToString()));
            }
        }

        [CommandMethod("level")]
        public void SetLevel(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            if (arg.BasicArgs.Count < 2)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }

            var id = arg.GetInt(0);
            var level = arg.GetInt(1);
            if (level < 1 || level > 80)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.InvalidLevel", I18nManager.Translate("Word.Level")));
                return;
            }

            if (id == -1)
            {
                arg.Target.Player!.AvatarManager!.AvatarData.Avatars.ForEach(avatar =>
                {
                    avatar.Level = Math.Min(level, 80);
                    avatar.Promotion = GameData.GetMinPromotionForLevel(avatar.Level);
                });
                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AllAvatarsLevelSet", I18nManager.Translate("Word.Level"), level.ToString()));

                // sync
                arg.Target.SendPacket(new PacketPlayerSyncScNotify(arg.Target.Player!.AvatarManager.AvatarData.Avatars));
            }
            else
            {
                var avatar = arg.Target.Player!.AvatarManager!.GetAvatar(id);
                if (avatar == null)
                {
                    arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AvatarNotFound"));
                    return;
                }
                avatar.Level = Math.Min(level, 80);
                avatar.Promotion = GameData.GetMinPromotionForLevel(avatar.Level);

                // sync
                arg.Target.SendPacket(new PacketPlayerSyncScNotify(avatar));

                arg.SendMsg(I18nManager.Translate("Game.Command.Avatar.AvatarLevelSet", avatar.Excel?.Name ?? id.ToString(), I18nManager.Translate("Word.Level"), level.ToString()));
            }
        }
    }
}
