using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("StageConfig.json,StageTestConfig.json", true)]
public class StageConfigExcel : ExcelResource
{
    public int StageID { get; set; } = 0;
    public HashName StageName { get; set; } = new();
    public List<StageMonsterList> MonsterList { get; set; } = [];
    public List<int> TrialAvatarList { get; set; } = [];


    public override int GetId()
    {
        return StageID;
    }

    public override void Loaded()
    {
        GameData.StageConfigData.TryAdd(StageID, this);
    }

    public List<SceneMonsterWave> ToProto()
    {
        var result = new List<SceneMonsterWave>();
        var waveId = 1;
        foreach (var monsters in MonsterList)
        {
            var proto = new SceneMonsterWave
            {
                BattleWaveId = (uint)waveId++,
                BattleStageId = (uint)StageID
            };

            if (monsters.Monster0 != 0)
                proto.MonsterList.Add(new SceneMonster
                {
                    MonsterId = (uint)monsters.Monster0
                });


            if (monsters.Monster1 != 0)
                proto.MonsterList.Add(new SceneMonster
                {
                    MonsterId = (uint)monsters.Monster1
                });

            if (monsters.Monster2 != 0)
                proto.MonsterList.Add(new SceneMonster
                {
                    MonsterId = (uint)monsters.Monster2
                });

            if (monsters.Monster3 != 0)
                proto.MonsterList.Add(new SceneMonster
                {
                    MonsterId = (uint)monsters.Monster3
                });

            if (monsters.Monster4 != 0)
                proto.MonsterList.Add(new SceneMonster
                {
                    MonsterId = (uint)monsters.Monster4
                });

            proto.MonsterParam = new SceneMonsterWaveParam();
            result.Add(proto);
        }

        return result;
    }
}

public class StageMonsterList
{
    public int Monster0 { get; set; } = 0;
    public int Monster1 { get; set; } = 0;
    public int Monster2 { get; set; } = 0;
    public int Monster3 { get; set; } = 0;
    public int Monster4 { get; set; } = 0;
}

public class HashName
{
    public long Hash { get; set; } = 0;
}