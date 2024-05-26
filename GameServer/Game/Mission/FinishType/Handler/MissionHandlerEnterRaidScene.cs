using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(MissionFinishTypeEnum.EnterRaidScene)]
    public class MissionHandlerEnterRaidScene : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (player.CurRaidId != info.ParamInt1)
            {
                // change raid
                GameData.RaidConfigData.TryGetValue(info.ParamInt1 * 100 + 0, out var raidConfig);
                if (raidConfig != null)
                {
                    if (player.CurRaidId == 0)  // set old info when not in raid
                    {
                        player.OldEntryId = player.Data.EntryId;
                        player.LastPos = player.Data.Pos;
                        player.LastRot = player.Data.Rot;
                    }
                    player.CurRaidId = raidConfig.RaidID;

                    raidConfig.MainMissionIDList.ForEach(missionId =>
                    {
                        player.MissionManager!.AcceptMainMission(missionId);
                    });

                    player.SendPacket(new PacketRaidInfoNotify((uint)raidConfig.RaidID));
                }
            }
            player.EnterScene(info.ParamInt2, 0, true);
            player.MissionManager!.FinishSubMission(info.ID);
        }
    }
}
