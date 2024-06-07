using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks.Sources;

namespace KcpSharp
{
    partial class KcpConversation
    {
#if NET6_0_OR_GREATER
        [ThreadStatic]
        private static KcpConversation? s_currentObject;
        private object? _flushStateMachine;

        private struct KcpFlushAsyncMethodBuilder
        {
            private readonly KcpConversation _conversation;
            private StateMachineBox? _task;

            private static readonly StateMachineBox s_syncSuccessSentinel = new SyncSuccessSentinelStateMachineBox();

            public KcpFlushAsyncMethodBuilder(KcpConversation conversation)
            {
                _conversation = conversation;
                _task = null;
            }

            public static KcpFlushAsyncMethodBuilder Create()
            {
                KcpConversation? conversation = s_currentObject;
                Debug.Assert(conversation is not null);
                s_currentObject = null;

                return new KcpFlushAsyncMethodBuilder(conversation);
            }

#pragma warning disable CA1822 // Mark members as static
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Start<TStateMachine>(ref TStateMachine stateMachine)
                where TStateMachine : IAsyncStateMachine
#pragma warning restore CA1822 // Mark members as static
            {
                Debug.Assert(stateMachine is not null);

                stateMachine.MoveNext();
            }

            public ValueTask Task
            {
                get
                {
                    if (ReferenceEquals(_task, s_syncSuccessSentinel))
                    {
                        return default;
                    }
                    StateMachineBox stateMachineBox = _task ??= CreateWeaklyTypedStateMachineBox();
                    return new ValueTask(stateMachineBox, stateMachineBox.Version);
                }
            }

#pragma warning disable CA1822 // Mark members as static
            public void SetStateMachine(IAsyncStateMachine stateMachine)
#pragma warning restore CA1822 // Mark members as static
            {
                Debug.Fail("SetStateMachine should not be used.");
            }

            public void SetResult()
            {
                if (_task == null)
                {
                    _task = s_syncSuccessSentinel;
                }
                else
                {
                    _task.SetResult();
                }
            }

            public void SetException(Exception exception)
            {
                SetException(exception, ref _task);
            }

            private static void SetException(Exception exception, ref StateMachineBox? boxFieldRef)
            {
                if (exception == null)
                {
                    throw new ArgumentNullException(nameof(exception));
                }
                (boxFieldRef ??= CreateWeaklyTypedStateMachineBox()).SetException(exception);
            }

            public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : INotifyCompletion
                where TStateMachine : IAsyncStateMachine
            {
                AwaitOnCompleted(ref awaiter, ref stateMachine, ref _task, _conversation);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
               where TAwaiter : ICriticalNotifyCompletion
               where TStateMachine : IAsyncStateMachine
            {
                AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine, ref _task, _conversation);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine, ref StateMachineBox? boxRef, KcpConversation conversation) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
            {
                StateMachineBox stateMachineBox = GetStateMachineBox(ref stateMachine, ref boxRef, conversation);
                AwaitUnsafeOnCompleted(ref awaiter, stateMachineBox);
            }

            private static void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine, ref StateMachineBox? box, KcpConversation conversation) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
            {
                try
                {
                    awaiter.OnCompleted(GetStateMachineBox(ref stateMachine, ref box, conversation).MoveNextAction);
                }
                catch (Exception exception)
                {
                    ExceptionDispatchInfo? edi = ExceptionDispatchInfo.Capture(exception);
                    ThreadPool.QueueUserWorkItem(static state => ((ExceptionDispatchInfo)state!).Throw(), edi);
                }
            }

            private static void AwaitUnsafeOnCompleted<TAwaiter>(ref TAwaiter awaiter, StateMachineBox box) where TAwaiter : ICriticalNotifyCompletion
            {
                try
                {
                    awaiter.UnsafeOnCompleted(box.MoveNextAction);
                }
                catch (Exception exception)
                {
                    ExceptionDispatchInfo? edi = ExceptionDispatchInfo.Capture(exception);
                    ThreadPool.QueueUserWorkItem(static state => ((ExceptionDispatchInfo)state!).Throw(), edi);
                }
            }


