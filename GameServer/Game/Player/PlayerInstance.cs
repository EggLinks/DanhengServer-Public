using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Database.Scene;
using EggLink.DanhengServer.Database.Tutorial;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Game.Activity;
using EggLink.DanhengServer.Game.Avatar;
using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Game.Friend;
using EggLink.DanhengServer.Game.ChessRogue;
using EggLink.DanhengServer.Game.Gacha;
using EggLink.DanhengServer.Game.Inventory;
using EggLink.DanhengServer.Game.Lineup;
using EggLink.DanhengServer.Game.Message;
using EggLink.DanhengServer.Game.Mission;
using EggLink.DanhengServer.Game.Rogue;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Game.Shop;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using System.Numerics;
using EggLink.DanhengServer.Game.Challenge;
using EggLink.DanhengServer.Game.Drop;

namespace EggLink.DanhengServer.Game.Player
{
    public class PlayerInstance(PlayerData data)
    {
        #region Managers

        public ActivityManager? ActivityManager { get; private set; }
        public AvatarManager? AvatarManager { get; private set; }
        public LineupManager? LineupManager { get; private set; }
        public InventoryManager? InventoryManager { get; private set; }
        public BattleManager? BattleManager { get; private set; }
        public BattleInstance? BattleInstance { get; set; }
        public MissionManager? MissionManager { get; private set; }
        public GachaManager? GachaManager { get; private set; }
        public MessageManager? MessageManager { get; private set; }

        public FriendManager? FriendManager { get; private set; }
        public RogueManager? RogueManager { get; private set; }
        public ChessRogueManager? ChessRogueManager { get; private set; }
        public ShopService? ShopService { get; private set; }
        public ChallengeManager? ChallengeManager { get; private set; }

        #endregion

        #region Datas

        public PlayerData Data { get; set; } = data;
        public PlayerUnlockData? PlayerUnlockData { get; private set; }
        public SceneData? SceneData { get; private set; }
        public TutorialData? TutorialData { get; private set; }
        public TutorialGuideData? TutorialGuideData { get; private set; }
        public SceneInstance? SceneInstance { get; private set; }
        public ushort Uid { get; set; }
        public Connection? Connection { get; set; }
        public bool Initialized { get; set; } = false;
        public bool IsNewPlayer { get; set; } = false;
        public int NextBattleId { get; set; } = 0;
        public int CurRaidId { get; set; } = 0;
        public int OldEntryId { get; set; } = 0;
        public int ChargerNum { get; set; } = 0;
        public Position? LastPos { get; set; }
        public Position? LastRot { get; set; }

        #endregion

        #region Initializers

        public PlayerInstance(int uid) : this(new PlayerData() { Uid = uid })
        {
            // new player
            IsNewPlayer = true;
            Data.NextStaminaRecover = Extensions.GetUnixSec() + GameConstants.STAMINA_RESERVE_RECOVERY_TIME;
            Data.Level = ConfigManager.Config.ServerOption.StartTrailblazerLevel;

            DatabaseHelper.SaveInstance(Data);

            InitialPlayerManager();

            AddAvatar(8001);
            AddAvatar(1001);
            
            if (ConfigManager.Config.ServerOption.EnableMission)
            {
                LineupManager?.AddSpecialAvatarToCurTeam(10010050);
                MissionManager!.AcceptMainMissionByCondition();
            } else
            {
                LineupManager?.AddAvatarToCurTeam(8001);
                Data.CurrentGender = Gender.Man;
                Data.CurBasicType = 8001;
            }

            Initialized = true;
        }

