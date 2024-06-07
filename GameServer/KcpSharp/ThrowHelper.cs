namespace EggLink.DanhengServer.KcpSharp
{
    internal static class ThrowHelper
    {
        public static void ThrowArgumentOutOfRangeException(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowTransportClosedForStreanException()
        {
            throw new IOException("The underlying transport is closed.");
        }
        public static Exception NewMessageTooLargeForBufferArgument()
        {
            return new ArgumentException("Message is too large.", "buffer");
        }
        public static Exception NewBufferTooSmallForBufferArgument()
        {
            return new ArgumentException("Buffer is too small.", "buffer");
        }
        public static Exception ThrowBufferTooSmall()
        {
            throw new ArgumentException("Buffer is too small.", "buffer");
        }
        public static Exception ThrowAllowPartialSendArgumentException()
        {
            throw new ArgumentException("allowPartialSend should not be set to true in non-stream mode.", "allowPartialSend");
        }
        public static Exception NewArgumentOutOfRangeException(string paramName)
        {
            return new ArgumentOutOfRangeException(paramName);
        }
        public static Exception NewConcurrentSendException()
        {
            return new InvalidOperationException("Concurrent send operations are not allowed.");
        }
        public static Exception NewConcurrentReceiveException()
        {
            return new InvalidOperationException("Concurrent receive operations are not allowed.");
        }
        public static Exception NewTransportClosedForStreamException()
        {
            throw new IOException("The underlying transport is closed.");
        }
        public static Exception NewOperationCanceledExceptionForCancelPendingSend(Exception? innerException, CancellationToken cancellationToken)
        {
            return new OperationCanceledException("This operation is cancelled by a call to CancelPendingSend.", innerException, cancellationToken);
        }
        public static Exception NewOperationCanceledExceptionForCancelPendingReceive(Exception? innerException, CancellationToken cancellationToken)
        {
            return new OperationCanceledException("This operation is cancelled by a call to CancelPendingReceive.", innerException, cancellationToken);
        }
        public static void ThrowConcurrentReceiveException()
        {
            throw new InvalidOperationException("Concurrent receive operations are not allowed.");
        }
        public static Exception NewObjectDisposedForKcpStreamException()
        {
            return new ObjectDisposedException(nameof(KcpStream));
        }
        public static void ThrowObjectDisposedForKcpStreamException()
        {
            throw new ObjectDisposedException(nameof(KcpStream));
        }
    }
}
