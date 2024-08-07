using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Battle;

public class BattleEventInstance(int battleEventId, int curSp, int maxSp = 10000)
{
    public int BattleEventId { get; set; } = battleEventId;
    public int CurSp { get; set; } = curSp;
    public int MaxSp { get; set; } = maxSp;

    public void AddSp(int sp)
    {
        CurSp = Math.Min(CurSp + sp, MaxSp);
    }

    public void SubSp(int sp)
    {
        CurSp = Math.Max(CurSp - sp, 0);
    }

    public BattleEventBattleInfo ToProto()
    {
        return new BattleEventBattleInfo
        {
            BattleEventId = (uint)BattleEventId,
            Status = new BattleEventProperty
            {
                SpBar = new SpBarInfo
                {
                    CurSp = (uint)CurSp,
                    MaxSp = (uint)MaxSp
                }
            }
        };
    }
}