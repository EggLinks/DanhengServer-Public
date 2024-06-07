using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Tutorial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Tutorial
{
    [Opcode(CmdIds.UnlockTutorialGuideCsReq)]
    public class HandlerUnlockTutorialGuideCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = UnlockTutorialGuideCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            if (!player.TutorialGuideData!.Tutorials.TryGetValue((int)req.GroupId, out var _))
            {
                player.TutorialGuideData!.Tutorials.Add((int)req.GroupId, TutorialStatus.TutorialUnlock);
                DatabaseHelper.Instance?.UpdateInstance(player.TutorialGuideData!);
            }
            connection.SendPacket(new PacketUnlockTutorialGuideScRsp(req.GroupId));
        }
    }
}
