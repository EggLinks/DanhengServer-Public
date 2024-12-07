namespace EggLink.DanhengServer.Internationalization.Message;

#region Root

public class LanguageEN
{
    public GameTextEN Game { get; } = new();
    public ServerTextEN Server { get; } = new();
    public WordTextEN Word { get; } = new(); // a placeholder for the actual word text
}

#endregion

#region Layer 1

/// <summary>
///     path: Game
/// </summary>
public class GameTextEN
{
    public CommandTextEN Command { get; } = new();
}

/// <summary>
///     path: Server
/// </summary>
public class ServerTextEN
{
    public WebTextEN Web { get; } = new();
    public ServerInfoTextEN ServerInfo { get; } = new();
}

/// <summary>
///     path: Word
/// </summary>
public class WordTextEN
{
    public string Rank => "Rank";
    public string Avatar => "Avatar";
    public string Material => "Material";
    public string Pet => "Pet";
    public string Relic => "Relic";
    public string Equipment => "Light Cone";
    public string Talent => "Talent";
    public string Banner => "Gacha";
    public string VideoKeys => "CG Keys";
    public string Activity => "Activity";
    public string Buff => "Blessing";
    public string Miracle => "Curio";
    public string Unlock => "Luxury";

    // server info
    public string Config => "Config File";
    public string Language => "Language";
    public string Log => "Log";
    public string GameData => "Game Data";
    public string Database => "Database";
    public string Command => "Command";
    public string WebServer => "Web Server";
    public string Plugin => "Plugin";
    public string Handler => "Packet Handler";
    public string Dispatch => "Global Dispatch";
    public string Game => "Game";
    public string Handbook => "Handbook";
    public string NotFound => "Not Found";
    public string Error => "Error";
    public string FloorInfo => "Floor File";
    public string FloorGroupInfo => "Floor Group File";
    public string FloorMissingResult => "Teleportation and World Generation";
    public string FloorGroupMissingResult => "Teleportation, Monster Battles, and World Generation";
    public string Mission => "Mission";
    public string MissionInfo => "Mission File";
    public string SubMission => "Sub Mission";
    public string SubMissionInfo => "Sub Mission File";
    public string MazeSkill => "Maze Skill";
    public string MazeSkillInfo => "Maze Skill File";
    public string Dialogue => "Simulated Universe Event";
    public string DialogueInfo => "Simulated Universe Event File";
    public string Performance => "Performance";
    public string PerformanceInfo => "Performance File";
    public string RogueChestMap => "Simulated Universe Map";
    public string RogueChestMapInfo => "Simulated Universe Map File";
    public string ChessRogueRoom => "Simulated Universe DLC";
    public string ChessRogueRoomInfo => "Simulated Universe DLC File";
    public string SummonUnit => "Summon Unit";
    public string SummonUnitInfo => "Summon Unit File";
    public string RogueTournRoom => "Divergent Rogue Room";
    public string RogueTournRoomInfo => "Divergent Rogue Room File";
    public string TypesOfRogue => "types of rogue";
    public string RogueMagicRoom => "Unknowable Domain Room";
    public string RogueMagicRoomInfo => "Unknowable Domain Room File";
    public string RogueDiceSurface => "Dice Surface Effect";
    public string RogueDiceSurfaceInfo => "Dice Surface Effect File";
    public string AdventureModifier => "AdventureModifier";
    public string AdventureModifierInfo => "AdventureModifier File";

    public string DatabaseAccount => "Database Account";
    public string Tutorial => "Tutorial";
}

#endregion

#region Layer 2

#region GameText

/// <summary>
///     path: Game.Command
/// </summary>
public class CommandTextEN
{
    public NoticeTextEN Notice { get; } = new();

    public HeroTextEN Hero { get; } = new();
    public AvatarTextEN Avatar { get; } = new();
    public GiveTextEN Give { get; } = new();
    public GiveAllTextEN GiveAll { get; } = new();
    public LineupTextEN Lineup { get; } = new();
    public HelpTextEN Help { get; } = new();
    public KickTextEN Kick { get; } = new();
    public MissionTextEN Mission { get; } = new();
    public RelicTextEN Relic { get; } = new();
    public ReloadTextEN Reload { get; } = new();
    public RogueTextEN Rogue { get; } = new();
    public SceneTextEN Scene { get; } = new();
    public UnlockAllTextEN UnlockAll { get; } = new();
    public MailTextEN Mail { get; } = new();
    public RaidTextEN Raid { get; } = new();
    public AccountTextEN Account { get; } = new();
    public UnstuckTextEN Unstuck { get; } = new();
    public SetlevelTextEN Setlevel { get; } = new();
}

#endregion

#region ServerTextEN

/// <summary>
///     path: Server.Web
/// </summary>
public class WebTextEN
{
    public string Maintain => "The server is undergoing maintenance, please try again later.";
}