            private static StateMachineBox CreateWeaklyTypedStateMachineBox()
            {
                return new StateMachineBox<IAsyncStateMachine>(null);
            }

            private static StateMachineBox GetStateMachineBox<TStateMachine>(ref TStateMachine stateMachine, ref StateMachineBox? boxFieldRef, KcpConversation conversation) where TStateMachine : IAsyncStateMachine
            {
                StateMachineBox<TStateMachine>? stateMachineBox = boxFieldRef as StateMachineBox<TStateMachine>;
                if (stateMachineBox != null)
                {
                    return stateMachineBox;
                }
                StateMachineBox<IAsyncStateMachine>? stateMachineBox2 = boxFieldRef as StateMachineBox<IAsyncStateMachine>;
                if (stateMachineBox2 != null)
                {
                    if (stateMachineBox2.StateMachine == null)
                    {
                        Debugger.NotifyOfCrossThreadDependency();
                        stateMachineBox2.StateMachine = stateMachine;
                    }
                    return stateMachineBox2;
                }
                Debugger.NotifyOfCrossThreadDependency();
                StateMachineBox<TStateMachine> stateMachineBox3 = (StateMachineBox<TStateMachine>)(boxFieldRef = StateMachineBox<TStateMachine>.GetOrCreateBox(conversation));
                stateMachineBox3.StateMachine = stateMachine;
                return stateMachineBox3;
            }

            private abstract class StateMachineBox : IValueTaskSource
            {
                protected ManualResetValueTaskSourceCore<bool> _mrvtsc;
                protected Action? _moveNextAction;

                public virtual Action MoveNextAction => _moveNextAction!;

                public short Version => _mrvtsc.Version;

                public void SetResult()
                {
                    _mrvtsc.SetResult(true);
                }

                public void SetException(Exception error)
                {
                    _mrvtsc.SetException(error);
                }

                public ValueTaskSourceStatus GetStatus(short token)
                {
                    return _mrvtsc.GetStatus(token);
                }

                public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
                {
                    _mrvtsc.OnCompleted(continuation, state, token, flags);
                }

                void IValueTaskSource.GetResult(short token)
                {
                    throw new NotSupportedException();
                }
            }

            private sealed class SyncSuccessSentinelStateMachineBox : StateMachineBox
            {
                public SyncSuccessSentinelStateMachineBox()
                {
                    SetResult();
                }
            }


            private sealed class StateMachineBox<TStateMachine> : StateMachineBox, IValueTaskSource where TStateMachine : IAsyncStateMachine
            {
                [MaybeNull]
                public TStateMachine StateMachine;
                private KcpConversation? _conversation;

                public override Action MoveNextAction => _moveNextAction ??= MoveNext;

                internal StateMachineBox(KcpConversation? conversation)
                {
                    _conversation = conversation;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static StateMachineBox<TStateMachine> GetOrCreateBox(KcpConversation conversation)
                {
                    if (conversation._flushStateMachine is StateMachineBox<TStateMachine> stateMachine)
                    {
                        stateMachine._conversation = conversation;
                        conversation._flushStateMachine = null;
                        return stateMachine;
                    }
                    return new StateMachineBox<TStateMachine>(conversation);
                }

                void IValueTaskSource.GetResult(short token)
                {
                    try
                    {
                        _mrvtsc.GetResult(token);
                    }
                    finally
                    {
                        ReturnOrDropBox();
                    }
                }

                public void MoveNext()
                {
                    if (StateMachine is not null)
                    {
                        StateMachine.MoveNext();
                    }
                }

                private void ReturnOrDropBox()
                {
                    StateMachine = default!;
                    _mrvtsc.Reset();
                    if (_conversation is not null)
                    {
                        _conversation._flushStateMachine = this;
                        _conversation = null;
                    }
                }
            }
        }
#endif
    }
}
