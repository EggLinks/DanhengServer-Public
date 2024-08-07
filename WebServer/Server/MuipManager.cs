using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Database.Lineup;
using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Database.Quests;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.WebServer.Response;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;

namespace EggLink.DanhengServer.WebServer.Server;

public static class MuipManager
{
    public delegate void ExecuteCommandDelegate(string message, MuipCommandSender sender);

    public delegate void GetPlayerStatusDelegate(int uid, out PlayerStatusEnum status,
        out PlayerSubStatusEnum subStatus);

    public delegate void ServerInformationDelegate(Dictionary<int, PlayerData> resultData);

    private static readonly Logger logger = Logger.GetByClassName();

    public static string RsaPublicKey { get; private set; } = "";
    public static string RsaPrivateKey { get; private set; } = "";

    public static Dictionary<string, MuipSession> Sessions { get; } = [];
    public static event ExecuteCommandDelegate? OnExecuteCommand;
    public static event ServerInformationDelegate? OnGetServerInformation;
    public static event GetPlayerStatusDelegate? OnGetPlayerStatus;

    public static CreateSessionResponse CreateSession(string keyType)
    {
        if (ConfigManager.Config.MuipServer.AdminKey == "")
            return new CreateSessionResponse(1, "This function is not enabled in this server!", null);
        var session = new MuipSession
        {
            SessionId = Guid.NewGuid().ToString(),
            RsaPublicKey = GetRsaKeyPair().Item1,
            ExpireTimeStamp = DateTime.Now.AddMinutes(15).ToUnixSec(),
            IsAdmin = true
        };

        if (keyType == "PEM")
            // convert to PEM
            session.RsaPublicKey = XMLToPEM_Pub(session.RsaPublicKey);

        Sessions.Add(session.SessionId, session);

        var data = new CreateSessionData
        {
            RsaPublicKey = session.RsaPublicKey,
            SessionId = session.SessionId,
            ExpireTimeStamp = session.ExpireTimeStamp
        };

        return new CreateSessionResponse(0, "Created!", data);
    }

    public static AuthAdminKeyResponse AuthAdmin(string sessionId, string key)
    {
        if (Sessions.TryGetValue(sessionId, out var value))
        {
            var session = value;
            if (session.ExpireTimeStamp < DateTime.Now.ToUnixSec())
            {
                Sessions.Remove(sessionId);
                return new AuthAdminKeyResponse(1, "Session has expired!", null);
            }

            // decrypt key
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(GetRsaKeyPair().Item2); // private key
            byte[] decrypted;

            try
            {
                decrypted = rsa.Decrypt(Convert.FromBase64String(key), RSAEncryptionPadding.Pkcs1);
            }
            catch
            {
                return new AuthAdminKeyResponse(3, "Wrong encrypted key", null);
            }

            var keyStr = Encoding.UTF8.GetString(decrypted);
            if (keyStr != ConfigManager.Config.MuipServer.AdminKey)
                return new AuthAdminKeyResponse(2, "Admin key is invalid!", null);

            session.IsAuthorized = true;

            var data = new AuthAdminKeyData
            {
                SessionId = session.SessionId,
                ExpireTimeStamp = session.ExpireTimeStamp
            };

            return new AuthAdminKeyResponse(0, "Authorized admin key successfully!", data);
        }

        return new AuthAdminKeyResponse(4, "Session not found!", null);
    }

    public static MuipSession? GetSession(string sessionId)
    {
        if (Sessions.TryGetValue(sessionId, out var value))
        {
            var session = value;
            if (session.ExpireTimeStamp < DateTime.Now.ToUnixSec())
            {
                Sessions.Remove(sessionId);
                return null;
            }

            return session;
        }

        return null;
    }

