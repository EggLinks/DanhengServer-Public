namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("BattleCollegeConfig.json")]
public class BattleCollegeConfigExcel : ExcelResource
{
    public List<int> TrialAvatarList { get; set; } = [];
    public string VideoCoverPath { get; set; } = string.Empty;
    public List<int> AimList { get; set; } = [];
    public int TutorialID { get; set; }
    public int BattleAreaGroupID { get; set; }
    public int StageIntroDescIDList { get; set; }
    public int BattleAreaID { get; set; }
    public int FloorID { get; set; }
    public int RewardID { get; set; }
    public int TutorialTypeGroupID { get; set; }
    public int StageID { get; set; }
    public int PlaneID { get; set; }
    public int VideoAssetID { get; set; }
    public HashName StageIntroTitle { get; set; } = new();
    public int SortID { get; set; }
    public int ID { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.BattleCollegeConfigData.TryAdd(ID, this);
    }
}