using System;
using System.IO;
using System.Reflection;
using System.Threading;
using CSCore.Codecs.WAV;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.SampleConverter;

namespace CSCore
{
    /// <summary>
    ///     Provides a few basic extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Converts a SampleSource to either a Pcm (8, 16, or 24 bit) or IeeeFloat (32 bit) WaveSource.
        /// </summary>
        /// <param name="sampleSource">Sample source to convert to a wave source.</param>
        /// <param name="bits">Bits per sample.</param>
        /// <returns>Wave source</returns>
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
            throw new ArgumentOutOfRangeException("bits");
        }

        /// <summary>
        ///     Converts a SampleSource to IeeeFloat (32bit) WaveSource.
        /// </summary>
        public static IWaveSource ToWaveSource(this ISampleSource sampleSource)
        {
            if (sampleSource == null)
                throw new ArgumentNullException("sampleSource");

            return new SampleToIeeeFloat32(sampleSource);
        }

        /// <summary>
        ///     Converts a WaveSource to a SampleSource.
        /// </summary>
        public static ISampleSource ToSampleSource(this IWaveSource waveSource)
        {
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");

            return WaveToSampleBase.CreateConverter(waveSource);
        }

        /// <summary>
        ///     Gets the length of a WaveStream as a TimeSpan.
        /// </summary>
        public static TimeSpan GetLength(this IWaveStream source)
        {
            return GetTime(source, source.Length);
        }

        /// <summary>
        ///     Gets the position of a WaveStream as a TimeSpan.
        /// </summary>
        public static TimeSpan GetPosition(this IWaveStream source)
        {
            return GetTime(source, source.Position);
        }

        /// <summary>
        ///     Sets the position of a WaveStream as a TimeSpan.
        /// </summary>
        public static void SetPosition(this IWaveStream source, TimeSpan position)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (position.TotalMilliseconds < 0)
                throw new ArgumentOutOfRangeException("position");

            long bytes = GetBytes(source, (long) position.TotalMilliseconds);
            source.Position = bytes;
        }

        /// <summary>
        ///     Converts a duration in bytes to a <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="source">
        ///     <see cref="IWaveSource" /> instance which provides the <see cref="WaveFormat" /> used to convert
        ///     the duration in bytes to a <see cref="TimeSpan" />.
        /// </param>
        /// <param name="bytes">Duration in bytes to convert to a <see cref="TimeSpan" />.</param>
        /// <returns>Duration as a <see cref="TimeSpan" />.</returns>
        public static TimeSpan GetTime(this IWaveStream source, long bytes)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (bytes < 0)
                throw new ArgumentNullException("bytes");
            if (source is ISampleSource)
                bytes *= 4;

            return TimeSpan.FromMilliseconds(GetMilliseconds(source, bytes));
        }

        /// <summary>
        ///     Converts a duration in bytes to a duration in milliseconds. 
        /// </summary>
        /// <param name="source">
        ///     <see cref="IWaveSource" /> instance which provides the <see cref="WaveFormat" /> used to convert
        ///     the duration in bytes to a duration in milliseconds.
        /// </param>
        /// <param name="bytes">Duration in bytes to convert to a duration in milliseconds.</param>
        /// <returns>Duration in milliseconds.</returns>
        /// <remarks>Note that a <see cref="ISampleSource"/> works with samples instead of bytes.</remarks>
        public static long GetMilliseconds(this IWaveStream source, long bytes)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (bytes < 0)
                throw new ArgumentOutOfRangeException("bytes");

            return source.WaveFormat.BytesToMilliseconds(bytes);
        }

        /// <summary>
        ///     Converts a duration as a <see cref="TimeSpan" /> to a duration in bytes.
        /// </summary>
        /// <param name="source">
        ///     <see cref="IWaveSource" /> instance which provides the <see cref="WaveFormat" /> used to convert
        ///     the duration as a <see cref="TimeSpan" /> to a duration in bytes.
        /// </param>
        /// <param name="timespan">Duration as a <see cref="TimeSpan" /> to convert to a duration in bytes.</param>
        /// <returns>Duration in bytes.</returns>
        public static long GetBytes(this IWaveStream source, TimeSpan timespan)
        {
            return GetBytes(source, (long) timespan.TotalMilliseconds);
        }

        /// <summary>
        ///     Converts a duration in milliseconds to a duration in bytes.
        /// </summary>
        /// <param name="source">
        ///     <see cref="IWaveSource" /> instance which provides the <see cref="WaveFormat" /> used to convert
        ///     the duration in milliseconds to a duration in bytes.
        /// </param>
        /// <param name="milliseconds">Duration in milliseconds to convert to a duration in bytes.</param>
        /// <returns>Duration in bytes.</returns>
        public static long GetBytes(this IWaveStream source, long milliseconds)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (milliseconds < 0)
                throw new ArgumentOutOfRangeException("milliseconds");

            if (source is IWaveSource)
                return source.WaveFormat.MillisecondsToBytes(milliseconds);
            if (source is ISampleSource)
                return source.WaveFormat.MillisecondsToBytes(milliseconds / 4);
            throw new NotSupportedException("IWaveStream-Subtype is not supported");
        }

        /// <summary>
        ///     Writes down all audio data of the <paramref name="source" /> to a file.
        /// </summary>
        /// <param name="source">Source which provides the audio data to write down to the file.</param>
        /// <param name="filename">Filename which specifies the file to use.</param>
        public static void WriteToFile(this IWaveSource source, string filename)
        {
            using (FileStream stream = File.OpenWrite(filename))
            {
                WriteToWaveStream(source, stream);
            }
        }

        /// <summary>
        ///     Writes down all audio data of the <paramref name="source" /> to a <see cref="Stream" />.
        /// </summary>
        /// <param name="source">Source which provides the audio data to write down to the <see cref="Stream" />.</param>
        /// <param name="stream"><see cref="Stream" /> to store the audio data in.</param>
        public static void WriteToWaveStream(this IWaveSource source, Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException();
            if (!stream.CanWrite)
                throw new ArgumentException("Stream not writeable.", "stream");

            using (var writer = new WaveWriter(stream, source.WaveFormat))
            {
                int read;
                var buffer = new byte[source.WaveFormat.BytesPerSecond];
                while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writer.Write(buffer, 0, read);
                }
            }
        }

        /// <summary>
        /// Checks the length of an array.
        /// </summary>
        /// <typeparam name="T">Type of the array.</typeparam>
        /// <param name="inst">The array to check. This parameter can be null.</param>
        /// <param name="size">The target length of the array.</param>
        /// <param name="exactSize">A value which indicates whether the length of the array has to fit exactly the specified <paramref name="size"/>.</param>
        /// <returns>Array which fits the specified requirements. Note that if a new array got created, the content of the old array won't get copied to the return value.</returns>
        public static T[] CheckBuffer<T>(this T[] inst, long size, bool exactSize = false)
        {
            if (inst == null || (!exactSize && inst.Length < size) || (exactSize && inst.Length != size))
                return new T[size];
            return inst;
        }

        /// <summary>
        /// Creates a thread-safe (synchronized) wrapper around the specified <typeparamref name="TWaveSource"/> object.
        /// </summary>
        /// <param name="waveSource">The <typeparamref name="TWaveSource"/> object to synchronize.</param>
        /// <typeparam name="TWaveSource">Type of the <paramref name="waveSource"/> argument.</typeparam>
        /// <returns>A thread-safe wrapper around the specified <typeparamref name="TWaveSource"/> object.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="waveSource"/> is null.</exception>
        public static SynchronizedWaveSource<TWaveSource> Synchronized<TWaveSource>(this TWaveSource waveSource)
            where TWaveSource : class, IWaveSource
        {
            if(waveSource == null)
                throw new ArgumentNullException("waveSource");

            return new SynchronizedWaveSource<TWaveSource>(waveSource);
        }

        internal static byte[] ReadBytes(this IWaveSource waveSource, int count)
        {
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");
            if (count <= 0 || (count % waveSource.WaveFormat.BlockAlign) != 0)
                throw new ArgumentOutOfRangeException("count");

            byte[] buffer = new byte[count];
            int read = waveSource.Read(buffer, 0, buffer.Length);
            if (read < count)
                Array.Resize(ref buffer, read);
            return buffer;
        }

        internal static bool IsClosed(this Stream stream)
        {
            return !stream.CanRead && !stream.CanWrite;
        }

        internal static bool IsEndOfStream(this Stream stream)
        {
            return stream.Position == stream.Length;
        }

        internal static int LowWord(this int number)
        {
            return number & 0x0000FFFF;
        }

        internal static int LowWord(this int number, int newValue)
        {
            return (int) ((number & 0xFFFF0000) + (newValue & 0x0000FFFF));
        }

        internal static int HighWord(this int number)
        {
            return (int) (number & 0xFFFF0000);
        }

        internal static int HighWord(this int number, int newValue)
        {
            return (number & 0x0000FFFF) + (newValue << 16);
        }

        internal static uint LowWord(this uint number)
        {
            return number & 0x0000FFFF;
        }

        internal static uint LowWord(this uint number, int newValue)
        {
            return (uint) ((number & 0xFFFF0000) + (newValue & 0x0000FFFF));
        }

        internal static uint HighWord(this uint number)
        {
            return number & 0xFFFF0000;
        }

        internal static uint HighWord(this uint number, int newValue)
        {
            return (uint) ((number & 0x0000FFFF) + (newValue << 16));
        }

        internal static Guid GetGuid(this Object obj)
        {
            return obj.GetType().GUID;
        }

        internal static void WaitForExit(this Thread thread)
        {
            if (thread == null)
                return;
            if (thread == Thread.CurrentThread)
                throw new InvalidOperationException("Deadlock detected.");

            thread.Join();
        }

        internal static bool WaitForExit(this Thread thread, int timeout)
        {
            if (thread == null)
                return true;
            if (thread == Thread.CurrentThread)
                throw new InvalidOperationException("Deadlock detected.");

            return thread.Join(timeout);
        }

