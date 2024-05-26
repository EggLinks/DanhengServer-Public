using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketSetGameplayBirthdayScRsp : BasePacket
    {
        public PacketSetGameplayBirthdayScRsp(uint birthday) : base(CmdIds.SetGameplayBirthdayScRsp)
        {
            var proto = new SetGameplayBirthdayScRsp()
            {
                Birthday = birthday,
            };

            SetData(proto);
        }

        public PacketSetGameplayBirthdayScRsp() : base(CmdIds.SetGameplayBirthdayScRsp)
        {
            var proto = new SetGameplayBirthdayScRsp()
            {
                Retcode = 1,
            };

            SetData(proto);
        }
    }
}
