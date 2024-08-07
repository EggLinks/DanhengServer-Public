namespace EggLink.DanhengServer.GameServer.Server.Packet;

[AttributeUsage(AttributeTargets.Class)]
public class Opcode(int cmdId) : Attribute
{
    public int CmdId = cmdId;
}