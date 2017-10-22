using System;
using System.Runtime.InteropServices;
using System.Security;
using CSCore.Win32;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Used to manage sound buffers.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [Guid("279AFA85-4981-11CE-A521-0020AF0BE560")]
    public unsafe class DirectSoundBuffer : ComObject
    {
        private const int MinVolume = -10000;
        private const int MaxVolume = 0;

        /// <summary>
        /// Left only.
        /// </summary>
        public const int PanLeft = -10000;
        /// <summary>
        /// 50% left, 50% right.
        /// </summary>
        public const int PanCenter = 0;
        /// <summary>
        /// Right only.
        /// </summary>
        public const int PanRight = 10000;

        /// <summary>
        /// The default frequency. For more information, see <see cref="SetFrequency"/>.
        /// </summary>
        public const int FrequencyOriginal = 0;
        private const int FrequencyMin = 100;
        private const int FrequencyMax = 20000;
        private const string InterfaceName = "IDirectSoundBuffer";

        internal DirectSoundBuffer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSoundBuffer"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public DirectSoundBuffer(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Gets the capabilities of the buffer object.
        /// </summary>
        public DSBufferCaps BufferCaps
        {
            get { return GetCaps(); }
        }

        /// <summary>
        /// Gets the status of the sound buffer.
        /// </summary>
        public DSBStatusFlags Status
        {
            get
            {
                DSBStatusFlags status;
                DirectSoundException.Try(GetStatusNative(out status), InterfaceName, "GetStatus");
                return status;
            }
        }

        /// <summary>
        /// Retrieves the capabilities of the buffer object. 
        /// </summary>
        /// <param name="bufferCaps">Receives the capabilities of this sound buffer.</param>
        /// <returns>DSResult</returns>
        public DSResult GetCapsNative(out DSBufferCaps bufferCaps)
        {
            bufferCaps = new DSBufferCaps();
            bufferCaps.Size = Marshal.SizeOf(bufferCaps);
            fixed (void* ptrbuffercaps = &bufferCaps)
            {
                var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptrbuffercaps, ((void**)(*(void**)UnsafeBasePtr))[3]);
                return result;
            }
        }

        /// <summary>
        /// Retrieves the capabilities of the buffer object. 
        /// </summary>
        /// <returns>The capabilities of this sound buffer.</returns>
        public DSBufferCaps GetCaps()
        {
            DSBufferCaps caps;
            DirectSoundException.Try(GetCapsNative(out caps), InterfaceName, "GetCaps");
            return caps;
        }

        /// <summary>
        /// Causes the sound buffer to play, starting at the play cursor. 
        /// </summary>
        /// <param name="flags">Flags specifying how to play the buffer.</param>
        public void Play(DSBPlayFlags flags)
        {
            Play(flags, 0);
        }

        /// <summary>
        /// Causes the sound buffer to play, starting at the play cursor. 
        /// </summary>
        /// <param name="flags">Flags specifying how to play the buffer.</param>
        /// <param name="priority">Priority for the sound, used by the voice manager when assigning hardware mixing resources. The lowest priority is 0, and the highest priority is 0xFFFFFFFF. If the buffer was not created with the <see cref="DSBufferCapsFlags.LocDefer"/> flag, this value must be 0.</param>
        public void Play(DSBPlayFlags flags, int priority)
        {
            DirectSoundException.Try(PlayNative(flags, priority), InterfaceName, "Play");
        }

        /// <summary>
        /// Causes the sound buffer to play, starting at the play cursor. 
        /// </summary>
        /// <param name="flags">Flags specifying how to play the buffer.</param>
        /// <param name="priority">Priority for the sound, used by the voice manager when assigning hardware mixing resources. The lowest priority is 0, and the highest priority is 0xFFFFFFFF. If the buffer was not created with the <see cref="DSBufferCapsFlags.LocDefer"/> flag, this value must be 0.</param>        
        /// <returns>DSResult</returns>
        public DSResult PlayNative(DSBPlayFlags flags, int priority)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, 0, priority, unchecked((int)flags), ((void**)(*(void**)UnsafeBasePtr))[12]);
        }

        /// <summary>
        /// Causes the sound buffer to stop playing. 
        /// </summary>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsoundbuffer8.idirectsoundbuffer8.stop(v=vs.85).aspx"/>.</remarks>        
        public void Stop()
        {
            DirectSoundException.Try(StopNative(), InterfaceName, "Stop");
        }

        /// <summary>
        /// Causes the sound buffer to stop playing. 
        /// </summary>
        /// <returns>DSResult</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsoundbuffer8.idirectsoundbuffer8.stop(v=vs.85).aspx"/>.</remarks>
        public DSResult StopNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[18]);
        }

        /// <summary>
        /// Restores the memory allocation for a lost sound buffer. 
        /// </summary>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsoundbuffer8.idirectsoundbuffer8.restore(v=vs.85).aspx"/>.</remarks>
        public void Restore()
        {
            DirectSoundException.Try(RestoreNative(), InterfaceName, "Restore");
        }

        /// <summary>
        /// Restores the memory allocation for a lost sound buffer. 
        /// </summary>
        /// <returns>DSResult</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsoundbuffer8.idirectsoundbuffer8.restore(v=vs.85).aspx"/>.</remarks>
        public DSResult RestoreNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[20]);
        }

        /// <summary>
        /// Readies all or part of the buffer for a data write and returns pointers to which data can be written. 
        /// </summary>
        /// <param name="offset">Offset, in bytes, from the start of the buffer to the point where the lock begins. This parameter is ignored if <see cref="DSBLock.FromWriteCursor"/> is specified in the <paramref name="lockFlags"/> parameter.</param>
        /// <param name="bytes">Size, in bytes, of the portion of the buffer to lock. The buffer is conceptually circular, so this number can exceed the number of bytes between <paramref name="offset"/> and the end of the buffer.</param>
        /// <param name="audioPtr1">Receives a pointer to the first locked part of the buffer.</param>
        /// <param name="audioBytes1">Receives the number of bytes in the block at <paramref name="audioPtr1"/>. If this value is less than <paramref name="bytes"/>, the lock has wrapped and <paramref name="audioPtr2"/> points to a second block of data at the beginning of the buffer.</param>
        /// <param name="audioPtr2">Receives a pointer to the second locked part of the capture buffer. If <see cref="IntPtr.Zero"/> is returned, the <paramref name="audioPtr1"/> parameter points to the entire locked portion of the capture buffer.</param>
        /// <param name="audioBytes2">Receives the number of bytes in the block at <paramref name="audioPtr2"/>. If <paramref name="audioPtr2"/> is <see cref="IntPtr.Zero"/>, this value is zero.</param>
        /// <param name="lockFlags">Flags modifying the lock event.</param>
        /// <returns>DSResult</returns>
        public DSResult LockNative(int offset, int bytes, out IntPtr audioPtr1, out int audioBytes1,
                             out IntPtr audioPtr2, out int audioBytes2, DSBLock lockFlags)
        {
            fixed (void* pAudioPtr1 = &audioPtr1, pAudioPtr2 = &audioPtr2)
            {
                fixed (void* pAudioBytes1 = &audioBytes1, pAudioBytes2 = &audioBytes2)
                {
                    var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, offset, bytes,
                                        pAudioPtr1, pAudioBytes1, pAudioPtr2, pAudioBytes2, unchecked((int)lockFlags),
                                        ((void**)(*(void**)UnsafeBasePtr))[11]);
                    return result;
                }
            }
        }

        /// <summary>
        /// Readies all or part of the buffer for a data write and returns pointers to which data can be written. 
        /// </summary>
        /// <param name="offset">Offset, in bytes, from the start of the buffer to the point where the lock begins. This parameter is ignored if <see cref="DSBLock.FromWriteCursor"/> is specified in the <paramref name="lockFlags"/> parameter.</param>
        /// <param name="bytes">Size, in bytes, of the portion of the buffer to lock. The buffer is conceptually circular, so this number can exceed the number of bytes between <paramref name="offset"/> and the end of the buffer.</param>
        /// <param name="audioPtr1">Receives a pointer to the first locked part of the buffer.</param>
        /// <param name="audioBytes1">Receives the number of bytes in the block at <paramref name="audioPtr1"/>. If this value is less than <paramref name="bytes"/>, the lock has wrapped and <paramref name="audioPtr2"/> points to a second block of data at the beginning of the buffer.</param>
        /// <param name="audioPtr2">Receives a pointer to the second locked part of the capture buffer. If <see cref="IntPtr.Zero"/> is returned, the <paramref name="audioPtr1"/> parameter points to the entire locked portion of the capture buffer.</param>
        /// <param name="audioBytes2">Receives the number of bytes in the block at <paramref name="audioPtr2"/>. If <paramref name="audioPtr2"/> is <see cref="IntPtr.Zero"/>, this value is zero.</param>
        /// <param name="lockFlags">Flags modifying the lock event.</param>
        public void Lock(int offset, int bytes, out IntPtr audioPtr1, out int audioBytes1,
            out IntPtr audioPtr2, out int audioBytes2, DSBLock lockFlags)
        {
            var result = LockNative(offset, bytes, out audioPtr1, out audioBytes1, out audioPtr2, out audioBytes2,
                lockFlags);
            DirectSoundException.Try(result, InterfaceName, "Lock");
        }


        /// <summary>
        /// Releases a locked sound buffer. 
        /// </summary>
        /// <param name="audioPtr1">Address of the value retrieved in the <c>audioPtr1</c> parameter of the <see cref="Lock"/> method.</param>
        /// <param name="audioBytes1">Number of bytes written to the portion of the buffer at <c>audioPtr1</c>.</param>
        /// <param name="audioPtr2">Address of the value retrieved in the <c>audioPtr2</c> parameter of the <see cref="Lock"/> method.</param>
        /// <param name="audioBytes2">Number of bytes written to the portion of the buffer at <c>audioPtr2</c>.</param>
        /// <returns>DSResult</returns>
        public DSResult UnlockNative(IntPtr audioPtr1, int audioBytes1, IntPtr audioPtr2, int audioBytes2)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)audioPtr1, audioBytes1, (void*)audioPtr2, audioBytes2, ((void**)(*(void**)UnsafeBasePtr))[19]);
        }

        /// <summary>
        /// Releases a locked sound buffer. 
        /// </summary>
        /// <param name="audioPtr1">Address of the value retrieved in the <c>audioPtr1</c> parameter of the <see cref="Lock"/> method.</param>
        /// <param name="audioBytes1">Number of bytes written to the portion of the buffer at <c>audioPtr1</c>.</param>
        /// <param name="audioPtr2">Address of the value retrieved in the <c>audioPtr2</c> parameter of the <see cref="Lock"/> method.</param>
        /// <param name="audioBytes2">Number of bytes written to the portion of the buffer at <c>audioPtr2</c>.</param>
        public void Unlock(IntPtr audioPtr1, int audioBytes1, IntPtr audioPtr2, int audioBytes2)
        {
            var result = UnlockNative(audioPtr1, audioBytes1, audioPtr2, audioBytes2);
            DirectSoundException.Try(result, InterfaceName, "Unlock");
        }

        /// <summary>
        /// Retrieves the position of the play and write cursors in the sound buffer. 
        /// </summary>
        /// <param name="playCursorPosition">Receives the offset, in bytes, of the play cursor.</param>
        /// <param name="writeCursorPosition">Receives the offset, in bytes, of the write cursor.</param>
        /// <returns>DSResult</returns>
        public DSResult GetCurrentPositionNative(out int playCursorPosition, out int writeCursorPosition)
        {
            fixed (void* pplaypos = &playCursorPosition, pwritepos = &writeCursorPosition)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pplaypos, pwritepos, ((void**)(*(void**)UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        /// Retrieves the position of the play and write cursors in the sound buffer. 
        /// </summary>
        /// <param name="playCursorPosition">Receives the offset, in bytes, of the play cursor.</param>
        /// <param name="writeCursorPosition">Receives the offset, in bytes, of the write cursor.</param>
        public void GetCurrentPosition(out int playCursorPosition, out int writeCursorPosition)
        {
            DirectSoundException.Try(GetCurrentPositionNative(out playCursorPosition, out writeCursorPosition),
                InterfaceName, "GetCurrentPosition");
        }

        /// <summary>
        /// Sets the position of the play cursor, which is the point at which the next byte of data is read from the buffer. 
        /// </summary>
        /// <param name="playPosition">Offset of the play cursor, in bytes, from the beginning of the buffer.</param>
        public void SetCurrentPosition(int playPosition)
        {
            DirectSoundException.Try(SetCurrentPositionNative(playPosition), InterfaceName, "SetCurrentPosition");
        }

        /// <summary>
        /// Sets the position of the play cursor, which is the point at which the next byte of data is read from the buffer. 
        /// </summary>
        /// <param name="playPosition">Offset of the play cursor, in bytes, from the beginning of the buffer.</param>
        /// <returns>DSResult</returns>
        public DSResult SetCurrentPositionNative(int playPosition)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, playPosition, ((void**)(*(void**)UnsafeBasePtr))[13]);
        }

        /// <summary>
        /// Initializes a sound buffer object if it has not yet been initialized. 
        /// </summary>
        /// <param name="directSound">The device object associated with this buffer.</param>
        /// <param name="bufferDescription">A <see cref="DSBufferDescription"/> structure that contains the values used to initialize this sound buffer.</param>
        /// <returns>DSResult</returns>
        public DSResult InitializeNative(DirectSoundBase directSound, DSBufferDescription bufferDescription)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, directSound.BasePtr.ToPointer(), &bufferDescription, ((void**)(*(void**)UnsafeBasePtr))[10]);
        }

        /// <summary>
        /// Initializes a sound buffer object if it has not yet been initialized. 
        /// </summary>
        /// <param name="directSound">The device object associated with this buffer.</param>
        /// <param name="bufferDescription">A <see cref="DSBufferDescription"/> structure that contains the values used to initialize this sound buffer.</param>
        public void Initialize(DirectSoundBase directSound, DSBufferDescription bufferDescription)
        {
            DirectSoundException.Try(InitializeNative(directSound, bufferDescription), InterfaceName, "Initialize");
        }

        /// <summary>
        /// Retrieves the status of the sound buffer. 
        /// <seealso cref="Status"/>        
        /// </summary>
        /// <param name="status">Receives the status of the sound buffer.</param>
        /// <returns>DSResult</returns>
        /// <remarks>Use the <see cref="Status"/> property instead.</remarks>
        public DSResult GetStatusNative(out DSBStatusFlags status)
        {
            fixed (void* pstatus = &status)
            {
                var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, pstatus, ((void**)(*(void**)UnsafeBasePtr))[9]);
                return result;
            }
        }

        /// <summary>
        /// Sets the frequency at which the audio samples are played. 
        /// </summary>
        /// <param name="frequency">Frequency, in hertz (Hz), at which to play the audio samples. A value of <see cref="FrequencyOriginal"/> resets the frequency to the default value of the buffer format.</param>
        /// <returns>DSResult</returns>
        /// <remarks>Before setting the frequency, you should ascertain whether the frequency is supported by checking the <see cref="DirectSoundCapabilities.MinSecondarySampleRate"/> and <see cref="DirectSoundCapabilities.MaxSecondarySampleRate"/> members of the <see cref="DirectSoundCapabilities"/> structure for the device. Some operating systems do not support frequencies greater than 100,000 Hz.</remarks>
        public DSResult SetFrequencyNative(int frequency)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, frequency, ((void**)(*(void**)UnsafeBasePtr))[17]);
        }

        /// <summary>
        /// Sets the frequency at which the audio samples are played. 
        /// </summary>
        /// <param name="frequency">Frequency, in hertz (Hz), at which to play the audio samples. A value of <see cref="FrequencyOriginal"/> resets the frequency to the default value of the buffer format.</param>
        /// <remarks>Before setting the frequency, you should ascertain whether the frequency is supported by checking the <see cref="DirectSoundCapabilities.MinSecondarySampleRate"/> and <see cref="DirectSoundCapabilities.MaxSecondarySampleRate"/> members of the <see cref="DirectSoundCapabilities"/> structure for the device. Some operating systems do not support frequencies greater than 100,000 Hz.</remarks>
        public void SetFrequency(int frequency)
        {
            if(frequency < FrequencyMin || frequency > FrequencyMax)
                throw new ArgumentOutOfRangeException("frequency", "Must be between 100 and 20000.");
            DirectSoundException.Try(SetFrequencyNative(frequency), InterfaceName, "SetFrequency");
        }

        /// <summary>
        /// Retrieves the frequency, in samples per second, at which the buffer is playing. 
        /// </summary>
        /// <param name="frequency">A variable that receives the frequency at which the audio buffer is being played, in hertz.</param>
        /// <returns>DSResult</returns>
        public DSResult GetFrequencyNative(out int frequency)
        {
            fixed (void* pfrequency = &frequency)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pfrequency, ((void**)(*(void**)UnsafeBasePtr))[8]);
            }
        }

        /// <summary>
        /// Gets the frequency, in samples per second, at which the buffer is playing. 
        /// </summary>
        /// <returns>The frequency at which the audio buffer is being played, in hertz.</returns>
        public int GetFrequency()
        {
            int f;
            DirectSoundException.Try(GetFrequencyNative(out f), InterfaceName, "GetFrequency");
            return f;
        }

        /// <summary>
        /// Sets the relative volume of the left and right channels. 
        /// </summary>
        /// <param name="pan">Relative volume between the left and right channels. Must be between <see cref="PanLeft"/> and <see cref="PanRight"/>.</param>
        /// <returns>DSResult</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsoundbuffer8.idirectsoundbuffer8.setpan(v=vs.85).aspx"/>.</remarks>
        public DSResult SetPanNative(int pan)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pan, ((void**)(*(void**)UnsafeBasePtr))[16]);
        }

        /// <summary>
        /// Sets the relative volume of the left and right channels. 
        /// </summary>
        /// <param name="pan">Relative volume between the left and right channels. Must be between <see cref="PanLeft"/> and <see cref="PanRight"/>.</param>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsoundbuffer8.idirectsoundbuffer8.setpan(v=vs.85).aspx"/>.</remarks>
        public void SetPan(int pan)
        {
            DirectSoundException.Try(SetPanNative(pan), InterfaceName, "SetPan");
        }

        /// <summary>
        /// Sets the relative volume of the left and right channels as a scalar value. 
        /// </summary>
        /// <param name="pan">Relative volume between the left and right channels. Must be between -1.0 and 1.0. 
        /// A value of -1.0 will set the volume of the left channel to 100% and the volume of the right channel to 0%. 
        /// A value of 1.0 will set the volume of the left channel to 0% and the volume of the right channel to 100%.</param>
        public void SetPanScalar(float pan)
        {
            int pani = 0;
            if (pan < 0)
            {
                pani = (int) ScalarValueToDBValue(Math.Abs(Math.Abs(pan) - 1), PanLeft, 0);
            }
            else if(pan > 0)
            {
                pani = (int) ScalarValueToDBValue(Math.Abs(pan - 1), PanLeft, 0) * -1;
            }
            SetPan(pani);
        }

        /// <summary>
        /// Retrieves the relative volume of the left and right audio channels. 
        /// </summary>
        /// <param name="pan">A variable that receives the relative volume, in hundredths of a decibel.</param>
        /// <returns>DSResult</returns>
        public DSResult GetPanNative(out int pan)
        {
            fixed (void* ppan = &pan)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ppan, ((void**)(*(void**)UnsafeBasePtr))[7]);
            }
        }

        /// <summary>
        /// Retrieves the relative volume of the left and right audio channels. 
        /// </summary>
        /// <returns>The relative volume, in hundredths of a decibel.</returns>
        public int GetPan()
        {
            int pani;
            var result = GetPanNative(out pani);
            DirectSoundException.Try(result, InterfaceName, "GetPan");
            return pani;
        }

        /// <summary>
        /// Gets the relative volume of the left and right channels as a scalar value.  
        /// </summary>
        /// <returns>The relative volume between the left and right channels. A value of -1.0 indicates that the volume of the left channel is set to 100% and the volume of the right channel to 0%.
        /// A value of 1.0 indicates that the volume of the left channel is set to 0% and the volume of the right channel is set to 100%.</returns>
        public float GetPanScalar()
        {
            float pan = 0;
            int pani = GetPan();
            if (pani < 0)
            {
                pan = (float) (-1 + DBToScalarValue(pani));
            }
            else if (pani > 0)
            {
                pan = (float) (1 - DBToScalarValue(pani * -1));
            }
            return pan;
        }

        /// <summary>
        /// Sets the attenuation of the sound. 
        /// </summary>
        /// <param name="volume">Attenuation, in hundredths of a decibel (dB).</param>
        /// <returns>DSResult</returns>
        public DSResult SetVolumeNative(int volume)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, volume, ((void**)(*(void**)UnsafeBasePtr))[15]);
        }

        /// <summary>
        /// Sets the attenuation of the sound. 
        /// </summary>
        /// <param name="volume">Attenuation, in hundredths of a decibel (dB).</param>        
        public void SetVolume(int volume)
        {
            DirectSoundException.Try(SetVolumeNative(volume), InterfaceName, "SetVolume");            
        }

        /// <summary>
        /// Sets the attenuation of the sound. 
        /// </summary>
        /// <param name="volume">The attenuation of the sound. The attenuation is expressed as a normalized value in the range from 0.0 to 1.0.</param>
        public void SetVolumeScalar(double volume)
        {
            var value = (int)ScalarValueToDBValue(volume, MinVolume, MaxVolume);
            SetVolume(value);
        }

        /// <summary>
        /// Retrieves the attenuation of the sound. 
        /// </summary>
        /// <param name="volume">A variable that receives the attenuation, in hundredths of a decibel.</param>
        /// <returns>DSResult</returns>
        public DSResult GetVolumeNative(out int volume)
        {
            fixed (void* pvolume = &volume)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pvolume, ((void**)(*(void**)UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        /// Returns the attenuation of the sound. 
        /// </summary>
        /// <returns>The attenuation, in hundredths of a decibel.</returns>
        public int GetVolume()
        {
            int dwvolume;
            DSResult result = GetVolumeNative(out dwvolume);
            DirectSoundException.Try(result, InterfaceName, "GetVolume");
            return dwvolume;
        }

        /// <summary>
        /// Returns the attenuation of the sound.
        /// </summary>
        /// <returns>The attenuation of the sound. The attenuation is expressed as a normalized value in the range from 0.0 to 1.0.</returns>
        public double GetVolumeScalar()
        {
            var volume = GetVolume();
            return DBToScalarValue(volume);
        }

        /// <summary>
        /// Retrieves a description of the format of the sound data in the buffer, or the buffer size needed to retrieve the format description. 
        /// </summary>
        /// <param name="format">Address of a <see cref="WaveFormat"/> or <see cref="WaveFormatExtensible"/> instance that receives a description of the sound data in the buffer. To retrieve the buffer size needed to contain the format description, specify <see cref="IntPtr.Zero"/>. In this case the variable at <paramref name="sizeWritten"/> receives the size of the structure needed to receive the data.</param>
        /// <param name="sizeAllocated">Size, in bytes, of the structure at <paramref name="format"/>. If <paramref name="format"/> is not <see cref="IntPtr.Zero"/>, this value must be equal to or greater than the size of the expected data.</param>
        /// <param name="sizeWritten">A variable that receives the number of bytes written to the structure at <paramref name="format"/>.</param>
        /// <returns>DSResult</returns>
        public DSResult GetFormatNative(IntPtr format, int sizeAllocated, out int sizeWritten)
        {
            fixed (void* p = &sizeWritten)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)format, sizeAllocated, p, ((void**)(*(void**)UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        /// Returns a description of the format of the sound data in the buffer.
        /// </summary>
        /// <returns>A description of the format of the sound data in the buffer. The returned description is either of the type <see cref="WaveFormat"/> or of the type <see cref="WaveFormatExtensible"/>.</returns>
        public WaveFormat GetWaveFormat()
        {
            int size;
            DSResult result = GetFormatNative(IntPtr.Zero, 0, out size);
            DirectSoundException.Try(result, InterfaceName, "GetWaveFormat");

            IntPtr ptr = Marshal.AllocCoTaskMem(size);
            try
            {
                int n;
                result = GetFormatNative(ptr, size, out n);
                DirectSoundException.Try(result, InterfaceName, "GetWaveFormat");
                return WaveFormatMarshaler.PointerToWaveFormat(ptr);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        /// <summary>
        /// Sets the format of the primary buffer. Whenever this application has the input focus, DirectSound will set the primary buffer to the specified format. 
        /// </summary>
        /// <param name="waveFormat">A waveformat that describes the new format for the primary sound buffer.</param>
        /// <returns>DSResult</returns>
        public DSResult SetFormatNative(WaveFormat waveFormat)
        {
            GCHandle hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            try
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)hWaveFormat.AddrOfPinnedObject(),
                    ((void**) (*(void**) UnsafeBasePtr))[14]);
            }
            finally
            {
                hWaveFormat.Free();
            }
        }

        /// <summary>
        /// Sets the format of the primary buffer. Whenever this application has the input focus, DirectSound will set the primary buffer to the specified format. 
        /// </summary>
        /// <param name="waveFormat">A waveformat that describes the new format for the primary sound buffer.</param>        
        public void SetFormat(WaveFormat waveFormat)
        {
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");

            DirectSoundException.Try(SetFormatNative(waveFormat), InterfaceName, "SetFormat");
        }

        /// <summary>
        /// Enables effects on a buffer. For this method to succeed, CoInitialize must have been called. Additionally, the buffer must not be playing or locked.
        /// </summary>
        /// <param name="effectsCount">Number of elements in the effectDescriptions and resultCodes arrays. If this value is 0, effectDescriptions and resultCodes must both be <see cref="IntPtr.Zero"/>. Set to 0 to remove all effects from the buffer.</param>
        /// <param name="effectDescriptions">Address of an array of DSEFFECTDESC structures, of size effectsCount, that specifies the effects wanted on the buffer. Must be <see cref="IntPtr.Zero"/> if effectsCount is 0.</param>
        /// <param name="resultCodes">Address of an array of DWORD elements, of size effectsCount.</param>
        /// <returns>DSResult</returns>
        [Obsolete("Use the effect classes of the CSCore.Streams.Effects namespace instead. Applying effects to directsound buffers is not supported by cscore.")]
        public DSResult SetFxNative(int effectsCount, IntPtr effectDescriptions, IntPtr resultCodes)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, effectsCount, (void*)effectDescriptions, (void*)resultCodes,
                ((void**) (*(void**) UnsafeBasePtr))[21]);
        }

#pragma warning disable 618
        /// <summary>
        /// Allocates resources for a buffer that was created with the DSBCAPS_LOCDEFER flag in the DSBUFFERDESC structure. 
        /// </summary>
        /// <param name="flags">Flags specifying how resources are to be allocated for a buffer created with the DSBCAPS_LOCDEFER flag.</param>
        /// <param name="effectsCount">Number of elements in the resultCodes array, or 0 if resultCodes is <see cref="IntPtr.Zero"/>.</param>
        /// <param name="resultCodes">Address of an array of DWORD variables that receives information about the effects associated with the buffer. This array must contain one element for each effect that was assigned to the buffer by <see cref="SetFxNative"/>.</param>
        /// <returns>DSResult</returns>
        [Obsolete("Use the effect classes of the CSCore.Streams.Effects namespace instead. Applying effects to directsound buffers is not supported by cscore.")]        
        public DSResult AcquireResourcesNative(int flags, int effectsCount, IntPtr resultCodes)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, effectsCount, (void*) resultCodes,
                ((void**) (*(void**) UnsafeBasePtr))[22]);
        }
