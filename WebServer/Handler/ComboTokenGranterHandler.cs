using EggLink.DanhengServer.Database.Account;
using EggLink.DanhengServer.WebServer.Objects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.WebServer.Handler;

public class ComboTokenGranterHandler
{
    public JsonResult Handle(int app_id, int channel_id, string data, string device, string sign)
    {
        var tokenData = JsonConvert.DeserializeObject<LoginTokenData>(data);
        var res = new ComboTokenResJson();
        if (tokenData == null)
        {
            res.retcode = -202;
            res.message = "Invalid login data";
            return new JsonResult(res);
        }

        var account = AccountData.GetAccountByUid(int.Parse(tokenData.uid!));
        if (account == null)
        {
            res.retcode = -201;
            res.message = "Game account cache information error";
            return new JsonResult(res);
        }

        res.message = "OK";
        res.data = new ComboTokenResJson.LoginData(account.Uid.ToString(), account.GenerateComboToken());
        return new JsonResult(res);
    }
}

public class LoginTokenData
{
    public string? uid { get; set; }
    public string? token { get; set; }
    public bool guest { get; set; }
}