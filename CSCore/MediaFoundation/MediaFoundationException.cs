using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    public class MediaFoundationException : System.Runtime.InteropServices.COMException
    {
        public static void Try(int result, string interfaceName, string member)
        {
            if (result != 0)
                throw new MediaFoundationException(result, interfaceName, member);
        }

        public int Result { get; private set; }

        public string InterfaceName { get; private set; }

        public string Member { get; private set; }

        public MediaFoundationException(int result, string interfaceName, string member)
            : base(String.Format("{0}::{1} returned 0x{2}", interfaceName, member, result.ToString("x")), (int)result)
        {
            Result = result;
            InterfaceName = interfaceName;
            Member = member;
        }
    }
}