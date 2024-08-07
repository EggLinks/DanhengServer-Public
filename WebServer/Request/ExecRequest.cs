namespace EggLink.DanhengServer.WebServer.Request;

public class AdminExecRequest
{
    public string SessionId { get; set; } = "";
    public string Command { get; set; } = "";
    public int TargetUid { get; set; } = 0;
}