/// <summary>
///     path: Server.ServerInfo
/// </summary>
public class ServerInfoTextEN
{
    public string Shutdown => "Shutting down...";
    public string CancelKeyPressed => "Cancel key pressed (Ctrl + C), server shutting down...";
    public string StartingServer => "Starting DanhengServer...";
    public string LoadingItem => "Loading {0}...";
    public string RegisterItem => "Registered {0} {1}(s).";
    public string FailedToLoadItem => "Failed to load {0}.";

    public string NewClientSecretKey =>
        "Client Secret Key does not exist and a new Client Secret Key is being generated.";

    public string FailedToInitializeItem => "Failed to initialize {0}.";
    public string FailedToReadItem => "Failed to read {0}, file {1}";
    public string GeneratedItem => "Generated {0}.";
    public string LoadedItem => "Loaded {0}.";
    public string LoadedItems => "Loaded {0} {1}(s).";
    public string ServerRunning => "{0} server listening on {1}";

    public string ServerStarted =>
        "Startup complete! Took {0}s, better than 99% of users. Type 'help' for command help"; // This is a meme, consider localizing in English

    public string MissionEnabled =>
        "Mission system enabled. This feature is still in development and may not work as expected. Please report any bugs to the developers.";

    public string ConfigMissing => "{0} is missing. Please check your resource folder: {1}, {2} may not be available.";
    public string UnloadedItems => "Unloaded all {0}.";
    public string SaveDatabase => "Database saved in {0}s";

    public string WaitForAllDone =>
        "You cannot enter the game yet. Please wait for all items to load before trying again";

    public string UnhandledException => "An unhandled exception occurred: {0}";
}

#endregion

#endregion

#region Layer 3

#region CommandText

/// <summary>
///     path: Game.Command.Notice
/// </summary>
public class NoticeTextEN
{
    public string PlayerNotFound => "Player not found!";
    public string InvalidArguments => "Invalid arguments!";
    public string NoPermission => "You do not have permission to do this!";
    public string CommandNotFound => "Command not found! Type '/help' to get help.";
    public string TargetNotFound => "Target {0} not found!";
    public string TargetOffline => "Target {0}({1}) is offline! Clear the target.";
    public string TargetFound => "Online player {0}({1}) is found, the next command will target it by default.";
    public string InternalError => "An error occurred while executing the command!";
}

/// <summary>
///     path: Game.Command.Hero
/// </summary>
public class HeroTextEN
{
    public string Desc =>
        "Switch the gender/type of the main character\nWhen switch the gender, 1 means male, 2 means female\nWhen switch the type(path), 8001 means Destruction, 8003 means Preservation, 8005 means Harmony.\nNotice: Switch gender will clear all the paths and talents of main character, this operation is irreversible!";

    public string Usage => "Usage: /hero gender [genderId]\n\nUsage: /hero type [typeId]";

    public string GenderNotSpecified => "Gender does not exist!";
    public string HeroTypeNotSpecified => "Main character type does not exist!";
    public string GenderChanged => "Gender has been changed!";
    public string HeroTypeChanged => "Main character type has been changed!";
}

/// <summary>
///     path: Game.Command.UnlockAll
/// </summary>
public class UnlockAllTextEN
{
    public string Desc =>
        "Unlock the objects in given category\n" +
        "Use '/unlockall mission' to finish all missions, and the target player will be kicked, after re-login, the player may be stuck in tutorial, please use with caution" +
        "Use '/unlockall tutorial' to unlock all tutorials, and the target player will be kicked, used for being stuck in some pages\n" +
        "Use '/unlockall rogue' to unlock all types of rogue, and the target player will be kicked, used with '/unlockall tutorial' to get better performance";

    public string Usage => "Usage：/unlockall [mission/tutorial/rogue]";
    public string UnlockedAll => "Unlocked/Finished All {0}!";
}

/// <summary>
///     path: Game.Command.Avatar
/// </summary>
public class AvatarTextEN
{
    public string Desc =>
        "Set the properties of the avatars player owned\nWhen set talent level, set to X level means set all talent point to X level, if greater than the point max level, set to max level\nNotice: -1 means all owned avatars";

    public string Usage =>
        "Usage: /avatar talent [Avatar ID/-1] [Talent Level]\n\nUsage: /avatar get [Avatar ID]\n\nUsage: /avatar rank [Avatar ID/-1] [Rank]\n\nUsage: /avatar level [Avatar ID/-1] [Avatar Level]";

    public string InvalidLevel => "Invalid {0} level";
    public string AllAvatarsLevelSet => "Set all characters' {0} level to {1}";
    public string AvatarLevelSet => "Set character {0}'s {1} level to {2}";
    public string AvatarNotFound => "Character does not exist!";
    public string AvatarGet => "Obtained character {0}!";
    public string AvatarFailedGet => "Failed to obtain character {0}!";
}

