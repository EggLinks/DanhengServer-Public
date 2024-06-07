using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Game.Battle.Skill;
using EggLink.DanhengServer.Game.Battle.Skill.Action;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Battle
{
    [Opcode(CmdIds.SceneCastSkillCsReq)]
    public class HandlerSceneCastSkillCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SceneCastSkillCsReq.Parser.ParseFrom(data);
            if (req != null)
            {
                connection.Player!.SceneInstance!.AvatarInfo.TryGetValue((int)req.AttackedByEntityId, out var info);
                MazeSkill mazeSkill = new([]);

                if (info != null)  // cast by player
                {
                    mazeSkill = MazeSkillManager.GetSkill(info.AvatarInfo.GetAvatarId(), (int)req.SkillIndex);
                }

                if (req.HitTargetEntityIdList.Count == 0)
                {
                    // didnt hit any target
                    if (info != null && req.SkillIndex > 0)
                    {
                        mazeSkill.OnCast(info);
                    }
                    connection.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
                }
                else
                {
                    connection.Player!.BattleManager!.StartBattle(req, mazeSkill);
                }
            }
        }
    }
}
