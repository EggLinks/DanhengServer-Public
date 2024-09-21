using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ChallengeMazeConfig.json,ChallengeStoryMazeConfig.json,ChallengeBossMazeConfig.json",
    true)]
public class ChallengeConfigExcel : ExcelResource
{
    [JsonIgnore] public ChallengeBossExtraExcel? BossExcel;

    [JsonIgnore] public ChallengeStoryExtraExcel? StoryExcel;

    // General item data
    public int ID { get; set; }
    public int GroupID { get; set; }
    public int MapEntranceID { get; set; }
    public int MapEntranceID2 { get; set; }
    public int StageNum { get; set; }
    public int ChallengeCountDown { get; set; }
    public int MazeBuffID { get; set; }

    public List<int>? ChallengeTargetID { get; set; } = [];

    public int MazeGroupID1 { get; set; }
    public List<int>? ConfigList1 { get; set; } = [];
    public List<int>? NpcMonsterIDList1 { get; set; } = [];
    public List<int>? EventIDList1 { get; set; } = [];

    public int MazeGroupID2 { get; set; }
    public List<int>? ConfigList2 { get; set; } = [];
    public List<int>? NpcMonsterIDList2 { get; set; } = [];
    public List<int>? EventIDList2 { get; set; } = [];

    [JsonIgnore] public Dictionary<int, ChallengeMonsterInfo> ChallengeMonsters1 { get; set; } = new();

    [JsonIgnore] public Dictionary<int, ChallengeMonsterInfo> ChallengeMonsters2 { get; set; } = new();

    public override int GetId()
    {
        return ID;
    }

    public bool IsStory()
    {
        return StoryExcel != null;
    }

    public bool IsBoss()
    {
        return BossExcel != null;
    }

    public void SetStoryExcel(ChallengeStoryExtraExcel storyExcel)
    {
        StoryExcel = storyExcel;
        ChallengeCountDown = storyExcel.TurnLimit;
    }

    public void SetBossExcel(ChallengeBossExtraExcel bossExcel)
    {
        BossExcel = bossExcel;
    }

    public override void Loaded()
    {
        // Cache challenge monsters
        for (var i = 0; i < ConfigList1?.Count; i++)
        {
            if (ConfigList1[i] == 0) break;

            var Monster = new ChallengeMonsterInfo(ConfigList1[i], NpcMonsterIDList1![i], EventIDList1![i]);
            ChallengeMonsters1.Add(Monster.ConfigId, Monster);
        }

        for (var i = 0; i < ConfigList2?.Count; i++)
        {
            if (ConfigList2[i] == 0) break;

            var Monster = new ChallengeMonsterInfo(ConfigList2[i], NpcMonsterIDList2![i], EventIDList2![i]);
            ChallengeMonsters2.Add(Monster.ConfigId, Monster);
        }

        ConfigList1 = null;
        NpcMonsterIDList1 = null;
        EventIDList1 = null;
        ConfigList2 = null;
        NpcMonsterIDList2 = null;
        EventIDList2 = null;

        GameData.ChallengeConfigData[ID] = this;
    }

    public class ChallengeMonsterInfo(int ConfigId, int NpcMonsterId, int EventId)
    {
        public int ConfigId = ConfigId;
        public int EventId = EventId;
        public int NpcMonsterId = NpcMonsterId;
    }
}