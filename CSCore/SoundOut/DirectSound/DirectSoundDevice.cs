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
                var devices = EnumerateDevices();
                var defaultDevice = devices.Where(x => x.Guid == DefaultPlaybackGuid).FirstOrDefault();
                return defaultDevice ?? (defaultDevice = devices.FirstOrDefault());
            }
        }

        public static List<DirectSoundDevice> EnumerateDevices()
        {
            return new DirectSoundDeviceEnumerator().Devices;
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