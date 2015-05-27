using System.Collections.Generic;

namespace CSCore.SoundOut.MMInterop
{
    internal static class Utils
    {
        public static WaveFormat[] SupportedFormatsFlagsToWaveFormats(MmDeviceFormats dwFormats)
        {
            List<WaveFormat> waveFormats = new List<WaveFormat>();
            if ((dwFormats & MmDeviceFormats.Format1M08) == MmDeviceFormats.Format1M08)
                waveFormats.Add(new WaveFormat(11025, 8, 1));
            if ((dwFormats & MmDeviceFormats.Format1M16) == MmDeviceFormats.Format1M16)
                waveFormats.Add(new WaveFormat(11025, 16, 1));

            if ((dwFormats & MmDeviceFormats.Format1S08) == MmDeviceFormats.Format1S08)
                waveFormats.Add(new WaveFormat(11025, 8, 2));
            if ((dwFormats & MmDeviceFormats.Format1S16) == MmDeviceFormats.Format1S16)
                waveFormats.Add(new WaveFormat(11025, 16, 2));

            if ((dwFormats & MmDeviceFormats.Format2M08) == MmDeviceFormats.Format2M08)
                waveFormats.Add(new WaveFormat(22050, 8, 1));
            if ((dwFormats & MmDeviceFormats.Format2M16) == MmDeviceFormats.Format2M16)
                waveFormats.Add(new WaveFormat(22050, 16, 1));

            if ((dwFormats & MmDeviceFormats.Format2S08) == MmDeviceFormats.Format2S08)
                waveFormats.Add(new WaveFormat(22050, 8, 2));
            if ((dwFormats & MmDeviceFormats.Format2S16) == MmDeviceFormats.Format2S16)
                waveFormats.Add(new WaveFormat(22050, 16, 2));

            if ((dwFormats & MmDeviceFormats.Format4M08) == MmDeviceFormats.Format4M08)
                waveFormats.Add(new WaveFormat(44100, 8, 1));
            if ((dwFormats & MmDeviceFormats.Format4M16) == MmDeviceFormats.Format4M16)
                waveFormats.Add(new WaveFormat(44100, 16, 1));

            if ((dwFormats & MmDeviceFormats.Format4S08) == MmDeviceFormats.Format4S08)
                waveFormats.Add(new WaveFormat(44100, 8, 2));
            if ((dwFormats & MmDeviceFormats.Format4S16) == MmDeviceFormats.Format4S16)
                waveFormats.Add(new WaveFormat(44100, 16, 2));

            if ((dwFormats & MmDeviceFormats.Format96M08) == MmDeviceFormats.Format96M08)
                waveFormats.Add(new WaveFormat(96000, 8, 1));
            if ((dwFormats & MmDeviceFormats.Format96M16) == MmDeviceFormats.Format96M16)
                waveFormats.Add(new WaveFormat(96000, 16, 1));

            if ((dwFormats & MmDeviceFormats.Format96S08) == MmDeviceFormats.Format96S08)
                waveFormats.Add(new WaveFormat(96000, 8, 2));
            if ((dwFormats & MmDeviceFormats.Format96S16) == MmDeviceFormats.Format96S16)
                waveFormats.Add(new WaveFormat(96000, 16, 2));

            return waveFormats.ToArray();
        }
    }
}