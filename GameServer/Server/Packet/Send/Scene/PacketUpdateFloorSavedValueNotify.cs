using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene
{
    public class PacketUpdateFloorSavedValueNotify : BasePacket
    {
        public PacketUpdateFloorSavedValueNotify(string name, int savedValue) : base(CmdIds.UpdateFloorSavedValueNotify)
        {
            var proto = new UpdateFloorSavedValueNotify();
            proto.SavedValue.Add(name, savedValue);

            SetData(proto);
        }
    }
}
