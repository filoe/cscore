using System;

namespace CSCore.DMO
{
    public class DmoException : System.Runtime.InteropServices.COMException
    {
        public static void Try(int result, string interfaceName, string member)
        {
            if (result != 0)
                throw new DmoException(result, interfaceName, member);
        }

        public int Result { get; private set; }

        public string InterfaceName { get; private set; }

        public string Member { get; private set; }

        public DmoException(int result, string interfaceName, string member)
            : base(String.Format("{0}::{1} returned 0x{2}{3}.", interfaceName, member, result.ToString("x"), TryGetFriendlyName(result)), 
                    result)
        {
            Result = result;
            InterfaceName = interfaceName;
            Member = member;
        }

        private static string TryGetFriendlyName(int result)
        {
            if (Enum.IsDefined(typeof(DmoErrorCodes), result))
                return String.Format(" ({0})", Enum.GetName(typeof(DmoErrorCodes), result));
            else if (Enum.IsDefined(typeof(Win32.HResult), result))
                return String.Format(" ({0})", Enum.GetName(typeof(Win32.HResult), result));
            else
                return String.Empty;
        }
    }
}