/// <summary>
///     path: Game.Command.Give
/// </summary>
public class GiveTextEN
{
    public string Desc => "Give player items, item id can be avatar id, but cant set level, talent, rank";
    public string Usage => "Usage: /give <item ID> l<level> x<amount> r<rank>";
    public string ItemNotFound => "Item not found!";
    public string GiveItem => "Gave @{0} {1} item(s) {2}";
}

/// <summary>
///     path: Game.Command.GiveAll
/// </summary>
public class GiveAllTextEN
{
    public string Desc =>
        "Give the player all specified types of items\navatar means characters, equipment means light cones, relic means relic(artifact), unlock means chatBubbles, avatar(head icon), wallpaper";

    public string Usage =>
        "Usage: /giveall avatar r<rank> l<level>\n\nUsage: /giveall equipment r<rank> l<level> x<amount>\n\nUsage: /giveall relic l<level> x<amount>\n\nUsage: /giveall unlock";

    public string GiveAllItems => "Gave all {0}, each {1} items";
}

/// <summary>
///     path: Game.Command.Lineup
/// </summary>
public class LineupTextEN
{
    public string Desc => "Manage player's lineup\nTechnique Point can gain 2 each time";
    public string Usage => "Usage: /lineup mp [amount]\n\nUsage: /lineup heal";
    public string PlayerGainedMp => "Player gained {0} Technique Points";
    public string HealedAllAvatars => "Successfully healed all characters in the current lineup";
}

/// <summary>
///     path: Game.Command.Help
/// </summary>
public class HelpTextEN
{
    public string Desc => "Show help information";
    public string Usage => "Usage: /help\n\nUsage: /help [cmd]";
    public string Commands => "Commands:";
    public string CommandUsage => "Usage: ";
    public string CommandPermission => "Need Permission: ";
    public string CommandAlias => "Command Alias：";
}

/// <summary>
///     path: Game.Command.Kick
/// </summary>
public class KickTextEN
{
    public string Desc => "Kick out player";
    public string Usage => "Usage: /kick";
    public string PlayerKicked => "Player {0} has been kicked out!";
}

/// <summary>
///     path: Game.Command.Mission
/// </summary>
public class MissionTextEN
{
    public string Desc =>
        "Manage player's missions\n" +
        "Use 'pass' to finish all running mission, this command will cause severe lagging, please use '/mission finish' instead\n" +
        "Use 'running' to get the running mission and possible stuck missions, after use, a longer mission list may appear, please note that\n" +
        "Use 'reaccept' to re-accept given main mission, please find main mission id in handbook";

    public string Usage =>
        "Usage: /mission pass\n\nUsage: /mission finish [Sub mission ID]\n\nUsage: /mission running\n\nUsage: /mission reaccept [main mission id]";

    public string AllMissionsFinished => "All tasks have been completed!";
    public string AllRunningMissionsFinished => "A total of {0} ongoing tasks have been completed!";
    public string MissionFinished => "Task {0} has been completed!";
    public string InvalidMissionId => "Invalid task ID!";
    public string NoRunningMissions => "No ongoing tasks!";

    public string RunningMissions => "Ongoing tasks:";
    public string PossibleStuckMissions => "Possibly stuck tasks:";
    public string MainMission => "Main task";

    public string MissionReAccepted => "Reaccepted task {0}!";
}

/// <summary>
///     path: Game.Command.Relic
/// </summary>
public class RelicTextEN
{
    public string Desc =>
        "Manage player's relics\nmain affix optional, sub affix optional, but at least one of them exists\nLevel limit: 1≤Level≤9999";

    public string Usage =>
        "Usage: /relic <relic ID> <main affix ID> <sub affix ID1:sub affix level> <sub affix ID2:sub affix level> <sub affix ID3:sub affix level> <sub affix ID4:sub affix level> l<level> x<amount>";

    public string RelicNotFound => "Relic does not exist!";
    public string InvalidMainAffixId => "Invalid main affix ID";
    public string InvalidSubAffixId => "Invalid sub affix ID";
    public string RelicGiven => "Gave player @{0} {1} relic(s) {2}, main affix {3}";
}

/// <summary>
///     path: Game.Command.Reload
/// </summary>
public class ReloadTextEN
{
    public string Desc => "Reload specified configuration\nConfig Name: banner - Gacha Pool, activity";
    public string Usage => "Usage: /reload <config name>";
    public string ConfigReloaded => "Configuration {0} has been reloaded!";
}

/// <summary>
///     path: Game.Command.Rogue
/// </summary>
public class RogueTextEN
{
    public string Desc =>
        "Manage player's data in the simulated universe\n-1 means all blessings (all owned blessings)\nUse 'buff' to get blessings\nUse 'enhance' to enhance blessings";

