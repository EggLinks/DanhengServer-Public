namespace EggLink.DanhengServer.WebServer.Objects;

public class LoginResJson
{
    public string? message { get; set; }
    public int retcode { get; set; }
    public VerifyData? data { get; set; }

    public class VerifyData
    {
        public VerifyData(string accountUid, string email, string token)
        {
            account.uid = accountUid;
            account.email = email;
            account.token = token;
        }

        public VerifyAccountData account { get; set; } = new();
        public bool device_grant_required { get; set; } = false;
        public string realname_operation { get; set; } = "NONE";
        public bool realperson_required { get; set; } = false;
        public bool safe_mobile_required { get; set; } = false;
    }

    public class VerifyAccountData
    {
        public string? uid { get; set; }
        public string name { get; set; } = "";
        public string email { get; set; } = "";
        public string mobile { get; set; } = "";
        public string is_email_verify { get; set; } = "0";
        public string realname { get; set; } = "";
        public string identity_card { get; set; } = "";
        public string? token { get; set; }
        public string? safe_mobile { get; set; } = "";
        public string facebook_name { get; set; } = "";
        public string twitter_name { get; set; } = "";
        public string game_center_name { get; set; } = "";
        public string google_name { get; set; } = "";
        public string apple_name { get; set; } = "";
        public string sony_name { get; set; } = "";
        public string tap_name { get; set; } = "";
        public string country { get; set; } = "US";
        public string reactivate_ticket { get; set; } = "";
        public string area_code { get; set; } = "**";
        public string device_grant_ticket { get; set; } = "";
    }
}