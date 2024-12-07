using EggLink.DanhengServer.Enums.Scene;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.Scene;

public class PropInfo : PositionInfo
{
    [JsonIgnore] public bool CommonConsole = false;
    public int MappingInfoID { get; set; }
    public int AnchorGroupID { get; set; }
    public int AnchorID { get; set; }
    public int PropID { get; set; }
    public int EventID { get; set; }
    public int CocoonID { get; set; }
    public int ChestID { get; set; }
    public int FarmElementID { get; set; }
    public bool IsClientOnly { get; set; }
    public bool LoadOnInitial { get; set; }

    public PropValueSource? ValueSource { get; set; }
    public string? InitLevelGraph { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public PropStateEnum State { get; set; } = PropStateEnum.Closed;

    [JsonIgnore] public Dictionary<int, List<int>> UnlockDoorID { get; set; } = [];

    [JsonIgnore] public Dictionary<int, List<int>> UnlockControllerID { get; set; } = [];

    [JsonIgnore] public bool IsLevelBtn { get; set; }

    public void Load(GroupInfo info)
    {
        if (ValueSource != null)
        {
            if (Name.StartsWith("Button_") &&
                ValueSource.Values.Find(x => x["Key"]?.ToString() == "AnchorName") != null)
                IsLevelBtn = true;

            foreach (var v in ValueSource.Values)
                try
                {
                    var key = v["Key"];
                    var value = v["Value"];
                    if (value != null && key != null)
                    {
                        if (key.ToString() == "ListenTriggerCustomString")
                        {
                            info.PropTriggerCustomString.TryGetValue(value.ToString(), out var list);
                            if (list == null)
                            {
                                list = [];
                                info.PropTriggerCustomString.Add(value.ToString(), list);
                            }

                            list.Add(ID);
                        }
                        else if (key.ToString().Contains("Door") ||
                                 key.ToString().Contains("Bridge") ||
                                 key.ToString().Contains("UnlockTarget") ||
                                 key.ToString().Contains("Rootcontamination") ||
                                 key.ToString().Contains("Portal"))
                        {
                            try
                            {
                                if (UnlockDoorID.ContainsKey(int.Parse(value.ToString().Split(",")[0])) == false)
                                    UnlockDoorID.Add(int.Parse(value.ToString().Split(",")[0]), []);
                                UnlockDoorID[int.Parse(value.ToString().Split(",")[0])]
                                    .Add(int.Parse(value.ToString().Split(",")[1]));
                            }
                            catch
                            {
                            }
                        }
                        else if (key.ToString().Contains("Controller"))
                        {
                            try
                            {
                                if (UnlockControllerID.ContainsKey(int.Parse(value.ToString().Split(",")[0])) == false)
                                    UnlockControllerID.Add(int.Parse(value.ToString().Split(",")[0]), []);
                                UnlockControllerID[int.Parse(value.ToString().Split(",")[0])]
                                    .Add(int.Parse(value.ToString().Split(",")[1]));
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch
                {
                }
        }
    }
}

public class PropValueSource
{
    public List<JObject> Values { get; set; } = [];
}