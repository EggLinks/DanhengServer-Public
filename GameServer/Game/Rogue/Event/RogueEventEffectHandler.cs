namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event;

public abstract class RogueEventEffectHandler
{
    public abstract void Init(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList,
        RogueEventParam? option);

    public abstract ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList,
        RogueEventParam? option);
}