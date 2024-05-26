using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Data.Excel;

namespace EggLink.DanhengServer.Data
{
    public static class GameData
    {
        #region Activity

        public static ActivityConfig ActivityConfig { get; set; } = new();

        #endregion

        #region Avatar

        public static Dictionary<int, AvatarConfigExcel> AvatarConfigData { get; private set; } = [];
        public static Dictionary<int, AvatarPromotionConfigExcel> AvatarPromotionConfigData { get; private set; } = [];
        public static Dictionary<int, AvatarExpItemConfigExcel> AvatarExpItemConfigData { get; private set; } = [];
        public static Dictionary<int, AvatarSkillTreeConfigExcel> AvatarSkillTreeConfigData { get; private set; } = [];
        public static Dictionary<int, ExpTypeExcel> ExpTypeData { get; private set; } = [];

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
        public static Dictionary<int, List<int>> ChessRogueContentGenData { get; set; } = [];
        public static Dictionary<int, ChessRogueCellConfig> ChessRogueCellGenData { get; set; } = [];
        public static Dictionary<int, Dictionary<int, List<int>>> ChessRogueLayerGenData { get; set; } = [];
        public static Dictionary<int, ChessRogueRoomConfig> ChessRogueRoomGenData { get; set; } = [];
        public static Dictionary<int, RogueDLCAreaExcel> RogueDLCAreaData { get; private set; } = [];
        public static Dictionary<int, RogueDLCBossDecayExcel> RogueDLCBossDecayData { get; private set; } = [];
        public static Dictionary<int, RogueDLCBossBpExcel> RogueDLCBossBpData { get; private set; } = [];
        public static Dictionary<int, RogueDLCDifficultyExcel> RogueDLCDifficultyData { get; private set; } = [];
        public static Dictionary<int, RogueNousAeonExcel> RogueNousAeonData { get; private set; } = [];
        public static Dictionary<int, RogueNousDiceBranchExcel> RogueNousDiceBranchData { get; private set; } = [];
        public static Dictionary<int, RogueNousDiceSurfaceExcel> RogueNousDiceSurfaceData { get; private set; } = [];
        public static Dictionary<int, RogueNousDifficultyLevelExcel> RogueNousDifficultyLevelData { get; private set; } = [];
        public static Dictionary<int, RogueNousMainStoryExcel> RogueNousMainStoryData { get; private set; } = [];
        public static Dictionary<int, RogueNousSubStoryExcel> RogueNousSubStoryData { get; private set; } = [];
        public static Dictionary<int, RogueNousTalentExcel> RogueNousTalentData { get; private set; } = [];

        #endregion

        #region Player

        public static Dictionary<int, QuestDataExcel> QuestDataData { get; private set; } = [];
        public static Dictionary<int, PlayerLevelConfigExcel> PlayerLevelConfigData { get; private set; } = [];
        public static Dictionary<int, BackGroundMusicExcel> BackGroundMusicData { get; private set; } = [];

        #endregion

        #region Maze

        public static Dictionary<int, NPCDataExcel> NpcDataData { get; private set; } = [];
        public static Dictionary<string, FloorInfo> FloorInfoData { get; private set; } = [];
        public static Dictionary<int, MapEntranceExcel> MapEntranceData { get; private set; } = [];
        public static Dictionary<int, MazePlaneExcel> MazePlaneData { get; private set; } = [];
        public static Dictionary<int, MazePropExcel> MazePropData { get; private set; } = [];
        public static Dictionary<int, PlaneEventExcel> PlaneEventData { get; private set; } = [];

        #endregion

        #region Items

        public static Dictionary<int, MappingInfoExcel> MappingInfoData { get; private set; } = [];
        public static Dictionary<int, ItemConfigExcel> ItemConfigData { get; private set; } = [];
        public static Dictionary<int, EquipmentConfigExcel> EquipmentConfigData { get; private set; } = [];
        public static Dictionary<int, EquipmentExpTypeExcel> EquipmentExpTypeData { get; private set; } = [];
        public static Dictionary<int, EquipmentExpItemConfigExcel> EquipmentExpItemConfigData { get; private set; } = [];
        public static Dictionary<int, EquipmentPromotionConfigExcel> EquipmentPromotionConfigData { get; private set; } = [];
        public static Dictionary<int, Dictionary<int, RelicMainAffixConfigExcel>> RelicMainAffixData { get; private set; } = [];  // groupId, affixId
        public static Dictionary<int, Dictionary<int, RelicSubAffixConfigExcel>> RelicSubAffixData { get; private set; } = [];  // groupId, affixId
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

        #endregion

        #region Item Exchange

        public static Dictionary<int, ShopConfigExcel> ShopConfigData { get; private set; } = [];
        public static Dictionary<int, ItemComposeConfigExcel> ItemComposeConfigData { get; private set; } = [];

        #endregion

        #region Rogue

        public static Dictionary<int, DialogueEventExcel> DialogueEventData { get; private set; } = [];
        public static Dictionary<int, Dictionary<int, DialogueDynamicContentExcel>> DialogueDynamicContentData { get; private set; } = [];
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
        public static Dictionary<int, RogueNPCDialogueExcel> RogueNPCDialogueData { get; private set; } = [];
        public static Dictionary<int, RogueRoomExcel> RogueRoomData { get; private set; } = [];
        public static Dictionary<int, RogueTalentExcel> RogueTalentData { get; private set; } = [];

        #endregion

        #region Banners

        public static BannersConfig BannersConfig { get; set; } = new();

        #endregion

        #region Actions

        public static void GetFloorInfo(int planeId, int floorId, out FloorInfo outer)
        {
            FloorInfoData.TryGetValue("P" + planeId + "_F" + floorId, out outer!);
        }
        public static int GetAvatarExpRequired(int group, int level)
        {
            ExpTypeData.TryGetValue((group * 100) + level, out var expType);
            return expType?.Exp ?? 0;
        }

        public static int GetEquipmentExpRequired(int group, int level)
        {
            EquipmentExpTypeData.TryGetValue((group * 100) + level, out var expType);
            return expType?.Exp ?? 0;
        }

        public static int GetMinPromotionForLevel(int level)
        {
            return Math.Max(Math.Min((int)((level - 11) / 10D), 6), 0);
        }

        #endregion
    }
}
