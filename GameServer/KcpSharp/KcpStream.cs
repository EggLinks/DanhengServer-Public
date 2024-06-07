using KcpSharp;

namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// A stream wrapper of <see cref="KcpConversation"/>.
    /// </summary>
    public sealed class KcpStream : Stream
    {
        private KcpConversation? _conversation;
        private readonly bool _ownsConversation;

        /// <summary>
        /// Create a stream wrapper over an existing <see cref="KcpConversation"/> instance.
        /// </summary>
        /// <param name="conversation">The conversation instance. It must be in stream mode.</param>
        /// <param name="ownsConversation">Whether to dispose the <see cref="KcpConversation"/> instance when <see cref="KcpStream"/> is disposed.</param>
        public KcpStream(KcpConversation conversation, bool ownsConversation)
        {
            if (conversation is null)
            {
                throw new ArgumentNullException(nameof(conversation));
            }
            if (!conversation.StreamMode)
            {
                throw new ArgumentException("Non-stream mode conversation is not supported.", nameof(conversation));
            }
            _conversation = conversation;
            _ownsConversation = ownsConversation;
        }

        /// <inheritdoc />
        public override bool CanRead => true;

        /// <inheritdoc />
        public override bool CanSeek => false;

        /// <inheritdoc />
        public override bool CanWrite => true;

        /// <summary>
        /// The length of the stream. This always throws <see cref="NotSupportedException"/>.
        /// </summary>
        public override long Length => throw new NotSupportedException();

        /// <summary>
        /// The position of the stream. This always throws <see cref="NotSupportedException"/>.
        /// </summary>
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        /// <inheritdoc />
        public override void SetLength(long value) => throw new NotSupportedException();

        /// <summary>
        /// Indicates data is available on the stream to be read. This property checks to see if at least one byte of data is currently available
        /// </summary>
        public bool DataAvailable
        {
            get
            {
                if (_conversation is null)
                {
                    ThrowHelper.ThrowObjectDisposedForKcpStreamException();
                }
                return _conversation!.TryPeek(out KcpConversationReceiveResult result) && result.BytesReceived != 0;
            }
        }

        /// <inheritdoc />
        public override void Flush() => throw new NotSupportedException();

        /// <inheritdoc />
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            if (_conversation is null)
            {
                return Task.FromException(ThrowHelper.NewObjectDisposedForKcpStreamException());
            }
            return _conversation!.FlushAsync(cancellationToken).AsTask();
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        /// <inheritdoc />
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (_conversation is null)
            {
                return Task.FromException<int>(new ObjectDisposedException(nameof(KcpStream)));
            }
            return _conversation.ReadAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();
        }

        /// <inheritdoc />
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (_conversation is null)
            {
                return Task.FromException(new ObjectDisposedException(nameof(KcpStream)));
            }
            return _conversation.WriteAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();
        }

        /// <inheritdoc />
        public override int ReadByte() => throw new NotSupportedException();

        /// <inheritdoc />
        public override void WriteByte(byte value) => throw new NotSupportedException();

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && _ownsConversation)
            {
                _conversation?.Dispose();
            }
            _conversation = null;
            base.Dispose(disposing);
        }

#if !NO_FAST_SPAN
        /// <inheritdoc />
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_conversation is null)
            {
                return new ValueTask<int>(Task.FromException<int>(new ObjectDisposedException(nameof(KcpStream))));
            }
            return _conversation.ReadAsync(buffer, cancellationToken);
        }

        /// <inheritdoc />
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_conversation is null)
            {
                return new ValueTask(Task.FromException(new ObjectDisposedException(nameof(KcpStream))));
            }
            return _conversation.WriteAsync(buffer, cancellationToken);
        }

        /// <inheritdoc />
        public override ValueTask DisposeAsync()
        {
            if (_conversation is not null)
            {
                _conversation.Dispose();
                _conversation = null;
            }
            return base.DisposeAsync();
        }

        /// <inheritdoc />
        public override int Read(Span<byte> buffer) => throw new NotSupportedException();

        /// <inheritdoc />
        public override void Write(ReadOnlySpan<byte> buffer) => throw new NotSupportedException();
#endif

    }
}
