using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketGetRogueHandbookDataScRsp : BasePacket
{
    public PacketGetRogueHandbookDataScRsp() : base(CmdIds.GetRogueHandbookDataScRsp)
    {
        var proto = new GetRogueHandbookDataScRsp
        {
            HandbookInfo = new RogueHandbook()
        };

        //foreach (var item in GameData.RogueHandbookMiracleData)
        //{
        //    proto.HandbookInfo.MiracleList.Add(new EHJPAIBFIPK()
        //    {
        //        HAGLJCEAMEK = (uint)item.Value.MiracleHandbookID,
        //        HasTakenReward = true,
        //    });
        //}

        foreach (var item in GameData.RogueMazeBuffData)
        {
            if (item.Value.Lv != 1)
                continue;
            proto.HandbookInfo.BuffList.Add(new RogueHandbookMazeBuff
            {
                MazeBuffId = (uint)item.Value.ID
            });
        }

        //foreach (var item in GameData.RogueAeonData)
        //{
        //    proto.HandbookInfo.RogueAeonList.Add(new DKNNNMJKPNO()
        //    {
        //        AeonId = (uint)item.Value.AeonID,
        //    });
        //}

        //foreach (var item in GameData.RogueHandBookEventData)
        //{
        //    proto.HandbookInfo.EventList.Add(new MMEGPGAAPDG()
        //    {
        //        BFHDEKMCJJA = (uint)item.Value.EventID,
        //        HasTakenReward = true,
        //    });
        //}

        SetData(proto);
    }
}