using System;
using System.IO;
using System.Reflection;
using System.Threading;
using CSCore.Codecs.WAV;
using CSCore.SoundOut;

namespace CSCore
{
    /// <summary>
    ///     Provides a few basic extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Gets the length of a <see cref="IAudioSource"/> as a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="source">The source to get the length for.</param>
        /// <returns>The length of the specified <paramref name="source"/> as a <see cref="TimeSpan"/> value.</returns>
        public static TimeSpan GetLength(this IAudioSource source)
        {
            return GetTime(source, source.Length);
        }

        /// <summary>
        ///     Gets the position of a <see cref="IAudioSource"/> as a <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="source">The source to get the position of.</param>
        /// <returns>The position of the specified <paramref name="source"/> as a <see cref="TimeSpan"/> value.</returns>
        /// <remarks>The source must support seeking to get or set the position. 
        /// Use the <see cref="IAudioSource.CanSeek"/> property to determine whether the stream supports seeking.
        /// Otherwise a call to this method may result in an exception.</remarks>
        public static TimeSpan GetPosition(this IAudioSource source)
        {
            return GetTime(source, source.Position);
        }

        /// <summary>
        ///     Sets the position of a <see cref="IAudioSource"/> as a <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="source">The source to set the new position for.</param>
        /// <param name="position">The new position as a <see cref="TimeSpan"/> value.</param>
        /// <remarks>
        /// The source must support seeking to get or set the position. 
        /// Use the <see cref="IAudioSource.CanSeek"/> property to determine whether the stream supports seeking.
        /// Otherwise a call to this method may result in an exception.
        /// </remarks>
        public static void SetPosition(this IAudioSource source, TimeSpan position)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (position.TotalMilliseconds < 0)
                throw new ArgumentOutOfRangeException("position");

