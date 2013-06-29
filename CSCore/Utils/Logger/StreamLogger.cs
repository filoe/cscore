using System;
using System.IO;

namespace CSCore.Utils.Logger
{
    public class StreamLogger : LoggerBase
    {
        protected Stream _stream;

        protected StreamWriter _writer;
        protected StreamWriter Writer
        {
            get
            {
                return _writer ?? (_writer = new StreamWriter(_stream) { AutoFlush = true });
            }
        }

        public StreamLogger(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanWrite)
                throw new ArgumentException("stream.CanWrite == false");

            _stream = stream;
        }

        public override void Log(LogLevel level, string msg)
        {
            if(level <= LoggerLevel)
                Writer.WriteLine(String.Format("[{0}]: {1}", level, msg));
        }

        public override void Log(LoggerBase.LogLevel level, string msg, string location)
        {
            Log(level, LoggerBase.LocationFormatter(msg, location));
        }
    }
}
