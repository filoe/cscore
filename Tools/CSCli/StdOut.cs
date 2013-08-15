using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCli
{
    public class StdOut
    {
        private static StdOut _instance;

        public static StdOut Instance
        {
            get { return _instance ?? (_instance = new StdOut()); }
        }

        public static void Error(string message)
        {
            Error(message, String.Empty);
        }

        public static void Error(string message, string location)
        {
            Instance.WriteError(message, location);
        }

        public static void Info(string message)
        {
            Instance.WriteInfo(message);
        }

        public static void Info(string message, params object[] values)
        {
            Info(String.Format(message, values));
        }

        private StdOut()
        {
        }

        public void WriteError(string message, string location)
        {
            Console.Error.WriteLine("[CSCLI][ERROR]: " + message);
            Console.Error.WriteLine(location + ":error:" + message); //see http://blogs.msdn.com/b/msbuild/archive/2006/11/03/msbuild-visual-studio-aware-error-messages-and-message-formats.aspx
        }

        public void WriteInfo(string message)
        {
            Console.WriteLine("[CSCLI]: " + message);
        }
    }
}