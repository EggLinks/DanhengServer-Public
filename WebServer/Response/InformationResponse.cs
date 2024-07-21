using EggLink.DanhengServer.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using EggLink.DanhengServer.WebServer.Server;


namespace EggLink.DanhengServer.WebServer.Response
{
    public class ServerInformationResponse(int code, string message, ServerInformationData? data = null) : BaseResponse<ServerInformationData>(code, message, data)
    {
    }

    public class ServerInformationData
    {
        public List<SimplePlayerInformationData> OnlinePlayers { get; set; } = [];
        public long ServerTime { get; set; } = 0;
        public float MaxMemory { get; set; } = 0;
        public float UsedMemory { get; set; } = 0;
        public float ProgramUsedMemory { get; set; } = 0;
        public float CpuUsage { get; set; } = MuipManager.GetCpuUsage();
        public string CpuModel { get; set; } = MuipManager.GetCpuDetails().ModelName;
        public int CpuCores { get; set; } = MuipManager.GetCpuDetails().Cores;
        public float CpuFrequency { get; set; } = MuipManager.GetCpuDetails().Frequency;
        public string SystemVersion { get; set; } = MuipManager.GetSystemVersion();
    }

    public class SimplePlayerInformationData
    {
        public int Uid { get; set; }
        public string Name { get; set; } = "";
        public int HeadIconId { get; set; }
    }

    public class PlayerInformationResponse(int code, string message, PlayerInformationData? data = null) : BaseResponse<PlayerInformationData>(code, message, data)
    {
    }

    public class PlayerInformationData
    {
        // Basic info
        public int Uid { get; set; }
        public string Name { get; set; } = "";
        public string Signature { get; set; } = "";
        public int HeadIconId { get; set; }
        public int Credit { get; set; }
        public int Jade { get; set; }

        // Scene info
        public int CurPlaneId { get; set; }
        public int CurFloorId { get; set; }

        // Player info
        [JsonConverter(typeof(StringEnumConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public PlayerStatusEnum PlayerStatus { get; set; } = PlayerStatusEnum.Explore;

        [JsonConverter(typeof(StringEnumConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public PlayerSubStatusEnum PlayerSubStatus { get; set; } = PlayerSubStatusEnum.None;
        public int Stamina { get; set; } = 0;
        public int RecoveryStamina { get; set; } = 0;
        public List<int> AssistAvatarList { get; set; } = [];
        public List<int> DisplayAvatarList { get; set; } = [];

        // Mission info
        public List<int> FinishedMainMissionIdList { get; set; } = [];
        public List<int> FinishedSubMissionIdList { get; set; } = [];
        public List<int> AcceptedMainMissionIdList { get; set; } = [];
        public List<int> AcceptedSubMissionIdList { get; set; } = [];

        // Lineup info
        public List<int> LineupBaseAvatarIdList { get; set; } = [];

    }
}