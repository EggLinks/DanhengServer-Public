using EggLink.DanhengServer.Util;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Account;

[SugarTable("Account")]
public class AccountData : BaseDatabaseDataHelper
{
    public string? Username { get; set; }

    [SugarColumn(IsNullable = true)] public string? ComboToken { get; set; }

    [SugarColumn(IsNullable = true)] public string? DispatchToken { get; set; }

    [SugarColumn(IsNullable = true)]
    public string? Permissions { get; set; } // type: permission1,permission2,permission3...

    public static AccountData? GetAccountByUserName(string username)
    {
        AccountData? result = null;
        DatabaseHelper.GetAllInstance<AccountData>()?.ForEach(account =>
        {
            if (account.Username == username) result = account;
        });
        return result;
    }

    public static AccountData? GetAccountByUid(int uid)
    {
        var result = DatabaseHelper.Instance?.GetInstance<AccountData>(uid);
        return result;
    }

    public string GenerateDispatchToken()
    {
        DispatchToken = Crypto.CreateSessionKey(Uid.ToString());
        return DispatchToken;
    }

    public string GenerateComboToken()
    {
        ComboToken = Crypto.CreateSessionKey(Uid.ToString());
        return ComboToken;
    }
}