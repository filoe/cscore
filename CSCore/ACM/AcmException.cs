using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.ACM
{
    public class AcmException : MmException
    {
        public static new void Try(MmResult result, string target)
        {
            if (result != MmResult.MMSYSERR_NOERROR)
            {
                throw new MmException(result, target);
            }
        }

        public AcmException(MmResult result, string function)
            : base(result, function)
        {
        }
    }
}
