using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.WebServer.Response;
using Org.BouncyCastle.Crypto.Parameters;
using System.Numerics;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using System.Text;

namespace EggLink.DanhengServer.WebServer.Server
{
    public static class MuipManager
    {
        public delegate void ExecuteCommandDelegate(string message, MuipCommandSender sender);
        public static event ExecuteCommandDelegate? OnExecuteCommand;

        public static string RsaPublicKey { get; private set; } = "";
        public static string RsaPrivateKey { get; private set; } = "";

        public static Dictionary<string, MuipSession> Sessions { get; } = [];

        public static AuthAdminKeyData? AuthAdminAndCreateSession(string key, string key_type)
        {
            if (ConfigManager.Config.MuipServer.AdminKey != key)
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
    }
}
