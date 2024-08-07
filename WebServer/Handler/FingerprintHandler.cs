using Microsoft.AspNetCore.Mvc;
using static EggLink.DanhengServer.WebServer.Handler.FingerprintResJson;

namespace EggLink.DanhengServer.WebServer.Handler;

public class FingerprintHandler
{
    public JsonResult GetFp(string device_fp)
    {
        FingerprintResJson res = new();
        if (device_fp == null)
        {
            res.retcode = -202;
            res.message = "Error";
        }
        else
        {
            res.message = "OK";
            res.data = new FingerprintDataJson(device_fp);
        }

        return new JsonResult(res);
    }
}

public class FingerprintResJson
{
    public string? message { get; set; }
    public int retcode { get; set; }
    public FingerprintDataJson? data { get; set; }

    public class FingerprintDataJson
    {
        public FingerprintDataJson(string fp)
        {
            code = 200;
            message = "OK";
            device_fp = fp;
        }

        public string device_fp { get; set; }
        public string message { get; set; }
        public int code { get; set; }
    }
}