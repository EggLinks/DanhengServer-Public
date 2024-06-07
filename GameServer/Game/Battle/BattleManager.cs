using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Game.Battle.Skill;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Battle;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Game.Battle
{
    public class BattleManager(PlayerInstance player) : BasePlayerManager(player)
    {
        public void StartBattle(SceneCastSkillCsReq req, MazeSkill skill)
        {
            if (Player.BattleInstance != null) return;
            var targetList = new List<EntityMonster>();
            var avatarList = new List<AvatarSceneInfo>();
            var propList = new List<EntityProp>();
            Player.SceneInstance!.AvatarInfo.TryGetValue((int)req.AttackedByEntityId, out var castAvatar);

            if (Player.SceneInstance!.AvatarInfo.ContainsKey((int)req.AttackedByEntityId))
            {
                foreach (var entity in req.HitTargetEntityIdList)
                {
                    Player.SceneInstance!.Entities.TryGetValue((int)entity, out var entityInstance);
                    if (entityInstance is EntityMonster monster)
                    {
                        targetList.Add(monster);
                    } else if (entityInstance is EntityProp prop)
                    {
                        propList.Add(prop);
                    }
                }

                foreach (var info in req.AssistMonsterEntityInfo)
                {
                    foreach (var entity in info.EntityIdList)
                    {
                        Player.SceneInstance!.Entities.TryGetValue((int)entity, out var entityInstance);
                        if (entityInstance is EntityMonster monster)
                        {
                            if (targetList.Contains(monster)) continue;  // avoid adding the same monster twice
                            targetList.Add(monster);
                        }
                    }
                }
            } else
            {
                bool isAmbushed = false;
                foreach (var entity in req.HitTargetEntityIdList)
                {
                    if (Player.SceneInstance!.AvatarInfo.ContainsKey((int)entity)) 
                    {
                        isAmbushed = true;
                        break;
                    }
                }
                if (!isAmbushed)
                {
                    Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
                    return;
                }
                var monsterEntity = Player.SceneInstance!.Entities[(int)req.AttackedByEntityId];
                if (monsterEntity is EntityMonster monster)
                {
                    targetList.Add(monster);
                }
            }
            if (targetList.Count == 0 && propList.Count == 0)
            {
                Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
                return;
            }

            foreach (var prop in propList)
            {
                Player.SceneInstance!.RemoveEntity(prop);
                if (prop.Excel.IsMpRecover)
                {
                    Player.LineupManager!.GainMp(2);
                } else if (prop.Excel.IsHpRecover)
                {
                    Player.LineupManager!.GetCurLineup()!.Heal(2000, false);
                    Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
                } else
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
                    Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
                    return;
                }
                if (castAvatar != null)
                {
                    skill.OnAttack(Player.SceneInstance!.AvatarInfo[(int)req.AttackedByEntityId], targetList);
                    skill.OnCast(castAvatar);
                }

                var triggerBattle = false;
                foreach (var target in targetList)
                {
                    if (target.IsAlive)
                    {
                        triggerBattle = true;
                        break;
                    }
                }
                if (!triggerBattle)
                {
                    Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
                    return;
                }

                BattleInstance battleInstance = new(Player, Player.LineupManager!.GetCurLineup()!, targetList)
                {
                    WorldLevel = Player.Data.WorldLevel,
                };

                foreach (var item in Player.LineupManager!.GetCurLineup()!.BaseAvatars!)  // get all avatars in the lineup and add them to the battle instance
                {
                    var avatar = Player.SceneInstance!.AvatarInfo.Values.FirstOrDefault(x => x.AvatarInfo.AvatarId == item.BaseAvatarId);
                    if (avatar != null)
                    {
                        avatarList.Add(avatar);
                    }
                }

                MazeBuff mazeBuff;
                if (castAvatar != null)
                {
                    var index = battleInstance.Lineup.BaseAvatars!.FindIndex(x => x.BaseAvatarId == castAvatar.AvatarInfo.AvatarId);
                    mazeBuff = new((int?)castAvatar.AvatarInfo.Excel?.DamageType ?? 0, 1, index);
                    mazeBuff.DynamicValues.Add("SkillIndex", skill.IsMazeSkill ? 2 : 1);
                } else
                {
                    mazeBuff = new(GameConstants.AMBUSH_BUFF_ID, 1, -1)
                    { 
                        WaveFlag = 1
                    };
                }

                if (mazeBuff.BuffID != 0)  // avoid adding a buff with ID 0
                {
                    battleInstance.Buffs.Add(mazeBuff);
                }

                battleInstance.AvatarInfo = avatarList;

                // call battle start
                Player.RogueManager!.GetRogueInstance()?.OnBattleStart(battleInstance);
                Player.ChallengeManager!.ChallengeInstance?.OnBattleStart(battleInstance);

                Player.BattleInstance = battleInstance;
                Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId, battleInstance));
            } else
            {
                Player.SendPacket(new PacketSceneCastSkillScRsp(req.CastEntityId));
            }
        }

        public void StartStage(int eventId)
        {
            if (Player.BattleInstance != null)
            {
                Player.SendPacket(new PacketSceneEnterStageScRsp(Player.BattleInstance));
                return;
            }

            GameData.StageConfigData.TryGetValue(eventId, out var stageConfig);
            if (stageConfig == null)
            {
                GameData.StageConfigData.TryGetValue(eventId * 10 + Player.Data.WorldLevel, out stageConfig);
                if (stageConfig == null)
                {
                    Player.SendPacket(new PacketSceneEnterStageScRsp());
                    return;
                }
            }

            BattleInstance battleInstance = new(Player, Player.LineupManager!.GetCurLineup()!, [stageConfig])
            {
                WorldLevel = Player.Data.WorldLevel,
                EventId = eventId,
            };

            var avatarList = new List<AvatarSceneInfo>();

            foreach (var item in Player.LineupManager!.GetCurLineup()!.BaseAvatars!)  // get all avatars in the lineup and add them to the battle instance
            {
                var avatar = Player.SceneInstance!.AvatarInfo.Values.FirstOrDefault(x => x.AvatarInfo.AvatarId == item.BaseAvatarId);
                if (avatar != null)
                {
                    avatarList.Add(avatar);
                }
            }

            battleInstance.AvatarInfo = avatarList;

            // call battle start
            Player.RogueManager!.GetRogueInstance()?.OnBattleStart(battleInstance);
            Player.ChallengeManager!.ChallengeInstance?.OnBattleStart(battleInstance);

            Player.BattleInstance = battleInstance;

            Player.SendPacket(new PacketSceneEnterStageScRsp(battleInstance));
        }

        public void StartCocoonStage(int cocoonId, int wave, int worldLevel)
        {
            if (Player.BattleInstance != null) return;

            GameData.CocoonConfigData.TryGetValue(cocoonId * 100 + worldLevel, out var config);
            if (config == null)
            {
                Player.SendPacket(new PacketStartCocoonStageScRsp());
                return;
            }
            wave = Math.Min(Math.Max(wave, 1), config.MaxWave);

            int cost = config.StaminaCost * wave;
            if (Player.Data.Stamina < cost)
            {
                Player.SendPacket(new PacketStartCocoonStageScRsp());
                return;
            }

            List<StageConfigExcel> stageConfigExcels = [];
            for (int i = 0; i < wave; i++)
            {
                var stageId = config.StageIDList.RandomElement();
                GameData.StageConfigData.TryGetValue(stageId, out var stageConfig);
                if (stageConfig == null) continue;

                stageConfigExcels.Add(stageConfig);
            }

            if (stageConfigExcels.Count == 0)
            {
                Player.SendPacket(new PacketStartCocoonStageScRsp());
                return;
            }

            BattleInstance battleInstance = new(Player, Player.LineupManager!.GetCurLineup()!, stageConfigExcels)
            {
                StaminaCost = cost,
                WorldLevel = config.WorldLevel,
                CocoonWave = wave,
                MappingInfoId = config.MappingInfoID,
            };

            var avatarList = new List<AvatarSceneInfo>();

            foreach (var item in Player.LineupManager!.GetCurLineup()!.BaseAvatars!)  // get all avatars in the lineup and add them to the battle instance
            {
                var avatar = Player.SceneInstance!.AvatarInfo.Values.FirstOrDefault(x => x.AvatarInfo.AvatarId == item.BaseAvatarId);
                if (avatar != null)
                {
                    avatarList.Add(avatar);
                }
            }

            battleInstance.AvatarInfo = avatarList;

            Player.BattleInstance = battleInstance;

            Player.SendPacket(new PacketStartCocoonStageScRsp(battleInstance, cocoonId, wave));
        }

        public void EndBattle(PVEBattleResultCsReq req)
        {
            if (Player.BattleInstance == null)
            {
                Player.SendPacket(new PacketPVEBattleResultScRsp());
                return;
            }
            Player.BattleInstance.BattleEndStatus = req.EndStatus;
            var battle = Player.BattleInstance;
            bool updateStatus = true;
            bool teleportToAnchor = false;
            var minimumHp = 0;
            var dropItems = new List<ItemData>();
            switch (req.EndStatus)
            {
                case BattleEndStatus.BattleEndWin:
                    // Drops
                    foreach (var monster in battle.EntityMonsters)
                    {
                        dropItems.AddRange(monster.Kill(false));
                    }
                    // Spend stamina
                    if (battle.StaminaCost > 0)
                    {
                        Player.SpendStamina(battle.StaminaCost);
                    }
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
                foreach (var avatar in req.Stt.AvatarBattleList)
                {
                    var avatarInstance = Player.AvatarManager!.GetAvatar((int)avatar.Id);
                    var prop = avatar.AvatarStatus;
                    int curHp = (int)Math.Max(Math.Round(prop.LeftHp / prop.MaxHp * 10000), minimumHp);
                    int curSp = (int)prop.LeftSp * 100;
                    if (avatarInstance == null)
                    {
                        GameData.SpecialAvatarData.TryGetValue((int)(avatar.Id * 10 + Player.Data.WorldLevel), out var specialAvatar);
                        if (specialAvatar == null) continue;
                        specialAvatar.CurHp[Player.Uid] = curHp;
                        specialAvatar.CurSp[Player.Uid] = curSp;
                        avatarInstance.SetCurHp(curHp, lineup.LineupType != 0);
                        avatarInstance.SetCurSp(curSp, lineup.LineupType != 0);
                    } else
                    {
                        avatarInstance.SetCurHp(curHp, lineup.LineupType != 0);
                        avatarInstance.SetCurSp(curSp, lineup.LineupType != 0);
                    }
                }

                DatabaseHelper.Instance?.UpdateInstance(Player.AvatarManager!.AvatarData!);
                Player.SendPacket(new PacketSyncLineupNotify(lineup));
            }
            if (teleportToAnchor)
            {
                var anchorProp = Player.SceneInstance?.GetNearestSpring(long.MaxValue);
                if (anchorProp != null && anchorProp.PropInfo != null)
                {
                    var anchor = Player!.SceneInstance?.FloorInfo?.GetAnchorInfo(
                            anchorProp.PropInfo.AnchorGroupID,
                            anchorProp.PropInfo.AnchorID
                    );
                    if (anchor != null)
                    {
                        Player.MoveTo(anchor.ToPositionProto());
                    }
                }
            }
            // call battle end
            battle.MonsterDropItems = dropItems;

            Player.BattleInstance = null;

            Player.MissionManager!.OnBattleFinish(req);
            Player.RogueManager!.GetRogueInstance()?.OnBattleEnd(battle, req);
            Player.ChallengeManager!.ChallengeInstance?.OnBattleEnd(battle, req);

            Player.SendPacket(new PacketPVEBattleResultScRsp(req, Player, battle));
        }
    }
}
