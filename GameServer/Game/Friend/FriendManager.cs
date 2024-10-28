using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Friend;
using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.GameServer.Command;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Chat;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Friend;

public class FriendManager(PlayerInstance player) : BasePlayerManager(player)
{
    public FriendData FriendData { get; set; } =
        DatabaseHelper.Instance!.GetInstanceOrCreateNew<FriendData>(player.Uid);

    public async ValueTask AddFriend(int targetUid)
    {
        var target = DatabaseHelper.Instance!.GetInstance<FriendData>(targetUid);
        if (target == null) return;

        if (FriendData.FriendList.Contains(targetUid)) // already friend
            return;

        if (FriendData.BlackList.Contains(targetUid)) // in black list
            return;

        if (FriendData.SendApplyList.Contains(targetUid)) // already send apply
            return;

        if (FriendData.ReceiveApplyList.Contains(targetUid)) // already receive apply
            return;

        FriendData.SendApplyList.Add(targetUid);
        target.ReceiveApplyList.Add(Player.Uid);

        var targetPlayer = Listener.GetActiveConnection(targetUid);
        if (targetPlayer != null)
        {
            await targetPlayer.SendPacket(new PacketSyncApplyFriendScNotify(Player.Data));
            targetPlayer.Player!.FriendManager!.FriendData.ReceiveApplyList.Add(Player.Uid);
        }

        DatabaseHelper.ToSaveUidList.Add(targetUid);
    }

    public async ValueTask<PlayerData?> ConfirmAddFriend(int targetUid)
    {
        var target = DatabaseHelper.Instance!.GetInstance<FriendData>(targetUid);
        if (target == null) return null;

        if (FriendData.FriendList.Contains(targetUid)) return null;

        if (FriendData.BlackList.Contains(targetUid)) return null;

        if (!FriendData.ReceiveApplyList.Contains(targetUid)) return null;

        FriendData.ReceiveApplyList.Remove(targetUid);
        FriendData.FriendList.Add(targetUid);
        target.FriendList.Add(Player.Uid);
        target.SendApplyList.Remove(Player.Uid);

        var targetData = PlayerData.GetPlayerByUid(targetUid)!;
        var targetPlayer = Listener.GetActiveConnection(targetUid);
        if (targetPlayer != null)
            await targetPlayer.SendPacket(new PacketSyncHandleFriendScNotify((uint)Player.Uid, true, Player.Data));

        await Player.SendPacket(new PacketSyncHandleFriendScNotify((uint)targetData.Uid, true, targetData));

        return targetData;
    }

    public void RefuseAddFriend(int targetUid)
    {
        var target = DatabaseHelper.Instance!.GetInstance<FriendData>(targetUid);
        if (target == null) return;

        if (!FriendData.ReceiveApplyList.Contains(targetUid)) return;

        FriendData.ReceiveApplyList.Remove(targetUid);
        target.SendApplyList.Remove(Player.Uid);

        var targetPlayer = Listener.GetActiveConnection(targetUid);
        targetPlayer?.Player!.FriendManager!.FriendData.SendApplyList.Remove(Player.Uid);
        DatabaseHelper.ToSaveUidList.Add(targetUid);
    }

    public void RemoveFriend()
    {
    }

    public async ValueTask SendMessage(int sendUid, int recvUid, string? message = null, int? extraId = null)
    {
        var data = new FriendChatData
        {
            SendUid = sendUid,
            ReceiveUid = recvUid,
            Message = message ?? "",
            ExtraId = extraId ?? 0,
            SendTime = Extensions.GetUnixSec()
        };

        if (!FriendData.ChatHistory.TryGetValue(recvUid, out var value))
        {
            FriendData.ChatHistory[recvUid] = new FriendChatHistory();
            value = FriendData.ChatHistory[recvUid];
        }

        value.MessageList.Add(data);

        PacketRevcMsgScNotify proto;
        if (message != null)
            proto = new PacketRevcMsgScNotify((uint)recvUid, (uint)sendUid, message);
        else
            proto = new PacketRevcMsgScNotify((uint)recvUid, (uint)sendUid, (uint)(extraId ?? 0));

        await Player.SendPacket(proto);

        if (recvUid == ConfigManager.Config.ServerOption.ServerProfile.Uid)
            if (message?.StartsWith('/') == true)
            {
                var cmd = message[1..];
                CommandExecutor.ExecuteCommand(new PlayerCommandSender(Player), cmd);
            }

        // receive message
        var recvPlayer = Listener.GetActiveConnection(recvUid)?.Player!;
        if (recvPlayer != null)
        {
            await recvPlayer.FriendManager!.ReceiveMessage(sendUid, recvUid, message, extraId);
        }
        else
        {
            // offline
            var friendData = DatabaseHelper.Instance!.GetInstance<FriendData>(recvUid);
            if (friendData == null) return; // not exist maybe server profile
            if (!friendData.ChatHistory.TryGetValue(sendUid, out var history))
            {
                friendData.ChatHistory[sendUid] = new FriendChatHistory();
                history = friendData.ChatHistory[sendUid];
            }

            history.MessageList.Add(data);

            DatabaseHelper.ToSaveUidList.Add(recvUid);
        }
    }

