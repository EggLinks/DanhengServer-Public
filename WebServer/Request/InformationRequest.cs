namespace EggLink.DanhengServer.WebServer.Request;

public class ServerInformationRequest
{
    public string SessionId { get; set; } = "";
}

public class PlayerInformationRequest
{
    public string SessionId { get; set; } = "";
    public int Uid { get; set; }
}