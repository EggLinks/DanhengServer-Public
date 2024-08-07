namespace EggLink.DanhengServer.WebServer.Objects;

public class NewLoginResJson
{
    public NewLoginResJson()
    {
    }

    public NewLoginResJson(string aid, string email, string token)
    {
        data = new VerifyData(aid, email, token);
    }

    public string? message { get; set; }
    public int retcode { get; set; }
    public VerifyData? data { get; set; }

    public class VerifyData
    {
        public VerifyData(string aid, string email, string token)
        {
            user_info.aid = aid;
            user_info.email = email;
            this.token.token = token;
        }

        public UserInfoData user_info { get; set; } = new();
        public TokenData token { get; set; } = new();
    }

    public class UserInfoData
    {
        public string account_name { get; set; } = "";
        public string aid { get; set; } = "";
        public string area_code { get; set; } = "";
        public string country { get; set; } = "";
        public string email { get; set; } = "";
        public int is_email_verify { get; set; } = 0;
        public string identity_code { get; set; } = "";

        public List<LinkData> links { get; set; } = new()
        {
            new LinkData()
        };

        public string mid { get; set; } = "";
        public string mobile { get; set; } = "";
        public string realname { get; set; } = "";
        public string rebind_area_code { get; set; } = "";
        public string rebind_mobile { get; set; } = "";
        public string rebind_mobile_time { get; set; } = "";
        public string safe_area_code { get; set; } = "";
        public string safe_mobile { get; set; } = "";
    }

    public class TokenData
    {
        public string token { get; set; } = "";
        public int token_type { get; set; } = 1;
    }

    public class LinkData
    {
        public string nickname { get; set; } = "";
        public string thirdparty { get; set; } = "";
        public string union_id { get; set; } = "";
    }
}