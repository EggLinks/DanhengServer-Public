using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Rogue;

namespace EggLink.DanhengServer.Data;

public static class GameData
{
    #region Activity

    public static ActivityConfig ActivityConfig { get; set; } = new();

    #endregion

    #region Banners

    public static BannersConfig BannersConfig { get; set; } = new();

    #endregion

    #region Avatar

    public static Dictionary<int, AvatarConfigExcel> AvatarConfigData { get; private set; } = [];
    public static Dictionary<int, AvatarPromotionConfigExcel> AvatarPromotionConfigData { get; private set; } = [];
    public static Dictionary<int, AvatarExpItemConfigExcel> AvatarExpItemConfigData { get; private set; } = [];
    public static Dictionary<int, AvatarSkillTreeConfigExcel> AvatarSkillTreeConfigData { get; private set; } = [];
    public static Dictionary<int, AvatarDemoConfigExcel> AvatarDemoConfigData { get; private set; } = [];
    public static Dictionary<int, ExpTypeExcel> ExpTypeData { get; } = [];

    public static Dictionary<int, MultiplePathAvatarConfigExcel> MultiplePathAvatarConfigData { get; private set; } =
        [];

    public static Dictionary<int, AdventurePlayerExcel> AdventurePlayerData { get; private set; } = [];
    public static Dictionary<int, SummonUnitExcel> SummonUnitData { get; private set; } = [];

    #endregion

    #region Challenge

    public static Dictionary<int, ChallengeConfigExcel> ChallengeConfigData { get; private set; } = [];
    public static Dictionary<int, ChallengeTargetExcel> ChallengeTargetData { get; private set; } = [];
    public static Dictionary<int, ChallengeGroupExcel> ChallengeGroupData { get; private set; } = [];
    public static Dictionary<int, List<ChallengeRewardExcel>> ChallengeRewardData { get; private set; } = [];

    #endregion

    #region Battle

    public static Dictionary<int, CocoonConfigExcel> CocoonConfigData { get; private set; } = [];
    public static Dictionary<int, StageConfigExcel> StageConfigData { get; private set; } = [];
    public static Dictionary<int, RaidConfigExcel> RaidConfigData { get; private set; } = [];
    public static Dictionary<int, MazeBuffExcel> MazeBuffData { get; private set; } = [];
    public static Dictionary<int, InteractConfigExcel> InteractConfigData { get; private set; } = [];
    public static Dictionary<int, NPCMonsterDataExcel> NpcMonsterDataData { get; private set; } = [];
    public static Dictionary<int, MonsterConfigExcel> MonsterConfigData { get; private set; } = [];
    public static Dictionary<int, MonsterDropExcel> MonsterDropData { get; private set; } = [];

    #endregion

    #region ChessRogue

    public static Dictionary<int, ActionPointOverdrawExcel> ActionPointOverdrawData { get; private set; } = [];

    public static Dictionary<RogueDLCBlockTypeEnum, List<ChessRogueRoomConfig>>
        ChessRogueRoomData { get; private set; } = [];

    public static Dictionary<int, RogueDLCAreaExcel> RogueDLCAreaData { get; private set; } = [];
    public static Dictionary<int, RogueDLCBossDecayExcel> RogueDLCBossDecayData { get; private set; } = [];
    public static Dictionary<int, RogueDLCBossBpExcel> RogueDLCBossBpData { get; private set; } = [];
    public static Dictionary<int, RogueDLCDifficultyExcel> RogueDLCDifficultyData { get; private set; } = [];
    public static Dictionary<int, RogueNousAeonExcel> RogueNousAeonData { get; private set; } = [];
    public static Dictionary<int, RogueNousDiceBranchExcel> RogueNousDiceBranchData { get; private set; } = [];
    public static Dictionary<int, RogueNousDiceSurfaceExcel> RogueNousDiceSurfaceData { get; private set; } = [];

    public static Dictionary<int, RogueNousDifficultyLevelExcel> RogueNousDifficultyLevelData { get; private set; } =
        [];

    public static Dictionary<int, RogueNousMainStoryExcel> RogueNousMainStoryData { get; private set; } = [];
    public static Dictionary<int, RogueNousSubStoryExcel> RogueNousSubStoryData { get; private set; } = [];
    public static Dictionary<int, RogueNousTalentExcel> RogueNousTalentData { get; private set; } = [];
    public static Dictionary<int, List<RogueDLCChessBoardExcel>> RogueSwarmChessBoardData { get; private set; } = [];
    public static Dictionary<int, List<RogueDLCChessBoardExcel>> RogueNousChessBoardData { get; private set; } = [];

    #endregion

    #region Player

