namespace EggLink.DanhengServer.WebServer.Objects;

public class ComboTokenResJson
{
    public string? message { get; set; }
    public int retcode { get; set; }
    public LoginData? data { get; set; } = null;

    public class LoginData
    {
        public LoginData(string openId, string comboToken)
        {
            open_id = openId;
            combo_token = comboToken;
        }

        public int account_type { get; set; } = 1;
        public bool heartbeat { get; set; }
        public string? combo_id { get; set; }
        public string? combo_token { get; set; }
        public string? open_id { get; set; }
        public string data { get; set; } = "{\"guest\":false}";
        public string? fatigue_remind { get; set; } = null; // ?
    }
}