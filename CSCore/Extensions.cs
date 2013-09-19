using CSCore.Codecs.MP3;
using CSCore.Codecs.WAV;
using CSCore.Streams.SampleConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSCore
{
    public static class Extensions
    {
        public static IWaveSource ToWaveSource(this ISampleSource sampleSource, int bits)
        {
            if (sampleSource == null)
                throw new ArgumentNullException("sampleSource");

            if (bits == 8)
                return new SampleToPcm8(sampleSource);
            if (bits == 16)
                return new SampleToPcm16(sampleSource);
            if (bits == 24)
                return new SampleToPcm24(sampleSource);
            if (bits == 32)
                return new SampleToIeeeFloat32(sampleSource);
            else
                throw new ArgumentOutOfRangeException("bits");
        }

        public static ISampleSource ToSampleSource(this IWaveSource waveSource)
        {
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");

            return WaveToSampleBase.CreateConverter(waveSource);
        }

        public static TimeSpan GetLength(this IWaveStream source)
        {
            return GetTime(source, source.Length);
        }

        public static TimeSpan GetPosition(this IWaveStream source)
        {
            return GetTime(source, source.Position);
        }

        public static void SetPosition(this IWaveStream source, TimeSpan position)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (position.TotalMilliseconds < 0)
                throw new ArgumentOutOfRangeException("position");

            var bytes = GetBytes(source, (long)position.TotalMilliseconds);
            source.Position = bytes;
        }

        public static TimeSpan GetTime(this IWaveStream source, long bytes)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (bytes < 0)
                throw new ArgumentNullException("bytes");
            return TimeSpan.FromMilliseconds(GetMilliseconds(source, bytes));
        }

        public static long GetMilliseconds(this IWaveStream source, long bytes)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (bytes < 0)
                throw new ArgumentOutOfRangeException("bytes");

            if (source is IWaveSource)
            {
                return source.WaveFormat.BytesToMilliseconds(bytes);
            }
            else if (source is ISampleSource)
            {
                return source.WaveFormat.BytesToMilliseconds(bytes * 4);
            }
            else
            {
                throw new NotSupportedException("IWaveStream-Subtype is not supported");
            }
        }

        public static long GetBytes(this IWaveStream source, TimeSpan timespan)
        {
            return GetBytes(source, (long)timespan.TotalMilliseconds);
        }

        public static long GetBytes(this IWaveStream source, long milliseconds)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (milliseconds < 0)
                throw new ArgumentOutOfRangeException("milliseconds");

            if (source is IWaveSource)
            {
                return source.WaveFormat.MillisecondsToBytes(milliseconds);
            }
            else if (source is ISampleSource)
            {
                return source.WaveFormat.MillisecondsToBytes(milliseconds / 4);
            }
            else
            {
                throw new NotSupportedException("IWaveStream-Subtype is not supported");
            }
        }

        public static void WriteToWaveStream(this IWaveSource source, string filename)
        {
            using (var stream = File.OpenWrite(filename))
            {
                WriteToWaveStream(source, stream);
            }
        }

        public static void WriteToWaveStream(this IWaveSource source, Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException();
            if (!stream.CanWrite)
                throw new ArgumentException("Stream not writeable.", "stream");

            using (var writer = new WaveWriter(stream, source.WaveFormat))
            {
                int read = 0;
                byte[] buffer = new byte[source.WaveFormat.BytesPerSecond];
                while((read = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writer.Write(buffer, 0, read);
                }
            }
        }

        public static T[] CheckBuffer<T>(this T[] inst, long size, bool exactSize = false)
        {
            if (inst == null || (!exactSize && inst.Length < size) || (exactSize && inst.Length != size))
            {
                return new T[size];
            }
            return inst;
        }

        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }

        public static bool IsClosed(this Stream stream)
        {
            return stream.CanRead || stream.CanWrite;
        }

        public static bool IsEndOfStream(this Stream stream)
        {
            return stream.Position == stream.Length;
        }

        public static int LowWord(this int number)
        { return number & 0x0000FFFF; }

        public static int LowWord(this int number, int newValue)
        { return (int)((number & 0xFFFF0000) + (newValue & 0x0000FFFF)); }

        public static int HighWord(this int number)
        { return (int)(number & 0xFFFF0000); }

        public static int HighWord(this int number, int newValue)
        { return (number & 0x0000FFFF) + (newValue << 16); }

        public static uint LowWord(this uint number)
        { return number & 0x0000FFFF; }

        public static uint LowWord(this uint number, int newValue)
        { return (uint)((number & 0xFFFF0000) + (newValue & 0x0000FFFF)); }

        public static uint HighWord(this uint number)
        { return (uint)(number & 0xFFFF0000); }

        public static uint HighWord(this uint number, int newValue)
        { return (uint)((number & 0x0000FFFF) + (newValue << 16)); }

        public static Guid GetGuid(this Object obj)
        {
            return obj.GetType().GUID;
        }
    }
}