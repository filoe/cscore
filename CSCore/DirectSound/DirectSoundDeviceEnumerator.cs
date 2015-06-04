using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Provides the functionality to enumerate directsound devices installed on the system.
    /// </summary>
    public sealed class DirectSoundDeviceEnumerator
    {
        private readonly List<DirectSoundDevice> _devices;

        /// <summary>
        /// Enumerates the directsound devices installed on the system.
        /// </summary>
        /// <returns>A readonly collection, containing all enumerated devices.</returns>
        public static ReadOnlyCollection<DirectSoundDevice> EnumerateDevices()
        {
            return new DirectSoundDeviceEnumerator()._devices.AsReadOnly();
        } 

        private DirectSoundDeviceEnumerator()
        {
            _devices = new List<DirectSoundDevice>();
            var callback = new DSEnumCallback(EnumCallback);
            DirectSoundException.Try(NativeMethods.DirectSoundEnumerate(callback, IntPtr.Zero),
                    "Interop", "DirectSoundEnumerate");
        }

        private bool EnumCallback(IntPtr lpGuid, IntPtr lpcstrDescription, IntPtr lpstrModule, IntPtr lpContext)
        {
            byte[] guidBuffer = new byte[16];
            Guid guid = Guid.Empty;
            string desc = String.Empty, module = String.Empty;

            if (lpGuid != IntPtr.Zero)
            {
                Marshal.Copy(lpGuid, guidBuffer, 0, 16);
                guid = new Guid(guidBuffer);
            }

            if(lpcstrDescription != IntPtr.Zero)
                desc = Marshal.PtrToStringAnsi(lpcstrDescription);
            if (lpstrModule != IntPtr.Zero)
                module = Marshal.PtrToStringAnsi(lpstrModule);

            _devices.Add(new DirectSoundDevice(desc, module, guid));

            return true;
        }
    }
}
