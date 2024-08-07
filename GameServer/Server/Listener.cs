using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server;

public class Listener : DanhengListener
{
    public static Connection? GetActiveConnection(int uid)
    {
        var con = Connections.Values.FirstOrDefault(c =>
            (c as Connection)?.Player?.Uid == uid && c.State == SessionStateEnum.ACTIVE) as Connection;
        return con;
    }
}