    public static ExecuteCommandResponse ExecuteCommand(string sessionId, string command, int targetUid)
    {
        if (Sessions.TryGetValue(sessionId, out var value))
        {
            var session = value;
            if (session.ExpireTimeStamp < DateTime.Now.ToUnixSec())
            {
                Sessions.Remove(sessionId);
                return new ExecuteCommandResponse(1, "Session has expired!");
            }

            if (!session.IsAuthorized)
                return new ExecuteCommandResponse(4, "Not authorized!");

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(GetRsaKeyPair().Item2);
            byte[] decrypted;

            try
            {
                decrypted = rsa.Decrypt(Convert.FromBase64String(command), RSAEncryptionPadding.Pkcs1);
            }
            catch
            {
                return new ExecuteCommandResponse(3, "Wrong encrypted key");
            }

            var commandStr = Encoding.UTF8.GetString(decrypted);
            logger.Info($"SessionId: {sessionId}, UID: {targetUid}, ExecuteCommand: {commandStr}");
            var returnStr = "";

            var sync = Task.Run(() => OnExecuteCommand?.Invoke(commandStr,
                new MuipCommandSender(session, msg => { returnStr += msg + "\r\n"; })
                {
                    SenderUid = targetUid
                }));

            sync.Wait();

            return new ExecuteCommandResponse(0, "Success", new ExecuteCommandData
            {
                SessionId = sessionId,
                Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(returnStr))
            });
        }