#pragma warning restore 618

        /// <summary>
        /// Retrieves an interface for an effect object associated with the buffer. 
        /// </summary>
        /// <param name="guidObject">Unique class identifier of the object being searched for, such as GUID_DSFX_STANDARD_ECHO. Set this parameter to GUID_All_Objects to search for objects of any class.</param>
        /// <param name="index">Index of the object within objects of that class in the path.</param>
        /// <param name="guidInterface">Unique identifier of the desired interface.</param>
        /// <param name="object">Address of a variable that receives the desired interface pointer.</param>
        /// <returns>DSResult</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsoundbuffer8.idirectsoundbuffer8.getobjectinpath%28v=vs.85%29.aspx"/>.</remarks>
        [Obsolete("Use the effect classes of the CSCore.Streams.Effects namespace instead. Applying effects to directsound buffers is not supported by cscore.")]        
        public DSResult GetObjectInPathNative(Guid guidObject, int index, Guid guidInterface, out IntPtr @object)
        {
            fixed (void* ptrEffect = &@object)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &guidObject, index, &guidInterface, ptrEffect, ((void**)(*(void**)UnsafeBasePtr))[23]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the buffer is lost. <c>True</c> means that the buffer is lost; Otherwise <c>False</c>.
        /// </summary>
        public bool IsBufferLost
        {
            get { return (Status & DSBStatusFlags.BufferLost) == DSBStatusFlags.BufferLost; }
        }

        /// <summary>
        /// Writes data to the buffer by locking the buffer, copying data to the buffer and finally unlocking it.
        /// </summary>
        /// <param name="buffer">The data to write to the buffer.</param>
        /// <param name="offset">The zero-based offset in the <paramref name="buffer"/> at which to start copying data.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <returns>Returns <c>true</c> if writing data was successful; Otherwise <c>false</c>.</returns>
        public bool Write(byte[] buffer, int offset, int count)
        {
            IntPtr ptr1, ptr2;
            int b1, b2;
            if (LockNative(offset, count, out ptr1, out b1, out ptr2, out b2, DSBLock.Default) == DSResult.Ok)
            {
                if (ptr1 != IntPtr.Zero)
                    Marshal.Copy(buffer, 0, ptr1, b1);
                if (ptr2 != IntPtr.Zero)
                    Marshal.Copy(buffer, b1 - 1, ptr2, b2);

                return UnlockNative(ptr1, b1, ptr2, b2) == DSResult.Ok;
            }
            return false;
        }

        /// <summary>
        /// Writes data to the buffer by locking the buffer, copying data to the buffer and finally unlocking it.
        /// </summary>
        /// <param name="buffer">The data to write to the buffer.</param>
        /// <param name="offset">The zero-based offset in the <paramref name="buffer"/> at which to start copying data.</param>
        /// <param name="count">The number of shorts to write.</param>
        /// <returns>Returns <c>true</c> if writing data was successful; Otherwise <c>false</c>.</returns>
        public bool Write(short[] buffer, int offset, int count)
        {
            int dscount = count * 2;

            IntPtr ptr1, ptr2;
            int b1, b2;
            if (LockNative(offset, dscount, out ptr1, out b1, out ptr2, out b2, DSBLock.Default) == DSResult.Ok)
            {
                if (ptr1 != IntPtr.Zero)
                    Marshal.Copy(buffer, 0, ptr1, Math.Min(b1, count));
                if (ptr2 != IntPtr.Zero)
                    Marshal.Copy(buffer, Math.Min(b1, count) - 2, ptr2, Math.Min(b2, count));

                return UnlockNative(ptr1, b1, ptr2, b2) == DSResult.Ok;
            }
            return false;
        }

        private double ScalarValueToDBValue(double value, int minValue, int maxValue)
        {
            const double dv = 10.0;
            const double z0 = 0.69314718055994529; //ln(2)

            //assume that dv = 10.0
            const double minAttentuation = 9.766E-4; //(1/2)^(100/dv)

            double result = minValue;
// ReSharper disable once CompareOfFloatsByEqualityOperator
            if (value != 0)
            {
                double attenuation = minAttentuation + value * (1 - minAttentuation);
                double db = dv * Math.Log(attenuation) / z0;
                result = (int)(db * 100);

                result = Math.Max(minValue, Math.Min(result, maxValue));
            }
            return result;
        }

        private double DBToScalarValue(double db)
        {
            //assume that dv = 10.0
            const double minAttentuation = 9.766E-4; //(1/2)^(100/dv)
            const double z1 = 0.001; //(1/100)/(dv)
            double result = (minAttentuation - Math.Pow(2, z1 * db)) / (minAttentuation - 1);

            result = Math.Min(1, Math.Max(0, result));
            return result;
        }
    }
}