    public static Dictionary<int, QuestDataExcel> QuestDataData { get; private set; } = [];
    public static Dictionary<int, FinishWayExcel> FinishWayData { get; private set; } = [];
    public static Dictionary<int, PlayerLevelConfigExcel> PlayerLevelConfigData { get; } = [];
    public static Dictionary<int, BackGroundMusicExcel> BackGroundMusicData { get; private set; } = [];
    public static Dictionary<int, ChatBubbleConfigExcel> ChatBubbleConfigData { get; private set; } = [];

    #endregion

    #region Maze

    public static Dictionary<int, NPCDataExcel> NpcDataData { get; private set; } = [];
    public static Dictionary<string, FloorInfo> FloorInfoData { get; } = [];
    public static Dictionary<int, MapEntranceExcel> MapEntranceData { get; private set; } = [];
    public static Dictionary<int, MazePlaneExcel> MazePlaneData { get; private set; } = [];
    public static Dictionary<int, MazePropExcel> MazePropData { get; private set; } = [];
    public static Dictionary<int, PlaneEventExcel> PlaneEventData { get; private set; } = [];
    public static Dictionary<int, ContentPackageConfigExcel> ContentPackageConfigData { get; private set; } = [];
    public static Dictionary<int, GroupSystemUnlockDataExcel> GroupSystemUnlockDataData { get; private set; } = [];
    public static Dictionary<int, FuncUnlockDataExcel> FuncUnlockDataData { get; private set; } = [];

    #endregion

    #region Items

    public static Dictionary<int, MappingInfoExcel> MappingInfoData { get; private set; } = [];
    public static Dictionary<int, ItemConfigExcel> ItemConfigData { get; private set; } = [];
    public static Dictionary<int, ItemUseBuffDataExcel> ItemUseBuffDataData { get; private set; } = [];
    public static Dictionary<int, EquipmentConfigExcel> EquipmentConfigData { get; private set; } = [];
    public static Dictionary<int, EquipmentExpTypeExcel> EquipmentExpTypeData { get; } = [];
    public static Dictionary<int, EquipmentExpItemConfigExcel> EquipmentExpItemConfigData { get; private set; } = [];

    public static Dictionary<int, EquipmentPromotionConfigExcel> EquipmentPromotionConfigData { get; private set; } =
        [];

    public static Dictionary<int, Dictionary<int, RelicMainAffixConfigExcel>> RelicMainAffixData { get; private set; } =
        []; // groupId, affixId

    public static Dictionary<int, Dictionary<int, RelicSubAffixConfigExcel>> RelicSubAffixData { get; private set; } =
        []; // groupId, affixId

    public static Dictionary<int, RelicConfigExcel> RelicConfigData { get; private set; } = [];
    public static Dictionary<int, RelicExpItemExcel> RelicExpItemData { get; private set; } = [];
    public static Dictionary<int, RelicExpTypeExcel> RelicExpTypeData { get; private set; } = [];

    #endregion

    #region Special Avatar

    public static Dictionary<int, SpecialAvatarExcel> SpecialAvatarData { get; private set; } = [];
    public static Dictionary<int, SpecialAvatarRelicExcel> SpecialAvatarRelicData { get; private set; } = [];

    #endregion

    #region Mission

    public static Dictionary<int, MainMissionExcel> MainMissionData { get; private set; } = [];
    public static Dictionary<int, SubMissionExcel> SubMissionData { get; private set; } = [];
    public static Dictionary<int, RewardDataExcel> RewardDataData { get; private set; } = [];
    public static Dictionary<int, MessageGroupConfigExcel> MessageGroupConfigData { get; private set; } = [];
    public static Dictionary<int, MessageSectionConfigExcel> MessageSectionConfigData { get; private set; } = [];
    public static Dictionary<int, MessageContactsConfigExcel> MessageContactsConfigData { get; private set; } = [];
    public static Dictionary<int, MessageItemConfigExcel> MessageItemConfigData { get; private set; } = [];
    public static Dictionary<int, PerformanceDExcel> PerformanceDData { get; private set; } = [];
    public static Dictionary<int, PerformanceEExcel> PerformanceEData { get; private set; } = [];
    public static Dictionary<int, StoryLineExcel> StoryLineData { get; private set; } = [];

    public static Dictionary<int, Dictionary<int, StoryLineFloorDataExcel>>
        StoryLineFloorDataData { get; private set; } = [];

    public static Dictionary<int, StroyLineTrialAvatarDataExcel> StroyLineTrialAvatarDataData { get; private set; } =
        [];

    public static Dictionary<int, HeartDialScriptExcel> HeartDialScriptData { get; private set; } = [];
    public static Dictionary<int, HeartDialDialogueExcel> HeartDialDialogueData { get; private set; } = [];

    #endregion

    #region Item Exchange

    public static Dictionary<int, ShopConfigExcel> ShopConfigData { get; private set; } = [];
    public static Dictionary<int, RollShopConfigExcel> RollShopConfigData { get; private set; } = [];
    public static Dictionary<int, RollShopRewardExcel> RollShopRewardData { get; private set; } = [];
    public static Dictionary<int, ItemComposeConfigExcel> ItemComposeConfigData { get; private set; } = [];