    public async ValueTask ReceiveMessage(int sendUid, int recvUid, string? message = null, int? extraId = null)
    {
        var data = new FriendChatData
        {
            SendUid = sendUid,
            ReceiveUid = recvUid,
            Message = message ?? "",
            ExtraId = extraId ?? 0,
            SendTime = Extensions.GetUnixSec()
        };

        if (!FriendData.ChatHistory.TryGetValue(sendUid, out var value))
        {
            FriendData.ChatHistory[sendUid] = new FriendChatHistory();
            value = FriendData.ChatHistory[sendUid];
        }

        value.MessageList.Add(data);

        PacketRevcMsgScNotify proto;
        if (message != null)
            proto = new PacketRevcMsgScNotify((uint)recvUid, (uint)sendUid, message);
        else
            proto = new PacketRevcMsgScNotify((uint)recvUid, (uint)sendUid, (uint)(extraId ?? 0));

        await Player.SendPacket(proto);
    }

    public List<ChatMessageData> GetHistoryInfo(int uid)
    {
        if (!FriendData.ChatHistory.TryGetValue(uid, out var history)) return [];

        var info = new List<ChatMessageData>();

        foreach (var chat in history.MessageList)
            info.Add(new ChatMessageData
            {
                CreateTime = (ulong)chat.SendTime,
                Content = chat.Message,
                ExtraId = (uint)chat.ExtraId,
                SenderId = (uint)chat.SendUid,
                MessageType = chat.ExtraId > 0 ? MsgType.Emoji : MsgType.CustomText
            });

        info.Reverse();

        return info;
    }

    public List<PlayerData> GetFriendList()
    {
        List<PlayerData> list = [];

        foreach (var friend in FriendData.FriendList)
        {
            var player = PlayerData.GetPlayerByUid(friend);

            if (player != null) list.Add(player);
        }

        return list;
    }

    public List<PlayerData> GetBlackList()
    {
        List<PlayerData> list = [];

        foreach (var friend in FriendData.BlackList)
        {
            var player = PlayerData.GetPlayerByUid(friend);

            if (player != null) list.Add(player);
        }

        return list;
    }

    public List<PlayerData> GetSendApplyList()
    {
        List<PlayerData> list = [];

        foreach (var friend in FriendData.SendApplyList)
        {
            var player = PlayerData.GetPlayerByUid(friend);

            if (player != null) list.Add(player);
        }

        return list;
    }

    public List<PlayerData> GetReceiveApplyList()
    {
        List<PlayerData> list = [];

        foreach (var friend in FriendData.ReceiveApplyList)
        {
            var player = PlayerData.GetPlayerByUid(friend);

            if (player != null) list.Add(player);
        }

        return list;
    }

    public GetFriendListInfoScRsp ToProto()
    {
        var proto = new GetFriendListInfoScRsp();

        var serverProfile = ConfigManager.Config.ServerOption.ServerProfile;

        proto.FriendList.Add(new FriendSimpleInfo
        {
            PlayerInfo = new PlayerSimpleInfo
            {
                Uid = (uint)serverProfile.Uid,
                HeadIcon = (uint)serverProfile.HeadIcon,
                IsBanned = false,
                Level = (uint)serverProfile.Level,
                Nickname = serverProfile.Name,
                ChatBubbleId = (uint)serverProfile.ChatBubbleId,
                OnlineStatus = FriendOnlineStatus.Online,
                Platform = PlatformType.Pc,
                Signature = serverProfile.Signature
            },
            IsMarked = false,
            RemarkName = ""
        });

        foreach (var player in GetFriendList())
        {
            var status = Listener.GetActiveConnection(player.Uid) == null
                ? FriendOnlineStatus.Offline
                : FriendOnlineStatus.Online;
            proto.FriendList.Add(new FriendSimpleInfo
            {
                PlayerInfo = player.ToSimpleProto(status),
                IsMarked = false,
                RemarkName = ""
            });
        }

        foreach (var player in GetBlackList())
        {
            var status = Listener.GetActiveConnection(player.Uid) == null
                ? FriendOnlineStatus.Offline
                : FriendOnlineStatus.Online;
            proto.BlackList.Add(player.ToSimpleProto(status));
        }

        return proto;
    }

    public GetFriendApplyListInfoScRsp ToApplyListProto()
    {
        GetFriendApplyListInfoScRsp proto = new();

        foreach (var player in GetSendApplyList()) proto.SendApplyList.Add((uint)player.Uid);

        foreach (var player in GetReceiveApplyList())
        {
            var status = Listener.GetActiveConnection(player.Uid) == null
                ? FriendOnlineStatus.Offline
                : FriendOnlineStatus.Online;
            proto.ReceiveApplyList.Add(new FriendApplyInfo
            {
                PlayerInfo = player.ToSimpleProto(status)
            });
        }

        return proto;
    }
}