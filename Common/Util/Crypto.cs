using System.Security.Cryptography;
using System.Text;

namespace EggLink.DanhengServer.Util;

public class Crypto
{
    private static readonly Random secureRandom = new();
    public static Logger logger = new("Crypto");

    public static void Xor(byte[] packet, byte[] key)
    {
        try
        {
            for (var i = 0; i < packet.Length; i++) packet[i] ^= key[i % key.Length];
        }
        catch (Exception e)
        {
            logger.Error("Crypto error.", e);
        }
    }

    // Simple way to create a unique session key
    public static string CreateSessionKey(string accountUid)
    {
        var random = new byte[64];
        secureRandom.NextBytes(random);

        var temp = accountUid + "." + DateTime.Now.Ticks + "." + secureRandom;

        try
        {
            var bytes = SHA512.HashData(Encoding.UTF8.GetBytes(temp));
            return Convert.ToBase64String(bytes);
        }
        catch
        {
            var bytes = SHA512.HashData(Encoding.UTF8.GetBytes(temp));
            return Convert.ToBase64String(bytes);
        }
    }
}