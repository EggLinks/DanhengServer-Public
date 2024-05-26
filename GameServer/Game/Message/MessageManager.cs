using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Message;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Util;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Message
{
    public class MessageManager(PlayerInstance player) : BasePlayerManager(player)
    {
        public MessageData Data { get; private set; } = DatabaseHelper.Instance!.GetInstanceOrCreateNew<MessageData>(player.Uid);
        public List<MessageSectionData> PendingMessagePerformSectionList { get; private set; } = [];

        #region Get

        public MessageSectionData? GetMessageSectionData(int sectionId)
        {
            GameData.MessageSectionConfigData.TryGetValue(sectionId, out var sectionConfig);
            if (sectionConfig == null)
            {
                return null;
            }
            var groupId = sectionConfig.GroupID;
            if (!Data.Groups.TryGetValue(groupId, out var group))
            {
                return null;
            }
            return group.Sections.FirstOrDefault(m => m.SectionId == sectionId);
        }

        public List<MessageGroup> GetMessageGroup(int contactId)
        {
            GameData.MessageContactsConfigData.TryGetValue(contactId, out var contactConfig);
            if (contactConfig == null)
            {
                return [];
            }
            var result = new List<MessageGroup>();
            foreach (var item in contactConfig.Groups)
            {
                if (Data.Groups.TryGetValue(item.ID, out var group))
                {
                    var groupInfo = new MessageGroup
                    {
                        Id = (uint)item.ID,
                        Status = group.Status,
                        RefreshTime = group.RefreshTime,
                    };
                    foreach (var section in group.Sections)
                    {
                        var sectionInfo = new MessageSection
                        {
                            Id = (uint)section.SectionId,
                            Status = section.Status,
                        };
                        sectionInfo.ToChooseItemId.AddRange(section.ToChooseItemId.Select(m => (uint)m));
                        sectionInfo.ItemList.AddRange(section.Items.Select(m => new MessageItem()
                        {
                            ItemId = (uint)m.ItemId,
                        }));
                        groupInfo.MessageSectionList.Add(sectionInfo);
                    }
                    groupInfo.MessageSectionId = (uint)group.CurrentSectionId;
                    result.Add(groupInfo);
                }
                else
                {
                    result.Add(new MessageGroup
                    {
                        Id = (uint)item.ID,
                        Status = MessageGroupStatus.MessageGroupNone,
                        RefreshTime = 0,
                    });
                }
            }
            return result;
        }

        #endregion

        #region Action

        public void AddMessageSection(int sectionId)
        {
            GameData.MessageSectionConfigData.TryGetValue(sectionId, out var sectionConfig);
            if (sectionConfig == null) return;
            foreach (var item in sectionConfig.StartMessageItemIDList)
            {
                AddMessageItem(item);
            }
        }

        public void AddMessageItem(int itemId)
        {
            GameData.MessageItemConfigData.TryGetValue(itemId, out var itemConfig);
            if (itemConfig == null) return;
            var groupId = itemConfig.GroupID;
            var sectionId = itemConfig.SectionID;
            if (!Data.Groups.TryGetValue(groupId, out var group))
            {
                group = new MessageGroupData
                {
                    GroupId = groupId,
                    CurrentSectionId = sectionId,
                    RefreshTime = Extensions.GetUnixSec(),
                    Status = MessageGroupStatus.MessageGroupDoing,
                };
                group.Sections.Add(new MessageSectionData
                {
                    SectionId = sectionId,
                    Status = MessageSectionStatus.MessageSectionDoing,
                    ToChooseItemId = itemConfig.NextItemIDList,
                });
                Data.Groups.Add(groupId, group);
            }
            else
            {
                group.CurrentSectionId = sectionId;
                group.RefreshTime = Extensions.GetUnixSec();
                group.Status = MessageGroupStatus.MessageGroupDoing;
                if (!group.Sections.Any(m => m.SectionId == sectionId))
                {
                    group.Sections.Add(new MessageSectionData
                    {
                        SectionId = sectionId,
                        Status = MessageSectionStatus.MessageSectionDoing,
                        ToChooseItemId = itemConfig.NextItemIDList,
                    });
                }
                else
                {
                    group.Sections.First(m => m.SectionId == sectionId).Status = MessageSectionStatus.MessageSectionDoing;
                }
            }

            DatabaseHelper.Instance!.UpdateInstance(Data);

            // sync
            var notify = new PacketPlayerSyncScNotify(group, group.Sections.First(m => m.SectionId == sectionId));
            Player.SendPacket(notify);
        }

        public void FinishSection(int sectionId)
        {
            GameData.MessageSectionConfigData.TryGetValue(sectionId, out var sectionConfig);
            if (sectionConfig == null) return;
            var groupId = sectionConfig.GroupID;
            if (!Data.Groups.TryGetValue(groupId, out var group)) return;
            var section = group.Sections.First(m => m.SectionId == sectionId);
            if (section.Status != MessageSectionStatus.MessageSectionDoing) return;
            section.Status = MessageSectionStatus.MessageSectionFinish;
            if (group.Sections.All(m => m.Status == MessageSectionStatus.MessageSectionFinish))
            {
                group.Status = MessageGroupStatus.MessageGroupFinish;
            }
            DatabaseHelper.Instance!.UpdateInstance(Data);

            // sync
            var notify = new PacketPlayerSyncScNotify(group, section);
            Player.SendPacket(notify);

            // broadcast to mission system
            Player.MissionManager!.HandleFinishType(Enums.MissionFinishTypeEnum.MessagePerformSectionFinish);
            Player.MissionManager!.HandleFinishType(Enums.MissionFinishTypeEnum.MessageSectionFinish);
        }

        public void FinishMessageItem(int itemId)
        {
            GameData.MessageItemConfigData.TryGetValue(itemId, out var itemConfig);
            if (itemConfig == null) return;
            var groupId = itemConfig.GroupID;
            var sectionId = itemConfig.SectionID;
            if (!Data.Groups.TryGetValue(groupId, out var group)) return;
            var section = group.Sections.First(m => m.SectionId == sectionId);
            if (section.Status != MessageSectionStatus.MessageSectionDoing) return;
            if (!section.ToChooseItemId.Contains(itemId)) return;
            section.ToChooseItemId.Clear();
            section.Items.Add(new MessageItemData
            {
                ItemId = itemId,
            });
            section.ToChooseItemId.AddRange(itemConfig.NextItemIDList);

            group.RefreshTime = Extensions.GetUnixSec();

            DatabaseHelper.Instance!.UpdateInstance(Data);
            // sync
            var notify = new PacketPlayerSyncScNotify(group, section);
            Player.SendPacket(notify);
        }

        #endregion
    }
}