        private void InitialPlayerManager()
        {
            Uid = (ushort)Data.Uid;
            ActivityManager = new(this);
            AvatarManager = new(this);
            LineupManager = new(this);
            InventoryManager = new(this);
            BattleManager = new(this);
            MissionManager = new(this);
            GachaManager = new(this);
            MessageManager = new(this);
            FriendManager = new(this);
            RogueManager = new(this);
            ShopService = new(this);
            ChessRogueManager = new(this);
            ChallengeManager = new(this);

            PlayerUnlockData = InitializeDatabase<PlayerUnlockData>();
            SceneData = InitializeDatabase<SceneData>();
            TutorialData = InitializeDatabase<TutorialData>();
            TutorialGuideData = InitializeDatabase<TutorialGuideData>();

            Data.LastActiveTime = Extensions.GetUnixSec();
            DatabaseHelper.Instance?.UpdateInstance(Data);

            ChallengeManager.ResurrectInstance();
            LoadScene(Data.PlaneId, Data.FloorId, Data.EntryId, Data.Pos!, Data.Rot!, false);
            if (SceneInstance == null)
            {
                EnterScene(2000101, 0, false);
            }

            if (LineupManager!.GetCurLineup() != null)  // null -> ignore(new player)
            {
                if (LineupManager!.GetCurLineup()!.IsExtraLineup())  // do not use extra lineup when login
                {
                    LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);
                    if (LineupManager!.GetCurLineup()!.IsExtraLineup())
                    {
                        LineupManager!.SetCurLineup(0);
                    }
                }

                foreach (var lineup in LineupManager.LineupData.Lineups)
                {
                    if (lineup.Value.BaseAvatars!.Count >= 5)
                    {
                        lineup.Value.BaseAvatars = lineup.Value.BaseAvatars.GetRange(0, 4);
                    }
                }

                foreach (var avatar in LineupManager.GetCurLineup()!.BaseAvatars!)
                {
                    var avatarData = AvatarManager.GetAvatar(avatar.BaseAvatarId);
                    if (avatarData != null && avatarData.CurrentHp <= 0)
                    {
                        // revive
                        avatarData.CurrentHp = 2000;
                    }
                }
            }
        }

        public T InitializeDatabase<T>() where T : class, new()
        {
            var instance = DatabaseHelper.Instance?.GetInstanceOrCreateNew<T>(Uid);
            return instance!;
        }

        #endregion

        #region Network
        public void OnLogin()
        {
            if (!Initialized)
            {
                InitialPlayerManager();
            }
            
            SendPacket(new PacketStaminaInfoScNotify(this));

        }

        public void OnLogoutAsync()
        {
            if (CurRaidId > 0)
            {
                EnterScene(OldEntryId, 0, false);
                MoveTo(LastPos!, LastRot!);
            }
        }

        public void SendPacket(BasePacket packet)
        {
            if (Connection?.IsOnline == true)
            {
                Connection?.SendPacket(packet);
            }
        }
        #endregion

        #region Actions

        public void ChangeHeroBasicType(HeroBasicTypeEnum type)
        {
            var id = (int)((int)type + Data.CurrentGender - 1);
            if (Data.CurBasicType == id) return;
            Data.CurBasicType = id;
            AvatarManager!.GetHero()!.HeroId = id;
            AvatarManager!.GetHero()!.ValidateHero();
            SendPacket(new PacketHeroBasicTypeChangedNotify(id));
            SendPacket(new PacketPlayerSyncScNotify(AvatarManager!.GetHero()!));
        }

        public void AddAvatar(int avatarId, bool sync = true)
        {
            AvatarManager?.AddAvatar(avatarId, sync);
        }

        public void SpendStamina(int staminaCost)
        {
            Data.Stamina -= staminaCost;
            SendPacket(new PacketStaminaInfoScNotify(this));
        }

        public void OnAddExp()
        {
            GameData.PlayerLevelConfigData.TryGetValue(Data.Level, out var config);
            GameData.PlayerLevelConfigData.TryGetValue(Data.Level + 1, out var config2);
            if (config == null || config2 == null) return;
            var nextExp = config2.PlayerExp - config.PlayerExp;

            while (Data.Exp >= nextExp)
            {
                Data.Exp -= nextExp;
                Data.Level++;
                GameData.PlayerLevelConfigData.TryGetValue(Data.Level, out config);
                GameData.PlayerLevelConfigData.TryGetValue(Data.Level + 1, out config2);
                if (config == null || config2 == null) break;
                nextExp = config2.PlayerExp - config.PlayerExp;
            }

            OnLevelChange();
            DatabaseHelper.Instance?.UpdateInstance(Data);
        }

