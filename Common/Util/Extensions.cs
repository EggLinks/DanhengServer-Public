using System.Buffers.Binary;
using EggLink.DanhengServer.Proto;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Util;

public static class Extensions
{
    public static Position ToPosition(this Vector vector)
    {
        return new Position
        {
            X = vector.X,
            Y = vector.Y,
            Z = vector.Z
        };
    }

    public static T RandomElement<T>(this List<T> values)
    {
        var index = new Random().Next(values.Count);
        return values[index];
    }

    public static ICollection<T> Clone<T>(this ICollection<T> values)
    {
        List<T> list = [.. values];

        return list;
    }

    public static int RandomInt(int from, int to)
    {
        return new Random().Next(from, to);
    }

    public static void SafeAdd<T>(this List<T> list, T item)
    {
        if (!list.Contains(item)) list.Add(item);
    }

    public static void SafeAddRange<T>(this List<T> list, List<T> item)
    {
        foreach (var i in item) list.SafeAdd(i);
    }

    public static long GetUnixSec()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public static long ToUnixSec(this DateTime dt)
    {
        return new DateTimeOffset(dt).ToUnixTimeSeconds();
    }

    public static long GetUnixMs()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    public static string ToArrayString<T>(this List<T> list)
    {
        return list.JoinFormat(", ", "");
    }

    public static string ToJsonString<TK, TV>(this Dictionary<TK, TV> dic) where TK : notnull
    {
        return JsonConvert.SerializeObject(dic);
    }

    #region Kcp Utils

    public static string JoinFormat<T>(this IEnumerable<T> list, string separator,
        string formatString)
    {
        formatString = string.IsNullOrWhiteSpace(formatString) ? "{0}" : formatString;
        return string.Join(separator,
            list.Select(item => string.Format(formatString, item)));
    }

    public static void WriteConvID(this BinaryWriter bw, long convId)
    {
        //bw.Write(convId);
        bw.Write((int)(convId >> 32));
        bw.Write((int)(convId & 0xFFFFFFFF));
    }

    public static long GetNextAvailableIndex<T>(this SortedList<long, T> sortedList)
    {
        long key = 1;
        long count = sortedList.Count;
        long counter = 0;
        do
        {
            if (count == 0) break;
            var nextKeyInList = sortedList.Keys.ElementAt((Index)counter++);
            if (key != nextKeyInList) break;
            key = nextKeyInList + 1;
        } while (count != 1 && counter != count && key == sortedList.Keys.ElementAt((Index)counter));

        return key;
    }

    public static long AddNext<T>(this SortedList<long, T> sortedList, T item)
    {
        var key = sortedList.GetNextAvailableIndex();
        sortedList.Add(key, item);
        return key;
    }

    public static int ReadInt32BE(this BinaryReader br)
    {
        return BinaryPrimitives.ReadInt32BigEndian(br.ReadBytes(sizeof(int)));
    }

    public static uint ReadUInt32BE(this BinaryReader br)
    {
        return BinaryPrimitives.ReadUInt32BigEndian(br.ReadBytes(sizeof(uint)));
    }

    public static ushort ReadUInt16BE(this BinaryReader br)
    {
        return BinaryPrimitives.ReadUInt16BigEndian(br.ReadBytes(sizeof(ushort)));
    }

    public static void WriteUInt16BE(this BinaryWriter bw, ushort value)
    {
        Span<byte> data = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(data, value);
        bw.Write(data);
    }

    public static void WriteInt32BE(this BinaryWriter bw, int value)
    {
        Span<byte> data = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(data, value);
        bw.Write(data);
    }

    public static void WriteUInt32BE(this BinaryWriter bw, uint value)
    {
        Span<byte> data = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(data, value);
        bw.Write(data);
    }

    public static void WriteUInt64BE(this BinaryWriter bw, ulong value)
    {
        Span<byte> data = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(data, value);
        bw.Write(data);
    }

    #endregion
}