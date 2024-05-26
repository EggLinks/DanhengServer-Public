using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config
{
    public class PropInfo : PositionInfo
    {
        public int MappingInfoID { get; set; }
        public int AnchorGroupID { get; set; }
        public int AnchorID { get; set; }
        public int PropID { get; set; }
        public int EventID { get; set; }
        public int CocoonID { get; set; }
        public int FarmElementID { get; set; }
        public bool IsClientOnly { get; set; }

        public PropValueSource? ValueSource { get; set; }
        public string? InitLevelGraph { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PropStateEnum State { get; set; } = PropStateEnum.Closed;

        [JsonIgnore()]
        public Dictionary<int, List<int>> UnlockDoorID { get; set; } = [];

        public void Load(GroupInfo info)
        {
            if (ValueSource != null)
            {
                foreach (var v in ValueSource.Values)
                {
                    try
                    {
                        var key = v["Key"];
                        var value = v["Value"];
                        if (value != null && key != null)
                        {
                            if (key.ToString().Contains("Door") || 
                                key.ToString().Contains("Bridge") || 
                                key.ToString().Contains("UnlockTarget") ||
                                key.ToString().Contains("Rootcontamination") ||
                                key.ToString().Contains("Portal"))
                            {
                                try
                                {
                                    if (UnlockDoorID.ContainsKey(int.Parse(value.ToString().Split(",")[0])) == false)
                                    {
                                        UnlockDoorID.Add(int.Parse(value.ToString().Split(",")[0]), []);
                                    }
                                    UnlockDoorID[int.Parse(value.ToString().Split(",")[0])].Add(int.Parse(value.ToString().Split(",")[1]));
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
        }
    }

    public class PropValueSource
    {
        public List<JObject> Values { get; set; } = [];
    }
}