        public void OnLevelChange()
        {
            if (!ConfigManager.Config.ServerOption.AutoUpgradeWorldLevel) return;
            int worldLevel = 0;
            foreach (var level in GameConstants.UpgradeWorldLevel)
            {
                if (level <= Data.Level)
                {
                    worldLevel++;
                }
            }

            if (Data.WorldLevel != worldLevel)
            {
                Data.WorldLevel = worldLevel;
            }
        }

        public void OnStaminaRecover()
        {
            var sendPacket = false;
            while (Data.NextStaminaRecover <= Extensions.GetUnixSec())
            {
                if (Data.Stamina >= GameConstants.MAX_STAMINA)
                {
                    if (Data.StaminaReserve >= GameConstants.MAX_STAMINA_RESERVE)  // needn't recover
                    {
                        break;
                    }
                    Data.StaminaReserve = Math.Min(Data.StaminaReserve + 1, GameConstants.MAX_STAMINA_RESERVE);
                }
                else
                {
                    Data.Stamina++;
                }
                Data.NextStaminaRecover = Data.NextStaminaRecover + (Data.Stamina >= GameConstants.MAX_STAMINA ? GameConstants.STAMINA_RESERVE_RECOVERY_TIME : GameConstants.STAMINA_RECOVERY_TIME);
                sendPacket = true;
            }

            if (sendPacket)
            {
                SendPacket(new PacketStaminaInfoScNotify(this));
            }
        }

        public void OnHeartBeat()
        {
            OnStaminaRecover();

            DatabaseHelper.ToSaveUidList.SafeAdd(Uid);
        }

        #endregion

        #region Scene Actions

        public void OnMove()
        {
            if (SceneInstance != null)
            {
                EntityProp? prop = SceneInstance.GetNearestSpring(25_000_000);

                bool isInRange = prop != null;

                if (isInRange)
                {
                    if (LineupManager?.GetCurLineup()?.Heal(10000, true) == true)
                    {
                        SendPacket(new PacketSyncLineupNotify(LineupManager.GetCurLineup()!));
                    }
                }
            }
        }

        public EntityProp? InteractProp(int propEntityId, int interactId)
        {
            if (SceneInstance != null)
            {
                SceneInstance.Entities.TryGetValue(propEntityId, out IGameEntity? entity);
                if (entity == null) return null;
                if (entity is EntityProp prop)
                {
                    GameData.InteractConfigData.TryGetValue(interactId, out var config);
                    if (config == null || config.SrcState != prop.State) return prop;
                    var oldState = prop.State;
                    prop.SetState(config.TargetState);
                    var newState = prop.State;
                    SendPacket(new PacketGroupStateChangeScNotify(Data.EntryId, prop.GroupID, prop.State));

                    switch (prop.Excel.PropType)
                    {
                        case PropTypeEnum.PROP_TREASURE_CHEST:
                            if (oldState == PropStateEnum.ChestClosed && newState == PropStateEnum.ChestUsed)
                            {
                                // TODO: Filter treasure chest
                                var items = DropService.CalculateDropsFromProp();
                                SceneInstance.Player.InventoryManager!.AddItems(items);
                            }
                            break;
                        case PropTypeEnum.PROP_DESTRUCT:
                            if (newState == PropStateEnum.Closed)
                            {
                                prop.SetState(PropStateEnum.Open);
                            }
                            break;
                        case PropTypeEnum.PROP_MAZE_PUZZLE:
                            if (newState == PropStateEnum.Closed || newState == PropStateEnum.Open)
                            {
                                foreach (var p in SceneInstance.GetEntitiesInGroup<EntityProp>(prop.GroupID))
                                {
                                    if (p.Excel.PropType == PropTypeEnum.PROP_TREASURE_CHEST)
                                    {
                                        p.SetState(PropStateEnum.ChestUsed);
                                    }
                                    else if (p.Excel.PropType == PropTypeEnum.PROP_MAZE_PUZZLE)
                                    {
                                        // Skip
                                    }
                                    else
                                    {
                                        p.SetState(PropStateEnum.Open);
                                    }
                                    MissionManager!.OnPlayerInteractWithProp();
                                }
                            }
                            break;
                    }

                    // for door unlock
                    if (prop.PropInfo.UnlockDoorID.Count > 0)
                    {
                        foreach (var id in prop.PropInfo.UnlockDoorID)
                        {
                            foreach (var p in SceneInstance.GetEntitiesInGroup<EntityProp>(id.Key))
                            {
                                if (id.Value.Contains(p.PropInfo.ID))
                                {
                                    p.SetState(PropStateEnum.Open);
                                    MissionManager!.OnPlayerInteractWithProp();
                                }
                            }
                        }
                    }

                    // for mission
                    MissionManager!.OnPlayerInteractWithProp();

                    // plane event
                    InventoryManager!.HandlePlaneEvent(prop.PropInfo.EventID);

                    return prop;
                }
            }
            return null;
        }

