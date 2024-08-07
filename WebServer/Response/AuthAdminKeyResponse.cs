namespace EggLink.DanhengServer.WebServer.Response;

public class AuthAdminKeyResponse(int code, string message, AuthAdminKeyData? data)
    : BaseResponse<AuthAdminKeyData>(code, message, data)
{
}

public class AuthAdminKeyData
{
    public string SessionId { get; set; } = "";
    public long ExpireTimeStamp { get; set; } = 0;
}