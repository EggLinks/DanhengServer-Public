using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
    public class PacketGetFirstTalkByPerformanceNpcScRsp : BasePacket
    { 
        public PacketGetFirstTalkByPerformanceNpcScRsp(GetFirstTalkByPerformanceNpcCsReq req) : base(CmdIds.GetFirstTalkByPerformanceNpcScRsp)
        {
            var rsp = new GetFirstTalkByPerformanceNpcScRsp();

            foreach (var id in req.FirstTalkIdList)
            {
                rsp.NpcMeetStatusList.Add(new NpcMeetStatusInfo
                {
                    MeetId = id,
                });
            }

            SetData(rsp);
        }
    }
}
