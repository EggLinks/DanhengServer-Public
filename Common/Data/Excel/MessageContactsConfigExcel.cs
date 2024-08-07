using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MessageContactsConfig.json")]
public class MessageContactsConfigExcel : ExcelResource
{
    public int ID { get; set; }

    [JsonIgnore] public List<MessageGroupConfigExcel> Groups { get; set; } = [];

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.MessageContactsConfigData.Add(ID, this);
    }
}