        return new ExecuteCommandResponse(2, "Session not found!");
    }

    public static ServerInformationResponse GetInformation(string sessionId)
    {
        if (Sessions.TryGetValue(sessionId, out var value))
        {
            var session = value;
            if (session.ExpireTimeStamp < DateTime.Now.ToUnixSec())
            {
                Sessions.Remove(sessionId);
                return new ServerInformationResponse(1, "Session has expired!");
            }

            if (!session.IsAuthorized)
                return new ServerInformationResponse(3, "Not authorized!");

            var currentProcess = Process.GetCurrentProcess();

            var currentProcessMemory = currentProcess.WorkingSet64;

            // get system info
            var totalMemory = -1f;
            var availableMemory = -1f;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                totalMemory = GetTotalMemoryWindows();
                availableMemory = GetAvailableMemoryWindows();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                totalMemory = GetTotalMemoryLinux();
                availableMemory = GetAvailableMemoryLinux();
            }

            var result = new Dictionary<int, PlayerData>();
            var sync = Task.Run(() => OnGetServerInformation?.Invoke(result));

            sync.Wait();

            return new ServerInformationResponse(0, "Success", new ServerInformationData
            {
                ServerTime = DateTime.Now.ToUnixSec(),
                MaxMemory = totalMemory,
                ProgramUsedMemory = currentProcessMemory / 1024 / 1024,
                UsedMemory = totalMemory - availableMemory,
                OnlinePlayers = result.Values.Select(x => new SimplePlayerInformationData
                {
                    Name = x.Name ?? "",
                    HeadIconId = x.HeadIcon,
                    Uid = x.Uid
                }).ToList()
            });
        }

        return new ServerInformationResponse(2, "Session not found!");
    }

    public static PlayerInformationResponse GetPlayerInformation(string sessionId, int uid)
    {
        if (Sessions.TryGetValue(sessionId, out var value))
        {
            var session = value;
            if (session.ExpireTimeStamp < DateTime.Now.ToUnixSec())
            {
                Sessions.Remove(sessionId);
                return new PlayerInformationResponse(1, "Session has expired!");
            }

            if (!session.IsAuthorized)
                return new PlayerInformationResponse(4, "Not authorized!");

            var player = DatabaseHelper.Instance?.GetInstance<PlayerData>(uid);
            if (player == null) return new PlayerInformationResponse(2, "Player not exist!");

            var status = PlayerStatusEnum.Offline;
            var subStatus = PlayerSubStatusEnum.None;

            var statusSync = Task.Run(() => OnGetPlayerStatus?.Invoke(player.Uid, out status, out subStatus));

            statusSync.Wait();

            var avatarData = DatabaseHelper.Instance!.GetInstance<AvatarData>(player.Uid)!;
            var lineupData = DatabaseHelper.Instance!.GetInstance<LineupData>(player.Uid)!;
            var missionData = DatabaseHelper.Instance!.GetInstance<MissionData>(player.Uid)!;

            var curLineupAvatars = new List<int>();
            var index = lineupData.CurExtraLineup > 0 ? lineupData.CurExtraLineup : lineupData.CurLineup;

            lineupData.Lineups.TryGetValue(index, out var lineup);

            if (lineup != null)
                foreach (var avatar in lineup.BaseAvatars ?? [])
                {
                    GameData.AvatarConfigData.TryGetValue(avatar.BaseAvatarId, out var excel);
                    if (excel != null) curLineupAvatars.Add(avatar.BaseAvatarId);
                }

            Dictionary<int, List<int>> missionDict = [];
            foreach (var subId in missionData.RunningSubMissionIds)
            {
                var subMission = GameData.SubMissionData.GetValueOrDefault(subId);
                if (subMission == null) continue;

                if (missionDict.ContainsKey(subMission.MainMissionID))
                    missionDict[subMission.MainMissionID].Add(subMission.SubMissionID);
                else
                    missionDict[subMission.MainMissionID] = [subMission.SubMissionID];
            }

            return new PlayerInformationResponse(0, "Success", new PlayerInformationData
            {
                Uid = player.Uid,
                Name = player.Name ?? "",
                Signature = player.Signature ?? "",
                Stamina = player.Stamina,
                RecoveryStamina = (int)player.StaminaReserve,
                HeadIconId = player.HeadIcon,
                CurFloorId = player.FloorId,
                CurPlaneId = player.PlaneId,
                AssistAvatarList = avatarData.AssistAvatars,
                DisplayAvatarList = avatarData.DisplayAvatars,
                AcceptedMissionList = missionDict,
                FinishedMainMissionIdList = missionData.FinishedMainMissionIds,
                FinishedSubMissionIdList = missionData.FinishedSubMissionIds,
                PlayerStatus = status,
                PlayerSubStatus = subStatus,
                Credit = player.Scoin,
                Jade = player.Hcoin,
                LineupBaseAvatarIdList = curLineupAvatars
            });
        }

        return new PlayerInformationResponse(3, "Session not found!");
    }

    #region Tools

    /// <summary>
    ///     get rsa key pair
    /// </summary>
    /// <returns>item 1 is public key, item 2 is private key</returns>
    public static (string, string) GetRsaKeyPair()
    {
        if (string.IsNullOrEmpty(RsaPublicKey) || string.IsNullOrEmpty(RsaPrivateKey))
        {
            var rsa = new RSACryptoServiceProvider(2048);
            RsaPublicKey = rsa.ToXmlString(false);
            RsaPrivateKey = rsa.ToXmlString(true);
        }

        return (RsaPublicKey, RsaPrivateKey);
    }


    public static string XMLToPEM_Pub(string xmlpubkey)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(xmlpubkey);
        var p = rsa.ExportParameters(false);
        var key = new RsaKeyParameters(false, new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent));
        var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key);
        var serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
        var publicKey = Convert.ToBase64String(serializedPublicBytes);
        return Format(publicKey, true);
    }


    private static string Format(string key, bool type)
    {
        var result = string.Empty;

        var length = key.Length / 64;
        for (var i = 0; i < length; i++)
        {
            var start = i * 64;
            result = result + key.Substring(start, 64) + "\r\n";
        }

        result = result + key.Substring(length * 64);
        if (type)
        {
            result = result.Insert(0, "-----BEGIN PUBLIC KEY-----\r\n");
            result += "\r\n-----END PUBLIC KEY-----";
        }
        else
        {
            result = result.Insert(0, "-----BEGIN PRIVATE KEY-----\r\n");
            result += "\r\n-----END PRIVATE KEY-----";
        }

        return result;
    }

    public static float GetTotalMemoryWindows()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (var obj in searcher.Get())
            {
                var memory = Convert.ToUInt64(obj["TotalPhysicalMemory"]);
                return memory / 1024 / 1024;
            }
        }

        return 0;
    }

    public static float GetAvailableMemoryWindows()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var pc = new PerformanceCounter("Memory", "Available MBytes");
            return pc.NextValue();
        }

        return 0;
    }

    public static float GetTotalMemoryLinux()
    {
        var lines = File.ReadAllLines("/proc/meminfo");
        foreach (var line in lines)
            if (line.StartsWith("MemTotal"))
                return float.Parse(line.Split(':')[1].Trim().Split(' ')[0]) / 1024;
        return 0;
    }

    public static float GetAvailableMemoryLinux()
    {
        var lines = File.ReadAllLines("/proc/meminfo");
        foreach (var line in lines)
            if (line.StartsWith("MemAvailable"))
                return float.Parse(line.Split(':')[1].Trim().Split(' ')[0]) / 1024;
        return 0;
    }

    public static float GetCpuUsage()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetCpuUsageLinux();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return GetCpuUsageWindows();
        throw new NotSupportedException("Unsupported OS platform");
    }

    private static float GetCpuUsageLinux()
    {
        var lines = File.ReadAllLines("/proc/stat");
        var cpuInfo = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var idleTime = float.Parse(cpuInfo[4]);
        float totalTime = 0;

        for (var i = 1; i < cpuInfo.Length; i++) totalTime += float.Parse(cpuInfo[i]);

        return 100 * (1 - idleTime / totalTime);
    }

    private static float GetCpuUsageWindows()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("PerformanceCounter is only supported on Windows");

        var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        cpuCounter.NextValue();
        Thread.Sleep(1000);
        return cpuCounter.NextValue();
    }

    public static (string ModelName, int Cores, float Frequency) GetCpuDetails()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetCpuDetailsLinux();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return GetCpuDetailsWindows();
        throw new NotSupportedException("Unsupported OS platform");
    }

    private static (string ModelName, int Cores, float Frequency) GetCpuDetailsLinux()
    {
        var lines = File.ReadAllLines("/proc/cpuinfo");
        var modelName = "";
        var cores = 0;
        float frequency = 0;

        foreach (var line in lines)
        {
            if (line.StartsWith("model name")) modelName = line.Split(':')[1].Trim();
            if (line.StartsWith("cpu cores")) cores = int.Parse(line.Split(':')[1].Trim());
            if (line.StartsWith("cpu MHz")) frequency = float.Parse(line.Split(':')[1].Trim());
        }

        return (modelName, cores, frequency);
    }

    private static (string ModelName, int Cores, float Frequency) GetCpuDetailsWindows()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("ManagementObjectSearcher is only supported on Windows");

        var modelName = "";
        var cores = 0;
        float frequency = 0;

        var searcher = new ManagementObjectSearcher("select * from Win32_Processor");
        foreach (var item in searcher.Get())
        {
            modelName = item["Name"]?.ToString() ?? "Unknown";
            cores = int.Parse(item["NumberOfCores"]?.ToString() ?? "0");
            frequency = float.Parse(item["MaxClockSpeed"]?.ToString() ?? "0") / 1000; // MHz to GHz
        }

        return (modelName, cores, frequency);
    }

    public static string GetSystemVersion()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return GetWindowsVersion();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetLinuxVersion();
        throw new PlatformNotSupportedException("This method only supports Windows and Linux.");
    }

    private static string GetWindowsVersion()
    {
        return Environment.OSVersion.VersionString;
    }

    private static string GetLinuxVersion()
    {
        var version = string.Empty;
        if (File.Exists("/etc/os-release"))
        {
            var lines = File.ReadAllLines("/etc/os-release");
            foreach (var line in lines)
                if (line.StartsWith("PRETTY_NAME"))
                {
                    version = line.Split('=')[1].Trim('"');
                    break;
                }
        }
        else if (File.Exists("/proc/version"))
        {
            version = File.ReadAllText("/proc/version");
        }

        return version;
    }

    #endregion
}