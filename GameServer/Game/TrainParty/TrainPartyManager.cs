using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.TrainParty;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using GameTrainPartyCardInfo = EggLink.DanhengServer.Database.TrainParty.GameTrainPartyCardInfo;

namespace EggLink.DanhengServer.GameServer.Game.TrainParty;

public class TrainPartyManager : BasePlayerManager
{
    public TrainPartyManager(PlayerInstance player) : base(player)
    {
        TrainPartyData =
            DatabaseHelper.Instance!.GetInstanceOrCreateNew<GameTrainPartyData>(player.Uid);

        foreach (var excel in GameData.TrainPartyAreaConfigData.Where(excel =>
                     !TrainPartyData.Areas.ContainsKey(excel.Key)))
            TrainPartyData.Areas[excel.Key] = new GameTrainPartyAreaInfo
            {
                AreaId = excel.Key,
                StepList = [excel.Value.FirstStep]
            };

        TeamExcel = GameData.TrainPartyTeamData.Values.ToList().RandomElement();
    }

    public GameTrainPartyData TrainPartyData { get; }
    public TrainPartyTeamExcel TeamExcel { get; }

    public async ValueTask AddCard(int cardId)
    {
        if (TrainPartyData.Cards.Values.FirstOrDefault(x => x.CardId == cardId) != null) return;

        var uniqueId = TrainPartyData.UniqueId++;
        TrainPartyData.Cards.Add(uniqueId, new GameTrainPartyCardInfo
        {
            CardId = cardId,
            UniqueId = uniqueId
        });

        await ValueTask.CompletedTask;
    }

    public async ValueTask AddGrid(int gridId)
    {
        var uniqueId = TrainPartyData.UniqueId++;
        TrainPartyData.Grids.Add(uniqueId, new GameTrainPartyGridInfo
        {
            GridId = gridId,
            UniqueId = uniqueId
        });

        await ValueTask.CompletedTask;
    }

    public GameTrainPartyAreaInfo? SetDynamicId(int areaId, int slotId, int dynamicId)
    {
        if (!TrainPartyData.Areas.TryGetValue(areaId, out var area)) return null;

        area.DynamicInfo[slotId] = dynamicId;

        return area;
    }

    public TrainPartyData ToProto()
    {
        var proto = new TrainPartyData
        {
            TrainPartyInfo = ToPartyInfo(),
            PassengerInfo = ToPassengerInfo(),
            TrainPartyGameInfo = ToGameInfo(),
            DPOLGBKEKLD = 6
        };

        return proto;
    }

    public TrainPartyInfo ToPartyInfo()
    {
        var proto = new TrainPartyInfo
        {
            AreaList = { TrainPartyData.Areas.Values.Select(x => x.ToProto()) },
            CoinCost = (uint)TrainPartyData.Areas.Values.Sum(x => x.GetCoinCost()),
            DynamicIdList = { GameData.TrainPartyDynamicConfigData.Select(x => (uint)x.Key) }
        };

        return proto;
    }

    public TrainPartyPassengerInfo ToPassengerInfo()
    {
        return new TrainPartyPassengerInfo
        {
            PassengerInfoList =
            {
                GameData.TrainPartyPassengerConfigData.Select(x => new TrainPartyPassenger
                {
                    PassengerId = (uint)x.Key
                })
            }
        };
    }

    public TrainPartyGameInfo ToGameInfo()
    {
        return new TrainPartyGameInfo
        {
            TeamId = (uint)TeamExcel.TeamID,
            TrainActionInfo = new TrainPartyActionInfo(),
            TrainPassengerInfo = ToGamePassengerInfo(),
            TrainPartyGridInfo = ToGameGridInfo(),
            TrainPartyItemInfo = ToGameItemInfo()
        };
    }

    public TrainPartyGamePassengerInfo ToGamePassengerInfo()
    {
        return new TrainPartyGamePassengerInfo
        {
            PassengerList =
            {
                TeamExcel.PassengerList.Select(x => new TrainPartyGamePassenger
                {
                    PassengerId = (uint)x
                })
            },
            CurPassengerId = (uint)TeamExcel.PassengerList.RandomElement(),
            MtRankId = 104
        };
    }

    public TrainPartyGameGridInfo ToGameGridInfo()
    {
        return new TrainPartyGameGridInfo
        {
            GridList = { TrainPartyData.Grids.Values.Select(x => x.ToProto()) }
        };
    }

    public TrainPartyGameItemInfo ToGameItemInfo()
    {
        return new TrainPartyGameItemInfo
        {
            TrainPartyCardInfo = new TrainPartyGameCardInfo
            {
                TrainPartyCardInfo = { TrainPartyData.Cards.Values.Select(x => x.ToProto()) }
            }
        };
    }
}