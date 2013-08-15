using System;

namespace CSCore.Utils.Logger
{
    public class ConsoleLogger : LoggerBase
    {
        public override void Log(LogLevel level, string msg)
        {
            ConsoleColor color, originalColor = Console.ForegroundColor;
            if (level == LogLevel.DEBUG) color = ConsoleColor.Green;
            else if (level == LogLevel.INFO) color = ConsoleColor.Blue;
            else if (level == LogLevel.ERROR) color = ConsoleColor.Red;
            else if (level == LogLevel.FATAL) color = ConsoleColor.DarkRed;
            else throw new InvalidOperationException();

            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = originalColor;
        }

        public override void Log(LogLevel level, string msg, string location)
        {
            Log(level, LoggerBase.LocationFormatter(msg, location));
        }
    }
}