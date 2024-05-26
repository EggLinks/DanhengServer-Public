#if NEED_TCS_SHIM

namespace System.Threading.Tasks
{
    internal class TaskCompletionSource : TaskCompletionSource<bool>
    {
        public void TrySetResult() => TrySetResult(true);
    }
}

#endif
