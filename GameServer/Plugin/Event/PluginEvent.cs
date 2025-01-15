using EggLink.DanhengServer.Command;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.Challenge;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;
using LineupInfo = EggLink.DanhengServer.Database.Lineup.LineupInfo;

namespace EggLink.DanhengServer.GameServer.Plugin.Event;

public static class PluginEvent
{
    public static void InvokeOnPlayerHeartBeat(PlayerInstance player)
    {
        OnPlayerHeartBeat?.Invoke(player);
    }

    public static void InvokeOnPlayerLogin(PlayerInstance player)
    {
        OnPlayerLogin?.Invoke(player);
    }

    public static void InvokeOnPlayerLogout(PlayerInstance player)
    {
        OnPlayerLogout?.Invoke(player);
    }

    public static void InvokeOnPlayerFinishSubMission(PlayerInstance player, int missionId)
    {
        OnPlayerFinishSubMission?.Invoke(player, missionId);
    }

    public static void InvokeOnPlayerFinishMainMission(PlayerInstance player, int missionId)
    {
        OnPlayerFinishMainMission?.Invoke(player, missionId);
    }

    public static void InvokeOnPlayerInteract(PlayerInstance player, EntityProp prop)
    {
        OnPlayerInteract?.Invoke(player, prop);
    }

    public static void InvokeOnPlayerLoadScene(PlayerInstance player, SceneInstance scene)
    {
        OnPlayerLoadScene?.Invoke(player, scene);
    }

    public static void InvokeOnPlayerEnterBattle(PlayerInstance player, BattleInstance battle)
    {
        OnPlayerEnterBattle?.Invoke(player, battle);
    }

    public static void InvokeOnPlayerQuitBattle(PlayerInstance player, PVEBattleResultCsReq result)
    {
        OnPlayerQuitBattle?.Invoke(player, result);
    }

    public static void InvokeOnPlayerEnterChallenge(PlayerInstance player, ChallengeInstance challenge)
    {
        OnPlayerEnterChallenge?.Invoke(player, challenge);
    }

    public static void InvokeOnPlayerQuitChallenge(PlayerInstance player, ChallengeInstance? challenge)
    {
        OnPlayerQuitChallenge?.Invoke(player, challenge);
    }

    public static void InvokeOnPlayerSyncLineup(PlayerInstance player, LineupInfo? lineup)
    {
        OnPlayerSyncLineup?.Invoke(player, lineup);
    }

    public static void InvokeOnPlayerUseCommand(ICommandSender sender, string command)
    {
        OnPlayerUseCommand?.Invoke(sender, command);
    }

    #region Player

    public delegate void OnPlayerHeartBeatHandler(PlayerInstance player);

    public delegate void OnPlayerLoginHandler(PlayerInstance player);

    public delegate void OnPlayerLogoutHandler(PlayerInstance player);

    public delegate void OnPlayerFinishSubMissionHandler(PlayerInstance player, int missionId);

    public delegate void OnPlayerFinishMainMissionHandler(PlayerInstance player, int missionId);

    public delegate void OnPlayerInteractHandler(PlayerInstance player, EntityProp prop);

    public delegate void OnPlayerLoadSceneHandler(PlayerInstance player, SceneInstance scene);

    public delegate void OnPlayerEnterBattleHandler(PlayerInstance player, BattleInstance battle);

    public delegate void OnPlayerQuitBattleHandler(PlayerInstance player, PVEBattleResultCsReq result);

    public delegate void OnPlayerEnterChallengeHandler(PlayerInstance player, ChallengeInstance challenge);

    public delegate void OnPlayerQuitChallengeHandler(PlayerInstance player, ChallengeInstance? challenge);

    public delegate void OnPlayerSyncLineupHandler(PlayerInstance player, LineupInfo? lineup);

    public delegate void OnPlayerUseCommandHandler(ICommandSender sender, string command);

    #endregion

    #region Common

    #endregion

    #region Event

    public static event OnPlayerHeartBeatHandler? OnPlayerHeartBeat;
    public static event OnPlayerLoginHandler? OnPlayerLogin;
    public static event OnPlayerLogoutHandler? OnPlayerLogout;
    public static event OnPlayerFinishSubMissionHandler? OnPlayerFinishSubMission;
    public static event OnPlayerFinishMainMissionHandler? OnPlayerFinishMainMission;
    public static event OnPlayerInteractHandler? OnPlayerInteract;
    public static event OnPlayerLoadSceneHandler? OnPlayerLoadScene;
    public static event OnPlayerEnterBattleHandler? OnPlayerEnterBattle;
    public static event OnPlayerEnterChallengeHandler? OnPlayerEnterChallenge;
    public static event OnPlayerQuitBattleHandler? OnPlayerQuitBattle;
    public static event OnPlayerQuitChallengeHandler? OnPlayerQuitChallenge;
    public static event OnPlayerSyncLineupHandler? OnPlayerSyncLineup;
    public static event OnPlayerUseCommandHandler? OnPlayerUseCommand;

    #endregion
}