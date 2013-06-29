using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.SoundOut
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct WaveOutCaps
    {
        public short wMid;
        public short wPid;
        public uint vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
        public string szPname;
        public WaveCapsFormats dwFormats;
        public short wChannels;
        public short wReserved1;
        public WaveCapsSupported dwSupport;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Name: " + szPname);
            builder.AppendLine("DriverVersion: " + vDriverVersion);
            builder.AppendLine("DriverSupported: " + dwSupport);
            builder.AppendLine("Formate: " + dwFormats.ToString());

            return builder.ToString();
        }
    }

    public enum WaveCapsFormats
    {
        /// <summary>
        /// 11.025 kHz, Mono,   8-bit
        /// </summary>
        WAVE_FORMAT_1M08 = 0x00000001,
        /// <summary>
        /// 11.025 kHz, Stereo, 8-bit
        /// </summary>
        WAVE_FORMAT_1S08 = 0x00000002,
        /// <summary>
        /// 11.025 kHz, Mono,   16-bit
        /// </summary>
        WAVE_FORMAT_1M16 = 0x00000004,
        /// <summary>
        /// 11.025 kHz, Stereo, 16-bit
        /// </summary>
        WAVE_FORMAT_1S16 = 0x00000008,
        /// <summary>
        /// 22.05  kHz, Mono,   8-bit
        /// </summary>
        WAVE_FORMAT_2M08 = 0x00000010,
        /// <summary>
        /// 22.05  kHz, Stereo, 8-bit 
        /// </summary>
        WAVE_FORMAT_2S08 = 0x00000020,
        /// <summary>
        /// 22.05  kHz, Mono,   16-bit
        /// </summary>
        WAVE_FORMAT_2M16 = 0x00000040,
        /// <summary>
        /// 22.05  kHz, Stereo, 16-bit
        /// </summary>
        WAVE_FORMAT_2S16 = 0x00000080,
        /// <summary>
        /// 44.1   kHz, Mono,   8-bit 
        /// </summary>
        WAVE_FORMAT_4M08 = 0x00000100,
        /// <summary>
        /// 44.1   kHz, Stereo, 8-bit 
        /// </summary>
        WAVE_FORMAT_4S08 = 0x00000200,
        /// <summary>
        /// 44.1   kHz, Mono,   16-bit
        /// </summary>
        WAVE_FORMAT_4M16 = 0x00000400,
        /// <summary>
        ///  44.1   kHz, Stereo, 16-bit
        /// </summary>
        WAVE_FORMAT_4S16 = 0x00000800,

        /// <summary>
        /// 44.1   kHz, Mono,   8-bit 
        /// </summary>
        WAVE_FORMAT_44M08 = 0x00000100,
        /// <summary>
        /// 44.1   kHz, Stereo, 8-bit 
        /// </summary>
        WAVE_FORMAT_44S08 = 0x00000200,
        /// <summary>
        /// 44.1   kHz, Mono,   16-bit
        /// </summary>
        WAVE_FORMAT_44M16 = 0x00000400,
        /// <summary>
        /// 44.1   kHz, Stereo, 16-bit
        /// </summary>
        WAVE_FORMAT_44S16 = 0x00000800,
        /// <summary>
        /// 48     kHz, Mono,   8-bit 
        /// </summary>
        WAVE_FORMAT_48M08 = 0x00001000,
        /// <summary>
        ///  48     kHz, Stereo, 8-bit
        /// </summary>
        WAVE_FORMAT_48S08 = 0x00002000,
        /// <summary>
        /// 48     kHz, Mono,   16-bit
        /// </summary>
        WAVE_FORMAT_48M16 = 0x00004000,
        /// <summary>
        /// 48     kHz, Stereo, 16-bit
        /// </summary>
        WAVE_FORMAT_48S16 = 0x00008000,
        /// <summary>
        /// 96     kHz, Mono,   8-bit 
        /// </summary>
        WAVE_FORMAT_96M08 = 0x00010000,
        /// <summary>
        /// 96     kHz, Stereo, 8-bit
        /// </summary>
        WAVE_FORMAT_96S08 = 0x00020000,
        /// <summary>
        /// 96     kHz, Mono,   16-bit
        /// </summary>
        WAVE_FORMAT_96M16 = 0x00040000,
        /// <summary>
        /// 96     kHz, Stereo, 16-bit
        /// </summary>
        WAVE_FORMAT_96S16 = 0x00080000,
    }
    [System.Flags]
    public enum WaveCapsSupported : int
    {
        WAVECAPS_PITCH = 1,
        WAVECAPS_PLAYBACKRATE = 2,
        WAVECAPS_VOLUME = 4,
        WAVECAPS_LRVOLUME = 8,
        WAVECAPS_SYNC = 16,
        WAVECAPS_SAMPLEACCURATE = 32,
        WAVECAPS_DIRECTSOUND = 64
    }
}