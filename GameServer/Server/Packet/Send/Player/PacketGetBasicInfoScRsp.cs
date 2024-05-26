using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketGetBasicInfoScRsp : BasePacket
    {
        public PacketGetBasicInfoScRsp(PlayerInstance player) : base(CmdIds.GetBasicInfoScRsp)
        {
            var proto = new GetBasicInfoScRsp()
            {
                CurDay = 1,
                NextRecoverTime = player.Data.NextStaminaRecover / 1000,
                GameplayBirthday = (uint)player.Data.Birthday,
                PlayerSettingInfo = new(),
            };

            SetData(proto);
        }
    }
}
