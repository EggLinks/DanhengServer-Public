using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Database.Quests;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Enums.Mission;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MissionInfo = EggLink.DanhengServer.Data.Config.MissionInfo;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MainMission.json")]
public class MainMissionExcel : ExcelResource
{
    public int MainMissionID { get; set; }
    public HashName Name { get; set; } = new();

    [JsonConverter(typeof(StringEnumConverter))]
    public OperationEnum TakeOperation { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public OperationEnum BeginOperation { get; set; }

    public List<MissionParam> TakeParam { get; set; } = [];
    public List<MissionParam> BeginParam { get; set; } = [];
    public int RewardID { get; set; }
    public List<int> SubRewardList { get; set; } = [];

    [JsonIgnore] private MissionInfo? InnerMissionInfo { get; set; }

    [JsonIgnore]
    public MissionInfo? MissionInfo
    {
        get => InnerMissionInfo;
        set
        {
            InnerMissionInfo = value;
            if (value != null)
                foreach (var sub in value.SubMissionList)
                {
                    SubMissionIds.Add(sub.ID);
                    GameData.SubMissionData.TryGetValue(sub.ID, out var subMission);
                    if (subMission != null)
                    {
                        subMission.MainMissionID = MainMissionID;
                        subMission.MainMissionInfo = InnerMissionInfo;
                        subMission.SubMissionInfo = sub;
                    }
                }
        }
    }

    [JsonIgnore] public List<int> SubMissionIds { get; set; } = [];


    public override int GetId()
    {
        return MainMissionID;
    }

    public override void Loaded()
    {
        GameData.MainMissionData[GetId()] = this;
    }

    public bool IsEqual(MissionData data)
    {
        var result = TakeOperation == OperationEnum.And;
        foreach (var param in TakeParam)
            if (param.IsEqual(data))
            {
                if (TakeOperation != OperationEnum.And) return true;
            }
            else
            {
                if (TakeOperation == OperationEnum.And) return false;
            }

        return result;
    }
}

public class MissionParam
{
    [JsonConverter(typeof(StringEnumConverter))]
    public MissionTakeTypeEnum Type { get; set; }

    public int Value { get; set; }

    public bool IsEqual(MissionData data)
    {
        switch (Type)
        {
            case MissionTakeTypeEnum.Auto:
                return true;
            case MissionTakeTypeEnum.PlayerLevel:
                return PlayerData.GetPlayerByUid(data.Uid)!.Level >= Value;
            case MissionTakeTypeEnum.WorldLevel:
                return PlayerData.GetPlayerByUid(data.Uid)!.WorldLevel >= Value;
            case MissionTakeTypeEnum.MultiSequence:
                var value = data.GetMainMissionStatus(Value);
                return value == MissionPhaseEnum.Finish;
            default:
                return false;
        }
    }
}