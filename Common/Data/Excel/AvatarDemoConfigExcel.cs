using System.Text.Json.Serialization;
using EggLink.DanhengServer.Database.Avatar;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("AvatarDemoConfig.json")]
public class AvatarDemoConfigExcel : ExcelResource
{
    public int StageID { get; set; }
    public int AvatarID { get; set; }
    public int[] TrialAvatarList { get; set; } = [];
    public int RewardID { get; set; }
    public int MazeGroupID1 { get; set; }
    public int RaidID { get; set; }
    public int MapEntranceID { get; set; }
    public int[]? ConfigList1 { get; set; } = [];
    public int[]? NpcMonsterIDList1 { get; set; } = [];
    public int[]? EventIDList1 { get; set; } = [];
    public bool EnableMazeSkillEffect { get; set; }

    [JsonIgnore] public Dictionary<int, StageMonsterInfo> StageMonsters1 { get; set; } = new();

    public override int GetId()
    {
        return StageID;
    }

    public override void Loaded()
    {
        // Cache challenge monsters
        for (var i = 0; i < ConfigList1?.Length; i++)
        {
            if (ConfigList1[i] == 0) break;

            var Monster = new StageMonsterInfo(ConfigList1[i], NpcMonsterIDList1![i], EventIDList1![i]);
            StageMonsters1.Add(Monster.ConfigId, Monster);
        }

        ConfigList1 = null;
        NpcMonsterIDList1 = null;
        EventIDList1 = null;

        GameData.AvatarDemoConfigData.Add(GetId(), this);
    }

    public List<AvatarInfo> ToAvatarData(int uid)
    {
        List<AvatarInfo> instance = [];

        foreach (var avatar in TrialAvatarList)
        {
            GameData.SpecialAvatarData.TryGetValue(GetId(), out var avatarConfig);
            instance.Add(avatarConfig!.ToAvatarData(uid));
        }

        return instance;
    }

    public class StageMonsterInfo(int ConfigId, int NpcMonsterId, int EventId)
    {
        public int ConfigId = ConfigId;
        public int EventId = EventId;
        public int NpcMonsterId = NpcMonsterId;
    }
}