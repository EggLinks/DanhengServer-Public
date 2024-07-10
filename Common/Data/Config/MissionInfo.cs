using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Config
{
    public class MissionInfo
    {
        public int MainMissionID { get; set; }
        public List<int> StartSubMissionList { get; set; } = [];
        public List<int> FinishSubMissionList { get; set; } = [];
        public List<SubMissionInfo> SubMissionList { get; set; } = [];
        public List<CustomValueInfo> MissionCustomValueList { get; set; } = [];
    }

    public class SubMissionInfo
    {
        public int ID { get; set; }
        public int LevelPlaneID { get; set; }
        public int LevelFloorID { get; set; }
        public int MainMissionID { get; set; }
        public string MissionJsonPath { get; set; } = "";

        [JsonConverter(typeof(StringEnumConverter))]
        public SubMissionTakeTypeEnum TakeType { get; set; }
        public List<int>? TakeParamIntList { get; set; } = [];  // the mission's prerequisites
        [JsonConverter(typeof(StringEnumConverter))]
        public MissionFinishTypeEnum FinishType { get; set; }
        public int ParamInt1 { get; set; }
        public int ParamInt2 { get; set; }
        public int ParamInt3 { get; set; }
        public string ParamStr1 { get; set; } = "";
        public List<int>? ParamIntList { get; set; } = [];
        public List<MaterialItem>? ParamItemList { get; set; } = [];
        public List<FinishActionInfo>? FinishActionList { get; set; } = [];
        public int Progress { get; set; }
        public List<int>? GroupIDList { get; set; } = [];
        public int SubRewardID { get; set; }

        [JsonIgnore]
        public SubMissionTask<EnterFloorTaskInfo> Task { get; set; } = new();
        [JsonIgnore]
        public SubMissionTask<PropStateTaskInfo> PropTask { get; set; } = new();
        [JsonIgnore]
        public SubMissionTask<StageWinTaskInfo> StageWinTask { get; set; } = new();

        [JsonIgnore]
        public int MapEntranceID { get; set; }

        [JsonIgnore]
        public int AnchorGroupID { get; set; }
        [JsonIgnore]
        public int AnchorID { get; set; }

        [JsonIgnore]
        public List<int> StageList { get; set; } = [];

        [JsonIgnore]
        public PropStateEnum SourceState { get; set; } = PropStateEnum.Closed;

        public void Loaded(int type)  // 1 for EnterFloor, 2 for PropState
        {
            if (type == 1)
            {
                try
                {
                    if (Task.OnStartSequece.Count > 0)
                    {
                        MapEntranceID = Task.OnStartSequece[0].TaskList[0].EntranceID;
                        AnchorGroupID = Task.OnStartSequece[0].TaskList[0].GroupID;
                        AnchorID = Task.OnStartSequece[0].TaskList[0].AnchorID;
                    }
                    else if (Task.OnInitSequece.Count > 0)
                    {
                        MapEntranceID = Task.OnInitSequece[0].TaskList[0].EntranceID;
                        AnchorGroupID = Task.OnInitSequece[0].TaskList[0].GroupID;
                        AnchorID = Task.OnInitSequece[0].TaskList[0].AnchorID;
                    }
                }
                catch
                {
                }
            } else if (type == 2)
            {
                foreach (var task in PropTask.OnStartSequece)
                {
                    foreach (var prop in task.TaskList)
                    {
                        if (prop.ButtonCallBack != null)
                        {
                            SourceState = prop.ButtonCallBack[0].State;
                        }
                    }
                }

                foreach (var task in PropTask.OnInitSequece)
                {
                    foreach (var prop in task.TaskList)
                    {
                        if (prop.ButtonCallBack != null)
                        {
                            SourceState = prop.ButtonCallBack[0].State;
                        }
                    }
                }
            } else if (type == 3)
            {
                foreach (var task in StageWinTask.OnStartSequece)
                {
                    foreach (var stageWinTask in task.TaskList)
                    {
                        if (stageWinTask.Type == "RPG.GameCore.TriggerBattle")
                        {
                            if (stageWinTask.EventID.GetValue() > 0)
                            {
                                StageList.Add(stageWinTask.EventID.GetValue());
                            }
                        }
                    }
                }
            }
        }
    }


    public class CustomValueInfo
    {
        public int Index { get; set; }
        public List<int> ValidValueParamList { get; set; } = [];
    }

    public class FinishActionInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public FinishActionTypeEnum FinishActionType { get; set; }
        public List<int> FinishActionPara { get; set; } = [];
        public List<string> FinishActionParaString { get; set; } = [];
    }

    public class SubMissionTask<T>
    {
        public List<SubMissionTaskInfo<T>> OnInitSequece { get; set; } = [];
        public List<SubMissionTaskInfo<T>> OnStartSequece { get; set; } = [];
    }

    public class SubMissionTaskInfo<T>
    {
        public List<T> TaskList { get; set; } = [];
    }

    public class EnterFloorTaskInfo
    {
        public int EntranceID { get; set; }
        public int GroupID { get; set; }
        public int AnchorID { get; set; }
    }

    public class PropStateTaskInfo
    {

        [JsonConverter(typeof(StringEnumConverter))]
        public PropStateEnum State { get; set; } = PropStateEnum.Closed;

        public List<PropStateTaskInfo>? ButtonCallBack { get; set; }
    }

    public class StageWinTaskInfo
    {
        public string Type { get; set; } = "";
        public StageWinTaskEventInfo EventID { get; set; } = new();
    }

    public class StageWinTaskEventInfo
    {
        public bool IsDynamic { get; set; }
        public FixedValueInfo<int> FixedValue { get; set; } = new();

        public int GetValue()
        {
            return IsDynamic ? 0 : FixedValue.Value;
        }
    }
}
