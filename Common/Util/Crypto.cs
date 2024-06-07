using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Util
{
    public class Crypto
    {
        private static Random secureRandom = new();
        public static Logger logger = new("Crypto");
        public static void Xor(byte[] packet, byte[] key)
        {
            try
            {
                for (int i = 0; i < packet.Length; i++)
                {
                    packet[i] ^= key[i % key.Length];
                }
            }
            catch (Exception e)
            {
                logger.Error("Crypto error.", e);
            }
        }

        // Simple way to create a unique session key
        public static string CreateSessionKey(string accountUid)
        {
            byte[] random = new byte[64];
            secureRandom.NextBytes(random);

            string temp = accountUid + "." + DateTime.Now.Ticks + "." + secureRandom.ToString();

            try
            {
                byte[] bytes = SHA512.HashData(Encoding.UTF8.GetBytes(temp));
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                byte[] bytes = SHA512.HashData(Encoding.UTF8.GetBytes(temp));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
