namespace EggLink.DanhengServer.WebServer.Response;

public class CreateSessionResponse(int code, string message, CreateSessionData? data)
    : BaseResponse<CreateSessionData>(code, message, data)
{
}

public class CreateSessionData
{
    public string RsaPublicKey { get; set; } = "";
    public string SessionId { get; set; } = "";
    public long ExpireTimeStamp { get; set; } = 0;
}