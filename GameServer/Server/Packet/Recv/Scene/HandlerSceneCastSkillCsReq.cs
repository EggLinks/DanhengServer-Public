using EggLink.DanhengServer.GameServer.Game.Battle.Skill;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.SceneCastSkillCsReq)]
public class HandlerSceneCastSkillCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SceneCastSkillCsReq.Parser.ParseFrom(data);

        var player = connection.Player!;
        MazeSkill mazeSkill = new([], req);

        // Get casting avatar
        connection.Player!.SceneInstance!.AvatarInfo.TryGetValue((int)req.AttackedByEntityId, out var caster);

        if (caster != null)
        {
            // Check if normal attack or technique was used
            if (req.SkillIndex > 0)
            {
                // Cast skill effects
                if (caster.AvatarInfo.Excel != null && caster.AvatarInfo.Excel!.MazeSkill != null)
                {
                    mazeSkill = MazeSkillManager.GetSkill(caster.AvatarInfo.GetAvatarId(), (int)req.SkillIndex, req);
                    mazeSkill.OnCast(caster, player);
                }
            }
            else
            {
                mazeSkill = MazeSkillManager.GetSkill(caster.AvatarInfo.GetAvatarId(), 0, req);
            }
        }

        if (req.AssistMonsterEntityIdList.Count > 0)
        {
            if (caster != null && caster.AvatarInfo.AvatarId == 1218 && req.SkillIndex == 1)
            {
                // Avoid Jiqoqiu's E skill
                await connection.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
            }
            else
            {
                var hitTargetEntityIdList = new List<uint>();
                if (req.AssistMonsterEntityIdList.Count > 0)
                    foreach (var id in req.AssistMonsterEntityIdList)
                        hitTargetEntityIdList.Add(id);
                else
                    foreach (var id in req.HitTargetEntityIdList)
                        hitTargetEntityIdList.Add(id);
                // Start battle
                await connection.Player!.BattleManager!.StartBattle(req, mazeSkill!, [.. hitTargetEntityIdList]);
            }
        }
        else
        {
            // We had no targets for some reason
            await connection.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
        }
    }
}