            long bytes = GetRawElements(source, position);
            source.Position = bytes;
        }


        /// <summary>
        /// Converts a duration in raw elements to a <see cref="TimeSpan"/> value. For more information about "raw elements" see remarks.
        /// </summary>
        /// <param name="source">The <see cref="IAudioSource"/> instance which provides the <see cref="WaveFormat"/> used 
        /// to convert the duration in "raw elements" to a <see cref="TimeSpan"/> value.</param>
        /// <param name="elementCount">The duration in "raw elements" to convert to a <see cref="TimeSpan"/> value.</param>
        /// <returns>The duration as a <see cref="TimeSpan"/> value.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// elementCount
        /// </exception>
        /// <remarks>
        /// The term "raw elements" describes the elements, an audio source uses. 
        /// What type of unit an implementation of the <see cref="IAudioSource"/> interface uses, depends on the implementation itself.
        /// For example, a <see cref="IWaveSource"/> uses bytes while a <see cref="ISampleSource"/> uses samples. 
        /// That means that a <see cref="IWaveSource"/> provides its position, length,... in bytes 
        /// while a <see cref="ISampleSource"/> provides its position, length,... in samples.
        /// <para></para>
        /// To get the length or the position of a <see cref="IAudioSource"/> as a <see cref="TimeSpan"/> value, use the 
        /// <see cref="GetLength"/> or the <see cref="GetPosition"/> property.
        /// <para></para><para></para>
        /// Internally this method uses the <see cref="TimeConverterFactory"/> class.
        /// </remarks>
        public static TimeSpan GetTime(this IAudioSource source, long elementCount)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (elementCount < 0)
                throw new ArgumentNullException("elementCount");

            return TimeConverterFactory.Instance.GetTimeConverterForSource(source)
                .ToTimeSpan(source.WaveFormat, elementCount);
        }
      
        /// <summary>
        /// Converts a duration in raw elements to a duration in milliseconds. For more information about "raw elements" see remarks.
        /// </summary>
        /// <param name="source">The <see cref="IAudioSource"/> instance which provides the <see cref="WaveFormat"/> used 
        /// to convert the duration in "raw elements" to a duration in milliseconds.</param>
        /// <param name="elementCount">The duration in "raw elements" to convert to duration in milliseconds.</param>
        /// <returns>The duration in milliseconds.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// elementCount
        /// </exception>
        /// <remarks>
        /// The term "raw elements" describes the elements, an audio source uses. 
        /// What type of unit an implementation of the <see cref="IAudioSource"/> interface uses, depends on the implementation itself.
        /// For example, a <see cref="IWaveSource"/> uses bytes while a <see cref="ISampleSource"/> uses samples. 
        /// That means that a <see cref="IWaveSource"/> provides its position, length,... in bytes 
        /// while a <see cref="ISampleSource"/> provides its position, length,... in samples.
        /// <para></para>
        /// To get the length or the position of a <see cref="IAudioSource"/> as a <see cref="TimeSpan"/> value, use the 
        /// <see cref="GetLength"/> or the <see cref="GetPosition"/> property.
        /// <para></para><para></para>
        /// Internally this method uses the <see cref="TimeConverterFactory"/> class.
        /// </remarks>
        public static long GetMilliseconds(this IAudioSource source, long elementCount)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (elementCount < 0)
                throw new ArgumentOutOfRangeException("elementCount");

            return (long) GetTime(source, elementCount).TotalMilliseconds;
        }

        /// <summary>
        ///     Converts a duration as a <see cref="TimeSpan" /> to a duration in "raw elements". For more information about "raw elements" see remarks.
        /// </summary>
        /// <param name="source">
        ///     <see cref="IWaveSource" /> instance which provides the <see cref="WaveFormat" /> used to convert
        ///     the duration as a <see cref="TimeSpan" /> to a duration in "raw elements".
        /// </param>
        /// <param name="timespan">Duration as a <see cref="TimeSpan" /> to convert to a duration in "raw elements".</param>
        /// <returns>Duration in "raw elements".</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        /// <remarks>
        /// The term "raw elements" describes the elements, an audio source uses. 
        /// What type of unit an implementation of the <see cref="IAudioSource"/> interface uses, depends on the implementation itself.
        /// For example, a <see cref="IWaveSource"/> uses bytes while a <see cref="ISampleSource"/> uses samples. 
        /// That means that a <see cref="IWaveSource"/> provides its position, length,... in bytes 
        /// while a <see cref="ISampleSource"/> provides its position, length,... in samples.
        /// <para></para>
        /// To get the length or the position of a <see cref="IAudioSource"/> as a <see cref="TimeSpan"/> value, use the 
        /// <see cref="GetLength"/> or the <see cref="GetPosition"/> property.
        /// <para></para><para></para>
        /// Internally this method uses the <see cref="TimeConverterFactory"/> class.
        /// </remarks>
        public static long GetRawElements(this IAudioSource source, TimeSpan timespan)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return TimeConverterFactory.Instance.GetTimeConverterForSource(source)
                .ToRawElements(source.WaveFormat, timespan);
        }

        /// <summary>
        /// Converts a duration in milliseconds to a duration in "raw elements". For more information about "raw elements" see remarks.
        /// </summary>
        /// <param name="source"><see cref="IWaveSource" /> instance which provides the <see cref="WaveFormat" /> used to convert
        /// the duration in milliseconds to a duration in "raw elements".</param>
        /// <param name="milliseconds">Duration in milliseconds to convert to a duration in "raw elements".</param>
        /// <returns>
        /// Duration in "raw elements".
        /// </returns>
        /// <remarks>
        /// The term "raw elements" describes the elements, an audio source uses. 
        /// What type of unit an implementation of the <see cref="IAudioSource"/> interface uses, depends on the implementation itself.
        /// For example, a <see cref="IWaveSource"/> uses bytes while a <see cref="ISampleSource"/> uses samples. 
        /// That means that a <see cref="IWaveSource"/> provides its position, length,... in bytes 
        /// while a <see cref="ISampleSource"/> provides its position, length,... in samples.
        /// <para></para>
        /// To get the length or the position of a <see cref="IAudioSource"/> as a <see cref="TimeSpan"/> value, use the 
        /// <see cref="GetLength"/> or the <see cref="GetPosition"/> property.
        /// <para></para><para></para>
        /// Internally this method uses the <see cref="TimeConverterFactory"/> class.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">source</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">milliseconds is less than zero.</exception>
        public static long GetRawElements(this IAudioSource source, long milliseconds)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (milliseconds < 0)
                throw new ArgumentOutOfRangeException("milliseconds");

            return GetRawElements(source, TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Creates a new file, writes all audio data of the <paramref name="source" /> to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="source">Source which provides the audio data to write to the file.</param>
        /// <param name="filename">The file to write to.</param>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static void WriteToFile(this IWaveSource source, string filename)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            using (FileStream stream = File.OpenWrite(filename))
            {
                WriteToWaveStream(source, stream);
            }
        }

        /// <summary>
        /// Writes all audio data of the <paramref name="source" /> to a wavestream (including a wav header).
        /// </summary>
        /// <param name="source">Source which provides the audio data to write to the <paramref name="stream" />.</param>
        /// <param name="stream"><see cref="Stream" /> to store the audio data in.</param>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// stream
        /// </exception>
        /// <exception cref="System.ArgumentException">Stream is not writeable.;stream</exception>
        public static void WriteToWaveStream(this IWaveSource source, Stream stream)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanWrite)
                throw new ArgumentException("Stream is not writeable.", "stream");

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
        /// Writes all audio data of the <paramref name="waveSource"/> to a stream. In comparison to the <see cref="WriteToWaveStream"/> method, 
        /// the <see cref="WriteToStream"/> method won't encode the provided audio to any particular format. No wav, aiff,... header won't be included.
        /// </summary>
        /// <param name="waveSource">The waveSource which provides the audio data to write to the <paramref name="stream"/>.</param>
        /// <param name="stream">The <see cref="Stream"/> to store the audio data in.</param>
        /// <exception cref="System.ArgumentNullException">
        /// waveSource
        /// or
        /// stream
        /// </exception>
        /// <exception cref="System.ArgumentException">Stream is not writeable.;stream</exception>
        public static void WriteToStream(this IWaveSource waveSource, Stream stream)
        {
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanWrite)
                throw new ArgumentException("Stream is not writeable.", "stream");


            var buffer = new byte[waveSource.WaveFormat.BytesPerSecond];
            int read;
            while ((read = waveSource.Read(buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, read);
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

        internal static byte[] ReadBytes(this IWaveSource waveSource, int count)
        {
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");
            count -= (count % waveSource.WaveFormat.BlockAlign);
            if(count <= 0)
                throw new ArgumentOutOfRangeException("count");

            byte[] buffer = new byte[count];
            int read = waveSource.Read(buffer, 0, buffer.Length);
            if(read < count)
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

        //copied from http://stackoverflow.com/questions/1436190/c-sharp-get-and-set-the-high-order-word-of-an-integer
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
        //--

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
                return AudioSubTypes.EncodingFromSubType(((WaveFormatExtensible) waveFormat).SubFormat);

            return waveFormat.WaveFormatTag;
        }

        /// <summary>
        ///     Blocks the current thread until the playback of the specified <paramref name="soundOut"/> instance stops or the specified timeout expires. 
        /// </summary>
        /// <param name="soundOut">The <see cref="ISoundOut"/> instance to wait for its playback to stop.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait. Pass <see cref="Timeout.Infinite"/> to wait indefinitely.</param>
        /// <returns><c>true</c> if the <paramref name="soundOut"/> got stopped; <c>false</c> if the specified <paramref name="millisecondsTimeout"/> expired.</returns>
        public static bool WaitForStopped(this ISoundOut soundOut, int millisecondsTimeout)
        {
            if (soundOut == null)
                throw new ArgumentNullException("soundOut");
            if (millisecondsTimeout < -1)
                throw new ArgumentOutOfRangeException("millisecondsTimeout");

            if (soundOut.PlaybackState == PlaybackState.Stopped)
                return true;

            using (var waitHandle = new AutoResetEvent(false))
            {
// ReSharper disable once AccessToDisposedClosure
                soundOut.Stopped += (s, e) => waitHandle.Set();
                return waitHandle.WaitOne(millisecondsTimeout);
            }
        }

        /// <summary>
        ///     Blocks the current thread until the playback of the specified <paramref name="soundOut"/> instance stops. 
        /// </summary>
        /// <param name="soundOut">The <see cref="ISoundOut"/> instance to wait for its playback to stop.</param>
        public static void WaitForStopped(this ISoundOut soundOut)
        {
            WaitForStopped(soundOut, Timeout.Infinite);
        }

        //copied from http://stackoverflow.com/questions/9927590/can-i-set-a-value-on-a-struct-through-reflection-without-boxing
        internal static void SetValueForValueType<T>(this FieldInfo field, ref T item, object value) where T : struct
        {
            field.SetValueDirect(__makeref(item), value);
        }
    }
}
