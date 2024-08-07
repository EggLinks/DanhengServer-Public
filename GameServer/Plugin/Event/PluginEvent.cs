using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;

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

    #region Player

    public delegate void OnPlayerHeartBeatHandler(PlayerInstance player);

    public delegate void OnPlayerLoginHandler(PlayerInstance player);

    public delegate void OnPlayerLogoutHandler(PlayerInstance player);

    public delegate void OnPlayerFinishSubMissionHandler(PlayerInstance player, int missionId);

    public delegate void OnPlayerFinishMainMissionHandler(PlayerInstance player, int missionId);

    public delegate void OnPlayerInteractHandler(PlayerInstance player, EntityProp prop);

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

    #endregion
}