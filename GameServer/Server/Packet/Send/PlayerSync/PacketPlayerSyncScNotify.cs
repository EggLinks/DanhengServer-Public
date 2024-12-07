using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Database.Message;
using EggLink.DanhengServer.Database.Quests;
using EggLink.DanhengServer.Enums.Item;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;

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
        foreach (var i in item) AddItemToProto(i, proto);
        SetData(proto);
    }

    public PacketPlayerSyncScNotify(AvatarInfo avatar) : base(CmdIds.PlayerSyncScNotify)
    {
        var proto = new PlayerSyncScNotify
        {
            AvatarSync = new AvatarSync()
        };
        proto.AvatarSync.AvatarList.Add(avatar.ToProto());

        if (GameData.MultiplePathAvatarConfigData.ContainsKey(avatar.AvatarId))
            proto.MultiPathAvatarInfoList.Add(avatar.ToAvatarPathProto());

        SetData(proto);
    }

    public PacketPlayerSyncScNotify(List<AvatarInfo> avatars) : base(CmdIds.PlayerSyncScNotify)
    {
        var proto = new PlayerSyncScNotify
        {
            AvatarSync = new AvatarSync()
        };

        foreach (var avatar in avatars)
        {
            proto.AvatarSync.AvatarList.Add(avatar.ToProto());
            if (GameData.MultiplePathAvatarConfigData.ContainsKey(avatar.AvatarId))
                proto.MultiPathAvatarInfoList.Add(avatar.ToAvatarPathProto());
        }

        SetData(proto);
    }

    public PacketPlayerSyncScNotify(AvatarInfo avatar, ItemData item) : base(CmdIds.PlayerSyncScNotify)
    {
        var proto = new PlayerSyncScNotify();
        AddItemToProto(item, proto);
        proto.AvatarSync = new AvatarSync();
        proto.AvatarSync.AvatarList.Add(avatar.ToProto());

        if (GameData.MultiplePathAvatarConfigData.ContainsKey(avatar.AvatarId))
            proto.MultiPathAvatarInfoList.Add(avatar.ToAvatarPathProto());

        SetData(proto);
    }

    public PacketPlayerSyncScNotify(MissionSync mission) : base(CmdIds.PlayerSyncScNotify)
    {
        var proto = new PlayerSyncScNotify
        {
            MissionSync = mission
        };

        SetData(proto);
    }

    public PacketPlayerSyncScNotify(PlayerBasicInfo info) : base(CmdIds.PlayerSyncScNotify)
    {
        var proto = new PlayerSyncScNotify
        {
            BasicInfo = info
        };

        SetData(proto);
    }

    public PacketPlayerSyncScNotify(PlayerBasicInfo info, List<ItemData> item) : base(CmdIds.PlayerSyncScNotify)
    {
        var proto = new PlayerSyncScNotify
        {
            BasicInfo = info
        };

        foreach (var i in item) AddItemToProto(i, proto);

        SetData(proto);
    }

    public PacketPlayerSyncScNotify(MessageGroupData? groupData, MessageSectionData? sectionData) : base(
        CmdIds.PlayerSyncScNotify)
    {
        var proto = new PlayerSyncScNotify();

        if (groupData != null)
            proto.MessageGroupStatus.Add(new GroupStatus
            {
                GroupId = (uint)groupData.GroupId,
                GroupStatus_ = groupData.Status,
                RefreshTime = groupData.RefreshTime
            });

        if (sectionData != null)
            proto.SectionStatus.Add(new SectionStatus
            {
                SectionId = (uint)sectionData.SectionId,
                SectionStatus_ = sectionData.Status
            });

        SetData(proto);
    }

    public PacketPlayerSyncScNotify(QuestInfo quest) : base(CmdIds.PlayerSyncScNotify)
    {
        var proto = new PlayerSyncScNotify();
        proto.QuestList.Add(quest.ToProto());

        SetData(proto);
    }

    public PacketPlayerSyncScNotify(List<QuestInfo> quest) : base(CmdIds.PlayerSyncScNotify)
    {
        var proto = new PlayerSyncScNotify();
        proto.QuestList.Add(quest.Select(x => x.ToProto()));

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
                    notify.EquipmentList.Add(item.ToEquipmentProto());
                else
                    notify.DelEquipmentList.Add((uint)item.UniqueId);
                break;
            case ItemMainTypeEnum.Relic:
                if (item.Count > 0)
                    notify.RelicList.Add(item.ToRelicProto());
                else
                    notify.DelRelicList.Add((uint)item.UniqueId);
                break;
            case ItemMainTypeEnum.Mission:
            case ItemMainTypeEnum.Material:
            case ItemMainTypeEnum.Pet:
            case ItemMainTypeEnum.Usable:
                notify.MaterialList.Add(item.ToMaterialProto());
                break;
        }
    }
}