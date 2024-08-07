namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MessageGroupConfig.json")]
public class MessageGroupConfigExcel : ExcelResource
{
    public int ID { get; set; }
    public int MessageContactsID { get; set; }
    public List<int> MessageSectionIDList { get; set; } = [];

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.MessageGroupConfigData.Add(ID, this);
    }

    public override void AfterAllDone()
    {
        GameData.MessageContactsConfigData[MessageContactsID].Groups.Add(this);
        MessageSectionIDList.ForEach(m =>
        {
            GameData.MessageSectionConfigData.TryGetValue(m, out var section);
            if (section != null) section.GroupID = ID;
        });
    }
}