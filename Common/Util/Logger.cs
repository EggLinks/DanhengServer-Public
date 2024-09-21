using System.Diagnostics;
using Spectre.Console;

namespace EggLink.DanhengServer.Util;

public class Logger(string moduleName)
{
    private static FileInfo? LogFile;
    private static readonly object _lock = new();
    private readonly string ModuleName = moduleName;

    public void Log(string message, LoggerLevel level)
    {
        lock (_lock)
        {
            AnsiConsole.Write(new Markup($"[[[bold deepskyblue3_1]{DateTime.Now:HH:mm:ss}[/]]] " +
                                         $"[[[gray]{ModuleName}[/]]] [[[{(ConsoleColor)level}]{level}[/]]] {message.Replace("[", "[[").Replace("]", "]]")}\n"));

            var logMessage = $"[{DateTime.Now:HH:mm:ss}] [{ModuleName}] [{level}] {message}";
            PluginEventCommon.InvokeOnConsoleLog(logMessage);
            WriteToFile(logMessage);
        }
    }

    public void Info(string message, Exception? e = null)
    {
        Log(message, LoggerLevel.INFO);
        if (e != null)
        {
            Log(e.Message, LoggerLevel.INFO);
            Log(e.StackTrace!, LoggerLevel.INFO);
        }
    }

    public void Warn(string message, Exception? e = null)
    {
        Log(message, LoggerLevel.WARN);
        if (e != null)
        {
            Log(e.Message, LoggerLevel.WARN);
            Log(e.StackTrace!, LoggerLevel.WARN);
        }
    }

    public void Error(string message, Exception? e = null)
    {
        Log(message, LoggerLevel.ERROR);
        if (e != null)
        {
            Log(e.Message, LoggerLevel.ERROR);
            Log(e.StackTrace!, LoggerLevel.ERROR);
        }
    }

    public void Fatal(string message, Exception? e = null)
    {
        Log(message, LoggerLevel.FATAL);
        if (e != null)
        {
            Log(e.Message, LoggerLevel.FATAL);
            Log(e.StackTrace!, LoggerLevel.FATAL);
        }
    }

    public void Debug(string message, Exception? e = null)
    {
        Log(message, LoggerLevel.DEBUG);
        if (e != null)
        {
            Log(e.Message, LoggerLevel.DEBUG);
            Log(e.StackTrace!, LoggerLevel.DEBUG);
        }
    }

    public static void SetLogFile(FileInfo file)
    {
        LogFile = file;
    }

    public static void WriteToFile(string message)
    {
        try
        {
            if (LogFile == null) throw new Exception("LogFile is not set");
            using var sw = LogFile.AppendText();
            sw.WriteLine(message);
        }
        catch
        {
        }
    }

    public static Logger GetByClassName()
    {
        return new Logger(new StackTrace().GetFrame(1)?.GetMethod()?.ReflectedType?.Name ?? "");
    }
}

public enum LoggerLevel
{
    INFO = ConsoleColor.Cyan,
    WARN = ConsoleColor.Yellow,
    ERROR = ConsoleColor.Red,
    FATAL = ConsoleColor.DarkRed,
    DEBUG = ConsoleColor.Blue
}

public class LoggerLevelHelper
{
}