using EggLink.DanhengServer.Game.Battle.Skill;
using EggLink.DanhengServer.Game.Player;
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

            PlayerInstance player = connection.Player!;
            MazeSkill mazeSkill = new([]);

            // Get casting avatar
            connection.Player!.SceneInstance!.AvatarInfo.TryGetValue((int)req.AttackedByEntityId, out var caster);

            if (caster != null)
            {
                // Check if normal attack or technique was used
                if (req.SkillIndex > 0)
                {
                    // Cast skill effects
                    if (caster.AvatarInfo.Excel!.MazeSkill != null)
                    {
                        mazeSkill = MazeSkillManager.GetSkill(caster.AvatarInfo.GetAvatarId(), (int)req.SkillIndex);
                        mazeSkill.OnCast(caster);
                    }
                }
                else
                {
                    mazeSkill = MazeSkillManager.GetSkill(caster.AvatarInfo.GetAvatarId(), 0);
                }
            }

            if (req.AssistMonsterEntityIdList.Count > 0)
            {
                List<uint> hitTargetEntityIdList = [];
                if (req.AssistMonsterEntityIdList.Count > 0)
                {
                    foreach (uint id in req.AssistMonsterEntityIdList)
                    {
                        hitTargetEntityIdList.Add(id);
                    }
                }
                else
                {
                    foreach (uint id in req.HitTargetEntityIdList)
                    {
                        hitTargetEntityIdList.Add(id);
                    }
                }
                // Start battle
                connection.Player!.BattleManager!.StartBattle(req, mazeSkill!, [.. hitTargetEntityIdList]);
            }
            else
            {
                // We had no targets for some reason
                connection.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
            }
        }
    }
}
