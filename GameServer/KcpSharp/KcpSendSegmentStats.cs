namespace EggLink.DanhengServer.KcpSharp
{
    internal readonly struct KcpSendSegmentStats
    {
        public KcpSendSegmentStats(uint resendTimestamp, uint rto, uint fastAck, uint transmitCount)
        {
            ResendTimestamp = resendTimestamp;
            Rto = rto;
            FastAck = fastAck;
            TransmitCount = transmitCount;
        }

        public uint ResendTimestamp { get; }
        public uint Rto { get; }
        public uint FastAck { get; }
        public uint TransmitCount { get; }


    }
}