        public void EnterScene(int entryId, int teleportId, bool sendPacket)
        {
            GameData.MapEntranceData.TryGetValue(entryId, out var entrance);
            if (entrance == null) return;

            GameData.GetFloorInfo(entrance.PlaneID, entrance.FloorID, out var floorInfo);
            if (floorInfo == null) return;

            int StartGroup = entrance.StartGroupID;
            int StartAnchor = entrance.StartAnchorID;

            if (teleportId != 0)
            {
                floorInfo.CachedTeleports.TryGetValue(teleportId, out var teleport);
                if (teleport != null)
                {
                    StartGroup = teleport.AnchorGroupID;
                    StartAnchor = teleport.AnchorID;
                }
            } else if (StartAnchor == 0)
            {
                StartGroup = floorInfo.StartGroupID;
                StartAnchor = floorInfo.StartAnchorID;
            }
            AnchorInfo? anchor = floorInfo.GetAnchorInfo(StartGroup, StartAnchor);

            MissionManager?.HandleFinishType(MissionFinishTypeEnum.EnterMapByEntrance, entryId);

            LoadScene(entrance.PlaneID, entrance.FloorID, entryId, anchor!.ToPositionProto(), anchor.ToRotationProto(), sendPacket);
        }

        public void EnterMissionScene(int entranceId, int anchorGroupId, int anchorId, bool sendPacket)
        {
            GameData.MapEntranceData.TryGetValue(entranceId, out var entrance);
            if (entrance == null) return;

            GameData.GetFloorInfo(entrance.PlaneID, entrance.FloorID, out var floorInfo);
            if (floorInfo == null) return;

            int StartGroup = anchorGroupId == 0 ? entrance.StartGroupID : anchorGroupId;
            int StartAnchor = anchorId == 0 ? entrance.StartAnchorID : anchorId;

            if (StartAnchor == 0)
            {
                StartGroup = floorInfo.StartGroupID;
                StartAnchor = floorInfo.StartAnchorID;
            }
            AnchorInfo? anchor = floorInfo.GetAnchorInfo(StartGroup, StartAnchor);

            LoadScene(entrance.PlaneID, entrance.FloorID, entranceId, anchor!.ToPositionProto(), anchor.ToRotationProto(), sendPacket);
        }

        public void MoveTo(Position position)
        {
            Data.Pos = position;
            SendPacket(new PacketSceneEntityMoveScNotify(this));
        }

        public void MoveTo(EntityMotion motion)
        {
            Data.Pos = motion.Motion.Pos.ToPosition();
            Data.Rot = motion.Motion.Rot.ToPosition();
        }
        
        public void MoveTo(Position pos, Position rot)
        {
            Data.Pos = pos;
            Data.Rot = rot;
            SendPacket(new PacketSceneEntityMoveScNotify(this));
        }

