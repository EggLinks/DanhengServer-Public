using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Util
{
    public class Logger(string moduleName)
    {
        private readonly string ModuleName = moduleName;
        private static FileInfo? LogFile;
        private static object _lock = new();

        public void Log(string message, LoggerLevel level)
        {
            lock (_lock)
            {
                Console.Write("[");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(DateTime.Now.ToString("HH:mm:ss"));
                Console.ResetColor();

                Console.Write("] ");
                Console.Write("[");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(ModuleName);
                Console.ResetColor();

                Console.Write("] ");
                Console.Write("[");

                Console.ForegroundColor = (ConsoleColor)level;
                Console.Write(level);
                Console.ResetColor();

                Console.WriteLine("] " + message);

                var logMessage = $"[{DateTime.Now:HH:mm:ss}] [{ModuleName}] [{level}] {message}";
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
                if (LogFile == null)
                {
                    throw new Exception("LogFile is not set");
                }
                using StreamWriter sw = LogFile.AppendText();
                sw.WriteLine(message);
            }
            catch
            {

            }
        }

#pragma warning disable CS8602 
        public static Logger GetByClassName() => new(new StackTrace().GetFrame(1).GetMethod().ReflectedType.Name);
#pragma warning restore CS8602
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
}
