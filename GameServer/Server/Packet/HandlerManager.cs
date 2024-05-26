using System.Reflection;

namespace EggLink.DanhengServer.Server.Packet
{
    public class HandlerManager
    {
        public Dictionary<int, Handler> handlers = [];

        public HandlerManager()
        {
            var classes = Assembly.GetExecutingAssembly().GetTypes();  // Get all classes in the assembly
            foreach (var cls in classes)
            {
                var attribute = (Opcode)Attribute.GetCustomAttribute(cls, typeof(Opcode))!;

                if (attribute != null)
                {
                    handlers.Add(attribute.CmdId, (Handler)Activator.CreateInstance(cls)!);
                }
            }
        }

        public Handler? GetHandler(int cmdId)
        {
            try
            {
                return handlers[cmdId];
            } catch
            {
                return null;
            }
        }
    }
}