    #endregion

    #region Rogue

    public static Dictionary<int, DialogueEventExcel> DialogueEventData { get; private set; } = [];

    public static Dictionary<int, Dictionary<int, DialogueDynamicContentExcel>> DialogueDynamicContentData
    {
        get;
        private set;
    } = [];

    public static Dictionary<int, RogueAeonExcel> RogueAeonData { get; private set; } = [];
    public static Dictionary<int, RogueBuffExcel> RogueAeonBuffData { get; private set; } = [];
    public static Dictionary<int, BattleEventDataExcel> RogueBattleEventData { get; private set; } = [];
    public static Dictionary<int, List<RogueBuffExcel>> RogueAeonEnhanceData { get; private set; } = [];
    public static Dictionary<int, RogueAreaConfigExcel> RogueAreaConfigData { get; private set; } = [];
    public static Dictionary<int, RogueBonusExcel> RogueBonusData { get; private set; } = [];
    public static Dictionary<int, RogueBuffExcel> RogueBuffData { get; private set; } = [];
    public static Dictionary<int, RogueBuffGroupExcel> RogueBuffGroupData { get; private set; } = [];
    public static Dictionary<int, RogueHandBookEventExcel> RogueHandBookEventData { get; private set; } = [];
    public static Dictionary<int, RogueHandbookMiracleExcel> RogueHandbookMiracleData { get; private set; } = [];
    public static Dictionary<int, RogueManagerExcel> RogueManagerData { get; private set; } = [];
    public static Dictionary<int, Dictionary<int, RogueMapExcel>> RogueMapData { get; private set; } = [];
    public static Dictionary<int, List<int>> RogueMapGenData { get; set; } = [];
    public static Dictionary<int, RogueMazeBuffExcel> RogueMazeBuffData { get; private set; } = [];
    public static Dictionary<int, RogueMiracleExcel> RogueMiracleData { get; private set; } = [];
    public static RogueMiracleEffectConfig RogueMiracleEffectData { get; set; } = new();
    public static Dictionary<int, List<int>> RogueMiracleGroupData { get; set; } = [];
    public static Dictionary<int, RogueMiracleDisplayExcel> RogueMiracleDisplayData { get; private set; } = [];
    public static Dictionary<int, RogueMonsterExcel> RogueMonsterData { get; private set; } = [];
    public static Dictionary<int, RogueNPCExcel> RogueNPCData { get; private set; } = [];
    public static Dictionary<int, RogueRoomExcel> RogueRoomData { get; private set; } = [];
    public static Dictionary<int, RogueTalentExcel> RogueTalentData { get; private set; } = [];

    #endregion

    #region TournRogue

    public static Dictionary<int, RogueTournAreaExcel> RogueTournAreaData { get; private set; } = [];
    public static Dictionary<int, RogueTournBuffExcel> RogueTournBuffData { get; private set; } = [];
    public static Dictionary<int, RogueTournFormulaExcel> RogueTournFormulaData { get; private set; } = [];
    public static Dictionary<int, RogueTournBuffGroupExcel> RogueTournBuffGroupData { get; private set; } = [];
    public static Dictionary<int, RogueTournHexAvatarBaseTypeExcel> RogueTournHexAvatarBaseTypeData { get; private set; } = [];
    public static Dictionary<int, RogueTournHandBookEventExcel> RogueTournHandBookEventData { get; private set; } = [];
    public static Dictionary<int, RogueTournHandbookMiracleExcel> RogueTournHandbookMiracleData { get; private set; } = [];
    public static Dictionary<int, RogueTournDifficultyCompExcel> RogueTournDifficultyCompData { get; private set; } = [];
    public static Dictionary<int, RogueTournPermanentTalentExcel> RogueTournPermanentTalentData { get; private set; } = [];

    #endregion

    #region Actions

    public static void GetFloorInfo(int planeId, int floorId, out FloorInfo outer)
    {
        FloorInfoData.TryGetValue("P" + planeId + "_F" + floorId, out outer!);
    }

    public static int GetPlayerExpRequired(int level)
    {
        var excel = PlayerLevelConfigData[level];
        var prevExcel = PlayerLevelConfigData[level - 1];
        return excel != null && prevExcel != null ? excel.PlayerExp - prevExcel.PlayerExp : 0;
    }

    public static int GetAvatarExpRequired(int group, int level)
    {
        ExpTypeData.TryGetValue(group * 100 + level, out var expType);
        return expType?.Exp ?? 0;
    }

    public static int GetEquipmentExpRequired(int group, int level)
    {
        EquipmentExpTypeData.TryGetValue(group * 100 + level, out var expType);
        return expType?.Exp ?? 0;
    }

    public static int GetMinPromotionForLevel(int level)
    {
        return Math.Max(Math.Min((int)((level - 11) / 10D), 6), 0);
    }

    #endregion
}