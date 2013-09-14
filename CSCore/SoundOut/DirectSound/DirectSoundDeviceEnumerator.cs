using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.SoundOut.DirectSound
{
    public class DirectSoundDeviceEnumerator
    {
        private List<DirectSoundDevice> _devices;
        public List<DirectSoundDevice> Devices
        {
            get { return _devices ?? (_devices = new List<DirectSoundDevice>()); }
        }

        public DirectSoundDeviceEnumerator()
        {
            var callback = new DSEnumCallback(EnumCallback);
            DirectSoundException.Try(NativeMethods.DirectSoundEnumerate(callback, IntPtr.Zero),
                    "Interop", "DirectSoundEnumerate");
        }

        private bool EnumCallback(IntPtr lpGuid, IntPtr lpcstrDescription, IntPtr lpstrModule, IntPtr lpContext)
        {
            byte[] guidBuffer = new byte[16];
            Guid guid;
            string desc = String.Empty, module = String.Empty;

            if (lpGuid != IntPtr.Zero)
            {
                Marshal.Copy(lpGuid, guidBuffer, 0, 16);
                guid = new Guid(guidBuffer);
            }
            else
            {
                guid = Guid.Empty;
            }

            desc = Marshal.PtrToStringAnsi(lpcstrDescription);
            if (lpstrModule != null)
                module = Marshal.PtrToStringAnsi(lpstrModule);

            Devices.Add(new DirectSoundDevice(desc, module, guid));

            return true;
        }
    }
}
