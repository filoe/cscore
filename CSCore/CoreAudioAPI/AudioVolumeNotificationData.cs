using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// see http://msdn.microsoft.com/en-us/library/dd370799(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioVolumeNotificationData
    {
        public Guid EventContext;
        public NativeBool Muted;
        public float MasterVolume;
        public uint Channels;
        public float ChannelVolumes; //array?

        public unsafe float[] GetAllChannelVolumes(IntPtr ptr)
        {
            float[] result = new float[Channels];

            IntPtr pchannels = (IntPtr)((long)ptr + (long)Marshal.OffsetOf(typeof(AudioVolumeNotificationData), "ChannelVolumes"));
            for (int i = 0; i < Channels; i++)
            {
                result[i] = (float)Marshal.PtrToStructure(pchannels, typeof(float));
                int size = Marshal.SizeOf(typeof(float));
                pchannels = new IntPtr((byte*)pchannels.ToPointer() + size);
            }

            return result;
        }
    }
}