
#if NEED_LINKEDLIST_SHIM
using LinkedListOfBufferItem = KcpSharp.NetstandardShim.LinkedList<KcpSharp.KcpSendReceiveBufferItem>;
using LinkedListNodeOfBufferItem = KcpSharp.NetstandardShim.LinkedListNode<KcpSharp.KcpSendReceiveBufferItem>;
#else
using LinkedListNodeOfBufferItem = System.Collections.Generic.LinkedListNode<EggLink.DanhengServer.KcpSharp.KcpSendReceiveBufferItem>;
using LinkedListOfBufferItem = System.Collections.Generic.LinkedList<EggLink.DanhengServer.KcpSharp.KcpSendReceiveBufferItem>;
#endif

namespace EggLink.DanhengServer.KcpSharp
{
    internal struct KcpSendReceiveBufferItemCache
    {
        private LinkedListOfBufferItem _items;
        private SpinLock _lock;

        public static KcpSendReceiveBufferItemCache Create()
        {
            return new KcpSendReceiveBufferItemCache
            {
                _items = new LinkedListOfBufferItem(),
                _lock = new SpinLock()
            };
        }

        public LinkedListNodeOfBufferItem Allocate(in KcpSendReceiveBufferItem item)
        {
            bool lockAcquired = false;
            try
            {
                _lock.Enter(ref lockAcquired);

                LinkedListNodeOfBufferItem? node = _items.First;
                if (node is null)
                {
                    node = new LinkedListNodeOfBufferItem(item);
                }
                else
                {
                    _items.Remove(node);
                    node.ValueRef = item;
                }
                return node;
            }
            finally
            {
                if (lockAcquired)
                {
                    _lock.Exit();
                }
            }
        }

        public void Return(LinkedListNodeOfBufferItem node)
        {
            bool lockAcquired = false;
            try
            {
                _lock.Enter(ref lockAcquired);

                node.ValueRef = default;
                _items.AddLast(node);
            }
            finally
            {
                if (lockAcquired)
                {
                    _lock.Exit();
                }
            }
        }
    }
}
