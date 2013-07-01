using System;
using System.Collections.Generic;

namespace CSCore.Utils.Logger
{
    public class LogDispatcher : LoggerBase
    {
        static List<LoggerBase> _loggers;
        public static List<LoggerBase> Loggers
        {
            get
            {
                return _loggers ?? (_loggers = new List<LoggerBase>());
            }
        }

        public List<LoggerBase> InstalledLogger
        {
            get
            {
                return LogDispatcher.Loggers;
            }
        }

        public void Debug(string msg, params object[] objects)
        {
            Debug(String.Format(msg, objects));
        }

        public void Debug(string msg, string where)
        {
            Log(LogLevel.DEBUG, msg, where);
        }

        public void Debug(string msg, string where, params object[] objects)
        {
            Debug(String.Format(msg, objects), where);
        }

        public void Debug(string msg)
        {
            Log(LogLevel.DEBUG, msg);
        }

        public void Info(string msg, params object[] objects)
        {
            Info(String.Format(msg, objects));
        }

        public void Info(string msg, string where)
        {
            Log(LogLevel.INFO, msg, where);
        }

        public void Info(string msg, string where, params object[] objects)
        {
            Info(String.Format(msg, objects), where);
        }

        public void Info(string msg)
        {
            Log(LogLevel.INFO, msg);
        }

        public void Error(string msg, params object[] objects)
        {
            Error(String.Format(msg, objects));   
        }

        public void Error(string msg, string where)
        {
            Log(LogLevel.ERROR, msg, where);
        }

        public void Error(Exception e, string where, bool throwEx = true)
        {
            Error(e.Message, where);
            if(throwEx)
                throw e;
        }

        public void Error(string msg)
        {
            Log(LogLevel.ERROR, msg);
        }

        public void Fatal(string msg, params object[] objects)
        {
            Fatal(String.Format(msg, objects));
        }

        public void Fatal(Exception e)
        {
            Fatal(e.Message);
        }

        public void Fatal(Exception e, string where, bool throwException = false)
        {
            Fatal(e.Message, where);
            if (throwException)
                throw e;
        }

        public void Fatal(string msg)
        {
            Log(LogLevel.FATAL, msg);
        }

        public void Fatal(string msg, string where)
        {
            Log(LogLevel.FATAL, msg, where);
        }

        public void MMResult(MmResult result, string functionName)
        {
            MMResult(result, functionName, "--");
            //Log(LogLevel.DEBUG, String.Format("[{0}]: {1}", MMResult, functionName));
        }

        public void MMResult(MmResult result, string functionName, string where, MMLogFlag flag = MMLogFlag.ThrowOnError)
        {
            //Log(LogLevel.DEBUG, String.Format("[{0}]: {1}", MMResult, functionName), where);
            if (result != MmResult.MMSYSERR_NOERROR && flag.HasFlag(MMLogFlag.ThrowOnError))
            {
                Fatal(new MmException(result, functionName, where), where, true);
            }
            else if (result != MmResult.MMSYSERR_NOERROR)
            {
                Error(String.Format("[{0}]: {1}", result, functionName), where);
            }
            else if(flag.HasFlag(MMLogFlag.LogAlways))
            {
                Debug(String.Format("[{0}]: {1}", result, functionName), where);
            }
        }

        [Flags]
        public enum MMLogFlag
        {
            ThrowOnError = 0x1,
            ThrowNever = 0x2,
            LogAlways = 0x4,
            ThrowNeverLogAlways = ThrowNever | MMLogFlag.LogAlways
        }

        public override void Log(LoggerBase.LogLevel level, string msg)
        {
            if (level == LogLevel.OFF) return;

            if (level <= LogLevel.ERROR)
                Context.Current.ErrorCount++;

            foreach (LoggerBase loggerBase in Loggers)
            {
                loggerBase.Log(level, msg);
            }
        }

        public override void Log(LoggerBase.LogLevel level, string msg, string location)
        {
            if (level == LogLevel.OFF) return;

            if (level <= LogLevel.ERROR)
                Context.Current.ErrorCount++;

            foreach (LoggerBase loggerbase in Loggers)
                loggerbase.Log(level, msg, location);
        }

        public void LogFormat(LoggerBase.LogLevel level, string msg, params object[] objects)
        {
            Log(level, String.Format(msg, objects));
        }
    }
}