        public void LoadScene(int planeId, int floorId, int entryId, Position pos, Position rot, bool sendPacket)
        {
            GameData.MazePlaneData.TryGetValue(planeId, out var plane);
            if (plane == null) return;

            if (plane.PlaneType == PlaneTypeEnum.Rogue && RogueManager!.GetRogueInstance() == null)
            {
                EnterScene(801120102, 0, sendPacket);
                return;
            } else if (plane.PlaneType == PlaneTypeEnum.Raid && CurRaidId == 0)
            {
                EnterScene(OldEntryId > 0 ? OldEntryId : 2000101, 0, sendPacket);
                return;
            }

            // TODO: Sanify check
            Data.Pos = pos;
            Data.Rot = rot;
            SceneInstance instance = new(this, plane, floorId, entryId);
            if (planeId != Data.PlaneId || floorId != Data.FloorId || entryId != Data.EntryId)
            {
                Data.PlaneId = planeId;
                Data.FloorId = floorId;
                Data.EntryId = entryId;
            }
            SceneInstance = instance;

            if (sendPacket)
            {
                Connection?.SendPacket(CmdIds.SyncServerSceneChangeNotify);
                SendPacket(new PacketEnterSceneByServerScNotify(instance));
            }

            MissionManager?.OnPlayerChangeScene();
        }

        public ScenePropData? GetScenePropData(int floorId, int groupId, int propId)
        {
            if (SceneData != null)
            {
                if (SceneData.ScenePropData.TryGetValue(floorId, out var floorData))
                {
                    if (floorData.TryGetValue(groupId, out var groupData))
                    {
                        var propData = groupData.Find(x => x.PropId == propId);
                        return propData;
                    }
                }
            }
            return null;
        }

        public void SetScenePropData(int floorId, int groupId, int propId, PropStateEnum state)
        {
            if (SceneData != null)
            {
                if (!SceneData.ScenePropData.TryGetValue(floorId, out var floorData))
                {
                    floorData = [];
                    SceneData.ScenePropData.Add(floorId, floorData);
                }
                if (!floorData.TryGetValue(groupId, out var groupData))
                {
                    groupData = [];
                    floorData.Add(groupId, groupData);
                }
                var propData = groupData.Find(x => x.PropId == propId);  // find prop data
                if (propData == null)
                {
                    propData = new ScenePropData()
                    {
                        PropId = propId,
                        State = state,
                    };
                    groupData.Add(propData);
                }
                else
                {
                    propData.State = state;
                }
            }
        }

        public void EnterSection(int sectionId)
        {
            if (SceneInstance != null)
            {
                SceneData!.UnlockSectionIdList.TryGetValue(SceneInstance.FloorId, out var unlockList);
                if (unlockList == null)
                {
                    unlockList = [sectionId];
                    SceneData.UnlockSectionIdList.Add(SceneInstance.FloorId, unlockList);
                } else
                {
                    SceneData.UnlockSectionIdList[SceneInstance.FloorId].Add(sectionId);
                }
            }
        }

        public void SetCustomSaveData(int entryId, int groupId, string data)
        {
            if (SceneData != null)
            {
                if (!SceneData.CustomSaveData.TryGetValue(entryId, out var entryData))
                {
                    entryData = [];
                    SceneData.CustomSaveData.Add(entryId, entryData);
                }
                entryData[groupId] = data;
            }
        }

        public void ForceQuitBattle()
        {
            if (BattleInstance != null)
            {
                BattleInstance = null;
                Connection!.SendPacket(CmdIds.QuitBattleScNotify);
            }
        }

        public void LeaveRaid()
        {
            if (CurRaidId == 0) return;
            GameData.RaidConfigData.TryGetValue(CurRaidId * 100 + 0, out var config);
            if (config == null) return;

            if (config.TeamType == RaidTeamTypeEnum.TrialOnly)
            {
                LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);
                SendPacket(new PacketSyncLineupNotify(LineupManager!.GetCurLineup()!));
            }

            if (config.FinishEntranceID > 0)
            {
                EnterScene(config.FinishEntranceID, 0, true);
            }
            else
            {
                EnterScene(OldEntryId, 0, true);
                MoveTo(LastPos!, LastRot!);
            }

            SendPacket(new PacketRaidInfoNotify((uint)CurRaidId, RaidStatus.Finish));

            CurRaidId = 0;
            OldEntryId = 0;
            LastPos = null;
            LastRot = null;
        }

        #endregion

        #region Serialization

        public PlayerBasicInfo ToProto()
        {
            return Data.ToProto();
        }

        public PlayerSimpleInfo ToSimpleProto()
        {
            return Data.ToSimpleProto(FriendOnlineStatus.Online);
        }

        #endregion
    }
}
