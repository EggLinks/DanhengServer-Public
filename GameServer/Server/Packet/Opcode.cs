using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Opcode(int cmdId) : Attribute
    {
        public int CmdId = cmdId;
    }
}
