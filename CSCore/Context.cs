using System;

namespace CSCore
{
    public class Context
    {
        static Context _context;
        public static Context Current
        {
            get
            {
                return _context ?? (_context = new Context());
            }
        }

        public int ErrorCount { get; set; }

        Utils.Logger.LogDispatcher _logger;
        public Utils.Logger.LogDispatcher Logger
        {
            get
            {
                return _logger ?? (_logger = new Utils.Logger.LogDispatcher());
            }
            set
            {
                _logger = value;
            }
        }

        protected Context()
        {
            //Utils.UnhandledException.AddHandler();
        }

        public void CreateDefaultLogger()
        {
            string filePath = String.Format("{0}\\CSLogs\\{1}.log",
                Environment.CurrentDirectory,
                DateTime.Now.ToString("yyyy_MM_dd__hh_mm_ss"));

            if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(filePath)) == false)
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));

            Utils.Logger.LogDispatcher.Loggers.Add(new Utils.Logger.DiagnosticLogger());
            Utils.Logger.LogDispatcher.Loggers.Add(new Utils.Logger.FileLogger(filePath, Utils.Logger.FileLogger.LogFileLayout.Lines));
        }
    }
}
