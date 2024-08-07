using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketUpdateFloorSavedValueNotify : BasePacket
{
    public PacketUpdateFloorSavedValueNotify(string name, int savedValue) : base(CmdIds.UpdateFloorSavedValueNotify)
    {
        var proto = new UpdateFloorSavedValueNotify();
        proto.SavedValue.Add(name, savedValue);

        SetData(proto);
    }
}