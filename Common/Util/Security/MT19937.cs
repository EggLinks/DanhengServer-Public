namespace EggLink.DanhengServer.Util.Security;

public class MT19937
{
    private const int N = 312;
    private const int M = 156;
    private const ulong MATRIX_A = 13043109905998158313UL;
    private const ulong UPPER_MASK = 18446744071562067968UL;
    private const ulong LOWER_MASK = 2147483647UL;

    private readonly ulong[] mt = new ulong[N];
    private int mti = N + 1;

    public MT19937()
    {
        Seed(5489UL); // MT19937 default seed
    }

    public MT19937(ulong seed)
    {
        Seed(seed);
    }

    public void Seed(ulong seed)
    {
        mt[0] = seed;
        for (mti = 1; mti < N; mti++)
            mt[mti] = 6364136223846793005UL * (mt[mti - 1] ^ (mt[mti - 1] >> 62)) + (ulong)mti;
    }

    public ulong NextUInt64()
    {
        ulong x;
        ulong[] mag01 = { 0UL, MATRIX_A };

        if (mti >= N)
        {
            int i;

            if (mti == N + 1) Seed(5489UL);

            for (i = 0; i < N - M; i++)
            {
                x = (mt[i] & UPPER_MASK) | (mt[i + 1] & LOWER_MASK);
                mt[i] = mt[i + M] ^ (x >> 1) ^ mag01[x & 1UL];
            }

            for (; i < N - 1; i++)
            {
                x = (mt[i] & UPPER_MASK) | (mt[i + 1] & LOWER_MASK);
                mt[i] = mt[i + (M - N)] ^ (x >> 1) ^ mag01[x & 1UL];
            }

            x = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
            mt[N - 1] = mt[M - 1] ^ (x >> 1) ^ mag01[x & 1UL];

            mti = 0;
        }

        x = mt[mti++];
        x ^= (x >> 29) & 0x5555555555555555UL;
        x ^= (x << 17) & 0x71D67FFFEDA60000UL;
        x ^= (x << 37) & 0xFFF7EEE000000000UL;
        x ^= x >> 43;

        return x;
    }

    public long NextInt63()
    {
        return (long)(NextUInt64() & 0x7FFFFFFFFFFFFFFFUL);
    }
}