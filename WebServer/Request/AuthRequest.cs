namespace EggLink.DanhengServer.WebServer.Request;

public class AuthAdminKeyRequestBody
{
    public string session_id { get; set; } = "";
    public string admin_key { get; set; } = "";
}