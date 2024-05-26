using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// The buffer rented and owned by KcpSharp.
    /// </summary>
    public readonly struct KcpRentedBuffer : IEquatable<KcpRentedBuffer>, IDisposable
    {
        private readonly object? _owner;
        private readonly Memory<byte> _memory;

        internal object? Owner => _owner;

        /// <summary>
        /// The rented buffer.
        /// </summary>
        public Memory<byte> Memory => _memory;

        /// <summary>
        /// The rented buffer.
        /// </summary>
        public Span<byte> Span => _memory.Span;

        /// <summary>
        /// Whether this struct contains buffer rented from the pool.
        /// </summary>
        public bool IsAllocated => _owner is not null;

        /// <summary>
        /// Whether this buffer contains no data.
        /// </summary>
        public bool IsEmpry => _memory.IsEmpty;

        internal KcpRentedBuffer(object? owner, Memory<byte> buffer)
        {
            _owner = owner;
            _memory = buffer;
        }

        /// <summary>
        /// Create the buffer from the specified <see cref="Memory{T}"/>.
        /// </summary>
        /// <param name="memory">The memory region of this buffer.</param>
        /// <returns>The rented buffer.</returns>
        public static KcpRentedBuffer FromMemory(Memory<byte> memory)
        {
            return new KcpRentedBuffer(null, memory);
        }

        /// <summary>
        /// Create the buffer from the shared array pool.
        /// </summary>
        /// <param name="size">The minimum size of the buffer required.</param>
        /// <returns>The rented buffer.</returns>
        public static KcpRentedBuffer FromSharedArrayPool(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            byte[] buffer = ArrayPool<byte>.Shared.Rent(size);
            return new KcpRentedBuffer(ArrayPool<byte>.Shared, buffer);
        }

        /// <summary>
        /// Create the buffer from the specified array pool.
        /// </summary>
        /// <param name="pool">The array pool to use.</param>
        /// <param name="buffer">The byte array rented from the specified pool.</param>
        /// <returns>The rented buffer.</returns>
        public static KcpRentedBuffer FromArrayPool(ArrayPool<byte> pool, byte[] buffer)
        {
            if (pool is null)
            {
                throw new ArgumentNullException(nameof(pool));
            }
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            return new KcpRentedBuffer(pool, buffer);
        }

        /// <summary>
        /// Create the buffer from the specified array pool.
        /// </summary>
        /// <param name="pool">The array pool to use.</param>
        /// <param name="arraySegment">The byte array segment rented from the specified pool.</param>
        /// <returns>The rented buffer.</returns>
        public static KcpRentedBuffer FromArrayPool(ArrayPool<byte> pool, ArraySegment<byte> arraySegment)
        {
            if (pool is null)
            {
                throw new ArgumentNullException(nameof(pool));
            }
            return new KcpRentedBuffer(pool, arraySegment);
        }

        /// <summary>
        /// Create the buffer from the specified array pool.
        /// </summary>
        /// <param name="pool">The array pool to use.</param>
        /// <param name="size">The minimum size of the buffer required.</param>
        /// <returns>The rented buffer.</returns>
        public static KcpRentedBuffer FromArrayPool(ArrayPool<byte> pool, int size)
        {
            if (pool is null)
            {
                throw new ArgumentNullException(nameof(pool));
            }
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            return new KcpRentedBuffer(pool, pool.Rent(size));
        }

        /// <summary>
        /// Create the buffer from the memory owner.
        /// </summary>
        /// <param name="memoryOwner">The owner of this memory region.</param>
        /// <returns>The rented buffer.</returns>
        public static KcpRentedBuffer FromMemoryOwner(IMemoryOwner<byte> memoryOwner)
        {
            if (memoryOwner is null)
            {
                throw new ArgumentNullException(nameof(memoryOwner));
            }
            return new KcpRentedBuffer(memoryOwner, memoryOwner.Memory);
        }


        /// <summary>
        /// Create the buffer from the memory owner.
        /// </summary>
        /// <param name="memoryOwner">The owner of this memory region.</param>
        /// <param name="memory">The memory region of the buffer.</param>
        /// <returns>The rented buffer.</returns>
        public static KcpRentedBuffer FromMemoryOwner(IDisposable memoryOwner, Memory<byte> memory)
        {
            if (memoryOwner is null)
            {
                throw new ArgumentNullException(nameof(memoryOwner));
            }
            return new KcpRentedBuffer(memoryOwner, memory);
        }

        /// <summary>
        /// Forms a slice out of the current buffer that begins at a specified index.
        /// </summary>
        /// <param name="start">The index at which to begin the slice.</param>
        /// <returns>An object that contains all elements of the current instance from start to the end of the instance.</returns>
        public KcpRentedBuffer Slice(int start)
        {
            Memory<byte> memory = _memory;
            if ((uint)start > (uint)memory.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(start));
            }
            return new KcpRentedBuffer(_owner, memory.Slice(start));
        }

        /// <summary>
        /// Forms a slice out of the current memory starting at a specified index for a specified length.
        /// </summary>
        /// <param name="start">The index at which to begin the slice.</param>
        /// <param name="length">The number of elements to include in the slice.</param>
        /// <returns>An object that contains <paramref name="length"/> elements from the current instance starting at <paramref name="start"/>.</returns>
        public KcpRentedBuffer Slice(int start, int length)
        {
            Memory<byte> memory = _memory;
            if ((uint)start > (uint)memory.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(start));
            }
            if ((uint)length > (uint)(memory.Length - start))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(length));
            }
            return new KcpRentedBuffer(_owner, memory.Slice(start, length));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Debug.Assert(_owner is null || _owner is ArrayPool<byte> || _owner is IDisposable);

            if (_owner is null)
            {
                return;
            }
            if (_owner is ArrayPool<byte> arrayPool)
            {
                if (MemoryMarshal.TryGetArray(_memory, out ArraySegment<byte> arraySegment))
                {
                    arrayPool.Return(arraySegment.Array!);
                    return;
                }
            }
            if (_owner is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        /// <inheritdoc />
        public bool Equals(KcpRentedBuffer other) => ReferenceEquals(_owner, other._owner) && _memory.Equals(other._memory);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is KcpRentedBuffer other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => _owner is null ? _memory.GetHashCode() : HashCode.Combine(RuntimeHelpers.GetHashCode(_owner), _memory);

        /// <inheritdoc />
        public override string ToString() => $"KcpSharp.KcpRentedBuffer[{_memory.Length}]";
    }
}
