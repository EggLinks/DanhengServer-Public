using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Miracle;

public class RogueMiracleSelectMenu(BaseRogueInstance instance)
{
    public List<int> Pools { get; set; } = [];

    public List<uint> Results { get; set; } = [];

    public void RollMiracle(List<int> pools)
    {
        Pools = pools;
        Results.Clear();

        for (var i = 0; i < 3; i++)
        {
            var id = Pools.RandomElement();
            Results.Add((uint)id);
            Pools.Remove(id);

            if (Pools.Count == 0) break;
        }
    }

    public RogueActionInstance GetActionInstance()
    {
        instance.CurActionQueuePosition += 3;
        return new RogueActionInstance
        {
            QueuePosition = instance.CurActionQueuePosition,
            RogueMiracleSelectMenu = this
        };
    }

    public RogueMiracleSelectInfo ToProto()
    {
        var info = new RogueMiracleSelectInfo();
        info.SelectMiracleList.AddRange(Results);
        info.MiracleHandbookList.AddRange(Results);

        return info;
    }
}