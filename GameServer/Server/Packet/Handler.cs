namespace EggLink.DanhengServer.Server.Packet
{
    public abstract class Handler
    {
        public abstract void OnHandle(Connection connection, byte[] header, byte[] data);
    }
}
