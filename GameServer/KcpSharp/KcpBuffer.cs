using System.Diagnostics;

namespace EggLink.DanhengServer.KcpSharp
{
    internal readonly struct KcpBuffer
    {
        private readonly object? _owner;
        private readonly Memory<byte> _memory;
        private readonly int _length;

        public ReadOnlyMemory<byte> DataRegion => _memory.Slice(0, _length);

        public int Length => _length;

        private KcpBuffer(object? owner, Memory<byte> memory, int length)
        {
            _owner = owner;
            _memory = memory;
            _length = length;
        }

        public static KcpBuffer CreateFromSpan(KcpRentedBuffer buffer, ReadOnlySpan<byte> dataSource)
        {
            Memory<byte> memory = buffer.Memory;
            if (dataSource.Length > memory.Length)
            {
                ThrowRentedBufferTooSmall();
            }
            dataSource.CopyTo(memory.Span);
            return new KcpBuffer(buffer.Owner, memory, dataSource.Length);
        }

        public KcpBuffer AppendData(ReadOnlySpan<byte> data)
        {
            if (_length + data.Length > _memory.Length)
            {
                ThrowRentedBufferTooSmall();
            }
            data.CopyTo(_memory.Span.Slice(_length));
            return new KcpBuffer(_owner, _memory, _length + data.Length);
        }

        public KcpBuffer Consume(int length)
        {
            Debug.Assert((uint)length <= (uint)_length);
            return new KcpBuffer(_owner, _memory.Slice(length), _length - length);
        }

        public void Release()
        {
            new KcpRentedBuffer(_owner, _memory).Dispose();
        }

        private static void ThrowRentedBufferTooSmall()
        {
            throw new InvalidOperationException("The rented buffer is not large enough to hold the data.");
        }
    }
}
