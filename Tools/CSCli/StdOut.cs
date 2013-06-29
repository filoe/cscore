using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCli
{
    public class StdOut
    {
        static StdOut _instance;
        public static StdOut Instance
        {
            get { return _instance ?? (_instance = new StdOut()); }
        }

        public static void Error(string message)
        {
            Instance.WriteError(message);
        }

        public static void Info(string message)
        {
            Instance.WriteInfo(message);
        }

        private StdOut() { }

        public void WriteError(string message)
        {
            Console.Error.WriteLine(message);
        }

        public void WriteInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}
