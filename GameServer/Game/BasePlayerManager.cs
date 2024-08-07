using EggLink.DanhengServer.GameServer.Game.Player;

namespace EggLink.DanhengServer.GameServer.Game;

public class BasePlayerManager(PlayerInstance player)
{
    public PlayerInstance Player { get; private set; } = player;
}