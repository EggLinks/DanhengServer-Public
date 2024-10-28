using EggLink.DanhengServer.Proto;
using Google.Protobuf;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Player;

[SugarTable("server_prefs_data")]
public class ServerPrefsData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)] public Dictionary<int, ServerPrefsInfo> ServerPrefsDict { get; set; } = [];

    public double Version { get; set; } = 2.4;

    public void SetData(int prefsId, string b64Data)
    {
        ServerPrefsDict[prefsId] = new ServerPrefsInfo
        {
            ServerPrefsId = prefsId,
            Data = b64Data
        };
    }
}

public class ServerPrefsInfo
{
    public int ServerPrefsId { get; set; }
    public string Data { get; set; } = "";

    public ServerPrefs ToProto()
    {
        return new ServerPrefs
        {
            Data = ByteString.FromBase64(Data),
            ServerPrefsId = (uint)ServerPrefsId
        };
    }
}