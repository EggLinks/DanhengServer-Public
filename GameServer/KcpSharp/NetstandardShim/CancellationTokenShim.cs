#if NEED_CANCELLATIONTOKEN_SHIM

namespace System.Threading
{
    internal static class CancellationTokenShim
    {
        public static CancellationTokenRegistration UnsafeRegister(this CancellationToken cancellationToken, Action<object?> callback, object? state)
            => cancellationToken.Register(callback, state);
    }
}


#endif
