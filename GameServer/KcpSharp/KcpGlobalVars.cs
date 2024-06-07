namespace EggLink.DanhengServer.KcpSharp
{
    internal static class KcpGlobalVars
    {
#if !CONVID32
        public const ushort CONVID_LENGTH = 8;
        public const ushort HEADER_LENGTH_WITH_CONVID = 28;
        public const ushort HEADER_LENGTH_WITHOUT_CONVID = 20;
#else
        public const ushort HEADER_LENGTH_WITH_CONVID = 24;
        public const ushort HEADER_LENGTH_WITHOUT_CONVID = 20;
#endif
    }
}
