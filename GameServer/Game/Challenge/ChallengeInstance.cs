using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database.Challenge;
using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Challenge;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Util;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace EggLink.DanhengServer.Game.Challenge
{
    public class ChallengeInstance
    {
        public Position StartPos { get; set; }
        public Position StartRot { get; set; }
        public int ChallengeId { get; set; }
        public int CurrentStage { get; set; }
        public int CurrentExtraLineup { get; set; }
        public int Status { get; set; }
        public bool HasAvatarDied { get; set; }

        public int SavedMp { get; set; }
        public int RoundsLeft { get; set; }
        public int Stars { get; set; }
        public int ScoreStage1 { get; set; }
        public int ScoreStage2 { get; set; }

        [JsonIgnore]
        public List<BattleTarget>? BossTarget1 { get; set; }
        [JsonIgnore]
        public List<BattleTarget>? BossTarget2 { get; set; }
        [JsonIgnore]
        PlayerInstance Player { get; set; }
        [JsonIgnore]
        public ChallengeConfigExcel Excel { get; set; }

        public List<int> StoryBuffs { get; set; } = [];
        public List<int> BossBuffs { get; set; } = [];

        public ChallengeInstance(PlayerInstance player, ChallengeConfigExcel excel)
        {
            Player = player;
            Excel = excel;
            ChallengeId = excel.GetId();
            StartPos = new Position();
            StartRot = new Position();
            CurrentStage = 1;
            RoundsLeft = Excel.IsStory() ? 5 : Excel.ChallengeCountDown;
            SetStatus(ChallengeStatus.ChallengeDoing);
            SetCurrentExtraLineup(ExtraLineupType.LineupChallenge);
        }

        public ChallengeInstance(PlayerInstance player, ChallengeConfigExcel excel, ChallengeInstanceData data)
        {
            Player = player;
            Excel = excel;

            StartPos = data.StartPos;
            StartRot = data.StartRot;
            ChallengeId = data.ChallengeId;
            CurrentStage = data.CurrentStage;
            CurrentExtraLineup = data.CurrentExtraLineup;
            Status = data.Status;
            HasAvatarDied = data.HasAvatarDied;
            SavedMp = data.SavedMp;
            RoundsLeft = data.RoundsLeft;
            Stars = data.Stars;
            ScoreStage1 = data.ScoreStage1;
            ScoreStage2 = data.ScoreStage2;
            StoryBuffs = data.StoryBuffs;
            BossBuffs = data.BossBuffs;
        }

        public SceneInstance GetScene()
        {
            return Player.SceneInstance!;
        }

        public int GetChallengeId()
        {
            return Excel.GetId();
        }

        public bool IsStory()
        {
            return Excel.IsStory();
        }

        // Early implementation for 2.3
        /* public bool IsBoss()
        {
            return Excel.IsBoss();
        } */

        public void SetStatus(ChallengeStatus status)
        {
            Status = (int)status;
        }

        public void SetCurrentExtraLineup(ExtraLineupType type)
        {
            CurrentExtraLineup = (int)type;
        }

        public int GetRoundsElapsed()
        {
            return Excel.ChallengeCountDown - RoundsLeft;
        }

        public int GetTotalScore()
        {
            return ScoreStage1 + ScoreStage2;
        }

        public bool IsWin()
        {
            return Status == (int)ChallengeStatus.ChallengeFinish;
        }

        #region Management

        public void OnBattleStart(BattleInstance battle)
        {
            battle.RoundLimit = RoundsLeft;
            
            if (StoryBuffs != null)
            {
                battle.Buffs.Add(new MazeBuff(Excel.MazeBuffID, 1, -1));

                if (StoryBuffs.Count >= CurrentStage)
                {
                    int buffId = StoryBuffs[CurrentStage - 1];
                    battle.Buffs.Add(new MazeBuff(buffId, 1, -1));
                }
            }

            if (Excel.StoryExcel != null)
            {
                battle.AddBattleTarget(1, 10001, GetTotalScore());

                foreach (var id in Excel.StoryExcel.BattleTargetID!)
                {
                    battle.AddBattleTarget(5, id, GetTotalScore());
                }
            }
        }

        public virtual void OnBattleEnd(BattleInstance battle, PVEBattleResultCsReq req)
        {
            if (IsStory())
            {
                // Calculate score for current stage
                int stageScore = (int)req.Stt.ChallengeScore - GetTotalScore();

                // Set score
                if (CurrentStage == 1)
                {
                    ScoreStage1 = stageScore;
                } else
                {
                    ScoreStage2 = stageScore;
                }
            }

            switch (req.EndStatus)
            {
                case BattleEndStatus.BattleEndWin:
                    // Check if any avatar in the lineup has died
                    foreach (var avatar in battle.Lineup.AvatarData!.Avatars)
                    {
                        if (avatar.CurrentHp <= 0)
                        {
                            HasAvatarDied = true;
                        }
                    }

                    // Get monster count in stage
                    long monsters = Player.SceneInstance!.Entities.Values.OfType<EntityMonster>().Count();

                    if (monsters == 0)
                    {
                        AdvanceStage();
                    }

                    // Calculate rounds left
                    if (IsStory())
                    {
                        RoundsLeft = (int)Math.Min(Math.Max(RoundsLeft - req.Stt.RoundCnt, 1), RoundsLeft);
                    }

                    // Set saved technique points (This will be restored if the player resets the challenge)
                    SavedMp = Player.LineupManager!.GetCurLineup()!.Mp;
                    break;
                case BattleEndStatus.BattleEndQuit:
                    // Reset technique points and move back to start position
                    var lineup = Player.LineupManager!.GetCurLineup()!;
                    lineup.Mp = SavedMp;
                    Player.MoveTo(StartPos, StartRot);
                    Player.SendPacket(new PacketSyncLineupNotify(lineup));
                    break;
                default:
                    // Determine challenge result
                    if ((IsStory()/* || IsBoss()*/) && req.Stt.EndReason == BattleEndReason.TurnLimit)
                    {
                        AdvanceStage();
                    }
                    else
                    {
                        // Fail challenge
                        Status = (int)ChallengeStatus.ChallengeFailed;

                        // Send challenge result data
                        Player.SendPacket(new PacketChallengeSettleNotify(this));
                    }
                    break;
            }
        }

        private void AdvanceStage()
        {
            if (CurrentStage >= Excel.StageNum)
            {
                // Last stage
                Status = (int)ChallengeStatus.ChallengeFinish;
                Stars = CalculateStars();

                // Save history
                Player.ChallengeManager!.AddHistory(ChallengeId, Stars, GetTotalScore());

                // Send challenge result data
                Player.SendPacket(new PacketChallengeSettleNotify(this)); // Deprecated in 2.3
                // Early implementation for 2.3
                /* if (IsBoss())
                {
                    Player.SendPacket(new PacketChallengeBossPhaseSettleNotify(this));
                }
                else
                {
                    Player.SendPacket(new PacketChallengeSettleNotify(this));
                } */
            }
            else
            {
                // Increment and reset stage
                CurrentStage++;

                // Load scene group for stage 2
                Player.SceneInstance!.EntityLoader!.LoadGroup(Excel.MazeGroupID2);

                // Change player line up
                SetCurrentExtraLineup(ExtraLineupType.LineupChallenge2);
                Player.LineupManager!.SetCurLineup(CurrentExtraLineup + 10);
                Player.SendPacket(new PacketChallengeLineupNotify((ExtraLineupType)CurrentExtraLineup));
                SavedMp = Player.LineupManager.GetCurLineup()!.Mp;

                // Move player
                Player.MoveTo(StartPos, StartRot);
            }
        }

        public void OnUpdate()
        {
            // End challenge if its done
            if (Status != (int)ChallengeStatus.ChallengeDoing)
            {
                Player.ChallengeManager!.ChallengeInstance = null;
            }
        }

        public int CalculateStars()
        {
            List<int> targets = Excel.ChallengeTargetID!;
            int stars = 0;

            for (int i = 0; i < targets.Count; i++)
            {
                if (!GameData.ChallengeTargetData.ContainsKey(targets[i])) continue;

                var target = GameData.ChallengeTargetData[targets[i]];

                switch (target.ChallengeTargetType)
                {
                    case ChallengeTargetExcel.ChallengeType.ROUNDS_LEFT:
                        if (RoundsLeft >= target.ChallengeTargetParam1)
                        {
                            stars += (1 << i);
                        }
                        break;
                    case ChallengeTargetExcel.ChallengeType.DEAD_AVATAR:
                        if (!HasAvatarDied)
                        {
                            stars += (1 << i);
                        }
                        break;
                    case ChallengeTargetExcel.ChallengeType.TOTAL_SCORE:
                        if (GetTotalScore() >= target.ChallengeTargetParam1)
                        {
                            stars += (1 << i);
                        }
                        break;
                    default:
                        break;
                }
            }

            return Math.Min(stars, 7);
        }

        #endregion

        #region Serialization

        public CurChallenge ToProto()
        {
            CurChallenge proto = new CurChallenge()
            {
                ChallengeId = (uint)Excel.GetId(),
                Status = (ChallengeStatus)Status,
                ScoreId = (uint)ScoreStage1,
                ScoreTwo = (uint)ScoreStage2,
                RoundCount = (uint)GetRoundsElapsed(),
                ExtraLineupType = (ExtraLineupType)CurrentExtraLineup,
                PlayerInfo = new ChallengeStoryInfo() { CurStoryBuff = new ChallengeStoryBuffInfo() }
            };

            if (StoryBuffs != null && StoryBuffs.Count >= CurrentStage)
            {
                proto.PlayerInfo.CurStoryBuff.BuffList.Add(StoryBuffs.Select(x => (uint)x));
            }

            // Early implementation for 2.3
            /* if (StoryBuffs != null && StoryBuffs.Count >= CurrentStage)
            {
                proto.PlayerInfo.CurBossBuff.BuffList.Add((uint)BossBuffs[CurrentStage - 1]);
            } */

            return proto;
        }

        #endregion
    }
}
