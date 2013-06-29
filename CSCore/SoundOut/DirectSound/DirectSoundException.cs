using System;

namespace CSCore.SoundOut.DirectSound
{
    public class DirectSoundException : System.Runtime.InteropServices.COMException
    {
        public static void Try(DSResult result, string interfaceName, string member)
        {
            if(result != DSResult.DS_OK)
                throw new DirectSoundException(result, interfaceName, member);
        }

        public DSResult Result { get; private set; }
        public string InterfaceName { get; private set; }
        public string Member { get; private set; }

        public DirectSoundException(DSResult result, string interfaceName, string member)
            : base(String.Format("{0}.{1} returned {2}", interfaceName, member, result), (int)result)
        {
            Result = result;
            InterfaceName = interfaceName;
            Member = member;
        }
    }
}
