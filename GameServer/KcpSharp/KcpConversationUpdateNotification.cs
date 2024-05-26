namespace EggLink.DanhengServer.KcpSharp
{
    internal readonly struct KcpConversationUpdateNotification : IDisposable
    {
        private readonly IKcpConversationUpdateNotificationSource? _source;
        private readonly bool _skipTimerNotification;

        public ReadOnlyMemory<byte> Packet => _source?.Packet ?? default;
        public bool TimerNotification => !_skipTimerNotification;

        public KcpConversationUpdateNotification(IKcpConversationUpdateNotificationSource? source, bool skipTimerNotification)
        {
            _source = source;
            _skipTimerNotification = skipTimerNotification;
        }

        public KcpConversationUpdateNotification WithTimerNotification(bool timerNotification)
        {
            return new KcpConversationUpdateNotification(_source, !_skipTimerNotification | timerNotification);
        }

        public void Dispose()
        {
            if (_source is not null)
            {
                _source.Release();
            }
        }
    }
}
