using System.Security.Cryptography;
using System.Text;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Util.Security;

namespace EggLink.DanhengServer.Util;

public class Crypto
{
    private static readonly Random SecureRandom = new();
    public static Logger Logger = new("Crypto");

    public static Ec2b? ClientSecretKey { get; set; }

    public static void Xor(byte[] packet, byte[] key)
    {
        try
        {
            for (var i = 0; i < packet.Length; i++) packet[i] ^= key[i % key.Length];
        }
        catch (Exception e)
        {
            Logger.Error("Crypto error.", e);
        }
    }

    public static byte[] GenerateXorKey(ulong seed)
    {
        var key = new byte[4096];
        var random = new MT19937(seed);

        for (var i = 0; i < key.Length / 8; i++)
        {
            var value = random.NextUInt64();

            key[i * 8 + 0] = (byte)((value >> 56) & 0xFF);
            key[i * 8 + 1] = (byte)((value >> 48) & 0xFF);
            key[i * 8 + 2] = (byte)((value >> 40) & 0xFF);
            key[i * 8 + 3] = (byte)((value >> 32) & 0xFF);
            key[i * 8 + 4] = (byte)((value >> 24) & 0xFF);
            key[i * 8 + 5] = (byte)((value >> 16) & 0xFF);
            key[i * 8 + 6] = (byte)((value >> 8) & 0xFF);
            key[i * 8 + 7] = (byte)(value & 0xFF);
        }

        return key;
    }

    public static Ec2b? InitEc2b()
    {
        var filePath = ConfigManager.Config.Path.ConfigPath + "/ClientSecretKey.ec2b";
        try
        {
            byte[] ec2bData;
            if (!File.Exists(filePath))
            {
                var newEc2b = Ec2b.GenerateEc2b();
                ec2bData = newEc2b.GetBytes();
                File.WriteAllBytes(filePath, ec2bData);
                Logger.Info(I18NManager.Translate("Server.ServerInfo.NewClientSecretKey"));
                return newEc2b;
            }

            ec2bData = File.ReadAllBytes(filePath);
            var ec2b = Ec2b.Read(ec2bData);
            return ec2b;
        }
        catch (Exception ex)
        {
            Logger.Error($"An error occurred while loading the Client Secret Key：{ex}");
            return null;
        }
    }

    // Simple way to create a unique session key
    public static string CreateSessionKey(string accountUid)
    {
        var random = new byte[64];
        SecureRandom.NextBytes(random);

        var temp = accountUid + "." + DateTime.Now.Ticks + "." + SecureRandom;

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