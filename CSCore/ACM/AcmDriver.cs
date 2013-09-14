using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.ACM
{
    public class AcmDriver
    {
        private IntPtr _acmDriverHandle;
        private AcmDriverDetails _details;

        public AcmDriver(IntPtr acmDriverHandle)
        {
            if (acmDriverHandle == IntPtr.Zero)
                throw new ArgumentNullException("acmDriverHandle");

            _acmDriverHandle = acmDriverHandle;
            _details = GetDetails(acmDriverHandle);
        }

        private AcmDriverDetails GetDetails(IntPtr driverHandle)
        {
            AcmDriverDetails result = new AcmDriverDetails();
            result.cbStruct = Marshal.SizeOf(result);
            var r = AcmInterop.acmDriverDetails(driverHandle, ref result, IntPtr.Zero);
            MmException.Try(r, "acmDriverDetails");
            return result;
        }
    }
}
