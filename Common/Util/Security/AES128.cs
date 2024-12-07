namespace EggLink.DanhengServer.Util.Security;

public class AES128
{
    private static void Xor(byte[] a, byte[] b, int n)
    {
        for (var i = 0; i < n; i++) a[i] ^= b[i];
    }

    private static void XorRoundKey(byte[] state, byte[] keys, int round)
    {
        Xor(state, keys.Skip(round * 16).Take(16).ToArray(), 16);
    }

    private static void SubBytes(byte[] a, int n)
    {
        for (var i = 0; i < n; i++) a[i] = Magic.LookupSbox[a[i]];
    }

    private static void SubBytesInv(byte[] a, int n)
    {
        for (var i = 0; i < n; i++) a[i] = Magic.LookupSboxInv[a[i]];
    }

    private static void KeyScheduleCore(byte[] a, int i)
    {
        var temp = a[0];
        a[0] = a[1];
        a[1] = a[2];
        a[2] = a[3];
        a[3] = temp;

        SubBytes(a, 4);

        a[0] ^= Magic.LookupRcon[i];
    }

    public static void Aes128LoadSchedule(byte[] key, out byte[] schedule)
    {
        schedule = new byte[16 * 11];
        var bytes = 16;
        var i = 1;
        var t = new byte[4];

        Array.Copy(key, schedule, 16);

        while (bytes < 176)
        {
            Array.Copy(schedule, bytes - 4, t, 0, 4);

            KeyScheduleCore(t, i);
            i++;

            for (var k = 0; k < 4; k++) t[k] ^= schedule[bytes - 16 + k];

            Array.Copy(t, 0, schedule, bytes, 4);
            bytes += 4;

            for (var j = 0; j < 3; j++)
            {
                Array.Copy(schedule, bytes - 4, t, 0, 4);

                for (var k = 0; k < 4; k++) t[k] ^= schedule[bytes - 16 + k];

                Array.Copy(t, 0, schedule, bytes, 4);
                bytes += 4;
            }
        }
    }

    private static void ShiftRows(byte[] state)
    {
        var temp = new byte[16];
        Array.Copy(state, temp, 16);

        for (var i = 0; i < 16; i++) state[i] = temp[Magic.ShiftRowsTable[i]];
    }

    private static void ShiftRowsInv(byte[] state)
    {
        var temp = new byte[16];
        Array.Copy(state, temp, 16);

        for (var i = 0; i < 16; i++) state[i] = temp[Magic.ShiftRowsTableInv[i]];
    }

    private static void MixCol(byte[] state, int offset)
    {
        var a0 = state[offset];
        var a1 = state[offset + 1];
        var a2 = state[offset + 2];
        var a3 = state[offset + 3];

        state[offset] = (byte)(Magic.LookupG2[a0] ^ Magic.LookupG3[a1] ^ a2 ^ a3);
        state[offset + 1] = (byte)(Magic.LookupG2[a1] ^ Magic.LookupG3[a2] ^ a3 ^ a0);
        state[offset + 2] = (byte)(Magic.LookupG2[a2] ^ Magic.LookupG3[a3] ^ a0 ^ a1);
        state[offset + 3] = (byte)(Magic.LookupG2[a3] ^ Magic.LookupG3[a0] ^ a1 ^ a2);
    }

    private static void MixCols(byte[] state)
    {
        MixCol(state, 0);
        MixCol(state, 4);
        MixCol(state, 8);
        MixCol(state, 12);
    }

    private static void MixColInv(byte[] state, int offset)
    {
        var a0 = state[offset];
        var a1 = state[offset + 1];
        var a2 = state[offset + 2];
        var a3 = state[offset + 3];

        state[offset] = (byte)(Magic.LookupG14[a0] ^ Magic.LookupG9[a3] ^ Magic.LookupG13[a2] ^ Magic.LookupG11[a1]);
        state[offset + 1] =
            (byte)(Magic.LookupG14[a1] ^ Magic.LookupG9[a0] ^ Magic.LookupG13[a3] ^ Magic.LookupG11[a2]);
        state[offset + 2] =
            (byte)(Magic.LookupG14[a2] ^ Magic.LookupG9[a1] ^ Magic.LookupG13[a0] ^ Magic.LookupG11[a3]);
        state[offset + 3] =
            (byte)(Magic.LookupG14[a3] ^ Magic.LookupG9[a2] ^ Magic.LookupG13[a1] ^ Magic.LookupG11[a0]);
    }

    private static void MixColsInv(byte[] state)
    {
        MixColInv(state, 0);
        MixColInv(state, 4);
        MixColInv(state, 8);
        MixColInv(state, 12);
    }

    public static void Aes128Enc(byte[] plaintext, byte[] schedule, byte[] ciphertext)
    {
        Array.Copy(plaintext, ciphertext, 16);
        XorRoundKey(ciphertext, schedule, 0);

        for (var i = 0; i < 9; i++)
        {
            SubBytes(ciphertext, 16);
            ShiftRows(ciphertext);
            MixCols(ciphertext);
            XorRoundKey(ciphertext, schedule, i + 1);
        }

        SubBytes(ciphertext, 16);
        ShiftRows(ciphertext);
        XorRoundKey(ciphertext, schedule, 10);
    }

    public static void RevAes128Enc(byte[] plaintext, byte[] schedule, byte[] ciphertext)
    {
        Array.Copy(plaintext, ciphertext, 16);
        XorRoundKey(ciphertext, schedule, 0);

        for (var i = 0; i < 9; i++)
        {
            SubBytesInv(ciphertext, 16);
            ShiftRowsInv(ciphertext);
            MixColsInv(ciphertext);
            XorRoundKey(ciphertext, schedule, i + 1);
        }

        SubBytesInv(ciphertext, 16);
        ShiftRowsInv(ciphertext);
        XorRoundKey(ciphertext, schedule, 10);
    }

    public static void Aes128Dec(byte[] ciphertext, byte[] schedule, byte[] plaintext)
    {
        Array.Copy(ciphertext, plaintext, 16);
        XorRoundKey(plaintext, schedule, 10);
        ShiftRowsInv(plaintext);
        SubBytesInv(plaintext, 16);

        for (var i = 0; i < 9; i++)
        {
            XorRoundKey(plaintext, schedule, 9 - i);
            MixColsInv(plaintext);
            ShiftRowsInv(plaintext);
            SubBytesInv(plaintext, 16);
        }

        XorRoundKey(plaintext, schedule, 0);
    }

    public static void RevAes128Dec(byte[] ciphertext, byte[] schedule, byte[] plaintext)
    {
        Array.Copy(ciphertext, plaintext, 16);
        XorRoundKey(plaintext, schedule, 10);
        ShiftRows(plaintext);
        SubBytes(plaintext, 16);

        for (var i = 0; i < 9; i++)
        {
            XorRoundKey(plaintext, schedule, 9 - i);
            MixCols(plaintext);
            ShiftRows(plaintext);
            SubBytes(plaintext, 16);
        }

        XorRoundKey(plaintext, schedule, 0);
    }
}