using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.WebServer.Response;
using Org.BouncyCastle.Crypto.Parameters;
using System.Numerics;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Database.Mission;
using EggLink.DanhengServer.Enums;
using Spectre.Console;
using EggLink.DanhengServer.Database.Lineup;
using EggLink.DanhengServer.Data;

namespace EggLink.DanhengServer.WebServer.Server
{
    public static class MuipManager
    {
        public delegate void ExecuteCommandDelegate(string message, MuipCommandSender sender);
        public static event ExecuteCommandDelegate? OnExecuteCommand;
        public delegate void ServerInformationDelegate(Dictionary<int, PlayerData> resultData);
        public static event ServerInformationDelegate? OnGetServerInformation;
        public delegate void GetPlayerStatusDelegate(int uid, out PlayerStatusEnum status, out PlayerSubStatusEnum subStatus);
        public static event GetPlayerStatusDelegate? OnGetPlayerStatus;

        public static string RsaPublicKey { get; private set; } = "";
        public static string RsaPrivateKey { get; private set; } = "";

        public static Dictionary<string, MuipSession> Sessions { get; } = [];

        public static AuthAdminKeyData? AuthAdminAndCreateSession(string key, string key_type)
        {
            if (ConfigManager.Config.MuipServer.AdminKey == "" || ConfigManager.Config.MuipServer.AdminKey != key)
            {
                return null;
            }

            var session = new MuipSession()
            {
                SessionId = Guid.NewGuid().ToString(),
                RsaPublicKey = GetRsaKeyPair().Item1,
                ExpireTimeStamp = DateTime.Now.AddMinutes(15).ToUnixSec(),
                IsAdmin = true,
            };

            if (key_type == "PEM")
            {
                // convert to PEM
                session.RsaPublicKey = XMLToPEM_Pub(session.RsaPublicKey);
            }

            Sessions.Add(session.SessionId, session);

            var data = new AuthAdminKeyData
            {
                RsaPublicKey = session.RsaPublicKey,
                SessionId = session.SessionId,
                ExpireTimeStamp = session.ExpireTimeStamp,
            };

            return data;
        }

        public static MuipSession? GetSession(string sessionId)
        {
            if (Sessions.TryGetValue(sessionId, out MuipSession? value))
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
            if (Sessions.TryGetValue(sessionId, out MuipSession? value))
            {
                var session = value;
                if (session.ExpireTimeStamp < DateTime.Now.ToUnixSec())
                {
                    Sessions.Remove(sessionId);
                    return new(1, "Session has expired!");
                }

                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(GetRsaKeyPair().Item2);
                byte[] decrypted;

                try
                {
                    decrypted = rsa.Decrypt(Convert.FromBase64String(command), RSAEncryptionPadding.Pkcs1);
                } catch
                {
                    return new(3, "Wrong encrypted key");
                }

                var commandStr = Encoding.UTF8.GetString(decrypted);
                var returnStr = "";

                var sync = Task.Run(() => OnExecuteCommand?.Invoke(commandStr, new MuipCommandSender(session, (msg) =>
                {
                    returnStr += msg + "\r\n";
                })
                {
                    SenderUid = targetUid,
                }));

                sync.Wait();

                return new(0, "Success", new()
                {
                    SessionId = sessionId,
                    Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(returnStr)),
                });
            }
            return new(2, "Session not found!");
        }

        public static ServerInformationResponse GetInformation(string sessionId)
        {
            if (Sessions.TryGetValue(sessionId, out MuipSession? value))
            {
                var session = value;
                if (session.ExpireTimeStamp < DateTime.Now.ToUnixSec())
                {
                    Sessions.Remove(sessionId);
                    return new(1, "Session has expired!", null);
                }
                Process currentProcess = Process.GetCurrentProcess();

                long currentProcessMemory = currentProcess.WorkingSet64;

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

                return new(0, "Success", new()
                {
                    ServerTime = DateTime.Now.ToUnixSec(),
                    MaxMemory = totalMemory,
                    ProgramUsedMemory = currentProcessMemory / 1024 / 1024,
                    UsedMemory = totalMemory - availableMemory,
                    OnlinePlayers = result.Values.Select(x => new SimplePlayerInformationData()
                    {
                        Name = x.Name ?? "",
                        HeadIconId = x.HeadIcon,
                        Uid = x.Uid
                    }).ToList(),
                });
            }
            return new(2, "Session not found!", null);
        }

        public static PlayerInformationResponse GetPlayerInformation(string sessionId, int uid)
        {
            if (Sessions.TryGetValue(sessionId, out MuipSession? value))
            {
                var session = value;
                if (session.ExpireTimeStamp < DateTime.Now.ToUnixSec())
                {
                    Sessions.Remove(sessionId);
                    return new(1, "Session has expired!", null);
                }

                var result = new Dictionary<int, PlayerData>();
                var player = DatabaseHelper.Instance?.GetInstance<PlayerData>(uid);
                if (player == null) return new(2, "Player not exist!", null);

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
                {
                    foreach (var avatar in lineup.BaseAvatars ?? [])
                    {
                        GameData.AvatarConfigData.TryGetValue(avatar.BaseAvatarId, out var excel);
                        if (excel != null)
                        {
                            curLineupAvatars.Add(avatar.BaseAvatarId);
                        }
                    }
                }


                return new(0, "Success", new()
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
                    AcceptedSubMissionIdList = missionData.RunningSubMissionIds,
                    AcceptedMainMissionIdList = missionData.RunningMainMissionIds,
                    FinishedMainMissionIdList = missionData.FinishedMainMissionIds,
                    FinishedSubMissionIdList = missionData.FinishedSubMissionIds,
                    PlayerStatus = status,
                    PlayerSubStatus = subStatus,
                    Credit = player.Scoin,
                    Jade = player.Hcoin,
                    LineupBaseAvatarIdList = curLineupAvatars
                });
            }
            return new(3, "Session not found!", null);
        }

        #region Tools

        /// <summary>
        /// get rsa key pair
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
            RsaKeyParameters key = new RsaKeyParameters(false, new Org.BouncyCastle.Math.BigInteger(1, p.Modulus), new Org.BouncyCastle.Math.BigInteger(1, p.Exponent));
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            string publicKey = Convert.ToBase64String(serializedPublicBytes);
            return Format(publicKey, true);
        }


        private static string Format(string key, bool type)
        {
            string result = string.Empty;

            int length = key.Length / 64;
            for (int i = 0; i < length; i++)
            {
                int start = i * 64;
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
                var searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
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
            string[] lines = File.ReadAllLines("/proc/meminfo");
            foreach (string line in lines)
            {
                if (line.StartsWith("MemTotal"))
                {
                    return float.Parse(line.Split(':')[1].Trim().Split(' ')[0]) / 1024;
                }
            }
            return 0;
        }

        public static float GetAvailableMemoryLinux()
        {
            string[] lines = File.ReadAllLines("/proc/meminfo");
            foreach (string line in lines)
            {
                if (line.StartsWith("MemAvailable"))
                {
                    return float.Parse(line.Split(':')[1].Trim().Split(' ')[0]) / 1024;
                }
            }
            return 0;
        }

        #endregion
    }
}
