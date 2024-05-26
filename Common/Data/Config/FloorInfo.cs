using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Util;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Config
{
    public class FloorInfo
    {
        public int FloorID { get; set; }
        public int StartGroupID { get; set; }
        public int StartAnchorID { get; set; }

        public List<FloorGroupInfo> GroupList { get; set; } = [];

        [JsonIgnore]
        public bool Loaded = false;
        [JsonIgnore]
        public Dictionary<int, GroupInfo> Groups = [];

        [JsonIgnore]
        public Dictionary<int, PropInfo> CachedTeleports = [];
        [JsonIgnore]
        public List<PropInfo> UnlockedCheckpoints = [];

        public AnchorInfo? GetAnchorInfo(int groupId, int anchorId)
        {
            Groups.TryGetValue(groupId, out GroupInfo? group);
            if (group == null) return null;

            return group.AnchorList.Find(info => info.ID == anchorId );
        }

        public void OnLoad()
        {
            if (Loaded) return;

            // Cache anchors
            foreach (var group in Groups.Values)
            {
                foreach (var prop in group.PropList)
                {
                    // Check if prop can be teleported to
                    if (prop.AnchorID > 0)
                    {
                        // Put inside cached teleport list to send to client when they request map info
                        CachedTeleports.TryAdd(prop.MappingInfoID, prop);
                        UnlockedCheckpoints.Add(prop);

                        // Force prop to be in the unlocked state
                        prop.State = PropStateEnum.CheckPointEnable;
                    }
                    else if (!string.IsNullOrEmpty(prop.InitLevelGraph))
                    {
                        string json = prop.InitLevelGraph;

                        // Hacky way to setup prop triggers
                        if (json.Contains("Maze_GroupProp_OpenTreasure_WhenMonsterDie"))
                        {
                            //prop.Trigger = new TriggerOpenTreasureWhenMonsterDie(group.Id);
                        }
                        else if (json.Contains("Common_Console"))
                        {
                            //prop.CommonConsole = true;
                        }

                        // Clear for garbage collection
                        prop.ValueSource = null;
                        prop.InitLevelGraph = null;
                    }
                }
            }

            Loaded = true;
        }

    }
    public class FloorGroupInfo
    {
        public string GroupPath { get; set; } = "";
        public bool IsDelete { get; set; }
        public int ID { get; set; }
    }

}
