using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using Google.Protobuf;

namespace EggLink.DanhengServer.Server.Http.Handler
{
    internal class QueryGatewayHandler
    {
        public static Logger Logger = new("GatewayServer");
        public string Data;
        public QueryGatewayHandler()
        {
            var config = ConfigManager.Config;
            var urlData = config.DownloadUrl;

            // build gateway proto
            var gateServer = new GateServer() {
                RegionName = config.GameServer.GameServerId,
                Ip = config.GameServer.PublicAddress,
                Port = config.GameServer.PublicPort,
                Msg = "Access verification failed. Please check if you have logged in to the correct account and server.",
                B1 = true,
                B2 = true,
                B3 = true,
                B4 = true,
                B5 = true,
            };

            if (urlData.AssetBundleUrl != null)
            {
                gateServer.AssetBundleUrl = urlData.AssetBundleUrl;
            }

            if (urlData.ExResourceUrl != null)
            {
                gateServer.ExResourceUrl = urlData.ExResourceUrl;
            }

            if (urlData.LuaUrl != null)
            {
                gateServer.LuaUrl = urlData.LuaUrl;
                gateServer.MdkResVersion = urlData.LuaUrl.Split('/')[^1].Split('_')[1];
            }
            
            if (urlData.IfixUrl != null)
            {
                gateServer.IfixUrl = urlData.IfixUrl;
                gateServer.IfixVersion = urlData.IfixUrl.Split('/')[^1].Split('_')[1];
            }
            Logger.Info("Client request: query_gateway");

            Data = Convert.ToBase64String(gateServer.ToByteArray());
        }
    }
}
