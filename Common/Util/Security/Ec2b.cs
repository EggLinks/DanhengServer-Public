using System.Buffers.Binary;

namespace EggLink.DanhengServer.Util.Security;

public class Ec2b
{
    private const int HEAD_MAGIC = 1647469381; // "Ec2b"
    private const int KEY_SIZE = 16;
    private const int DATA_SIZE = 2048;
    private readonly byte[] Data;

    private readonly byte[] Key;

    private ulong Seed;
    private byte[] XorKey;

#pragma warning disable CS8618 // CS8618 - Non-nullable variable must contain a non-null value when exiting constructor.
    private Ec2b(byte[] key, byte[] data)
#pragma warning restore CS8618 // CS8618 - Non-nullable variable must contain a non-null value when exiting constructor.
    {
        Key = key;
        Data = data;
        var scrambledKey = new byte[KEY_SIZE];
        Array.Copy(Key, scrambledKey, KEY_SIZE);
        KeyScramble(scrambledKey);
        GenerateDecryptionVector(scrambledKey, Data);
    }

    public static Ec2b Read(byte[] input)
    {
        if (input == null || input.Length < 2076) throw new Exception("Input data is too short.");

        var offset = 0;

        var magic = BinaryPrimitives.ReadInt32LittleEndian(input.AsSpan(offset, 4));
        if (magic != HEAD_MAGIC) throw new Exception($"Magic mismatch, expected: {HEAD_MAGIC}, got: {magic}");
        offset += 4;

        var keySize = BinaryPrimitives.ReadInt32LittleEndian(input.AsSpan(offset, 4));
        offset += 4;
        if (keySize != KEY_SIZE) throw new Exception($"Invalid key size, expected: {KEY_SIZE}, got: {keySize}");

        if (input.Length < offset + KEY_SIZE) throw new Exception("Input data is too short for key.");
        var key = new byte[KEY_SIZE];
        Array.Copy(input, offset, key, 0, KEY_SIZE);
        offset += KEY_SIZE;

        if (input.Length < offset + 4) throw new Exception("Input data is too short for data size.");
        var dataSize = BinaryPrimitives.ReadInt32LittleEndian(input.AsSpan(offset, 4));
        offset += 4;
        if (dataSize != DATA_SIZE) throw new Exception($"Invalid data size, expected: {DATA_SIZE}, got: {dataSize}");

        if (input.Length < offset + DATA_SIZE) throw new Exception("Input data is too short for data.");
        var data = new byte[DATA_SIZE];
        Array.Copy(input, offset, data, 0, DATA_SIZE);

        return new Ec2b(key, data);
    }

    public static Ec2b GenerateEc2b()
    {
        var key = new byte[KEY_SIZE];
        var data = new byte[DATA_SIZE];

        var random = new MT19937((ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        for (var i = 0; i < KEY_SIZE; i++)
            key[i] = (byte)(random.NextUInt64() % 256);

        for (var i = 0; i < DATA_SIZE; i++)
            data[i] = (byte)(random.NextUInt64() % 256);

        return new Ec2b(key, data);
    }

    public byte[] GetBytes()
    {
        var output = new byte[4 + 4 + KEY_SIZE + 4 + DATA_SIZE];
        var offset = 0;

        BinaryPrimitives.WriteInt32LittleEndian(output.AsSpan(offset, 4), HEAD_MAGIC);
        offset += 4;

        BinaryPrimitives.WriteInt32LittleEndian(output.AsSpan(offset, 4), KEY_SIZE);
        offset += 4;

        Array.Copy(Key, 0, output, offset, KEY_SIZE);
        offset += KEY_SIZE;

        BinaryPrimitives.WriteInt32LittleEndian(output.AsSpan(offset, 4), DATA_SIZE);
        offset += 4;

        Array.Copy(Data, 0, output, offset, DATA_SIZE);

        return output;
    }

    public ulong GetSeed()
    {
        return Seed;
    }

    public byte[] GetXorKey()
    {
        return XorKey;
    }

    private void GenerateDecryptionVector(byte[] key, byte[] crypt)
    {
        var output = new List<byte>(4096);

        var val = 18446744073709551615;
        for (var i = 0; i < crypt.Length; i += 8)
        {
            var chunk = BitConverter.ToUInt64(crypt, i);
            val ^= chunk;
        }

        var key_qword_0 = BitConverter.ToUInt64(key, 0);
        var key_qword_1 = BitConverter.ToUInt64(key, 8);

        Seed = key_qword_1 ^ 0b1100111010101100001110110101101010000110011110000011011110101100 ^ val ^ key_qword_0;

        var mt = new MT19937(Seed);
        for (var i = 0; i < 512; i++)
        {
            var next = mt.NextUInt64();
            var bytes = BitConverter.GetBytes(next);
            output.AddRange(bytes);
        }

        XorKey = output.ToArray();
    }

    private static void KeyScramble(byte[] key)
    {
        var roundKeys = new byte[176];
        for (var round = 0; round < 11; round++)
        for (var i = 0; i < 16; i++)
        for (var j = 0; j < 16; j++)
        {
            var idx = (round << 8) + i * 16 + j;
            roundKeys[round * 16 + i] ^= (byte)(Magic.aesXorTable[1, idx] ^ Magic.aesXorTable[0, idx]);
        }

        var chip = new byte[16];
        AES128.RevAes128Enc(key, roundKeys, chip);

        for (var i = 0; i < KEY_SIZE; i++) chip[i] ^= Magic.keyXorTable[i];

        Array.Copy(chip, key, KEY_SIZE);
    }
}