namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RewardData.json")]
public class RewardDataExcel : ExcelResource
{
    public int RewardID { get; set; }
    public int Hcoin { get; set; }
    public int ItemID_1 { get; set; }
    public int Count_1 { get; set; }
    public int ItemID_2 { get; set; }
    public int Count_2 { get; set; }
    public int ItemID_3 { get; set; }
    public int Count_3 { get; set; }
    public int ItemID_4 { get; set; }
    public int Count_4 { get; set; }
    public int ItemID_5 { get; set; }
    public int Count_5 { get; set; }
    public int ItemID_6 { get; set; }
    public int Count_6 { get; set; }

    public override int GetId()
    {
        return RewardID;
    }

    public override void Loaded()
    {
        GameData.RewardDataData[RewardID] = this;
    }

    public List<(int, int)> GetItems()
    {
        var items = new List<(int, int)>();
        if (ItemID_1 != 0) items.Add((ItemID_1, Count_1));
        if (ItemID_2 != 0) items.Add((ItemID_2, Count_2));
        if (ItemID_3 != 0) items.Add((ItemID_3, Count_3));
        if (ItemID_4 != 0) items.Add((ItemID_4, Count_4));
        if (ItemID_5 != 0) items.Add((ItemID_5, Count_5));
        if (ItemID_6 != 0) items.Add((ItemID_6, Count_6));
        return items;
    }
}