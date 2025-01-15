using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Proto;
using SqlSugar;

namespace EggLink.DanhengServer.Database.TrainParty;

[SugarTable("train_party_data")]
public class GameTrainPartyData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true, ColumnDataType = "TEXT")]
    public Dictionary<int, GameTrainPartyAreaInfo> Areas { get; set; } = [];

    [SugarColumn(IsJson = true, ColumnDataType = "TEXT")]
    public Dictionary<int, GameTrainPartyGridInfo> Grids { get; set; } = [];

    [SugarColumn(IsJson = true, ColumnDataType = "TEXT")]
    public Dictionary<int, GameTrainPartyCardInfo> Cards { get; set; } = [];

    public int UniqueId { get; set; } = 1;
}

public class GameTrainPartyGridInfo
{
    public int GridId { get; set; }
    public int UniqueId { get; set; }

    public TrainPartyGameGrid ToProto()
    {
        return new TrainPartyGameGrid
        {
            GridId = (uint)GridId,
            UniqueId = (uint)UniqueId,
            GAEIOFOPLFN = (uint)UniqueId
        };
    }
}

public class GameTrainPartyCardInfo
{
    public int CardId { get; set; }
    public int UniqueId { get; set; }

    public TrainPartyGameCard ToProto()
    {
        return new TrainPartyGameCard
        {
            CardId = (uint)CardId,
            UniqueId = (uint)UniqueId
        };
    }
}

public class GameTrainPartyAreaInfo
{
    public int AreaId { get; set; }
    public List<int> StepList { get; set; } = [];
    public Dictionary<int, int> DynamicInfo { get; set; } = [];

    public TrainPartyArea ToProto()
    {
        var info = new TrainPartyArea
        {
            AreaId = (uint)AreaId,
            StepIdList = { StepList.Select(x => (uint)x) },
            AreaStepInfo = new AreaStepInfo
            {
                AreaGlobalId = (uint)GameData.TrainPartyAreaGoalConfigData.First(x => x.Value.AreaID == AreaId).Key,
                AreaStepList =
                {
                    StepList.Select(x => new BuildAreaStep
                    {
                        StepId = (uint)x,
                        Status = BuildGoalStep.Finish
                    })
                }
            },
            Progress = 100,
            DynamicInfo =
            {
                DynamicInfo.Select(x => new AreaDynamicInfo
                {
                    DiceSlotId = (uint)x.Key,
                    DiyDynamicId = (uint)x.Value
                })
            }
        };

        foreach (var step in StepList)
        {
            GameData.TrainPartyStepConfigData.TryGetValue(step, out var stepExcel);
            if (stepExcel == null) continue;

            info.StaticPropIdList.AddRange(stepExcel.StaticPropIDList.Select(x => (uint)x));
        }

        return info;
    }

    public int GetCoinCost()
    {
        var cost = 0;

        foreach (var step in StepList)
        {
            GameData.TrainPartyStepConfigData.TryGetValue(step, out var stepExcel);
            if (stepExcel == null) continue;

            cost += stepExcel.CoinCost;
        }

        return cost;
    }
}