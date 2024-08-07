namespace EggLink.DanhengServer.Util;

public static class PluginEventCommon
{
    #region Common

    public delegate void OnConsoleLogHandler(string message);

    #endregion

    #region Event

    public static event OnConsoleLogHandler? OnConsoleLog;

    #endregion

    public static void InvokeOnConsoleLog(string message)
    {
        OnConsoleLog?.Invoke(message);
    }
}