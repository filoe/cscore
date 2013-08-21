using System;

namespace CSCore
{
    public class MmException : Exception
    {
        public static void Try(MmResult result, string target)
        {
            if (result != MmResult.MMSYSERR_NOERROR)
            {
                throw new MmException(result, target);
            }
        }

        public MmResult Result { get; private set; }

        public string Target { get; private set; }

        public MmException(MmResult result, string function)
        {
            Result = result;
            Target = function;
        }
    }
}