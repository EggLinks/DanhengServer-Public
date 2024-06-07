using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Database.Message;
using EggLink.DanhengServer.Enums.Item;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketPlayerSyncScNotify : BasePacket
    {
        public PacketPlayerSyncScNotify(ItemData item) : base(CmdIds.PlayerSyncScNotify)
        {
            var proto = new PlayerSyncScNotify();
            AddItemToProto(item, proto);
            SetData(proto);
        }

        public PacketPlayerSyncScNotify(List<ItemData> item) : base(CmdIds.PlayerSyncScNotify)
        {
            var proto = new PlayerSyncScNotify();
            foreach (var i in item)
            {
                AddItemToProto(i, proto);
            }
            SetData(proto);
        }

        public PacketPlayerSyncScNotify(AvatarInfo avatar) : base(CmdIds.PlayerSyncScNotify)
        {
            var proto = new PlayerSyncScNotify
            {
                AvatarSync = new()
            };
            proto.AvatarSync.AvatarList.Add(avatar.ToProto());

            if (avatar.HeroId > 0)
            {
                proto.BasicTypeInfoList.Add(avatar.ToHeroProto());
            }

            SetData(proto);
        }

        public PacketPlayerSyncScNotify(List<AvatarInfo> avatars) : base(CmdIds.PlayerSyncScNotify)
        {
            var proto = new PlayerSyncScNotify
            {
                AvatarSync = new()
            };

            foreach (var avatar in avatars)
            {
                proto.AvatarSync.AvatarList.Add(avatar.ToProto());
                if (avatar.HeroId > 0)
                {
                    proto.BasicTypeInfoList.Add(avatar.ToHeroProto());
                }
            }

            SetData(proto);
        }
        
        public PacketPlayerSyncScNotify(AvatarInfo avatar, ItemData item) : base(CmdIds.PlayerSyncScNotify)
        {
            var proto = new PlayerSyncScNotify();
            AddItemToProto(item, proto);
            proto.AvatarSync = new();
            proto.AvatarSync.AvatarList.Add(avatar.ToProto());

            if (avatar.HeroId > 0)
            {
                proto.BasicTypeInfoList.Add(avatar.ToHeroProto());
            }

            SetData(proto);
        }

        public PacketPlayerSyncScNotify(MissionSync mission) : base(CmdIds.PlayerSyncScNotify)
        {
            var proto = new PlayerSyncScNotify
            {
                MissionSync = mission,
            };

            SetData(proto);
        }

        public PacketPlayerSyncScNotify(PlayerBasicInfo info) : base(CmdIds.PlayerSyncScNotify)
        {
            var proto = new PlayerSyncScNotify()
            {
                BasicInfo = info,
            };

            SetData(proto);
        }

        public PacketPlayerSyncScNotify(MessageGroupData? groupData, MessageSectionData? sectionData) : base(CmdIds.PlayerSyncScNotify)
        {
            var proto = new PlayerSyncScNotify();

            if (groupData != null)
            {
                proto.MessageGroupStatus.Add(new GroupStatus
                {
                    GroupId = (uint)groupData.GroupId,
                    GroupStatus_ = groupData.Status,
                    RefreshTime = groupData.RefreshTime,
                });
            }

            if (sectionData != null)
            {
                proto.SectionStatus.Add(new SectionStatus
                {
                    SectionId = (uint)sectionData.SectionId,
                    SectionStatus_ = sectionData.Status,
                });
            }

            SetData(proto);
        }

        private void AddItemToProto(ItemData item, PlayerSyncScNotify notify)
        {
            GameData.ItemConfigData.TryGetValue(item.ItemId, out var itemConfig);
            if (itemConfig == null) return;
            switch (itemConfig.ItemMainType)
            {
                case ItemMainTypeEnum.Equipment:
                    if (item.Count > 0)
                    {
                        notify.EquipmentList.Add(item.ToEquipmentProto());
                    } else
                    {
                        notify.DelEquipmentList.Add((uint)item.UniqueId);
                    }
                    break;
                case ItemMainTypeEnum.Relic:
                    if (item.Count > 0)
                    {
                        notify.RelicList.Add(item.ToRelicProto());
                    }
                    else
                    {
                        notify.DelRelicList.Add((uint)item.UniqueId);
                    }
                    break;
                case ItemMainTypeEnum.Mission:
                case ItemMainTypeEnum.Material:
                case ItemMainTypeEnum.Usable:
                    notify.MaterialList.Add(item.ToMaterialProto());
                    break;
            }

            return;
        }
    }
}
