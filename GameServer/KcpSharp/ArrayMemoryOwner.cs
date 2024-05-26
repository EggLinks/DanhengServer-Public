#if !NEED_POH_SHIM

using System.Buffers;

namespace EggLink.DanhengServer.KcpSharp
{
    internal sealed class ArrayMemoryOwner : IMemoryOwner<byte>
    {
        private readonly byte[] _buffer;

        public ArrayMemoryOwner(byte[] buffer)
        {
            _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        public Memory<byte> Memory => _buffer;

        public void Dispose() { }
    }
}

#endif
