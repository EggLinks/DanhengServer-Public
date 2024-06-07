
#if NEED_LINKEDLIST_SHIM
using LinkedListOfQueueItem = KcpSharp.NetstandardShim.LinkedList<(KcpSharp.KcpBuffer Data, byte Fragment)>;
using LinkedListNodeOfQueueItem = KcpSharp.NetstandardShim.LinkedListNode<(KcpSharp.KcpBuffer Data, byte Fragment)>;
#else
using LinkedListNodeOfQueueItem = System.Collections.Generic.LinkedListNode<(EggLink.DanhengServer.KcpSharp.KcpBuffer Data, byte Fragment)>;
using LinkedListOfQueueItem = System.Collections.Generic.LinkedList<(EggLink.DanhengServer.KcpSharp.KcpBuffer Data, byte Fragment)>;
#endif

namespace EggLink.DanhengServer.KcpSharp
{
    internal sealed class KcpSendReceiveQueueItemCache
    {
        private LinkedListOfQueueItem _list = new();
        private SpinLock _lock;

        public LinkedListNodeOfQueueItem Rent(in KcpBuffer buffer, byte fragment)
        {
            bool lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);

                LinkedListNodeOfQueueItem? node = _list.First;
                if (node is null)
                {
                    node = new LinkedListNodeOfQueueItem((buffer, fragment));
                }
                else
                {
                    node.ValueRef = (buffer, fragment);
                    _list.RemoveFirst();
                }

                return node;
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                }
            }
        }

        public void Return(LinkedListNodeOfQueueItem node)
        {
            node.ValueRef = default;

            bool lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);

                _list.AddLast(node);
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                }
            }
        }

        public void Clear()
        {
            bool lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);

                _list.Clear();
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                }
            }
        }
    }
}
