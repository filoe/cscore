using System;
using System.IO;
using System.Xml;

namespace CSCore.Utils.Logger
{
    public class FileLogger : StreamLogger, IDisposable
    {
        public LogFileLayout Layout { get; private set; }

        private XmlWriter _xmlw;

        private Func<string, string> _timeFormatter = new Func<string, string>((s) =>
            {
                return String.Format("[{0}]{1}", DateTime.Now.ToString("HH_mm_ss.f"), s);
            });

        public Func<string, string> TimeFormatter
        {
            get
            {
                return _timeFormatter;
            }
            set
            {
                _timeFormatter = value;
            }
        }

        public bool LogTime { get; set; }

        public FileLogger(string fileName, LogFileLayout layout)
            : this(File.OpenWrite(fileName), layout)
        {
        }

        public FileLogger(Stream stream, LogFileLayout layout)
            : base(stream)
        {
            Layout = layout;
            if (layout == LogFileLayout.Xml)
                _xmlw = XmlWriter.Create(stream);
        }

        private const string messageTag = "MSG";
        private const string locationTag = "LOCATION";
        private const string timeAttributeName = "Time";

        public override void Log(LogLevel level, string msg)
        {
            if (level > LoggerLevel) return;

            if (Layout == LogFileLayout.Lines)
            {
                if (!LogTime)
                    base.Log(level, msg);
                else
                {
                    Writer.WriteLine(LoggerBase.DateTimeFormatter(LoggerBase.LevelFormatter(level, msg)));
                }
            }
            else
            {
                _xmlw.WriteStartElement(level.ToString());

                _xmlw.WriteElementString(messageTag, msg);

                _xmlw.WriteEndElement();
            }
        }

        public override void Log(LogLevel level, string msg, string location)
        {
            if (level > LoggerLevel) return;

            if (Layout == LogFileLayout.Lines)
                Log(level, LocationFormatter(msg, location));
            else
            {
                if (LogTime)
                    _xmlw.WriteAttributeString(timeAttributeName, DateTime.Now.ToString("HH_mm_ss.f"));
                _xmlw.WriteStartElement(level.ToString());

                _xmlw.WriteElementString(messageTag, msg);
                _xmlw.WriteElementString(locationTag, location);

                _xmlw.WriteEndElement();
            }
        }

        public enum LogFileLayout
        {
            Lines,
            Xml
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (_writer != null)
                {
                    _writer.Flush();
                    _writer.Dispose();
                    _writer = null;
                }
            }
            catch (Exception) { }
        }

        ~FileLogger()
        {
            Dispose(false);
        }
    }
}