    public string Usage =>
        "Usage: /rogue money [Universe Debris Amount]\n\nUsage: /rogue buff [Blessing Id/-1]\n\nUsage: /rogue miracle [Miracle ID]\n\nUsage: /rogue enhance [Blessing ID/-1]\n\nUsage: /rogue unstuck - Leave event";

    public string PlayerGainedMoney => "Player gained {0} universe debris";
    public string PlayerGainedAllItems => "Player gained all {0}";
    public string PlayerGainedItem => "Player gained {0} {1}";
    public string PlayerEnhancedBuff => "Player enhanced blessing {0}";
    public string PlayerEnhancedAllBuffs => "Player enhanced all blessings";
    public string PlayerUnstuck => "Player unstuck from event";
    public string NotFoundItem => "{0} not found!";
    public string PlayerNotInRogue => "Player is not in the simulated universe!";
}

/// <summary>
///     path: Game.Command.Scene
/// </summary>
public class SceneTextEN
{
    public string Desc =>
        "Manage player scenes\n" +
        "Note: Most commands in this group are for debugging purposes. Please ensure you understand what you are doing before using any command.\n" +
        "Use 'prop' to set the state of a prop. For a list of states, refer to Common/Enums/Scene/PropStateEnum.cs\n" +
        "Use 'unlockall' to unlock all props in the scene (i.e., set all props that can be opened to the 'open' state). This command may cause the game to load to about 90%. Use '/scene reset <floorId>' to resolve this issue.\n" +
        "Use 'change' to enter a specified scene. For EntryId, refer to Resources/MapEntrance.json\n" +
        "Use 'reload' to reload the current scene and return to the initial position.\n" +
        "Use 'reset' to reset the state of all props in the specified scene. For the current FloorId, use '/scene cur'.";

    public string Usage =>
        "Usage: /scene prop [groupId] [propId] [state]\n\nUsage: /scene remove [entityId]\n\nUsage: /scene unlockall\n\nUsage: /scene change [entryId]\n\nUsage: /scene reload\n\nUsage: /scene reset <floorId>";

    public string LoadedGroups => "Loaded groups: {0}";
    public string PropStateChanged => "Prop: {0} state set to {1}";
    public string PropNotFound => "Prop not found!";
    public string EntityRemoved => "Entity {0} has been removed";
    public string EntityNotFound => "Entity not found!";
    public string AllPropsUnlocked => "All props have been unlocked!";
    public string SceneChanged => "Entered scene {0}";
    public string SceneReloaded => "Scene has been reloaded!";
    public string SceneReset => "The prop state in floor {0} has been reset!";
    public string CurrentScene => "Current Scene Entry Id: {0}, Plane Id: {1}, Floor Id: {2}";
}

/// <summary>
///     path: Game.Command.Mail
/// </summary>
public class MailTextEN
{
    public string Desc => "Manage player's mails";

    public string Usage => "Usage: /mail [senderName] [templateId] [expiryDays] _TITLE [title] _CONTENT [content]";

    public string MailSent => "Mail has been sent!";
    public string MailSentWithAttachment => "Mail with attachments has been sent!";
}

/// <summary>
///     path: Game.Command.Raid
/// </summary>
public class RaidTextEN
{
    public string Desc => "Manage player's temporary scene";
    public string Usage => "Usage: /raid leave - leave temporary scene";
    public string Leaved => "Leaved temporary scene!";
}

/// <summary>
///     path: Game.Command.Account
/// </summary>
public class AccountTextEN
{
    public string Desc => "Create an account\nNote: This command is untested, use with caution!";
    public string Usage => "Usage: /account create <username>";
    public string InvalidUid => "Invalid UID argument!";
    public string CreateError => "An internal error occurred {0}";
    public string CreateSuccess => "New account {0} created successfully!";
    public string DuplicateAccount => "Account {0} already exists!";
    public string DuplicateUID => "UID {0} already exists!";
    public string DataError => "Failed to retrieve new account! {0}!";
}

/// <summary>
///     path: Game.Command.Unstuck
/// </summary>
public class UnstuckTextEN
{
    public string Desc => "Teleport player back to default location";
    public string Usage => "Usage: /unstuck <UID>";
    public string UnstuckSuccess => "Successfully teleported the player back to default location";
    public string UidNotExist => "The UID does not exist!";
    public string PlayerIsOnline => "The player is online!";
}

/// <summary>
///     path: Game.Command.Setlevel
/// </summary>
public class SetlevelTextEN
{
    public string Desc => "Set player level";
    public string Usage => "Usage: /setlevel <Level>";
    public string SetlevelSuccess => "Successfully set player level!";
}

#endregion

#endregion