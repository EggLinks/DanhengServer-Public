using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Lineup
{
    public class PacketGetCurLineupDataScRsp : BasePacket
    {
        public PacketGetCurLineupDataScRsp(PlayerInstance player) : base(CmdIds.GetCurLineupDataScRsp)
        {
            var data = new GetCurLineupDataScRsp()
            {
                Lineup = player.LineupManager?.GetCurLineup()?.ToProto() ?? new(),
            };

            SetData(data);
        }
    }
}
