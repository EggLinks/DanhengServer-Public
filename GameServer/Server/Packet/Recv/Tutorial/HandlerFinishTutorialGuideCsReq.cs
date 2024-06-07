using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Tutorial;

namespace EggLink.DanhengServer.Server.Packet.Recv.Tutorial
{
    [Opcode(CmdIds.FinishTutorialGuideCsReq)]
    public class HandlerFinishTutorialGuideCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = FinishTutorialGuideCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            if (player.TutorialGuideData!.Tutorials.TryGetValue((int)req.GroupId, out var res))
            {
                if (res != TutorialStatus.TutorialFinish)
                {
                    player.InventoryManager!.AddItem(1, 1, false);
                    player.TutorialGuideData!.Tutorials[(int)req.GroupId] = TutorialStatus.TutorialFinish;
                    DatabaseHelper.Instance?.UpdateInstance(player.TutorialGuideData!);
                }
            }

            connection.SendPacket(new PacketFinishTutorialGuideScRsp(req.GroupId));
        }
    }
}
