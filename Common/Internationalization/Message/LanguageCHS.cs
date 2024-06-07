using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Internationalization.Message
{
    #region Root

    public class LanguageCHS
    {
        public GameTextCHS Game { get; } = new();
        public ServerTextCHS Server { get; } = new();
        public WordTextCHS Word { get; } = new();  // a placeholder for the actual word text
    }

    #endregion

    #region Layer 1

    /// <summary>
    /// path: Game
    /// </summary>
    public class GameTextCHS
    {
        public CommandTextCHS Command { get; } = new();
    }

    /// <summary>
    /// path: Server
    /// </summary>
    public class ServerTextCHS
    {
        public WebTextCHS Web { get; } = new();
    }

    /// <summary>
    /// path: Word
    /// </summary>
    public class WordTextCHS
    {
        public string Rank { get; } = "星魂";
        public string Avatar { get; } = "角色";
        public string Material { get; } = "材料";
        public string Relic { get; } = "遗器";
        public string Equipment { get; } = "光锥";
        public string Talent { get; } = "行迹";
        public string Banner { get; } = "卡池";
        public string Activity { get; } = "活动";
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
    public class CommandTextCHS
    {
        public NoticeTextCHS Notice { get; } = new();

        public HeroTextCHS Hero { get; } = new();
        public AvatarTextCHS Avatar { get; } = new();
        public GiveTextCHS Give { get; } = new();
        public GiveAllTextCHS GiveAll { get; } = new();
        public LineupTextCHS Lineup { get; } = new();
        public HelpTextCHS Help { get; } = new();
        public KickTextCHS Kick { get; } = new();
        public MissionTextCHS Mission { get; } = new();
        public RelicTextCHS Relic { get; } = new();
        public ReloadTextCHS Reload { get; } = new();
        public RogueTextCHS Rogue { get; } = new();
        public SceneTextCHS Scene { get; } = new();
        public UnlockAllTextCHS UnlockAll { get; } = new();
        public MailTextCHS Mail { get; } = new();
    }

    #endregion

    #region ServerText

    /// <summary>
    /// path: Server.Web
    /// </summary>
    public class WebTextCHS
    {
    }

    #endregion

    #endregion

    #region Layer 3

    #region CommandText

    /// <summary>
    /// path: Game.Command.Notice
    /// </summary>
    public class NoticeTextCHS
    {
        public string PlayerNotFound { get; } = "未找到玩家!";
        public string InvalidArguments { get; } = "无效的参数!";
        public string NoPermission { get; } = "你没有权限这么做!";
        public string CommandNotFound { get; } = "未找到命令! 输入 '/help' 来获取帮助";
        public string TargetOffline { get; } = "目标 {0}({1}) 离线了！清除当前目标";
        public string TargetFound { get; } = "找到目标 {0}({1})，下一次命令将默认对其执行";
        public string TargetNotFound { get; } = "未找到目标 {0}!";
        public string InternalError { get; } = "在处理命令时发生了内部错误!";
    }

    /// <summary>
    /// path: Game.Command.Hero
    /// </summary>
    public class HeroTextCHS
    {
        public string Desc { get; } = "切换主角的性别/形态";
        public string Usage { get; } = "/hero <gender [1/2 - 1为男性,2为女性]>/<type [8001/8003/8005 - 分别为 毁灭 存护 同谐]>";
        public string GenderNotSpecified { get; } = "性别不存在!";
        public string HeroTypeNotSpecified { get; } = "主角类型不存在!";
        public string GenderChanged { get; } = "性别已更改!";
        public string HeroTypeChanged { get; } = "主角类型已更改!";
    }

    /// <summary>
    /// path: Game.Command.UnlockAll
    /// </summary>
    public class UnlockAllTextCHS
    {
        public string Desc { get; } = "解锁所有在类别内的对象";
        public string Usage { get; } = "/unlockall <mission - mission为任务>";
        public string AllMissionsUnlocked { get; } = "所有任务已解锁!";
    }

    /// <summary>
    /// path: Game.Command.Avatar
    /// </summary>
    public class AvatarTextCHS
    {
        public string Desc { get; } = "设定玩家已有角色的属性";
        public string Usage { get; } = "/avatar <talent [角色ID/-1] [行迹等级] - 设置行迹等级 角色ID为-1意为所有拥有角色>/<get [角色ID] - 获取角色>/<rank [角色ID/-1] [星魂]>/<level [角色ID/-1] [角色等级]>";
        public string InvalidLevel { get; } = "无效 {0}等级";
        public string AllAvatarsLevelSet { get; } = "已将全部角色 {0}等级设置为 {1}";
        public string AvatarLevelSet { get; } = "已将 {0} 角色 {1}等级设置为 {2}";
        public string AvatarNotFound { get; } = "角色不存在!";
        public string AvatarGet { get; } = "获取到角色 {0}!";
        public string AvatarFailedGet { get; } = "获取角色 {0} 失败!";
    }

    /// <summary>
    /// path: Game.Command.Give
    /// </summary>
    public class GiveTextCHS
    {
        public string Desc { get; } = "给予玩家物品";
        public string Usage { get; } = "/give <物品ID> l<等级> x<数量> r<叠影>";
        public string ItemNotFound { get; } = "未找到物品!";
        public string GiveItem { get; } = "给予 @{0} {1} 个物品 {2}";
    }

    /// <summary>
    /// path: Game.Command.GiveAll
    /// </summary>
    public class GiveAllTextCHS
    {
        public string Desc { get; } = "给予玩家全部指定类型的物品";
        public string Usage { get; } = "/giveall <avatar - 角色/equipment - 光锥/relic - 遗器/unlock - 气泡等奢侈品> r<叠影> l<等级> x<数量>";
        public string GiveAllItems { get; } = "已给予所有 {0}, 各 {1} 个";
    }

    /// <summary>
    /// path: Game.Command.Lineup
    /// </summary>
    public class LineupTextCHS
    {
        public string Desc { get; } = "管理玩家的队伍";
        public string Usage { get; } = "/lineup <mp [秘技点数量 - 最高两个]>/<heal - 治愈>";
        public string PlayerGainedMp { get; } = "玩家已获得 {0} 秘技点";
        public string HealedAllAvatars { get; } = "成功治愈当前队伍中的所有角色";
    }

    /// <summary>
    /// path: Game.Command.Help
    /// </summary>
    public class HelpTextCHS
    {
        public string Desc { get; } = "显示帮助信息";
        public string Usage { get; } = "/help";
        public string Commands { get; } = "命令:";
        public string CommandUsage { get; } = "用法: ";
    }

    /// <summary>
    /// path: Game.Command.Kick
    /// </summary>
    public class KickTextCHS
    {
        public string Desc { get; } = "踢出玩家";
        public string Usage { get; } = "/kick";
        public string PlayerKicked { get; } = "玩家 {0} 已被踢出!";
    }

    /// <summary>
    /// path: Game.Command.Mission
    /// </summary>
    public class MissionTextCHS
    {
        public string Desc { get; } = "管理玩家的任务";
        public string Usage { get; } = "/mission <unlockall - 完成所有任务>/<pass - 完成所有正在进行的任务>/<finish [副任务ID] - 完成指定任务>/<running - 获取正在进行的任务以及可能卡住的任务>/<reaccept [主任务ID] - 重新进行指定主任务>";
        public string AllMissionsFinished { get; } = "所有任务已完成!";
        public string AllRunningMissionsFinished { get; } = "共 {0} 个进行中的任务已完成!";
        public string MissionFinished { get; } = "任务 {0} 已完成!";
        public string InvalidMissionId { get; } = "无效的任务ID!";
        public string NoRunningMissions { get; } = "没有正在进行的任务!";

        public string RunningMissions { get; } = "正在进行的任务:";
        public string PossibleStuckMissions { get; } = "可能卡住的任务:";
        public string MainMission { get; } = "主任务";

        public string MissionReAccepted { get; } = "重新接受任务 {0}!";
    }

    /// <summary>
    /// path: Game.Command.Relic
    /// </summary>
    public class RelicTextCHS
    {
        public string Desc { get; } = "管理玩家的遗器";
        public string Usage { get; } = "/relic <遗器ID> <主词条ID> <小词条ID1:小词条等级> <小词条ID2:小词条等级> <小词条ID3:小词条等级> <小词条ID4:小词条等级> l<等级> x<数量>";
        public string RelicNotFound { get; } = "遗器不存在!";
        public string InvalidMainAffixId { get; } = "主词条ID无效";
        public string InvalidSubAffixId { get; } = "副词条ID无效";
        public string RelicGiven { get; } = "给予玩家 @{0} {1} 个遗器 {2}, 主词条 {3}";
    }

    /// <summary>
    /// path: Game.Command.Reload
    /// </summary>
    public class ReloadTextCHS
    {
        public string Desc { get; } = "重新加载指定的配置";
        public string Usage { get; } = "/reload <配置名>(banner - 卡池, activity - 活动)";
        public string ConfigReloaded { get; } = "配置 {0} 已重新加载!";
    }

    /// <summary>
    /// path: Game.Command.Rogue
    /// </summary>
    public class RogueTextCHS
    {
        public string Desc { get; } = "管理玩家模拟宇宙中的数据";
        public string Usage { get; } = "/rogue <money [宇宙碎片数量]>/<buff [祝福ID/-1 (-1 - 全部祝福)]>/<miracle [奇物ID]>/<enhance [祝福ID/-1]>/<unstuck - 脱离事件>";
        public string PlayerGainedMoney { get; } = "玩家已获得 {0} 宇宙碎片";
        public string PlayerGainedAllItems { get; } = "玩家已获得所有{0}";
        public string PlayerGainedItem { get; } = "玩家已获得{0} {1}";
        public string PlayerEnhancedBuff { get; } = "玩家已强化祝福 {0}";
        public string PlayerEnhancedAllBuffs { get; } = "玩家已强化所有祝福";
        public string PlayerUnstuck { get; } = "玩家已脱离事件";
        public string NotFoundItem { get; } = "未找到 {0}!";
    }

    /// <summary>
    /// path: Game.Command.Scene
    /// </summary>
    public class SceneTextCHS
    {
        public string Desc { get; } = "管理玩家场景";
        public string Usage { get; } = "/scene <prop [组ID] [道具ID] [状态] - 设置道具状态>/<remove [实体ID] - 移除实体>/<unlockall - 解锁所有道具>/<change [场景entranceId] - 进入指定场景>/<reload - 重新进入场景>";
        public string LoadedGroups { get; } = "已加载组: {0}";
        public string PropStateChanged { get; } = "道具: {0} 的状态已设置为 {1}";
        public string PropNotFound { get; } = "未找到道具!";
        public string EntityRemoved { get; } = "实体 {0} 已被移除";
        public string EntityNotFound { get; } = "未找到实体!";
        public string AllPropsUnlocked { get; } = "所有道具已解锁!";
        public string SceneChanged { get; } = "已进入场景 {0}";
        public string SceneReloaded { get; } = "场景已重新加载!";
    }

    /// <summary>
    /// path: Game.Command.Mail
    /// </summary>
    public class MailTextCHS
    {
        public string Desc { get; } = "管理玩家的邮件";
        public string Usage { get; } = "/mail <send [发送名称] [标题] [内容] [模板ID] [过期天数] - 发送邮件>/<send [发送者] [标题] [内容] [模板ID] [过期天数] [附件] - 发送带附件的邮件>";
        public string MailSent { get; } = "邮件已发送!";
        public string MailSentWithAttachment { get; } = "带附件的邮件已发送!";
    }

    #endregion

    #endregion
}
