using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueMagic.Adventure;

public class RogueMagicAdventureInstance(RogueMagicAdventureRoomExcel excel)
{
    public RogueMagicAdventureRoomExcel Excel { get; set; } = excel;

    public int RemainMonsterNum { get; set; } =
        excel.AdventureType == RogueAdventureGameplayTypeEnum.RogueCaptureMonster ? 16 : 0;

    public int CaughtMonsterNum { get; set; }
    public RogueAdventureRoomStatus Status { get; set; } = RogueAdventureRoomStatus.None;
    public int Score { get; set; } = 0;
    public List<RogueAdventureWolfGunTargetInstance> WolfGunTargets { get; set; } = [];

    public void Prepare()
    {
        Status = RogueAdventureRoomStatus.Started;
        if (Excel.AdventureType == RogueAdventureGameplayTypeEnum.RogueWolfGun)
        {
            WolfGunTargets.Clear();
            var randomList = new RandomList<RogueAdventureWolfGunTargetInstance>();
            randomList.Add(new RogueAdventureWolfGunTargetInstance { IsMoney = true, TargetId = 10 }, 5);
            randomList.Add(
                new RogueAdventureWolfGunTargetInstance
                {
                    IsMiracle = true,
                    TargetId = GameData.RogueWolfGunMiracleTargetData.Values
                        .Where(x => x.GameMode == GameModeTypeEnum.MagicRogue).ToList().RandomElement().MiracleID
                }, 3);

            randomList.Add(
                new RogueAdventureWolfGunTargetInstance
                {
                    IsMiracle = true,
                    TargetId = GameData.RogueWolfGunMiracleTargetData.Values
                        .Where(x => x.GameMode == GameModeTypeEnum.MagicRogue).ToList().RandomElement().MiracleID
                }, 3);

            randomList.Add(new RogueAdventureWolfGunTargetInstance { IsMoney = true, TargetId = 50 }, 5);
            randomList.Add(new RogueAdventureWolfGunTargetInstance { IsMoney = true, TargetId = 100 }, 5);

            randomList.Add(new RogueAdventureWolfGunTargetInstance { IsRuanmei = true }, 100);

            for (var i = 0; i < 4; i++)
            {
                var res = randomList.GetRandom();
                if (res == null) continue;
                WolfGunTargets.Add(res);
                randomList.Remove(res);
            }
        }
    }

    public AdventureRoomInfo ToProto()
    {
        var proto = new AdventureRoomInfo
        {
            RemainMonsterNum = (uint)RemainMonsterNum,
            CaughtMonsterNum = (uint)CaughtMonsterNum,
            Sus = 1,
            Status = Status,
            ScoreId = (uint)Score
        };

        if (WolfGunTargets.Count > 0)
            proto.QueryInfo = new RogueAdventureRoomGameplayWolfGunInfo
            {
                GameInfo = new RogueAdventureRoomGameplayWolfGunGameInfo
                {
                    GameTargetNum = (uint)WolfGunTargets.Count,
                    BattleTargetList = { WolfGunTargets.Select(x => x.ToProto()) }
                }
            };

        return proto;
    }
}

public class RogueAdventureWolfGunTargetInstance
{
    public bool IsMoney { get; set; }
    public int TargetId { get; set; }
    public bool IsMiracle { get; set; }
    public bool IsRuanmei { get; set; }
    public bool IsNone { get; set; }

    public RogueAdventureRoomGameplayWolfGunTarget ToProto()
    {
        var proto = new RogueAdventureRoomGameplayWolfGunTarget();

        if (IsMoney)
            proto.TargetCoin = new RogueAdventureRoomTargetCoin
            {
                Count = TargetId
            };
        else if (IsMiracle)
            proto.TargetMiracle = new RogueAdventureRoomTargetMiracle
            {
                MiracleId = (uint)TargetId
            };
        else if (IsRuanmei)
            proto.TargetRuanmei = new RogueAdventureRoomTargetRuanmei();
        else if (IsNone)
            proto.TargetNone = new RogueAdventureRoomTargetNone();

        return proto;
    }
}