using EggLink.DanhengServer.Server.Http.Handler;
using Microsoft.AspNetCore.Mvc;

namespace EggLink.DanhengServer.WebServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class GateServerRoutes
    {
        [HttpGet("/query_gateway")]
        public string QueryGateway() => new QueryGatewayHandler().Data;
    }
}
