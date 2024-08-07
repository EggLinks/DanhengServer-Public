namespace EggLink.DanhengServer.WebServer.Response;

public class ExecuteCommandResponse(int code, string message, ExecuteCommandData? data = null)
    : BaseResponse<ExecuteCommandData>(code, message, data)
{
}

public class ExecuteCommandData
{
    public string SessionId { get; set; } = "";
    public string Message { get; set; } = "";
}