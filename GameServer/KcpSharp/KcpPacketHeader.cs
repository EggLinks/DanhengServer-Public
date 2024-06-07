using System.Buffers.Binary;
using System.Diagnostics;

namespace EggLink.DanhengServer.KcpSharp
{
    internal readonly struct KcpPacketHeader : IEquatable<KcpPacketHeader>
    {
        public KcpPacketHeader(KcpCommand command, byte fragment, ushort windowSize, uint timestamp, uint serialNumber, uint unacknowledged)
        {
            Command = command;
            Fragment = fragment;
            WindowSize = windowSize;
            Timestamp = timestamp;
            SerialNumber = serialNumber;
            Unacknowledged = unacknowledged;
        }

        internal KcpPacketHeader(byte fragment)
        {
            Command = 0;
            Fragment = fragment;
            WindowSize = 0;
            Timestamp = 0;
            SerialNumber = 0;
            Unacknowledged = 0;
        }

        public KcpCommand Command { get; }
        public byte Fragment { get; }
        public ushort WindowSize { get; }
        public uint Timestamp { get; }
        public uint SerialNumber { get; }
        public uint Unacknowledged { get; }

        public bool Equals(KcpPacketHeader other) => Command == other.Command && Fragment == other.Fragment && WindowSize == other.WindowSize && Timestamp == other.Timestamp && SerialNumber == other.SerialNumber && Unacknowledged == other.Unacknowledged;
        public override bool Equals(object? obj) => obj is KcpPacketHeader other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Command, Fragment, WindowSize, Timestamp, SerialNumber, Unacknowledged);

        public static KcpPacketHeader Parse(ReadOnlySpan<byte> buffer)
        {
            Debug.Assert(buffer.Length >= 16);
            return new KcpPacketHeader(
                (KcpCommand)buffer[0],
                buffer[1],
                BinaryPrimitives.ReadUInt16LittleEndian(buffer.Slice(2)),
                BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(4)),
                BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(8)),
                BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(12))
                );
        }

        internal void EncodeHeader(ulong? conversationId, int payloadLength, Span<byte> destination, out int bytesWritten)
        {
            Debug.Assert(destination.Length >= 20);
            if (conversationId.HasValue)
            {
                BinaryPrimitives.WriteUInt64BigEndian(destination, conversationId.GetValueOrDefault());
                destination = destination.Slice(8);
                bytesWritten = 28;
            }
            else
            {
                bytesWritten = 20;
            }
            Debug.Assert(destination.Length >= 20);
            destination[1] = Fragment;
            destination[0] = (byte)Command;
            BinaryPrimitives.WriteUInt16LittleEndian(destination.Slice(2), WindowSize);
            BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(4), Timestamp);
            BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(8), SerialNumber);
            BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(12), Unacknowledged);
            BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(16), (uint)payloadLength);
        }
    }
}
