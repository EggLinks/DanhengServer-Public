using EggLink.DanhengServer.Database.Account;

namespace EggLink.DanhengServer.WebServer.Server;

public class MuipSession
{
    public string RsaPublicKey { get; set; } = "";
    public string SessionId { get; set; } = "";
    public long ExpireTimeStamp { get; set; } = 0;

    public bool IsAdmin { get; set; } = false;
    public bool IsAuthorized { get; set; } = false;
    public AccountData? Account { get; set; } = null;
}