using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.GameServer.Game.Battle.Skill;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Battle;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using Microsoft.Extensions.Logging;

namespace EggLink.DanhengServer.GameServer.Game.Battle;

public class BattleManager(PlayerInstance player) : BasePlayerManager(player)
{
    public async ValueTask StartBattle(SceneCastSkillCsReq req, MazeSkill skill, List<uint> hitTargetEntityIdList)
    {
        if (Player.BattleInstance != null) return;
        var targetList = new List<EntityMonster>();
        var avatarList = new List<AvatarSceneInfo>();
        var propList = new List<EntityProp>();
        Player.SceneInstance!.AvatarInfo.TryGetValue((int)req.AttackedByEntityId, out var castAvatar);

        if (Player.SceneInstance!.AvatarInfo.ContainsKey((int)req.AttackedByEntityId))
        {
            foreach (var entity in hitTargetEntityIdList)
            {
                Player.SceneInstance!.Entities.TryGetValue((int)entity, out var entityInstance);
                switch (entityInstance)
                {
                    case EntityMonster monster:
                        targetList.Add(monster);
                        break;
                    case EntityProp prop:
                        propList.Add(prop);
                        break;
                }
            }

            foreach (var info in req.AssistMonsterEntityInfo)
            foreach (var entity in info.EntityIdList)
            {
                Player.SceneInstance!.Entities.TryGetValue((int)entity, out var entityInstance);
                if (entityInstance is not EntityMonster monster) continue;
                if (targetList.Contains(monster)) continue; // avoid adding the same monster twice
                targetList.Add(monster);
            }
        }
        else
        {
            var isAmbushed = hitTargetEntityIdList.Any(entity => Player.SceneInstance!.AvatarInfo.ContainsKey((int)entity));

            if (!isAmbushed)
            {
                await Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
                return;
            }

            var monsterEntity = Player.SceneInstance!.Entities[(int)req.AttackedByEntityId];
            if (monsterEntity is EntityMonster monster) targetList.Add(monster);
        }

        if (targetList.Count == 0 && propList.Count == 0)
        {
            await Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
            return;
        }

        foreach (var prop in propList)
        {
            await Player.SceneInstance!.RemoveEntity(prop);
            if (prop.Excel.IsMpRecover)
            {
                await Player.LineupManager!.GainMp(2, true, SyncLineupReason.SyncReasonMpAddPropHit);
            }
            else if (prop.Excel.IsHpRecover)
            {
                Player.LineupManager!.GetCurLineup()!.Heal(2000, false);
                await Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
            }
            else
            {
                Player.InventoryManager!.HandlePlaneEvent(prop.PropInfo.EventID);
            }

            Player.RogueManager!.GetRogueInstance()?.OnPropDestruct(prop);
        }

        if (targetList.Count > 0)
        {
            // Skill handle
            if (!skill.TriggerBattle)
            {
                skill.OnHitTarget(Player.SceneInstance!.AvatarInfo[(int)req.AttackedByEntityId], targetList);
                await Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
                return;
            }

            if (castAvatar != null)
            {
                skill.OnAttack(Player.SceneInstance!.AvatarInfo[(int)req.AttackedByEntityId], targetList);
                skill.OnCast(castAvatar, Player);
            }

            var triggerBattle = targetList.Any(target => target.IsAlive);

            if (!triggerBattle)
            {
                await Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
                return;
            }

            BattleInstance battleInstance = new(Player, Player.LineupManager!.GetCurLineup()!, targetList)
            {
                WorldLevel = Player.Data.WorldLevel
            };

            avatarList.AddRange(Player.LineupManager!.GetCurLineup()!.BaseAvatars!
                .Select(item => Player.SceneInstance!.AvatarInfo.Values.FirstOrDefault(x => x.AvatarInfo.AvatarId == item.BaseAvatarId))
                .OfType<AvatarSceneInfo>());

            MazeBuff? mazeBuff = null;
            if (castAvatar != null)
            {
                var index = battleInstance.Lineup.BaseAvatars!.FindIndex(x =>
                    x.BaseAvatarId == castAvatar.AvatarInfo.AvatarId);
                GameData.AvatarConfigData.TryGetValue(castAvatar.AvatarInfo.GetAvatarId(), out var avatarExcel);
                if (avatarExcel != null)
                {
                    mazeBuff = new MazeBuff((int)avatarExcel.DamageType, 1, index);
                    mazeBuff.DynamicValues.Add("SkillIndex", skill.IsMazeSkill ? 2 : 1);
                }
            }
            else
            {
                mazeBuff = new MazeBuff(GameConstants.AMBUSH_BUFF_ID, 1, -1)
                {
                    WaveFlag = 1
                };
            }

            if (mazeBuff != null && mazeBuff.BuffID != 0) // avoid adding a buff with ID 0
                battleInstance.Buffs.Add(mazeBuff);

            battleInstance.AvatarInfo = avatarList;

            // call battle start
            Player.RogueManager!.GetRogueInstance()?.OnBattleStart(battleInstance);
            Player.ChallengeManager!.ChallengeInstance?.OnBattleStart(battleInstance);
            Player.QuestManager!.OnBattleStart(battleInstance);

            Player.BattleInstance = battleInstance;
            await Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId, battleInstance));
            Player.SceneInstance?.ClearSummonUnit();
        }
        else
        {
            await Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
        }
    }

    public async ValueTask StartStage(int eventId)
    {
        if (Player.BattleInstance != null)
        {
            await Player.SendPacket(new PacketSceneEnterStageScRsp(Player.BattleInstance));
            return;
        }

        GameData.StageConfigData.TryGetValue(eventId, out var stageConfig);
        if (stageConfig == null)
        {
            GameData.StageConfigData.TryGetValue(eventId * 10 + Player.Data.WorldLevel, out stageConfig);
            if (stageConfig == null)
            {
                await Player.SendPacket(new PacketSceneEnterStageScRsp());
                return;
            }
        }

        BattleInstance battleInstance = new(Player, Player.LineupManager!.GetCurLineup()!, [stageConfig])
        {
            WorldLevel = Player.Data.WorldLevel,
            EventId = eventId
        };

        var avatarList = Player.LineupManager!.GetCurLineup()!.BaseAvatars!.Select(item => Player.SceneInstance!.AvatarInfo.Values.FirstOrDefault(x => x.AvatarInfo.AvatarId == item.BaseAvatarId)).OfType<AvatarSceneInfo>().ToList();

        battleInstance.AvatarInfo = avatarList;

        // call battle start
        Player.RogueManager!.GetRogueInstance()?.OnBattleStart(battleInstance);
        Player.ChallengeManager!.ChallengeInstance?.OnBattleStart(battleInstance);
        Player.QuestManager!.OnBattleStart(battleInstance);

        Player.BattleInstance = battleInstance;

        await Player.SendPacket(new PacketSceneEnterStageScRsp(battleInstance));
        Player.SceneInstance?.ClearSummonUnit();
    }

    public async ValueTask StartCocoonStage(int cocoonId, int wave, int worldLevel)
    {
        if (Player.BattleInstance != null) return;

        GameData.CocoonConfigData.TryGetValue(cocoonId * 100 + worldLevel, out var config);
        if (config == null)
        {
            await Player.SendPacket(new PacketStartCocoonStageScRsp());
            return;
        }

        wave = Math.Min(Math.Max(wave, 1), config.MaxWave);

        var cost = config.StaminaCost * wave;
        if (Player.Data.Stamina < cost)
        {
            await Player.SendPacket(new PacketStartCocoonStageScRsp());
            return;
        }

        List<StageConfigExcel> stageConfigExcels = [];
        for (var i = 0; i < wave; i++)
        {
            var stageId = config.StageIDList.RandomElement();
            GameData.StageConfigData.TryGetValue(stageId, out var stageConfig);
            if (stageConfig == null) continue;

            stageConfigExcels.Add(stageConfig);
        }

        if (stageConfigExcels.Count == 0)
        {
            await Player.SendPacket(new PacketStartCocoonStageScRsp());
            return;
        }

        BattleInstance battleInstance = new(Player, Player.LineupManager!.GetCurLineup()!, stageConfigExcels)
        {
            StaminaCost = cost,
            WorldLevel = config.WorldLevel,
            CocoonWave = wave,
            MappingInfoId = config.MappingInfoID
        };

        var avatarList = Player.LineupManager!.GetCurLineup()!.BaseAvatars!.Select(item => Player.SceneInstance!.AvatarInfo.Values.FirstOrDefault(x => x.AvatarInfo.AvatarId == item.BaseAvatarId)).OfType<AvatarSceneInfo>().ToList();

        battleInstance.AvatarInfo = avatarList;

        Player.BattleInstance = battleInstance;
        Player.QuestManager!.OnBattleStart(battleInstance);

        await Player.SendPacket(new PacketStartCocoonStageScRsp(battleInstance, cocoonId, wave));
        Player.SceneInstance?.ClearSummonUnit();
    }

    public (Retcode, BattleInstance?) StartBattleCollege(int collegeId)
    {
        if (Player.BattleInstance != null)
        {
            return (Retcode.RetInBattleNow, null);
        }

        GameData.BattleCollegeConfigData.TryGetValue(collegeId, out var config);
        if (config == null) return (Retcode.RetFail, null);

        var stageId = config.StageID;

        GameData.StageConfigData.TryGetValue(stageId, out var stageConfig);
        if (stageConfig == null) return (Retcode.RetStageConfigNotExist, null);

        BattleInstance battleInstance = new(Player, Player.LineupManager!.GetCurLineup()!, [stageConfig])
        {
            WorldLevel = Player.Data.WorldLevel,
            CollegeConfigExcel = config,
            AvatarInfo = []
        };

        // call battle start
        Player.RogueManager!.GetRogueInstance()?.OnBattleStart(battleInstance);
        Player.ChallengeManager!.ChallengeInstance?.OnBattleStart(battleInstance);
        Player.QuestManager!.OnBattleStart(battleInstance);

        Player.BattleInstance = battleInstance;

        return (Retcode.RetSucc, battleInstance);
    }

    public async ValueTask EndBattle(PVEBattleResultCsReq req)
    {
        if (Player.BattleInstance == null)
        {
            await Player.SendPacket(new PacketPVEBattleResultScRsp());
            return;
        }

        Player.BattleInstance.BattleEndStatus = req.EndStatus;
        var battle = Player.BattleInstance;
        var updateStatus = true;
        var teleportToAnchor = false;
        var minimumHp = 0;
        var dropItems = new List<ItemData>();
        switch (req.EndStatus)
        {
            case BattleEndStatus.BattleEndWin:
                // Drops
                foreach (var monster in battle.EntityMonsters) dropItems.AddRange(await monster.Kill(false));
                // Spend stamina
                if (battle.StaminaCost > 0) await Player.SpendStamina(battle.StaminaCost);
                break;
            case BattleEndStatus.BattleEndLose:
                // Set avatar hp to 20% if the player's party is downed
                minimumHp = 2000;
                teleportToAnchor = true;
                break;
            default:
                teleportToAnchor = true;
                if (battle.CocoonWave > 0) teleportToAnchor = false;
                updateStatus = false;
                break;
        }

        if (updateStatus)
        {
            var lineup = Player.LineupManager!.GetCurLineup()!;
            // Update battle status
            foreach (var avatar in req.Stt.BattleAvatarList)
            {
                var avatarInstance = Player.AvatarManager!.GetAvatar((int)avatar.Id);
                var prop = avatar.AvatarStatus;
                var curHp = (int)Math.Max(Math.Round(prop.LeftHp / prop.MaxHp * 10000), minimumHp);
                var curSp = (int)prop.LeftSp * 100;
                if (avatarInstance == null)
                {
                    GameData.SpecialAvatarData.TryGetValue((int)(avatar.Id * 10 + Player.Data.WorldLevel),
                        out var specialAvatar);
                    if (specialAvatar == null) continue;
                    specialAvatar.CurHp[Player.Uid] = curHp;
                    specialAvatar.CurSp[Player.Uid] = curSp;
                    avatarInstance?.SetCurHp(curHp, lineup.LineupType != 0);
                    avatarInstance?.SetCurSp(curSp, lineup.LineupType != 0);
                }
                else
                {
                    avatarInstance.SetCurHp(curHp, lineup.LineupType != 0);
                    avatarInstance.SetCurSp(curSp, lineup.LineupType != 0);
                }
            }

            await Player.SendPacket(new PacketSyncLineupNotify(lineup));
        }

        if (teleportToAnchor)
        {
            var anchorProp = Player.SceneInstance?.GetNearestSpring(long.MaxValue);
            if (anchorProp != null)
            {
                var anchor = Player.SceneInstance?.FloorInfo?.GetAnchorInfo(
                    anchorProp.PropInfo.AnchorGroupID,
                    anchorProp.PropInfo.AnchorID
                );
                if (anchor != null) await Player.MoveTo(anchor.ToPositionProto());
            }
        }

        // call battle end
        battle.MonsterDropItems = dropItems;
        battle.BattleResult = req;

        Player.BattleInstance = null;

        await Player.MissionManager!.OnBattleFinish(req, battle);
        if (Player.RogueManager?.GetRogueInstance() != null)
            await Player.RogueManager!.GetRogueInstance()!.OnBattleEnd(battle, req);

        if (Player.ChallengeManager?.ChallengeInstance != null)
            await Player.ChallengeManager!.ChallengeInstance.OnBattleEnd(battle, req);

        if (Player.ActivityManager!.TrialActivityInstance != null && req.EndStatus == BattleEndStatus.BattleEndWin)
            await Player.ActivityManager.TrialActivityInstance.EndActivity(TrialActivityStatus.Finish);

        await Player.SendPacket(new PacketPVEBattleResultScRsp(req, Player, battle));
    }
}