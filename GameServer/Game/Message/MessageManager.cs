using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Message;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Message;

public class MessageManager(PlayerInstance player) : BasePlayerManager(player)
{
    public MessageData Data { get; } = DatabaseHelper.Instance!.GetInstanceOrCreateNew<MessageData>(player.Uid);
    public List<MessageSectionData> PendingMessagePerformSectionList { get; private set; } = [];

    #region Get

    public MessageSectionData? GetMessageSectionData(int sectionId)
    {
        GameData.MessageSectionConfigData.TryGetValue(sectionId, out var sectionConfig);
        if (sectionConfig == null) return null;
        var groupId = sectionConfig.GroupID;
        if (!Data.Groups.TryGetValue(groupId, out var group)) return null;
        return group.Sections.FirstOrDefault(m => m.SectionId == sectionId);
    }

    public List<MessageGroup> GetMessageGroup(int contactId)
    {
        GameData.MessageContactsConfigData.TryGetValue(contactId, out var contactConfig);
        if (contactConfig == null) return [];
        var result = new List<MessageGroup>();
        foreach (var item in contactConfig.Groups)
            if (Data.Groups.TryGetValue(item.ID, out var group))
            {
                var groupInfo = new MessageGroup
                {
                    Id = (uint)item.ID,
                    Status = group.Status,
                    RefreshTime = group.RefreshTime
                };
                foreach (var section in group.Sections)
                {
                    var sectionInfo = new MessageSection
                    {
                        Id = (uint)section.SectionId,
                        Status = section.Status
                    };
                    sectionInfo.MessageItemList.AddRange(section.ToChooseItemId.Select(m => (uint)m));
                    sectionInfo.ItemList.AddRange(section.Items.Select(m => new MessageItem
                    {
                        ItemId = (uint)m.ItemId
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
                    RefreshTime = 0
                });
            }

        return result;
    }

    #endregion

    #region Action

    public async ValueTask AddMessageSection(int sectionId)
    {
        GameData.MessageSectionConfigData.TryGetValue(sectionId, out var sectionConfig);
        if (sectionConfig == null) return;

        if (Data.Groups.TryGetValue(sectionConfig.GroupID, out var group) &&
            group.Sections.Find(x => x.SectionId == sectionId) != null)
            // already exist
            return;

        foreach (var item in sectionConfig.StartMessageItemIDList) await AddMessageItem(item);
    }

    public async ValueTask AddMessageItem(int itemId, bool sendPacket = true)
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
                Status = MessageGroupStatus.MessageGroupDoing
            };
            group.Sections.Add(new MessageSectionData
            {
                SectionId = sectionId,
                Status = MessageSectionStatus.MessageSectionDoing,
                ToChooseItemId = itemConfig.NextItemIDList
            });
            Data.Groups.Add(groupId, group);
        }
        else
        {
            group.CurrentSectionId = sectionId;
            group.RefreshTime = Extensions.GetUnixSec();
            group.Status = MessageGroupStatus.MessageGroupDoing;
            if (group.Sections.All(m => m.SectionId != sectionId)) // new section
            {
                group.Sections.Add(new MessageSectionData
                {
                    SectionId = sectionId,
                    Status = MessageSectionStatus.MessageSectionDoing,
                    ToChooseItemId = itemConfig.NextItemIDList
                });

                if (itemConfig.NextItemIDList.Count == 1) await FinishMessageItem(itemConfig.NextItemIDList[0], false);
            }
            else // old
            {
                group.Sections.First(m => m.SectionId == sectionId).Status = MessageSectionStatus.MessageSectionDoing;
            }
        }

        // sync
        if (sendPacket)
        {
            var notify = new PacketPlayerSyncScNotify(group, group.Sections.First(m => m.SectionId == sectionId));
            await Player.SendPacket(notify);
        }
    }

    public async ValueTask FinishSection(int sectionId, bool sendPacket = true)
    {
        GameData.MessageSectionConfigData.TryGetValue(sectionId, out var sectionConfig);
        if (sectionConfig == null) return;
        var groupId = sectionConfig.GroupID;
        if (!Data.Groups.TryGetValue(groupId, out var group)) return;
        var section = group.Sections.First(m => m.SectionId == sectionId);
        if (section.Status != MessageSectionStatus.MessageSectionDoing) return;
        section.Status = MessageSectionStatus.MessageSectionFinish;
        if (group.Sections.All(m => m.Status == MessageSectionStatus.MessageSectionFinish))
            group.Status = MessageGroupStatus.MessageGroupFinish;

        // sync
        if (sendPacket)
        {
            var notify = new PacketPlayerSyncScNotify(group, section);
            await Player.SendPacket(notify);
        }

        // broadcast to mission system
        await Player.MissionManager!.HandleFinishType(MissionFinishTypeEnum.MessagePerformSectionFinish);
        await Player.MissionManager!.HandleFinishType(MissionFinishTypeEnum.MessageSectionFinish);
    }

    public async ValueTask FinishMessageItem(int itemId, bool sendPacket = true)
    {
        GameData.MessageItemConfigData.TryGetValue(itemId, out var itemConfig);
        if (itemConfig == null) return;
        var groupId = itemConfig.GroupID;
        var sectionId = itemConfig.SectionID;
        if (!Data.Groups.TryGetValue(groupId, out var group)) return;
        var section = group.Sections.First(m => m.SectionId == sectionId);
        if (section.Status != MessageSectionStatus.MessageSectionDoing) return;
        //if (!section.ToChooseItemId.Contains(itemId)) return;
        section.ToChooseItemId.Clear();
        section.Items.Add(new MessageItemData
        {
            ItemId = itemId
        });
        section.ToChooseItemId.AddRange(itemConfig.NextItemIDList);

        group.RefreshTime = Extensions.GetUnixSec();

        if (section.ToChooseItemId.Count == 1) // if only one item, auto finish
            await FinishMessageItem(section.ToChooseItemId[0], false);

        if (sendPacket)
        {
            // sync
            var notify = new PacketPlayerSyncScNotify(group, section);
            await Player.SendPacket(notify);
        }
    }

    #endregion
}