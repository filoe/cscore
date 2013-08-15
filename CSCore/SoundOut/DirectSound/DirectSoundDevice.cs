using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace CSCore.SoundOut.DirectSound
{
    public class DirectSoundDevice
    {
        public static readonly Guid DefaultPlaybackGuid = new Guid("DEF00000-9C6D-47ED-AAF1-4DDA8F2B5C03");

        public static DirectSoundDevice DefaultDevice
        {
            get
            {
                return EnumerateDevices().FirstOrDefault();
            }
        }

        private static Dictionary<IntPtr, List<DirectSoundDevice>> _activeContexts;
        private static Mutex _mutex;

        public static List<DirectSoundDevice> EnumerateDevices()
        {
            if (_activeContexts == null) _activeContexts = new Dictionary<IntPtr, List<DirectSoundDevice>>();
            if (_mutex == null) _mutex = new Mutex();

            _mutex.WaitOne();

            IntPtr context = IntPtr.Zero;
            if (_activeContexts.Count > 0)
                context = new IntPtr((int)_activeContexts.Last().Key + 1);
            _activeContexts.Add(context, new List<DirectSoundDevice>());
            _mutex.ReleaseMutex();

            DirectSoundException.Try(DSInterop.DirectSoundEnumerate(new DSEnumCallback(EnumCallback), context),
                "Interop", "DirectSoundEnumerate");

            var result = _activeContexts[context];
            _activeContexts.Remove(context);
            return result;
        }

        private static bool EnumCallback(IntPtr lpGuid, IntPtr lpcstrDescription, IntPtr lpstrModule, IntPtr lpContext)
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

            _activeContexts[lpContext].Add(new DirectSoundDevice(desc, module, guid));

            return true;
        }

        public string Description { get; private set; }

        public string Module { get; private set; }

        public Guid Guid { get; private set; }

        internal DirectSoundDevice()
        {
        }

        public DirectSoundDevice(string description, string module, Guid guid)
        {
            Description = description;
            Module = module;
            Guid = guid;
        }

        public static explicit operator Guid(DirectSoundDevice device)
        {
            return device.Guid;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}