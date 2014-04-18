using CSCore.Codecs.MP3;
using CSCore.Codecs.WAV;
using CSCore.SoundOut;
using CSCore.Streams.SampleConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace CSCore
{
    //[CLSCompliant(true)]
    public static class Extensions
    {
        /// <summary>
        /// Converts a SampleSource to either a Pcm (8, 16, or 24 bit) or IeeeFloat (32 bit) WaveSource.
        /// </summary>
        /// <param name="sampleSource"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts a SampleSource to IeeeFloat (32bit) WaveSource.
        /// </summary>
        public static IWaveSource ToWaveSource(this ISampleSource sampleSource)
        {
            if (sampleSource == null)
                throw new ArgumentNullException("sampleSource");

            return new SampleToIeeeFloat32(sampleSource);
        }

        /// <summary>
        /// Converts a WaveSource to a SampleSource.
        /// </summary>
        public static ISampleSource ToSampleSource(this IWaveSource waveSource)
        {
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");

            return WaveToSampleBase.CreateConverter(waveSource);
        }

        /// <summary>
        /// Gets the length of a WaveStream as a TimeSpan.
        /// </summary>
        public static TimeSpan GetLength(this IWaveStream source)
        {
            return GetTime(source, source.Length);
        }

        /// <summary>
        /// Gets the position of a WaveStream as a TimeSpan.
        /// </summary>
        public static TimeSpan GetPosition(this IWaveStream source)
        {
            return GetTime(source, source.Position);
        }

        /// <summary>
        /// Sets the position of a WaveStream as a TimeSpan.
        /// </summary>
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

        public static void WriteToFile(this IWaveSource source, string filename)
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

        public static void WaitForExit(this Thread thread)
        {
            if (thread == null)
                return;
            if (thread == Thread.CurrentThread)
                throw new InvalidOperationException("Deadlock detected.");

            thread.Join();
        }

        public static bool WaitForExit(this Thread thread, int timeout)
        {
            if (thread == null)
                return true;
            if (thread == Thread.CurrentThread)
                throw new InvalidOperationException("Deadlock detected.");

            return thread.Join(timeout);
        }

        public static bool IsPCM(this WaveFormat waveFormat)
        {
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            if (waveFormat is WaveFormatExtensible)
                return ((WaveFormatExtensible)waveFormat).SubFormat == DMO.MediaTypes.MEDIATYPE_Pcm;
            else
                return waveFormat.WaveFormatTag == AudioEncoding.Pcm;
        }

        public static bool IsIeeeFloat(this WaveFormat waveFormat)
        {
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            if (waveFormat is WaveFormatExtensible)
                return ((WaveFormatExtensible)waveFormat).SubFormat == DMO.MediaTypes.MEDIATYPE_IeeeFloat;
            else
                return waveFormat.WaveFormatTag == AudioEncoding.IeeeFloat;
        }

        public static AudioEncoding GetWaveFormatTag(this WaveFormat waveFormat)
        {
            if(waveFormat is WaveFormatExtensible)
            {
                return DMO.MediaTypes.EncodingFromMediaType(((WaveFormatExtensible)waveFormat).SubFormat);
            }

            return waveFormat.WaveFormatTag;
        }

        /// <summary>
        /// Not tested. This method can be buggy.
        /// </summary>
        /// <param name="soundOut"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool WaitForStopped(this ISoundOut soundOut, int timeout)
        {
            if (soundOut == null)
                throw new ArgumentNullException("soundOut");
            if (timeout < 0)
                throw new ArgumentOutOfRangeException("timeout");

            if (soundOut.PlaybackState == PlaybackState.Stopped)
                return true;

            using(var waitHandle = new AutoResetEvent(false))
            {
                soundOut.Stopped += (s, e) => waitHandle.Set();
                return waitHandle.WaitOne(timeout);
            }
        }

        /// <summary>
        /// Not tested. This method can be buggy.
        /// </summary>
        /// <param name="soundOut"></param>
        public static void WaitForStopped(this ISoundOut soundOut)
        {
            WaitForStopped(soundOut, Int32.MaxValue);
        }

        //copied from http://stackoverflow.com/questions/9927590/can-i-set-a-value-on-a-struct-through-reflection-without-boxing
        internal static void SetValueForValueType<T>(this FieldInfo field, ref T item, object value) where T : struct
        {
            field.SetValueDirect(__makeref(item), value);
        }
    }
}