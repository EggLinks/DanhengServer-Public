namespace EggLink.DanhengServer.Data.Custom;

public class VideoKeysConfig
{
    public List<ActivityVideoKeyInfoList> ActivityVideoKeyData { get; set; } = new();
    public List<VideoKeyInfoList> VideoKeyInfoData { get; set; } = new();
    public int TotalCount => ActivityVideoKeyData.Count + VideoKeyInfoData.Count;
}

public class ActivityVideoKeyInfoList
{
    public int Id { get; set; }
    public ulong VideoKey { get; set; }
}

public class VideoKeyInfoList
{
    public int Id { get; set; }
    public ulong VideoKey { get; set; }
}