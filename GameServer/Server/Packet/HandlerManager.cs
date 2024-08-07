using System.Reflection;

namespace EggLink.DanhengServer.GameServer.Server.Packet;

public static class HandlerManager
{
    public static Dictionary<int, Handler> handlers = [];

    public static void Init()
    {
        var classes = Assembly.GetExecutingAssembly().GetTypes(); // Get all classes in the assembly
        foreach (var cls in classes)
        {
            var attribute = (Opcode?)Attribute.GetCustomAttribute(cls, typeof(Opcode));

            if (attribute != null) handlers.Add(attribute.CmdId, (Handler)Activator.CreateInstance(cls)!);
        }
    }

    public static Handler? GetHandler(int cmdId)
    {
        try
        {
            return handlers[cmdId];
        }
        catch
        {
            return null;
        }
    }
}