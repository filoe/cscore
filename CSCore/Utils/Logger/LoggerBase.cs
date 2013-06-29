using System;

namespace CSCore.Utils.Logger
{
    public abstract class LoggerBase
    {

        static Func<string, string, string> _locationFormatter = (m, l) =>
            {
                return String.Format("{0}: {1}", l, m);
            };
        public static Func<string, string, string> LocationFormatter
        {
            get
            {
                return _locationFormatter;
            }
            set
            {
                _locationFormatter = value;
            }
        }

        static Func<LogLevel, string, string> _levelFormatter = (l, m) =>
            {
                return String.Format("[{0}]:{1}", l, m);
            };
        public static Func<LogLevel, string, string> LevelFormatter
        {
            get
            {
                return _levelFormatter;
            }
            set
            {
                _levelFormatter = value;
            }
        }

        static Func<string, string> _dateTimeFormatter = (m) =>
            {
                return String.Format("[{0}]{1}", DateTime.Now.ToString("HH_mm_ss.f"), m);
            };
        public static Func<string, string> DateTimeFormatter
        {
            get
            {
                return _dateTimeFormatter;
            }
            set
            {
                _dateTimeFormatter = value;
            }
        }

        public LoggerBase( )
        {
            LoggerLevel = LoggerBase.LogLevel.DEBUG;
        }

        public enum LogLevel
        {
            OFF,
            FATAL,
            ERROR,
            INFO,
            DEBUG
        }

        public virtual LogLevel LoggerLevel
        {
            get;
            set;
        }

        public abstract void Log(LogLevel level, string msg);
        public abstract void Log(LogLevel level, string msg, string location);
    }
}
