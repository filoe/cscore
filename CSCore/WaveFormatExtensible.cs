using CSCore.DMO;
using System;
using System.Runtime.InteropServices;

namespace CSCore
{
    //http://msdn.microsoft.com/en-us/library/windows/hardware/ff536383(v=vs.85).aspx
    //http://msdn.microsoft.com/en-us/library/windows/hardware/gg463006.aspx
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public class WaveFormatExtensible : WaveFormat
    {
        //extrasize: 22 bytes
        private short _validBitsPerSample;

        private ChannelMask _channelMask;
        private Guid _subFormat;

        public static Guid SubTypeFromWaveFormat(WaveFormat waveFormat)
        {
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            if (waveFormat is WaveFormatExtensible)
                return ((WaveFormatExtensible)waveFormat).SubFormat;
            else
            {
                return MediaTypes.MediaTypeFromEncoding(waveFormat.WaveFormatTag);
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
                if ((channelMask & (CSCore.ChannelMask)totalChannelMaskValues.GetValue(i)) == (CSCore.ChannelMask)totalChannelMaskValues.GetValue(i))
                    valuesSet++;
            }

            if (channels != valuesSet)
                throw new ArgumentException("Channels has to equal the set bits in the channelmask");

            _channelMask = channelMask;
        }

        public bool IsPcm
        {
            get { return this.IsPCM(); }
        }

        public bool IsIeeeFloat
        {
            get { return this.IsIeeeFloat(); }
        }

        public WaveFormat ToWaveFormat()
        {
            return new WaveFormat(SampleRate, BitsPerSample, Channels, MediaTypes.EncodingFromMediaType(SubFormat));
        }
    }
}