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
        public ServerInfoTextCHT ServerInfo { get; } = new();
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

        // server info
        public string Config { get; } = "配置文件";
        public string Language { get; } = "語言";
        public string Log { get; } = "日誌";
        public string GameData { get; } = "遊戲數據";
        public string Database { get; } = "數據庫";
        public string Command { get; } = "命令";
        public string WebServer { get; } = "Web服務器";
        public string Plugin { get; } = "插件";
        public string Handler { get; } = "包處理器";
        public string Dispatch { get; } = "全局分發";
        public string Game { get; } = "遊戲";
        public string Handbook { get; } = "手冊";
        public string NotFound { get; } = "未找到";
        public string Error { get; } = "錯誤";
        public string FloorInfo { get; } = "區域文件";
        public string FloorGroupInfo { get; } = "區域組文件";
        public string FloorMissingResult { get; } = "傳送與世界生成";
        public string FloorGroupMissingResult { get; } = "傳送、怪物戰鬥與世界生成";
        public string Mission { get; } = "任務";
        public string MissionInfo { get; } = "任務文件";
        public string MazeSkill { get; } = "角色秘技";
        public string MazeSkillInfo { get; } = "角色秘技文件";
        public string Dialogue { get; } = "模擬宇宙事件";
        public string DialogueInfo { get; } = "模擬宇宙事件文件";
        public string Performance { get; } = "劇情操作";
        public string PerformanceInfo { get; } = "劇情操作文件";
        public string RogueChestMap { get; } = "模擬宇宙地圖";
        public string RogueChestMapInfo { get; } = "模擬宇宙地圖文件";
        public string ChessRogueRoom { get; } = "模擬宇宙DLC";
        public string ChessRogueRoomInfo { get; } = "模擬宇宙DLC文件";
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
        public RaidTextCHT Raid { get; } = new();
        public AccountTextCHT Account { get; } = new();
    }

    #endregion

    #region ServerText

    /// <summary>
    /// path: Server.Web
    /// </summary>
    public class WebTextCHT
    {
    }

    /// <summary>
    /// path: Server.ServerInfo
    /// </summary>
    public class ServerInfoTextCHT
    {
        public string Shutdown { get; } = "關閉中…";
        public string CancelKeyPressed { get; } = "已按下取消鍵 (Ctrl + C)，服務器即將關閉…";
        public string StartingServer { get; } = "正在啟動 DanhengServer…";
        public string LoadingItem { get; } = "正在加載 {0}…";
        public string RegisterItem { get; } = "註冊了 {0} 個 {1}。";
        public string FailedToLoadItem { get; } = "加載 {0} 失敗。";
        public string FailedToInitializeItem { get; } = "初始化 {0} 失敗。";
        public string FailedToReadItem { get; } = "讀取 {0} 失敗，文件{1}";
        public string GeneratedItem { get; } = "已生成 {0}。";
        public string LoadedItem { get; } = "已加載 {0}。";
        public string LoadedItems { get; } = "已加載 {0} 個 {1}。";
        public string ServerRunning { get; } = "{0} 服務器正在監聽 {1}";
        public string ServerStarted { get; } = "啟動完成！用時 {0}s，擊敗了99%的用戶，輸入 『help』 來獲取命令幫助";  // 玩梗，考慮英語版本將其本土化
        public string MissionEnabled { get; } = "任務系統已啟用，此功能仍在開發中，且可能不會按預期工作，如果遇見任何bug，請匯報給開發者。";

        public string ConfigMissing { get; } = "{0} 缺失，請檢查你的資源文件夾：{1}，{2} 可能不能使用。";
        public string UnloadedItems { get; } = "卸載了所有 {0}。";
        public string SaveDatabase { get; } = "已保存數據庫，用時 {0}s";
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
        public string PlayerNotFound { get; } = "未找到玩家!";
        public string InvalidArguments { get; } = "無效的參數!";
        public string NoPermission { get; } = "你沒有權限這麼做!";
        public string CommandNotFound { get; } = "未找到命令! 輸入 '/help' 來獲取幫助";
        public string TargetOffline { get; } = "目標 {0}({1}) 離線了！清除當前目標";
        public string TargetFound { get; } = "找到目標 {0}({1})，下一次命令將默認對其執行";
        public string TargetNotFound { get; } = "未找到目標 {0}!";
        public string InternalError { get; } = "在處理命令時發生了內部錯誤!";
    }

    /// <summary>
    /// path: Game.Command.Hero
    /// </summary>
    public class HeroTextCHT
    {
        public string Desc { get; } = "切換主角的性別/形態";
        public string Usage { get; } = "/hero <gender [1/2 - 1為男性,2為女性]>/<type [8001/8003/8005 - 分別為 毀滅 存護 同諧]>";
        public string GenderNotSpecified { get; } = "性別不存在!";
        public string HeroTypeNotSpecified { get; } = "主角類型不存在!";
        public string GenderChanged { get; } = "性別已更改!";
        public string HeroTypeChanged { get; } = "主角類型已更改!";
    }

    /// <summary>
    /// path: Game.Command.UnlockAll
    /// </summary>
    public class UnlockAllTextCHT
    {
        public string Desc { get; } = "解鎖所有在類別內的對象";
        public string Usage { get; } = "/unlockall <mission - mission為任務>";
        public string AllMissionsUnlocked { get; } = "所有任務已解鎖!";
    }

    /// <summary>
    /// path: Game.Command.Avatar
    /// </summary>
    public class AvatarTextCHT
    {
        public string Desc { get; } = "設定玩家已有角色的屬性";
        public string Usage { get; } = "/avatar <talent [角色ID/-1] [行跡等級] - 設置行跡等級 角色ID為-1意為所有擁有角色>/<get [角色ID] - 獲取角色>/<rank [角色ID/-1] [星魂]>/<level [角色ID/-1] [角色等級]>";
        public string InvalidLevel { get; } = "無效 {0}等級";
        public string AllAvatarsLevelSet { get; } = "已將全部角色 {0}等級設置為 {1}";
        public string AvatarLevelSet { get; } = "已將 {0} 角色 {1}等級設置為 {2}";
        public string AvatarNotFound { get; } = "角色不存在!";
        public string AvatarGet { get; } = "獲取到角色 {0}!";
        public string AvatarFailedGet { get; } = "獲取角色 {0} 失敗!";
    }

    /// <summary>
    /// path: Game.Command.Give
    /// </summary>
    public class GiveTextCHT
    {
        public string Desc { get; } = "給予玩家物品";
        public string Usage { get; } = "/give <物品ID> l<等級> x<數量> r<疊影>";
        public string ItemNotFound { get; } = "未找到物品!";
        public string GiveItem { get; } = "給予 @{0} {1} 個物品 {2}";
    }

    /// <summary>
    /// path: Game.Command.GiveAll
    /// </summary>
    public class GiveAllTextCHT
    {
        public string Desc { get; } = "給予玩家全部指定類型的物品";
        public string Usage { get; } = "/giveall <avatar - 角色/equipment - 光錐/relic - 遺器/unlock - 氣泡等奢侈品> r<疊影> l<等級> x<數量>";
        public string GiveAllItems { get; } = "已給予所有 {0}, 各 {1} 個";
    }

    /// <summary>
    /// path: Game.Command.Lineup
    /// </summary>
    public class LineupTextCHT
    {
        public string Desc { get; } = "管理玩家的隊伍";
        public string Usage { get; } = "/lineup <mp [秘技點數量 - 最高兩個]>/<heal - 治癒>";
        public string PlayerGainedMp { get; } = "玩家已獲得 {0} 秘技點";
        public string HealedAllAvatars { get; } = "成功治癒當前隊伍中的所有角色";
    }

    /// <summary>
    /// path: Game.Command.Help
    /// </summary>
    public class HelpTextCHT
    {
        public string Desc { get; } = "顯示幫助信息";
        public string Usage { get; } = "/help";
        public string Commands { get; } = "命令:";
        public string CommandUsage { get; } = "用法: ";
    }

    /// <summary>
    /// path: Game.Command.Kick
    /// </summary>
    public class KickTextCHT
    {
        public string Desc { get; } = "踢出玩家";
        public string Usage { get; } = "/kick";
        public string PlayerKicked { get; } = "玩家 {0} 已被踢出!";
    }

    /// <summary>
    /// path: Game.Command.Mission
    /// </summary>
    public class MissionTextCHT
    {
        public string Desc { get; } = "管理玩家的任務";
        public string Usage { get; } = "/mission <unlockall - 完成所有任務>/<pass - 完成所有正在進行的任務>/<finish [副任務ID] - 完成指定任務>/<running - 獲取正在進行的任務以及可能卡住的任務>/<reaccept [主任務ID] - 重新進行指定主任務>";
        public string AllMissionsFinished { get; } = "所有任務已完成!";
        public string AllRunningMissionsFinished { get; } = "共 {0} 個進行中的任務已完成!";
        public string MissionFinished { get; } = "任務 {0} 已完成!";
        public string InvalidMissionId { get; } = "無效的任務ID!";
        public string NoRunningMissions { get; } = "沒有正在進行的任務!";

        public string RunningMissions { get; } = "正在進行的任務:";
        public string PossibleStuckMissions { get; } = "可能卡住的任務:";
        public string MainMission { get; } = "主任務";

        public string MissionReAccepted { get; } = "重新接受任務 {0}!";
    }

    /// <summary>
    /// path: Game.Command.Relic
    /// </summary>
    public class RelicTextCHT
    {
        public string Desc { get; } = "管理玩家的遺器";
        public string Usage { get; } = "/relic <遺器ID> <主詞條ID> <小詞條ID1:小詞條等級> <小詞條ID2:小詞條等級> <小詞條ID3:小詞條等級> <小詞條ID4:小詞條等級> l<等級> x<數量>";
        public string RelicNotFound { get; } = "遺器不存在!";
        public string InvalidMainAffixId { get; } = "主詞條ID無效";
        public string InvalidSubAffixId { get; } = "副詞條ID無效";
        public string RelicGiven { get; } = "給予玩家 @{0} {1} 個遺器 {2}, 主詞條 {3}";
    }

    /// <summary>
    /// path: Game.Command.Reload
    /// </summary>
    public class ReloadTextCHT
    {
        public string Desc { get; } = "重新加載指定的配置";
        public string Usage { get; } = "/reload <配置名>(banner - 卡池, activity - 活動)";
        public string ConfigReloaded { get; } = "配置 {0} 已重新加載!";
    }

    /// <summary>
    /// path: Game.Command.Rogue
    /// </summary>
    public class RogueTextCHT
    {
        public string Desc { get; } = "管理玩家模擬宇宙中的數據";
        public string Usage { get; } = "/rogue <money [宇宙碎片數量]>/<buff [祝福ID/-1 (-1 - 全部祝福)]>/<miracle [奇物ID]>/<enhance [祝福ID/-1]>/<unstuck - 脫離事件>";
        public string PlayerGainedMoney { get; } = "玩家已獲得 {0} 宇宙碎片";
        public string PlayerGainedAllItems { get; } = "玩家已獲得所有{0}";
        public string PlayerGainedItem { get; } = "玩家已獲得{0} {1}";
        public string PlayerEnhancedBuff { get; } = "玩家已強化祝福 {0}";
        public string PlayerEnhancedAllBuffs { get; } = "玩家已強化所有祝福";
        public string PlayerUnstuck { get; } = "玩家已脫離事件";
        public string NotFoundItem { get; } = "未找到 {0}!";
    }

    /// <summary>
    /// path: Game.Command.Scene
    /// </summary>
    public class SceneTextCHT
    {
        public string Desc { get; } = "管理玩家場景";
        public string Usage { get; } = "/scene <prop [組ID] [道具ID] [狀態] - 設置道具狀態>/<remove [實體ID] - 移除實體>/<unlockall - 解鎖所有道具>/<change [場景entranceId] - 進入指定場景>/<reload - 重新進入場景>";
        public string LoadedGroups { get; } = "已加載組: {0}";
        public string PropStateChanged { get; } = "道具: {0} 的狀態已設置為 {1}";
        public string PropNotFound { get; } = "未找到道具!";
        public string EntityRemoved { get; } = "實體 {0} 已被移除";
        public string EntityNotFound { get; } = "未找到實體!";
        public string AllPropsUnlocked { get; } = "所有道具已解鎖!";
        public string SceneChanged { get; } = "已進入場景 {0}";
        public string SceneReloaded { get; } = "場景已重新加載!";
        public string SceneReset { get; } = "已重置場景 {0} 中所有道具狀態！";
    }

    /// <summary>
    /// path: Game.Command.Mail
    /// </summary>
    public class MailTextCHT
    {
        public string Desc { get; } = "管理玩家的郵件";
        public string Usage { get; } = "/mail <send [發送名稱] [標題] [內容] [模板ID] [過期天數] - 發送郵件>/<send [發送者] [標題] [內容] [模板ID] [過期天數] [附件] - 發送帶附件的郵件>";
        public string MailSent { get; } = "郵件已發送!";
        public string MailSentWithAttachment { get; } = "帶附件的郵件已發送!";
    }

    /// <summary>
    /// path: Game.Command.Raid
    /// </summary>
    public class RaidTextCHT
    {
        public string Desc { get; } = "管理玩家的任務臨時場景";
        public string Usage { get; } = "/raid <leave - 離開臨時場景>";
        public string Leaved { get; } = "已離開臨時場景!";
    }

    /// <summary>
    /// path: Game.Command.Account
    /// </summary>
    public class AccountTextCHT
    {
        public string Desc { get; } = "創建賬號";
        public string Usage { get; } = "/account create <用戶名>";
        public string InvalidUid { get; } = "無效UID參數！";
        public string CreateError { get; } = "出現內部錯誤 {0} ";
        public string CreateSuccess { get; } = "新賬號 {0} 創建成功!";
        public string DuplicateAccount { get; } = "賬號 {0} 已存在!";
        public string DuplicateUID { get; } = "UID {0} 已存在!";
        public string DataError { get; } = "新賬號獲取失敗! {0}!";
    }
    #endregion

    #endregion
}
