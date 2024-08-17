namespace EggLink.DanhengServer.Data.Config.Rogue;

public class RogueDialogueEventOptionConfigInfo
{
    public int OptionID { get; set; }
    public int DisplayID { get; set; }
    public int SpecialOptionID { get; set; }
    public Dictionary<int, RogueDialogueEventOptionDynamicConfigInfo> DynamicMap { get; set; } = [];
    public int DescValue { get; set; }
    public int DescValue2 { get; set; }
    public int DescValue3 { get; set; }
    public int DescValue4 { get; set; }
}