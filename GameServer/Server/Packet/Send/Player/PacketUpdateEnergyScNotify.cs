using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketUpdateEnergyScNotify : BasePacket
    {
        public PacketUpdateEnergyScNotify(int curNum, int maxNum) : base(CmdIds.UpdateEnergyScNotify)
        {
            var proto = new UpdateEnergyScNotify()
            {
                EnergyInfo = new RotatorEnergyInfo()
                {
                    MaxNum = (uint)maxNum,
                    CurNum = (uint)curNum,
                }
            };

            SetData(proto);
        }
    }
}
