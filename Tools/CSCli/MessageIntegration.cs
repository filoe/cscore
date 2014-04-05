using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSCli
{
    public static class MessageIntegration
    {
        private static string location;

        static MessageIntegration()
        {
            location = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        }

        public static void WriteError(string message)
        {
            Console.WriteLine("{0}:error:[CSCli]{1}", location, message);
        }

        public static void WriteWarning(string message)
        {
            WriteWarning(message, String.Empty);
        }

        public static void WriteWarning(string message, string code)
        {
            if (!String.IsNullOrWhiteSpace(code) && !code.StartsWith(" "))
                code = code.Insert(0, " ");

            Console.WriteLine("{0}:warning{2}:[CSCli]{1}", location, message, code);
        }

        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Info(string format, params object[] args)
        {
            Info(String.Format(format, args));
        }
    }
}