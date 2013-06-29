using System;
using System.Runtime.InteropServices;

namespace CSCore
{
    //http://msdn.microsoft.com/en-us/library/windows/hardware/ff536383(v=vs.85).aspx
    //http://msdn.microsoft.com/en-us/library/windows/hardware/gg463006.aspx
    [StructLayout(LayoutKind.Sequential, Pack=2, CharSet=CharSet.Ansi)]
    public class WaveFormatExtensible : WaveFormat
    {
        //extrasize: 22 bytes
        short _validBitsPerSample;
        ChannelMask _channelMask;
        Guid _subFormat;

        public static Guid SubTypeFromWaveFormat(WaveFormat waveFormat)
        {
            if(waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            if (waveFormat is WaveFormatExtensible)
                return ((WaveFormatExtensible)waveFormat).SubFormat;
            else
            {
                if (waveFormat.WaveFormatTag == AudioEncoding.IeeeFloat)
                    return DMO.MediaTypes.MEDIASUBTYPE_IEEE_FLOAT;
                else if (waveFormat.WaveFormatTag == AudioEncoding.Pcm)
                    return DMO.MediaTypes.MEDIASUBTYPE_PCM;
                else throw new NotSupportedException("Waveformat encoding is not supported: " + waveFormat.WaveFormatTag.ToString());
                //todo: mp3, gsm,...?
            }
        }

        public short ValidBitsPerSample
        {
            get { return _validBitsPerSample; }
        }

        public ChannelMask ChannelMask
        {
            get { return _channelMask; }
        }

        public Guid SubFormat
        {
            get { return _subFormat; }
        }

        internal WaveFormatExtensible()
        {
        }

        public WaveFormatExtensible(int sampleRate, int bits, int channels, Guid subFormat)
            : base(sampleRate, bits, channels, AudioEncoding.Extensible, 22)
        {
            _validBitsPerSample = (short)bits;
            _subFormat = SubTypeFromWaveFormat(this);
            int cm = 0;
            for (int i = 0; i < channels; i++)
            {
                cm |= (1 << i);
            }

            _channelMask = (CSCore.ChannelMask)cm;
            _subFormat = subFormat;
        }

        public WaveFormatExtensible(int sampleRate, int bits, int channels, Guid subFormat, ChannelMask channelMask)
            : this(sampleRate, bits, channels, subFormat)
        {
            var totalChannelMaskValues = Enum.GetValues(typeof(ChannelMask));
            int valuesSet = 0;
            for (int i = 0; i < totalChannelMaskValues.Length; i++)
            {
                if (channelMask.HasFlag((CSCore.ChannelMask)totalChannelMaskValues.GetValue(i)))
                    valuesSet++;
            }

            if (channels != valuesSet)
                throw new ArgumentException("Channels has to equal the set bits in the channelmask");

            _channelMask = channelMask;
        }
    }

    /// <summary>
    /// Channelmask for WaveFormatExtensible. For more infos see http://msdn.microsoft.com/en-us/library/windows/desktop/dd757714(v=vs.85).aspx
    /// </summary>
    [Flags]
    public enum ChannelMask
    {
        SpeakerFrontLeft = 0x1,
        SpeakerFrontRight = 0x2,
        SpeakerFrontCenter = 0x4,
        SpeakerLowFrequency = 0x8,
        SpeakerBackLeft = 0x10,
        SpeakerBackRight = 0x20,
        SpeakerFrontLeftOfCenter = 0x40,
        SpeakerFrontRightOfCenter = 0x80,
        SpeakerBackCenter = 0x100,
        SpeakerSideLeft = 0x200,
        SpeakerSideRight = 0x400,
        SpeakerTopCenter = 0x800,
        SpeakerTopFrontLeft = 0x1000,
        SpeakerTopFrontCenter = 0x2000,
        SpeakerTopFrontRight = 0x4000,
        SpeakerTopBackLeft = 0x8000,
        SpeakerTopBackCenter = 0x10000,
        SpeakerTopBackRight = 0x20000
    }
}
