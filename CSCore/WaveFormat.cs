using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class WaveFormat
    {
        protected AudioEncoding _encoding;
        protected short _channels;
        protected int _sampleRate;
        protected int _bytesPerSecond;

        protected short blockAlign;
        protected short bitsPerSample;
        protected short extraSize;

        public short Channels
        {
            get { return _channels; }
        }

        public int SampleRate
        {
            get { return _sampleRate; }
        }

        public int BytesPerSecond
        {
            get { return _bytesPerSecond; }
        }

        /// <summary>
        /// Frame-Size = [channels>] * (( [bits/sample]+7) / 8)
        /// </summary>
        public int BlockAlign
        {
            get { return blockAlign; }
        }

        public short BitsPerSample
        {
            get { return bitsPerSample; }
        }

        public int ExtraSize
        {
            get { return extraSize; }
            internal set { extraSize = (short)value; }
        }

        public int BytesPerSample
        {
            get { return BitsPerSample / 8; }
        }

        public int BytesPerBlock
        {
            get { return BytesPerSample * Channels; }
        }

        public AudioEncoding WaveFormatTag { get { return _encoding; } }

        /// <summary>
        /// 44100Hz, 16bps, 2 channels, pcm
        /// </summary>
        public WaveFormat()
            : this(44100, 16, 2)
        {
        }

        public WaveFormat(WaveFormat waveFormat, int sampleRate)
            : this(sampleRate, waveFormat.BitsPerSample, waveFormat.Channels, waveFormat._encoding)
        {
        }

        /// <summary>
        /// PCM
        /// </summary>
        public WaveFormat(int sampleRate, int bits, int channels)
            : this(sampleRate, bits, channels, AudioEncoding.Pcm)
        {
        }

        public WaveFormat(int sampleRate, int bits, int channels, AudioEncoding encoding)
            : this(sampleRate, bits, channels, encoding, 0)
        {
        }

        public WaveFormat(int sampleRate, int bits, int channels, AudioEncoding encoding, int extraSize)
        {
            if (sampleRate < 1)
                throw new ArgumentOutOfRangeException("_sampleRate");
            if (bits < 0)
                throw new ArgumentOutOfRangeException("bits");
            if (channels < 1)
                throw new ArgumentOutOfRangeException("Channels must be > 0");

            this._sampleRate = sampleRate;
            this.bitsPerSample = (short)bits;
            this._channels = (short)channels;
            this._encoding = encoding;
            this.blockAlign = (short)(channels * (bits / 8));
            this._bytesPerSecond = (sampleRate * blockAlign);
            this.ExtraSize = (short)extraSize;
        }

        public long MillisecondsToBytes(long milliseconds)
        {
            long result = (long)((BytesPerSecond / 1000.0) * milliseconds);
            result -= result % BlockAlign;
            return result;
        }

        public long BytesToMilliseconds(long bytes)
        {
            bytes -= bytes % BlockAlign;
            long result = (long)(((double)bytes / (double)BytesPerSecond) * 1000);
            return result;
        }

        public override string ToString()
        {
            return GetInformation().ToString();
        }

        protected StringBuilder GetInformation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ChannelsAvailable: " + Channels);
            builder.Append("|SampleRate: " + SampleRate);
            builder.Append("|Bps: " + BytesPerSecond);
            builder.Append("|BlockAlign: " + BlockAlign);
            builder.Append("|BitsPerSample: " + BitsPerSample);
            builder.Append("|Encoding: " + _encoding);

            return builder;
        }
    }

    
}