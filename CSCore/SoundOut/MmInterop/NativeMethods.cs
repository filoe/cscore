using System;
using System.Runtime.InteropServices;
using System.Text;
using CSCore.Utils;

namespace CSCore.SoundOut.MMInterop
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
        public const int MAXERRORTEXTLENGTH = 256;

        [Flags]
        public enum WaveInOutOpenFlags
        {
            CALLBACK_NULL = 0,
            CALLBACK_FUNCTION = 0x30000,
            CALLBACK_EVENT = 0x50000,
            CALLBACK_WINDOW = 0x10000,
            CALLBACK_THREAD = 0x20000,

            WAVE_FORMAT_QUERY = 1,
            WAVE_MAPPED = 4,
            WAVE_FORMAT_DIRECT = 8
        }

        #region WaveOut

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutPrepareHeader(IntPtr hWaveOut, WaveHeader lpWaveOutHdr, int uSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutWrite(IntPtr hWaveOut, WaveHeader lpWaveOutHdr, int uSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutUnprepareHeader(IntPtr hWaveOut, WaveHeader lpWaveOutHdr, int uSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutOpen(out IntPtr hWaveOut, IntPtr device, WaveFormat lpFormat, WaveCallback dwCallback, IntPtr dwInstance, WaveInOutOpenFlags dwFlags);

        [DllImport("winmm.dll", EntryPoint = "waveOutOpen")]
        public static extern MmResult waveOutOpenWithWindow(out IntPtr hWaveOut, IntPtr device, WaveFormat lpFormat, IntPtr window, IntPtr dwInstance, WaveInOutOpenFlags dwFlags);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutReset(IntPtr hWaveOut);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutClose(IntPtr hWaveOut);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutPause(IntPtr hWaveOut);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutRestart(IntPtr hWaveOut);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutSetVolume(IntPtr hWaveOut, uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetVolume(IntPtr hWaveOut, out uint pdwVolume);

        [DllImport("winmm.dll")]
        public static extern Int32 waveOutGetNumDevs();

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetDevCaps(IntPtr deviceID, out WaveOutCaps waveOutCaps, uint cbwaveOutCaps);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetPitch(IntPtr hWaveOut, IntPtr pdwPitch);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutSetPitch(IntPtr hWaveOut, int dwPitch);

        //http://msdn.microsoft.com/en-us/library/aa910385.aspx
        [DllImport("winmm.dll")]
        public static extern MmResult waveOutSetPlaybackRate(IntPtr hWaveOut, int dwRate);

        //http://msdn.microsoft.com/en-us/library/aa910193.aspx
        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetPlaybackRate(IntPtr hWaveOut, IntPtr pdwRate);

        #endregion WaveOut

        #region waveIn

        [DllImport("winmm.dll")]
        public static extern MmResult waveInOpen(out IntPtr hWaveIn, IntPtr device, WaveFormat pwfx, WaveCallback dwCallback, IntPtr dwInstance, WaveInOutOpenFlags fdwOpen);

        [DllImport("winmm.dll", EntryPoint = "waveInOpen")]
        public static extern MmResult waveInOpenWithWindow(out IntPtr hWaveIn, IntPtr device, WaveFormat pwfx, IntPtr window, IntPtr dwInstance, WaveInOutOpenFlags fdwOpen);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInPrepareHeader(IntPtr hWaveIn, WaveHeader waveHdr, int headerSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInUnprepareHeader(IntPtr hWaveIn, WaveHeader waveHdr, int headerSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInAddBuffer(IntPtr hWaveIn, WaveHeader waveHdr, int headerSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInStart(IntPtr hWaveIn);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInStop(IntPtr hWaveIn);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInReset(IntPtr hWaveIn);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInClose(IntPtr hWaveIn);

        [DllImport("winmm.dll")]
        public static extern Int32 waveInGetNumDevs();

        [DllImport("winmm.dll")]
        public static extern MmResult waveInGetDevCaps(IntPtr deviceID, out CSCore.SoundIn.WaveInCaps waveInCaps, uint cbWaveInCaps);


        #endregion waveIn

        public static MmResult SetSystemVolume(uint dwVolume)
        {
            return waveOutSetVolume(IntPtr.Zero, dwVolume);
        }

        public static MmResult GetSystemVolume(out uint pdwVolume)
        {
            return waveOutGetVolume(IntPtr.Zero, out pdwVolume);
        }

        public static void SetVolume(IntPtr waveOut, float left, float right)
        {
            uint tmp = (uint)(left * 0xFFFF) + ((uint)(right * 0xFFFF) << 16);
            MmResult result = waveOutSetVolume(waveOut, tmp);
            MmException.Try(result,
                "waveOutSetVolume");
        }

        public static float GetVolume(IntPtr waveOut)
        {
            uint volume;
            MmResult result = waveOutGetVolume(waveOut, out volume);
            MmException.Try(result, "waveOutGetVolume");
            HightLowConverterUInt32 u = new HightLowConverterUInt32(volume);
            uint left = u.High;
            uint right = u.Low;
            return (float)(((right + left) / 2.0) * (1.0 / 0xFFFF));
        }

        public static void GetVolume(IntPtr waveOut, out float left, out float right)
        {
            uint volume;
            MmException.Try(waveOutGetVolume(waveOut, out volume), "waveOutGetVolume");
            left = (float)volume / 0xFFFF;
            right = ((volume / 0xFFFF) << 16);
        }
    }
}