// ReSharper disable once InconsistentNaming
        internal static bool IsPCM(this WaveFormat waveFormat)
        {
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            if (waveFormat is WaveFormatExtensible)
                return ((WaveFormatExtensible) waveFormat).SubFormat == AudioSubTypes.Pcm;
            return waveFormat.WaveFormatTag == AudioEncoding.Pcm;
        }

        internal static bool IsIeeeFloat(this WaveFormat waveFormat)
        {
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            if (waveFormat is WaveFormatExtensible)
                return ((WaveFormatExtensible) waveFormat).SubFormat == AudioSubTypes.IeeeFloat;
            return waveFormat.WaveFormatTag == AudioEncoding.IeeeFloat;
        }

        internal static AudioEncoding GetWaveFormatTag(this WaveFormat waveFormat)
        {
            if (waveFormat is WaveFormatExtensible)
                return AudioSubTypes.EncodingFromMediaType(((WaveFormatExtensible) waveFormat).SubFormat);

            return waveFormat.WaveFormatTag;
        }

        /// <summary>
        ///     Not tested. This method can be buggy.
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

            using (var waitHandle = new AutoResetEvent(false))
            {
// ReSharper disable once AccessToDisposedClosure
                soundOut.Stopped += (s, e) => waitHandle.Set();
                return waitHandle.WaitOne(timeout);
            }
        }

        /// <summary>
        ///     Not tested. This method can be buggy.
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