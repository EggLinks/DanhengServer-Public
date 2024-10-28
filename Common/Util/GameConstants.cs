namespace EggLink.DanhengServer.Util;

public static class GameConstants
{
    public const int MATERIAL_HCOIN_ID = 1; // Material id for jades. DO NOT CHANGE
    public const int MATERIAL_COIN_ID = 2; // Material id for credits. DO NOT CHANGE
    public const int TRAILBLAZER_EXP_ID = 22;
    public const int RELIC_REMAINS_ID = 235;

    public const int INVENTORY_MAX_EQUIPMENT = 1500;
    public const int INVENTORY_MAX_RELIC = 1500;
    public const int INVENTORY_MAX_MATERIAL = 2000;

    public const int MAX_LINEUP_COUNT = 9;

    public const int AMBUSH_BUFF_ID = 1000102;

    public const int MAX_STAMINA = 240;
    public const int MAX_STAMINA_RESERVE = 2400;
    public const int STAMINA_RECOVERY_TIME = 360; // 6 minutes
    public const int STAMINA_RESERVE_RECOVERY_TIME = 1080; // 18 minutes

    public const int CHALLENGE_ENTRANCE = 100000103;
    public const int CHALLENGE_STORY_ENTRANCE = 102020107;
    public const int CHALLENGE_BOSS_ENTRANCE = 1030402;

    public static readonly List<int> UpgradeWorldLevel = [20, 30, 40, 50, 60, 65];
    public static readonly List<int> AllowedChessRogueEntranceId = [8020701, 8020901, 8020401, 8020201];
}