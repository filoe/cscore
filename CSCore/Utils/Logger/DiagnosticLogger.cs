using System;

namespace CSCore.Utils.Logger
{
    public class DiagnosticLogger : LoggerBase
    {
        private bool? _isDebuggerAttached;

        private bool IsDebuggerAttached
        {
            get
            {
                return _isDebuggerAttached ?? (bool)(_isDebuggerAttached = System.Diagnostics.Debugger.IsAttached);
            }
        }

        public override void Log(LoggerBase.LogLevel level, string msg)
        {
            if (level <= LoggerLevel)
            {
                if (IsDebuggerAttached)
                    System.Diagnostics.Debug.WriteLine(LoggerBase.LevelFormatter(level, msg));
                else
                    Console.WriteLine(LoggerBase.LevelFormatter(level, msg));
            }
        }

        public override void Log(LogLevel level, string msg, string location)
        {
            Log(level, LoggerBase.LocationFormatter(msg, location));
        }
    }
}