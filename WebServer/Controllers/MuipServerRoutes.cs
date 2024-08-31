using EggLink.DanhengServer.WebServer.Request;
using EggLink.DanhengServer.WebServer.Server;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EggLink.DanhengServer.WebServer.Controllers;

[ApiController]
[EnableCors("AllowAll")]
[Route("/")]
public class MuipServerRoutes
{
    [HttpPost("/muip/create_session")]
    public IActionResult CreateSession([FromBody] CreateSessionRequestBody req)
    {
        var resp = MuipManager.CreateSession(req.key_type);
        return new JsonResult(resp);
    }

    [HttpPost("/muip/auth_admin")]
    public IActionResult AuthAdminKey([FromBody] AuthAdminKeyRequestBody req)
    {
        var resp = MuipManager.AuthAdmin(req.session_id, req.admin_key);
        return new JsonResult(resp);
    }

    [HttpGet("/muip/exec_cmd")]
    public IActionResult ExecuteCommandGet([FromQuery] AdminExecRequest req)
    {
        var resp = MuipManager.ExecuteCommand(req.SessionId, req.Command, req.TargetUid);
        return new JsonResult(resp);
    }

    [HttpPost("/muip/exec_cmd")]
    public IActionResult ExecuteCommandPost([FromBody] AdminExecRequest req)
    {
        var resp = MuipManager.ExecuteCommand(req.SessionId, req.Command, req.TargetUid);
        return new JsonResult(resp);
    }

    [HttpGet("/muip/server_information")]
    public IActionResult GetServerInformationGet([FromQuery] ServerInformationRequest req)
    {
        var resp = MuipManager.GetInformation(req.SessionId);
        return new JsonResult(resp);
    }


    [HttpGet("/server/type")]
    public IActionResult DanhengVerify()
    {
        return new ContentResult
        {
            Content =
                "{\"serverType\": \"DanhengServer\"}",
            ContentType = "application/json"
        };
    }

    [HttpPost("/muip/server_information")]
    public IActionResult GetServerInformationPost([FromBody] ServerInformationRequest req)
    {
        var resp = MuipManager.GetInformation(req.SessionId);
        return new JsonResult(resp);
    }

    [HttpGet("/muip/player_information")]
    public IActionResult GetPlayerInformationGet([FromQuery] PlayerInformationRequest req)
    {
        var resp = MuipManager.GetPlayerInformation(req.SessionId, req.Uid);
        return new JsonResult(resp);
    }

    [HttpPost("/muip/player_information")]
    public IActionResult GetPlayerInformationPost([FromBody] PlayerInformationRequest req)
    {
        var resp = MuipManager.GetPlayerInformation(req.SessionId, req.Uid);
        return new JsonResult(resp);
    }
}