using EggLink.DanhengServer.Configuration;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.WebServer.Handler;
using EggLink.DanhengServer.WebServer.Objects;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.WebServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class DispatchRoutes
    {
        public static ConfigContainer config = ConfigManager.Config;
        public static Logger Logger = new("DispatchServer");

        [HttpGet("query_dispatch")]
        public string QueryDispatch()
        {
            var data = new Dispatch();
            data.RegionList.Add(new RegionInfo()
            {
                Name = config.GameServer.GameServerId,
                DispatchUrl = $"{config.HttpServer.GetDisplayAddress()}/query_gateway",
                EnvType = "2",
                DisplayName = config.GameServer.GameServerName
            });
            Logger.Info("Client request: query_dispatch");
            return Convert.ToBase64String(data.ToByteArray());
        }

        [HttpPost("/account/risky/api/check")]
        public ContentResult RiskyCheck() => new() { Content = "{\"retcode\":0,\"message\":\"OK\",\"data\":{\"id\":\"none\",\"action\":\"ACTION_NONE\",\"geetest\":null}}", ContentType = "application/json" };

        // === AUTHENTICATION ===
        [HttpPost("/hkrpg_global/mdk/shield/api/login")]
        public JsonResult Login([FromBody] LoginReqJson req) => new UsernameLoginHandler().Handle(req.account!, req.password!, req.is_crypto);
        [HttpPost("/hkrpg_global/mdk/shield/api/verify")]
        public JsonResult Verify([FromBody] VerifyReqJson req) => new TokenLoginHandler().Handle(req.uid!, req.token!);
        [HttpPost("/hkrpg_global/combo/granter/login/v2/login")]
        public JsonResult LoginV2([FromBody] LoginV2ReqJson req) => new ComboTokenGranterHandler().Handle(req.app_id, req.channel_id, req.data!, req.device!, req.sign!);

        [HttpGet("/hkrpg_global/combo/granter/api/getConfig")]
        public ContentResult GetConfig() => new() { Content = "{\"retcode\":0,\"message\":\"OK\",\"data\":{\"protocol\":true,\"qr_enabled\":false,\"log_level\":\"INFO\",\"announce_url\":\"\",\"push_alias_type\":0,\"disable_ysdk_guard\":true,\"enable_announce_pic_popup\":false,\"app_name\":\"崩�??RPG\",\"qr_enabled_apps\":{\"bbs\":false,\"cloud\":false},\"qr_app_icons\":{\"app\":\"\",\"bbs\":\"\",\"cloud\":\"\"},\"qr_cloud_display_name\":\"\",\"enable_user_center\":true,\"functional_switch_configs\":{}}}", ContentType = "application/json" };
        

        [HttpGet("/hkrpg_global/mdk/shield/api/loadConfig")]
        public ContentResult LoadConfig() => new() { Content = "{\"retcode\":0,\"message\":\"OK\",\"data\":{\"id\":24,\"game_key\":\"hkrpg_global\",\"client\":\"PC\",\"identity\":\"I_IDENTITY\",\"guest\":false,\"ignore_versions\":\"\",\"scene\":\"S_NORMAL\",\"name\":\"崩�??RPG\",\"disable_regist\":false,\"enable_email_captcha\":false,\"thirdparty\":[\"fb\",\"tw\",\"gl\",\"ap\"],\"disable_mmt\":false,\"server_guest\":false,\"thirdparty_ignore\":{},\"enable_ps_bind_account\":false,\"thirdparty_login_configs\":{\"tw\":{\"token_type\":\"TK_GAME_TOKEN\",\"game_token_expires_in\":2592000},\"ap\":{\"token_type\":\"TK_GAME_TOKEN\",\"game_token_expires_in\":604800},\"fb\":{\"token_type\":\"TK_GAME_TOKEN\",\"game_token_expires_in\":2592000},\"gl\":{\"token_type\":\"TK_GAME_TOKEN\",\"game_token_expires_in\":604800}},\"initialize_firebase\":false,\"bbs_auth_login\":false,\"bbs_auth_login_ignore\":[],\"fetch_instance_id\":false,\"enable_flash_login\":false}}", ContentType = "application/json" };


        // === EXTRA ===

        [HttpPost("/hkrpg_global/combo/granter/api/compareProtocolVersion")]
        public ContentResult CompareProtocolVer() => new() { Content = "{\"retcode\":0,\"message\":\"OK\",\"data\":{\"modified\":false,\"protocol\":null}}", ContentType = "application/json" };
        [HttpGet("/hkrpg_global/mdk/agreement/api/getAgreementInfos")]
        public ContentResult GetAgreementInfo() => new() { Content = "{\"retcode\":0,\"message\":\"OK\",\"data\":{\"marketing_agreements\":[]}}", ContentType = "application/json" };

        [HttpGet("/combo/box/api/config/sdk/combo")]
        public ContentResult Combo() => new() { Content = "{\"retcode\":0,\"message\":\"OK\",\"data\":{\"vals\":{\"kibana_pc_config\":\"{ \\\"enable\\\": 0, \\\"level\\\": \\\"Info\\\",\\\"modules\\\": [\\\"download\\\"] }\\n\",\"network_report_config\":\"{ \\\"enable\\\": 0, \\\"status_codes\\\": [206], \\\"url_paths\\\": [\\\"dataUpload\\\", \\\"red_dot\\\"] }\\n\",\"list_price_tierv2_enable\":\"false\\n\",\"pay_payco_centered_host\":\"bill.payco.com\",\"telemetry_config\":\"{\\n \\\"dataupload_enable\\\": 0,\\n}\",\"enable_web_dpi\":\"true\"}}}", ContentType = "application/json" };
        [HttpGet("/combo/box/api/config/sw/precache")]
        public ContentResult Precache() => new() { Content = "{\"retcode\":0,\"message\":\"OK\",\"data\":{\"vals\":{\"url\":\"\",\"enable\":\"false\"}}}", ContentType = "application/json" };

        [HttpGet("/device-fp/api/getFp")]
        public JsonResult GetFp([FromQuery] string device_fp) => new FingerprintHandler().GetFp(device_fp);
        [HttpGet("/device-fp/api/getExtList")]
        public ContentResult GetExtList() => new() { Content = "{\"retcode\":0,\"message\":\"OK\",\"data\":{\"code\":200,\"msg\":\"ok\",\"ext_list\":[],\"pkg_list\":[],\"pkg_str\":\"/vK5WTh5SS3SAj8Zm0qPWg==\"}}", ContentType = "application/json" };

        [HttpPost("/data_abtest_api/config/experiment/list")]
        public ContentResult GetExperimentList() => new() { Content = "{\"retcode\":0,\"success\":true,\"message\":\"\",\"data\":[{\"code\":1000,\"type\":2,\"config_id\":\"14\",\"period_id\":\"6125_197\",\"version\":\"1\",\"configs\":{\"cardType\":\"direct\"}}]}", ContentType = "application/json" };
    }
}
