namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event;

public abstract class RogueEventEffectHandler
{
    public abstract ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList);
}