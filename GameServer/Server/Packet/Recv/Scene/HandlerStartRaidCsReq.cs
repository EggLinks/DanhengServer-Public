using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.StartRaidCsReq)]
    public class HandlerStartRaidCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = StartRaidCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            GameData.RaidConfigData.TryGetValue((int)(req.RaidId * 100 + req.WorldLevel), out var raidConfig);
            if (raidConfig != null)
            {
                player.OldEntryId = player.Data.EntryId;
                player.CurRaidId = raidConfig.RaidID;
                player.LastPos = player.Data.Pos;
                player.LastRot = player.Data.Rot;

                var entranceId = 0;
                if (raidConfig.RaidID == 1)
                {
                    // set
                    entranceId = 2013301;
                }
                else
                {
                    if (GameData.RaidConfigData.ContainsKey(raidConfig.RaidID))
                    {
                        entranceId = raidConfig.RaidID;
                    }
                    else
                    {
                        // set
                        var firstMission = raidConfig.MainMissionIDList[0];
                        var subMissionId = GameData.MainMissionData[firstMission].MissionInfo!.StartSubMissionList[0];
                        var subMission = GameData.SubMissionData[subMissionId];
                        entranceId = int.Parse(subMission.SubMissionInfo!.LevelFloorID.ToString().Replace("00", "0"));
                        if (!GameData.MapEntranceData.ContainsKey(entranceId))
                        {
                            entranceId = subMission.SubMissionInfo!.LevelFloorID;
                        }
                    }

                    if (raidConfig.TeamType == Enums.Scene.RaidTeamTypeEnum.TrialOnly)
                    {
                        // set lineup
                        player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, raidConfig.TrialAvatarList);
                        player.SendPacket(new PacketSyncLineupNotify(player.LineupManager!.GetCurLineup()!));
                    }

                    player.EnterScene(entranceId, 0, true);
                    connection.SendPacket(new PacketRaidInfoNotify((uint)raidConfig.RaidID));
                }

                raidConfig.MainMissionIDList.ForEach(missionId =>
                {
                    player.MissionManager!.ReAcceptMainMission(missionId);
                });

                connection.SendPacket(CmdIds.StartRaidScRsp);
            }
        }
    }
}
