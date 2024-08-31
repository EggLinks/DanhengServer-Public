using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using Google.Protobuf;

namespace EggLink.DanhengServer.WebServer.Handler;

internal class QueryGatewayHandler
{
    public static Logger Logger = new("GatewayServer");
    public string Data;

    public QueryGatewayHandler()
    {
        var config = ConfigManager.Config;
        var urlData = config.DownloadUrl;

        // build gateway proto
        var gateServer = new GateServer
        {
            RegionName = config.GameServer.GameServerId,
            Ip = config.GameServer.PublicAddress,
            Port = config.GameServer.Port,
            Msg = I18NManager.Translate("Server.Web.Maintain"),
            Unk1 = true,
            Unk2 = true,
            Unk3 = true,
            Unk4 = true,
            Unk5 = true,
            Unk6 = true,
            Unk7 = true
        };

        if (!ResourceManager.IsLoaded)
        {
            gateServer.Retcode = 2;
        }

        if (urlData.AssetBundleUrl != null && urlData.AssetBundleUrl.Length > 0)
            gateServer.AssetBundleUrl = urlData.AssetBundleUrl;

        if (urlData.ExResourceUrl != null && urlData.ExResourceUrl.Length > 0)
            gateServer.ExResourceUrl = urlData.ExResourceUrl;

        if (urlData.LuaUrl != null && urlData.LuaUrl.Length > 0)
        {
            gateServer.LuaUrl = urlData.LuaUrl;
            gateServer.MdkResVersion = urlData.LuaUrl.Split('/')[^1].Split('_')[1];
        }

        if (urlData.IfixUrl != null && urlData.IfixUrl.Length > 0)
        {
            gateServer.IfixUrl = urlData.IfixUrl;
            gateServer.IfixVersion = urlData.IfixUrl.Split('/')[^1].Split('_')[1];
        }

        Logger.Info("Client request: query_gateway");

        Data = Convert.ToBase64String(gateServer.ToByteArray());
    }
}