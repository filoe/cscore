using CSCore.SoundOut.MMInterop;
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

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
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
}