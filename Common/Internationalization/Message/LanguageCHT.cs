using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Internationalization.Message
{
    #region Root

    public class LanguageCHT
    {
        public GameTextCHT Game { get; } = new();
        public ServerTextCHT Server { get; } = new();
        public WordTextCHT Word { get; } = new();  // a placeholder for the actual word text
    }

    #endregion

    #region Layer 1

    /// <summary>
    /// path: Game
    /// </summary>
    public class GameTextCHT
    {
        public CommandTextCHT Command { get; } = new();
    }

    /// <summary>
    /// path: Server
    /// </summary>
    public class ServerTextCHT
    {
        public WebTextCHT Web { get; } = new();
    }

    /// <summary>
    /// path: Word
    /// </summary>
    public class WordTextCHT
    {
        public string Rank { get; } = "星魂";
        public string Avatar { get; } = "角色";
        public string Material { get; } = "材料";
        public string Relic { get; } = "遺器";
        public string Equipment { get; } = "光錐";
        public string Talent { get; } = "行跡";
        public string Banner { get; } = "卡池";
        public string Activity { get; } = "活動";
        public string Buff { get; } = "祝福";
        public string Miracle { get; } = "奇物";
        public string Unlock { get; } = "奢侈品";
    }

    #endregion

    #region Layer 2

    #region GameText

    /// <summary>
    /// path: Game.Command
    /// </summary>
    public class CommandTextCHT
    {
        public NoticeTextCHT Notice { get; } = new();

        public HeroTextCHT Hero { get; } = new();
        public AvatarTextCHT Avatar { get; } = new();
        public GiveTextCHT Give { get; } = new();
        public GiveAllTextCHT GiveAll { get; } = new();
        public LineupTextCHT Lineup { get; } = new();
        public HelpTextCHT Help { get; } = new();
        public KickTextCHT Kick { get; } = new();
        public MissionTextCHT Mission { get; } = new();
        public RelicTextCHT Relic { get; } = new();
        public ReloadTextCHT Reload { get; } = new();
        public RogueTextCHT Rogue { get; } = new();
        public SceneTextCHT Scene { get; } = new();
        public UnlockAllTextCHT UnlockAll { get; } = new();
        public MailTextCHT Mail { get; } = new();
    }

    #endregion

    #region ServerText

    /// <summary>
    /// path: Server.Web
    /// </summary>
    public class WebTextCHT
    {
    }

    #endregion

    #endregion

    #region Layer 3

    #region CommandText

    /// <summary>
    /// path: Game.Command.Notice
    /// </summary>
    public class NoticeTextCHT
    {
        public string PlayerNotFound { get; } = "未找到玩家！";
        public string InvalidArguments { get; } = "無效的參數！";
        public string NoPermission { get; } = "你沒有權限執行此動作！";
        public string CommandNotFound { get; } = "該指令不存在！輸入 '/help' 來取得幫助";
        public string TargetOffline { get; } = "目標玩家 {0}({1}) 已離線！清除目前目標";
        public string TargetFound { get; } = "找到目標玩家 {0}({1})，下次輸入指令時將預設對其執行";
        public string TargetNotFound { get; } = "未找到目標玩家 {0}!";
        public string InternalError { get; } = "處理指令時發生內部錯誤！";
    }

    /// <summary>
    /// path: Game.Command.Hero
    /// </summary>
    public class HeroTextCHT
    {
        public string Desc { get; } = "切換主角的性別或型態";
        public string Usage { get; } = "/hero <gender [1/2 - 1為男性,2為女性]>/<type [8001/8003/8005 - 分別為 毀滅 存護 同諧]>";
        public string GenderNotSpecified { get; } = "該性別不存在！";
        public string HeroTypeNotSpecified { get; } = "主角類型不存在！";
        public string GenderChanged { get; } = "性別已變更！";
        public string HeroTypeChanged { get; } = "主角類型已變更！";
    }

    /// <summary>
    /// path: Game.Command.UnlockAll
    /// </summary>
    public class UnlockAllTextCHT
    {
        public string Desc { get; } = "解鎖所有在類別內的對象";
        public string Usage { get; } = "/unlockall <mission - mission為任務>";
        public string AllMissionsUnlocked { get; } = "已解鎖所有任務！";
    }

    /// <summary>
    /// path: Game.Command.Avatar
    /// </summary>
    public class AvatarTextCHT
    {
        public string Desc { get; } = "設定玩家已有角色之屬性";
        public string Usage { get; } = "/avatar <talent [角色ID/-1] [行跡等級] - 設定行跡等級 角色ID為「-1」將鎖定所有角色>/<get [角色ID] - 獲取角色>/<rank [角色ID/-1] [星魂]>/<level [角色ID/-1] [角色等級]>";
        public string InvalidLevel { get; } = "無效之{0}等級";
        public string AllAvatarsLevelSet { get; } = "已將所有角色之 {0} 等級設定為{1}";
        public string AvatarLevelSet { get; } = "已將 {0} 角色之 {1} 等級設定為{2}";
        public string AvatarNotFound { get; } = "該角色不存在！";
        public string AvatarGet { get; } = "已成功取得角色 {0}！";
        public string AvatarFailedGet { get; } = "取得角色 {0} 失敗！";
    }

    /// <summary>
    /// path: Game.Command.Give
    /// </summary>
    public class GiveTextCHT
    {
        public string Desc { get; } = "給予玩家物品";
        public string Usage { get; } = "/give <物品ID> l<等級> x<數量> r<疊影>";
        public string ItemNotFound { get; } = "該物品不存在！";
        public string GiveItem { get; } = "已給予玩家 @{0} {1} 個物品 {2}";
    }

    /// <summary>
    /// path: Game.Command.GiveAll
    /// </summary>
    public class GiveAllTextCHT
    {
        public string Desc { get; } = "給予玩家全部指令類型之物品";
        public string Usage { get; } = "/giveall <avatar - 角色/equipment - 光錐/relic - 遺器/unlock - 汽泡等奢侈品> r<疊影> l<等級> x<數量>";
        public string GiveAllItems { get; } = "已給予所有 {0}, 各 {1} 個";
    }

    /// <summary>
    /// path: Game.Command.Lineup
    /// </summary>
    public class LineupTextCHT
    {
        public string Desc { get; } = "管理玩家之隊伍";
        public string Usage { get; } = "/lineup <mp [密技點數量 - 最高為2]>/<heal - 治癒>";
        public string PlayerGainedMp { get; } = "玩家已獲得 {0} 密技點";
        public string HealedAllAvatars { get; } = "已成功治癒所有於目前隊伍中之角色";
    }

    /// <summary>
    /// path: Game.Command.Help
    /// </summary>
    public class HelpTextCHT
    {
        public string Desc { get; } = "顯示幫助訊息";
        public string Usage { get; } = "/help";
        public string Commands { get; } = "指令：";
        public string CommandUsage { get; } = "｜使用方法：";
    }

    /// <summary>
    /// path: Game.Command.Kick
    /// </summary>
    public class KickTextCHT
    {
        public string Desc { get; } = "踢出玩家";
        public string Usage { get; } = "/kick";
        public string PlayerKicked { get; } = "玩家 {0} 已被踢出！";
    }

    /// <summary>
    /// path: Game.Command.Mission
    /// </summary>
    public class MissionTextCHT
    {
        public string Desc { get; } = "管理玩家之任務";
        public string Usage { get; } = "/mission <unlockall - 完成所有任務>/<pass - 完成所有進行中之任務>/<finish [副任務ID] - 完成指定任務>/<running - 取得正在進行以及可能存在問題之任務>/<reaccept [主任務ID] - 重新進行指定之主任務>";
        public string AllMissionsFinished { get; } = "已完成所有任務！";
        public string AllRunningMissionsFinished { get; } = "已完成共 {0} 個進行中之任務！";
        public string MissionFinished { get; } = "任務 {0} 已完成！";
        public string InvalidMissionId { get; } = "無效之任務ID！";
        public string NoRunningMissions { get; } = "目前沒有進行中之任務！";

        public string RunningMissions { get; } = "正在進行的任務：";
        public string PossibleStuckMissions { get; } = "可岑存在問題之任務：";
        public string MainMission { get; } = "主任務";

        public string MissionReAccepted { get; } = "已重新接受任務 {0}";
    }

    /// <summary>
    /// path: Game.Command.Relic
    /// </summary>
    public class RelicTextCHT
    {
        public string Desc { get; } = "管理玩家之遺器";
        public string Usage { get; } = "/relic <遺器ID> <主詞條ID> <小詞條ID1:小詞條等級> <小詞條ID2:小詞條等級> <小詞條ID3:小詞條等級> <小詞條ID4:小詞條等級> l<等級> x<數量>";
        public string RelicNotFound { get; } = "該遺器不存在！";
        public string InvalidMainAffixId { get; } = "該主詞條ID不存在！";
        public string InvalidSubAffixId { get; } = "該副詞條ID不存在！";
        public string RelicGiven { get; } = "已給予玩家 @{0} {1} 個遺器 {2}, 主詞條 {3}";
    }

    /// <summary>
    /// path: Game.Command.Reload
    /// </summary>
    public class ReloadTextCHT
    {
        public string Desc { get; } = "重新載入指定的設定";
        public string Usage { get; } = "/reload <設定名稱>(banner - 卡池, activity - 活動)";
        public string ConfigReloaded { get; } = "設定 {0} 已重新載入完成！";
    }

    /// <summary>
    /// path: Game.Command.Rogue
    /// </summary>
    public class RogueTextCHT
    {
        public string Desc { get; } = "管理玩家模擬宇宙中的資料";
        public string Usage { get; } = "/rogue <money [宇宙碎片數量]>/<buff [祝福ID/-1 (-1 - 全部祝福)]>/<miracle [奇物ID]>/<enhance [祝福ID/-1]>/<unstuck - 脫離事件>";
        public string PlayerGainedMoney { get; } = "玩家已獲得 {0} 宇宙碎片";
        public string PlayerGainedAllItems { get; } = "玩家已獲得所有{0}";
        public string PlayerGainedItem { get; } = "玩家已獲得{0} {1}";
        public string PlayerEnhancedBuff { get; } = "玩家已強化祝福 {0}";
        public string PlayerEnhancedAllBuffs { get; } = "玩家已強化所有祝福";
        public string PlayerUnstuck { get; } = "玩家已脫離事件";
        public string NotFoundItem { get; } = "{0} 不存在！";
    }

    /// <summary>
    /// path: Game.Command.Scene
    /// </summary>
    public class SceneTextCHT
    {
        public string Desc { get; } = "管理玩家之場景";
        public string Usage { get; } = "/scene <prop [組ID] [道具ID] [狀態] - 設定道具狀態>/<remove [實體ID] - 移除實體>/<unlockall - 解鎖所有道具>/<change [場景entranceId] - 進入指定場景>/<reload - 重新載入場景>";
        public string LoadedGroups { get; } = "已載入組: {0}";
        public string PropStateChanged { get; } = "道具: {0} 的狀態已設定為 {1}";
        public string PropNotFound { get; } = "該道具不存在！";
        public string EntityRemoved { get; } = "實體 {0} 已被移除";
        public string EntityNotFound { get; } = "該實體不存在！";
        public string AllPropsUnlocked { get; } = "已解鎖全部道具！";
        public string SceneChanged { get; } = "已進入場景 {0}";
        public string SceneReloaded { get; } = "場景已重新讀取！";
    }

    /// <summary>
    /// path: Game.Command.Mail
    /// </summary>
    public class MailTextCHT
    {
        public string Desc { get; } = "管理員家的郵件";
        public string Usage { get; } = "/mail <send [發送名稱] [標題] [內容] [模板ID] [過期天數] - 發送郵件>/<send [發送者] [標題] [內容] [模板ID] [過期天數] [附件] - 發送帶附件的郵件>";
        public string MailSent { get; } = "郵件已發送！";
        public string MailSentWithAttachment { get; } = "帶附件的郵件已發送！";
    }

    #endregion

    #endregion
}
