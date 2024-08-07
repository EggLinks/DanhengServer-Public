namespace EggLink.DanhengServer.WebServer.Objects;

public class LoginReqJson
{
    public string? account { get; set; }
    public string? password { get; set; }
    public bool is_crypto { get; set; }
}

public class NewLoginReqJson
{
    public string? account { get; set; }
    public string? password { get; set; }
}

public class VerifyReqJson
{
    public string? uid { get; set; }
    public string? token { get; set; }
}

public class LoginV2ReqJson
{
    public int app_id { get; set; }
    public int channel_id { get; set; }
    public string? data { get; set; }
    public string? device { get; set; }
    public string? sign { get; set; }
}