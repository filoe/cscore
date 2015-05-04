using System;
using System.Runtime.InteropServices;
using System.Threading;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     Enables a client to create and initialize an audio stream between an audio application and the audio engine (for a
    ///     shared-mode stream) or the hardware buffer of an audio endpoint device (for an exclusive-mode stream). For more
    ///     information, see
    ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370865(v=vs.85).aspx" />.
    /// </summary>
    [Guid("1CB9AD4C-DBFA-4c32-B178-C2F568A703B2")]
    public class AudioClient : ComObject
    {
        private const string InterfaceName = "IAudioClient";

        /// <summary>
        ///     IID of the IAudioClient-interface.
        /// </summary>
// ReSharper disable once InconsistentNaming
        public static readonly Guid IID_IAudioClient = new Guid("1CB9AD4C-DBFA-4c32-B178-C2F568A703B2");

        /// <summary>
        ///     Initializes a new instance of the <see cref="AudioClient" /> class.
        /// </summary>
        /// <param name="ptr">Native pointer.</param>
        /// <remarks>Use the <see cref="FromMMDevice" /> method to create a new <see cref="AudioClient" /> instance.</remarks>
        public AudioClient(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Gets the default interval between periodic processing passes by the audio engine. The time is expressed in
        ///     100-nanosecond units.
        /// </summary>
        public long DefaultDevicePeriod
        {
            get
            {
                long n0, n1;
                CoreAudioAPIException.Try(GetDevicePeriodNative(out n0, out n1), InterfaceName, "GetDevicePeriod");
                return n0;
            }
        }

        /// <summary>
        ///     Gets the minimum interval between periodic processing passes by the audio endpoint device. The time is expressed in
        ///     100-nanosecond units.
        /// </summary>
        public long MinimumDevicePeriod
        {
            get
            {
                long n0, n1;
                CoreAudioAPIException.Try(GetDevicePeriodNative(out n0, out n1), InterfaceName, "GetDevicePeriod");
                return n1;
            }
        }

        /// <summary>
        ///     Gets the maximum capacity of the endpoint buffer.
        /// </summary>
        public int BufferSize
        {
            get { return GetBufferSize(); }
        }

        /// <summary>
        ///     Gets the number of frames of padding in the endpoint buffer.
        /// </summary>
        public int CurrentPadding
        {
            get { return GetCurrentPadding(); }
        }

        /// <summary>
        ///     Gets the stream format that the audio engine uses for its
        ///     internal processing of shared-mode streams.
        /// </summary>
        public WaveFormat MixFormat
        {
            get { return GetMixFormat(); }
        }

        /// <summary>
        ///     Gets the maximum latency for the current stream and can
        ///     be called any time after the stream has been initialized.
        /// </summary>
        public long StreamLatency
        {
            get { return GetStreamLatency(); }
        }

        /// <summary>
        ///     Returns a new instance of the <see cref="AudioClient" /> class.
        /// </summary>
        /// <param name="device">Device which should be used to create the <see cref="AudioClient" /> instance.</param>
        /// <returns><see cref="AudioClient" /> instance.</returns>
// ReSharper disable once InconsistentNaming
        public static AudioClient FromMMDevice(MMDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            IntPtr ptr;
            int result = device.ActivateNative(IID_IAudioClient, CLSCTX.CLSCTX_ALL, IntPtr.Zero, out ptr);
            CoreAudioAPIException.Try(result, "IMMDevice", "Activate");

            return new AudioClient(ptr);
        }

        /// <summary>
        ///     Initializes the audio stream.
        /// </summary>
        /// <param name="shareMode">
        ///     The sharing mode for the connection. Through this parameter, the client tells the audio engine
        ///     whether it wants to share the audio endpoint device with other clients.
        /// </param>
        /// <param name="streamFlags">Flags to control creation of the stream.</param>
        /// <param name="hnsBufferDuration">
        ///     The buffer capacity as a time value (expressed in 100-nanosecond units). This parameter
        ///     contains the buffer size that the caller requests for the buffer that the audio application will share with the
        ///     audio engine (in shared mode) or with the endpoint device (in exclusive mode). If the call succeeds, the method
        ///     allocates a buffer that is a least this large.
        /// </param>
        /// <param name="hnsPeriodicity">
        ///     The device period. This parameter can be nonzero only in exclusive mode. In shared mode,
        ///     always set this parameter to 0. In exclusive mode, this parameter specifies the requested scheduling period for
        ///     successive buffer accesses by the audio endpoint device. If the requested device period lies outside the range that
        ///     is set by the device's minimum period and the system's maximum period, then the method clamps the period to that
        ///     range. If this parameter is 0, the method sets the device period to its default value. To obtain the default device
        ///     period, call the <see cref="GetDevicePeriodNative" /> method. If the
        ///     <see cref="AudioClientStreamFlags.StreamFlagsEventCallback" /> stream flag is set and
        ///     <see cref="AudioClientShareMode.Exclusive" /> is set as the <paramref name="shareMode" />, then
        ///     <paramref name="hnsPeriodicity" /> must be nonzero and equal to <paramref name="hnsBufferDuration" />.
        /// </param>
        /// <param name="waveFormat">
        ///     The format descriptor. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370875(v=vs.85).aspx" />.
        /// </param>
        /// <param name="audioSessionGuid">
        ///     A value that identifies the audio session that the stream belongs to. If the
        ///     <see cref="Guid" /> identifies a session that has been previously opened, the method adds the stream to that
        ///     session. If the GUID does not identify an existing session, the method opens a new session and adds the stream to
        ///     that session. The stream remains a member of the same session for its lifetime. Use <see cref="Guid.Empty" /> to
        ///     use the default session.
        /// </param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370875(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int InitializeNative(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags,
            long hnsBufferDuration, long hnsPeriodicity,
            WaveFormat waveFormat, Guid audioSessionGuid)
        {
            GCHandle hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            try
            {
                return InteropCalls.CallI(UnsafeBasePtr, shareMode, streamFlags, hnsBufferDuration, hnsPeriodicity,
                    hWaveFormat.AddrOfPinnedObject().ToPointer(), audioSessionGuid,
                    ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
            finally
            {
                hWaveFormat.Free();
            }
        }

        /// <summary>
        ///     Initializes the audio stream.
        /// </summary>
        /// <param name="shareMode">
        ///     The sharing mode for the connection. Through this parameter, the client tells the audio engine
        ///     whether it wants to share the audio endpoint device with other clients.
        /// </param>
        /// <param name="streamFlags">Flags to control creation of the stream.</param>
        /// <param name="hnsBufferDuration">
        ///     The buffer capacity as a time value (expressed in 100-nanosecond units). This parameter
        ///     contains the buffer size that the caller requests for the buffer that the audio application will share with the
        ///     audio engine (in shared mode) or with the endpoint device (in exclusive mode). If the call succeeds, the method
        ///     allocates a buffer that is a least this large.
        /// </param>
        /// <param name="hnsPeriodicity">
        ///     The device period. This parameter can be nonzero only in exclusive mode. In shared mode,
        ///     always set this parameter to 0. In exclusive mode, this parameter specifies the requested scheduling period for
        ///     successive buffer accesses by the audio endpoint device. If the requested device period lies outside the range that
        ///     is set by the device's minimum period and the system's maximum period, then the method clamps the period to that
        ///     range. If this parameter is 0, the method sets the device period to its default value. To obtain the default device
        ///     period, call the <see cref="GetDevicePeriodNative" /> method. If the
        ///     <see cref="AudioClientStreamFlags.StreamFlagsEventCallback" /> stream flag is set and
        ///     <see cref="AudioClientShareMode.Exclusive" /> is set as the <paramref name="shareMode" />, then
        ///     <paramref name="hnsPeriodicity" /> must be nonzero and equal to <paramref name="hnsBufferDuration" />.
        /// </param>
        /// <param name="waveFormat">
        ///     Pointer to the format descriptor. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370875(v=vs.85).aspx" />.
        /// </param>
        /// <param name="audioSessionGuid">
        ///     A value that identifies the audio session that the stream belongs to. If the
        ///     <see cref="Guid" /> identifies a session that has been previously opened, the method adds the stream to that
        ///     session. If the GUID does not identify an existing session, the method opens a new session and adds the stream to
        ///     that session. The stream remains a member of the same session for its lifetime. Use <see cref="Guid.Empty" /> to
        ///     use the default session.
        /// </param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370875(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int InitializeNative(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags,
            long hnsBufferDuration, long hnsPeriodicity,
            IntPtr waveFormat, Guid audioSessionGuid)
        {
            return InteropCalls.CallI(UnsafeBasePtr, shareMode, streamFlags, hnsBufferDuration, hnsPeriodicity,
                waveFormat.ToPointer(), audioSessionGuid,
                ((void**) (*(void**) UnsafeBasePtr))[3]);
        }

        /// <summary>
        ///     Initializes the audio stream.
        /// </summary>
        /// <param name="shareMode">
        ///     The sharing mode for the connection. Through this parameter, the client tells the audio engine
        ///     whether it wants to share the audio endpoint device with other clients.
        /// </param>
        /// <param name="streamFlags">Flags to control creation of the stream.</param>
        /// <param name="hnsBufferDuration">
        ///     The buffer capacity as a time value (expressed in 100-nanosecond units). This parameter
        ///     contains the buffer size that the caller requests for the buffer that the audio application will share with the
        ///     audio engine (in shared mode) or with the endpoint device (in exclusive mode). If the call succeeds, the method
        ///     allocates a buffer that is a least this large.
        /// </param>
        /// <param name="hnsPeriodicity">
        ///     The device period. This parameter can be nonzero only in exclusive mode. In shared mode,
        ///     always set this parameter to 0. In exclusive mode, this parameter specifies the requested scheduling period for
        ///     successive buffer accesses by the audio endpoint device. If the requested device period lies outside the range that
        ///     is set by the device's minimum period and the system's maximum period, then the method clamps the period to that
        ///     range. If this parameter is 0, the method sets the device period to its default value. To obtain the default device
        ///     period, call the <see cref="GetDevicePeriodNative" /> method. If the
        ///     <see cref="AudioClientStreamFlags.StreamFlagsEventCallback" /> stream flag is set and
        ///     <see cref="AudioClientShareMode.Exclusive" /> is set as the <paramref name="shareMode" />, then
        ///     <paramref name="hnsPeriodicity" /> must be nonzero and equal to <paramref name="hnsBufferDuration" />.
        /// </param>
        /// <param name="waveFormat">
        ///     The format descriptor. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370875(v=vs.85).aspx" />.
        /// </param>
        /// <param name="audioSessionGuid">
        ///     A value that identifies the audio session that the stream belongs to. If the
        ///     <see cref="Guid" /> identifies a session that has been previously opened, the method adds the stream to that
        ///     session. If the GUID does not identify an existing session, the method opens a new session and adds the stream to
        ///     that session. The stream remains a member of the same session for its lifetime. Use <see cref="Guid.Empty" /> to
        ///     use the default session.
        /// </param>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370875(v=vs.85).aspx" />.
        /// </remarks>
        public void Initialize(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags,
            long hnsBufferDuration, long hnsPeriodicity, WaveFormat waveFormat, Guid audioSessionGuid)
        {
            CoreAudioAPIException.Try(
                InitializeNative(shareMode, streamFlags, hnsBufferDuration, hnsPeriodicity, waveFormat, audioSessionGuid),
                InterfaceName, "Initialize");
        }

        /// <summary>
        ///     Retrieves the size (maximum capacity) of the endpoint buffer.
        /// </summary>
        /// <param name="bufferFramesCount">Retrieves the number of audio frames that the buffer can hold.</param>
        /// <remarks>
        ///     The size of one frame = <c>(number of bits per sample)/8 * (number of channels)</c>
        /// </remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetBufferSizeNative(out Int32 bufferFramesCount)
        {
            fixed (void* pbfc = &bufferFramesCount)
            {
                return InteropCalls.CallI(UnsafeBasePtr, pbfc, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        ///     Returns the size (maximum capacity) of the endpoint buffer.
        /// </summary>
        /// <returns>The number of audio frames that the buffer can hold.</returns>
        /// <remarks>
        ///     The size of one frame = <c>(number of bits per sample)/8 * (number of channels)</c>
        /// </remarks>
        /// <returns>HRESULT</returns>
        public int GetBufferSize()
        {
            int bufferSize;
            CoreAudioAPIException.Try(GetBufferSizeNative(out bufferSize), InterfaceName, "GetBufferSize");
            return bufferSize;
        }

        /// <summary>
        ///     Retrieves the maximum latency for the current stream and can
        ///     be called any time after the stream has been initialized.
        /// </summary>
        /// <param name="hnsLatency">Retrieves a value representing the latency. The time is expressed in 100-nanosecond units.</param>
        /// <remarks>
        ///     Rendering clients can use this latency value to compute the minimum amount of data that
        ///     they can write during any single processing pass. To write less than this minimum is to
        ///     risk introducing glitches into the audio stream. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370874(v=vs.85).aspx" />.
        /// </remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetStreamLatencyNative(out long hnsLatency)
        {
            fixed (void* pl = &hnsLatency)
            {
                return InteropCalls.CallI(UnsafeBasePtr, pl, ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        ///     Retrieves the maximum latency for the current stream and can
        ///     be called any time after the stream has been initialized.
        /// </summary>
        /// <remarks>
        ///     Rendering clients can use this latency value to compute the minimum amount of data that
        ///     they can write during any single processing pass. To write less than this minimum is to
        ///     risk introducing glitches into the audio stream. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370874(v=vs.85).aspx" />.
        /// </remarks>
        /// <returns>A value representing the latency. The time is expressed in 100-nanosecond units.</returns>
        public long GetStreamLatency()
        {
            long latency;
            CoreAudioAPIException.Try(GetStreamLatencyNative(out latency), InterfaceName, "GetStreamLatency");
            return latency;
        }

        /// <summary>
        ///     Retrieves the number of frames of padding in the endpoint buffer.
        /// </summary>
        /// <param name="numPaddingFrames">Retrieves the frame count (the number of audio frames of padding in the buffer).</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     The size of one frame = <c>(number of bits per sample)/8 * (number of channels)</c>
        /// </remarks>
        public unsafe int GetCurrentPaddingNative(out Int32 numPaddingFrames)
        {
            fixed (void* pndf = &numPaddingFrames)
            {
                return InteropCalls.CallI(UnsafeBasePtr, pndf, ((void**) (*(void**) UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        ///     Retrieves the number of frames of padding in the endpoint
        ///     buffer.
        /// </summary>
        /// <returns>The frame count (the number of audio frames of padding in the buffer).</returns>
        /// <remarks>
        ///     The size of one frame = <c>(number of bits per sample)/8 * (number of channels)</c>
        /// </remarks>
        public int GetCurrentPadding()
        {
            int padding;
            CoreAudioAPIException.Try(GetCurrentPaddingNative(out padding), InterfaceName, "GetCurrentPadding");
            return padding;
        }

        /// <summary>
        ///     Indicates whether the audio endpoint device
        ///     supports a particular stream format.
        /// </summary>
        /// <param name="shareMode">
        ///     The sharing mode for the stream format. Through this parameter, the client indicates whether it
        ///     wants to use the specified format in exclusive mode or shared mode.
        /// </param>
        /// <param name="waveFormat">The stream format to test whether it is supported by the <see cref="AudioClient" /> or not.</param>
        /// <param name="closestMatch">
        ///     Retrieves the supported format that is closest to the format that the client specified
        ///     through the <paramref name="waveFormat" /> parameter. If <paramref name="shareMode" /> is
        ///     <see cref="AudioClientShareMode.Shared" />, the <paramref name="closestMatch" /> will be always null.
        /// </param>
        /// <returns>
        ///     HRESULT code. If the method returns 0 (= <see cref="HResult.S_OK" />), the endpoint device supports the specified
        ///     <paramref name="waveFormat" />. If the method returns
        ///     1 (= <see cref="HResult.S_FALSE" />), the method succeeded with a <paramref name="closestMatch" /> to the specified
        ///     <paramref name="waveFormat" />. If the method returns
        ///     0x88890008 (= <see cref="HResult.AUDCLNT_E_UNSUPPORTED_FORMAT" />), the method succeeded but the specified format
        ///     is not supported in exclusive mode. If the method returns anything else, the method failed.
        /// </returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370876(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int IsFormatSupportedNative(AudioClientShareMode shareMode, WaveFormat waveFormat,
            out WaveFormat closestMatch)
        {
            closestMatch = null;
            IntPtr pclosestMatch = IntPtr.Zero;
            GCHandle hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            int result;
            try
            {
                result = InteropCalls.CallI(UnsafeBasePtr, shareMode, hWaveFormat.AddrOfPinnedObject().ToPointer(),
                    shareMode == AudioClientShareMode.Shared ? &pclosestMatch : IntPtr.Zero.ToPointer(),
                    ((void**) (*(void**) UnsafeBasePtr))[7]);

                if (pclosestMatch != IntPtr.Zero)
                {
                    closestMatch = (WaveFormat) Marshal.PtrToStructure(pclosestMatch, typeof (WaveFormat));
                    if (closestMatch.ExtraSize == WaveFormatExtensible.WaveFormatExtensibleExtraSize)
                    {
                        closestMatch =
                            (WaveFormatExtensible) Marshal.PtrToStructure(pclosestMatch, typeof (WaveFormatExtensible));
                    }
                }
            }
            finally
            {
                hWaveFormat.Free();
                if (pclosestMatch != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(pclosestMatch);
            }

            return result;
        }

        /// <summary>
        ///     Indicates whether the audio endpoint device
        ///     supports a particular stream format.
        /// </summary>
        /// <param name="shareMode">
        ///     The sharing mode for the stream format. Through this parameter, the client indicates whether it
        ///     wants to use the specified format in exclusive mode or shared mode.
        /// </param>
        /// <param name="waveFormat">The stream format to test whether it is supported by the <see cref="AudioClient" /> or not.</param>
        /// <param name="closestMatch">
        ///     Retrieves the supported format that is closest to the format that the client specified
        ///     through the <paramref name="waveFormat" /> parameter. If <paramref name="shareMode" /> is
        ///     <see cref="AudioClientShareMode.Shared" />, the <paramref name="closestMatch" /> will be always null.
        /// </param>
        /// <returns>
        ///     <c>True</c> if the <paramref name="waveFormat" /> is supported. <c>False</c> if the
        ///     <paramref name="waveFormat" /> is not supported.
        /// </returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370876(v=vs.85).aspx" />.
        /// </remarks>
        public bool IsFormatSupported(AudioClientShareMode shareMode, WaveFormat waveFormat, out WaveFormat closestMatch)
        {
            int result = IsFormatSupportedNative(shareMode, waveFormat, out closestMatch);
            switch (result)
            {
                case 0x0:
                    return true;

                case 0x1:
                case unchecked((int) 0x88890008):
                    return false;

                default:
                    CoreAudioAPIException.Try(result, InterfaceName, "IsFormatSupported");
                    return false;
            }
        }

        /// <summary>
        ///     Indicates whether the audio endpoint device
        ///     supports a particular stream format.
        /// </summary>
        /// <param name="shareMode">
        ///     The sharing mode for the stream format. Through this parameter, the client indicates whether it
        ///     wants to use the specified format in exclusive mode or shared mode.
        /// </param>
        /// <param name="waveFormat">The stream format to test whether it is supported by the <see cref="AudioClient" /> or not.</param>
        /// <returns>
        ///     <c>True</c> if the <paramref name="waveFormat" /> is supported. <c>False</c> if the
        ///     <paramref name="waveFormat" /> is not supported.
        /// </returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370876(v=vs.85).aspx" />.
        /// </remarks>
        public bool IsFormatSupported(AudioClientShareMode shareMode, WaveFormat waveFormat)
        {
            WaveFormat tmp;
            return IsFormatSupported(shareMode, waveFormat, out tmp);
        }

        /// <summary>
        ///     Retrieves the stream format that the audio engine uses for its
        ///     internal processing of shared-mode streams.
        /// </summary>
        /// <param name="deviceFormat">
        ///     Retrieves the mix format that the audio engine uses for its internal processing of
        ///     shared-mode streams.
        /// </param>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370872(v=vs.85).aspx" />.
        /// </remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetMixFormatNative(out WaveFormat deviceFormat)
        {
            IntPtr pdeviceFormat = IntPtr.Zero;
            int result;
            if ((result = InteropCalls.CallI(UnsafeBasePtr, &pdeviceFormat, ((void**) (*(void**) UnsafeBasePtr))[8])) ==
                0 && pdeviceFormat != IntPtr.Zero)
            {
                try
                {
                    //deviceFormat = Marshal.PtrToStructure(pdeviceFormat, typeof (WaveFormat)) as WaveFormat;
                    //if (deviceFormat != null && deviceFormat.WaveFormatTag == AudioEncoding.Extensible)
                    //{
                    //    deviceFormat =
                    //        Marshal.PtrToStructure(pdeviceFormat, typeof (WaveFormatExtensible)) as WaveFormatExtensible;
                    //}
                    deviceFormat = WaveFormatMarshaler.PointerToWaveFormat(pdeviceFormat);
                }
                finally
                {
                    Marshal.FreeCoTaskMem(pdeviceFormat);
                }
                return result;
            }
            deviceFormat = null;
            return result;
        }

        /// <summary>
        ///     Retrieves the stream format that the audio engine uses for its
        ///     internal processing of shared-mode streams.
        /// </summary>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370872(v=vs.85).aspx" />.
        /// </remarks>
        /// <returns>The mix format that the audio engine uses for its internal processing of shared-mode streams.</returns>
        public WaveFormat GetMixFormat()
        {
            WaveFormat waveFormat;
            CoreAudioAPIException.Try(GetMixFormatNative(out waveFormat), InterfaceName, "GetMixFormat");
            return waveFormat;
        }

        /// <summary>
        ///     Retrieves the length of the periodic interval separating
        ///     successive processing passes by the audio engine on the data in the endpoint buffer.
        /// </summary>
        /// <param name="hnsDefaultDevicePeriod">
        ///     Retrieves a time value specifying the default interval between periodic processing
        ///     passes by the audio engine. The time is expressed in 100-nanosecond units.
        /// </param>
        /// <param name="hnsMinimumDevicePeriod">
        ///     Retrieves a time value specifying the minimum interval between periodic processing
        ///     passes by the audio endpoint device. The time is expressed in 100-nanosecond units.
        /// </param>
        /// <remarks>
        ///     Use the <paramref name="hnsDefaultDevicePeriod" /> and the <paramref name="hnsMinimumDevicePeriod" /> properties instead of
        ///     the <see cref="GetDevicePeriodNative" /> method.
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370871(v=vs.85).aspx" />.
        /// </remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetDevicePeriodNative(out long hnsDefaultDevicePeriod, out long hnsMinimumDevicePeriod)
        {
            fixed (void* ddp = &hnsDefaultDevicePeriod, mdp = &hnsMinimumDevicePeriod)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ddp, mdp, ((void**) (*(void**) UnsafeBasePtr))[9]);
            }
        }

        /// <summary>
        ///     Starts the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370879(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int StartNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[10]);
        }

        /// <summary>
        ///     Starts the audio stream.
        /// </summary>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370879(v=vs.85).aspx" />.
        /// </remarks>
        public void Start()
        {
            CoreAudioAPIException.Try(StartNative(), InterfaceName, "Start");
        }

        /// <summary>
        ///     Stops the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370880(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int StopNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[11]);
        }

        /// <summary>
        ///     Stops the audio stream.
        /// </summary>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370880(v=vs.85).aspx" />.
        /// </remarks>
        public void Stop()
        {
            CoreAudioAPIException.Try(StopNative(), InterfaceName, "Stop");
        }

        /// <summary>
        ///     Resets the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370877(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int ResetNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[12]);
        }

        /// <summary>
        ///     Resets the audio stream.
        /// </summary>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370877(v=vs.85).aspx" />.
        /// </remarks>
        public void Reset()
        {
            CoreAudioAPIException.Try(ResetNative(), InterfaceName, "Reset");
        }

        /// <summary>
        ///     Sets the event handle that the system signals when an audio
        ///     buffer is ready to be processed by the client.
        /// </summary>
        /// <param name="handle">The event handle.</param>
        /// <returns>
        ///     HRESULT
        /// </returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370878(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int SetEventHandleNative(IntPtr handle)
        {
            return InteropCalls.CallI(UnsafeBasePtr, handle.ToPointer(), ((void**) (*(void**) UnsafeBasePtr))[13]);
        }

        /// <summary>
        ///     Sets the event handle that the system signals when an audio
        ///     buffer is ready to be processed by the client.
        /// </summary>
        /// <param name="handle">The event handle.</param>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370878(v=vs.85).aspx" />.
        /// </remarks>
        public void SetEventHandle(IntPtr handle)
        {
            CoreAudioAPIException.Try(SetEventHandleNative(handle), InterfaceName, "SetEventHandle");
        }

        /// <summary>
        ///     Sets the event handle that the system signals when an audio
        ///     buffer is ready to be processed by the client.
        /// </summary>
        /// <param name="waitHandle">The event handle.</param>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370878(v=vs.85).aspx" />.
        /// </remarks>
        public void SetEventHandle(WaitHandle waitHandle)
        {
            if (waitHandle == null)
                throw new ArgumentNullException("waitHandle");
            SetEventHandle(waitHandle.SafeWaitHandle.DangerousGetHandle());
        }

        /// <summary>
        ///     Accesses additional services from the audio client object.
        /// </summary>
        /// <param name="riid">
        ///     The interface ID for the requested service. For a list of all available values, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370873(v=vs.85).aspx" />.
        /// </param>
        /// <param name="ppv">
        ///     A pointer variable into which the method writes the address of an instance of the
        ///     requested interface. Through this method, the caller obtains a counted reference to the interface. The caller is
        ///     responsible for releasing the interface, when it is no longer needed, by calling the interface's Release method. If
        ///     the GetService call fails, *ppv is <see cref="IntPtr.Zero" />.
        /// </param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370873(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int GetServiceNative(Guid riid, out IntPtr ppv)
        {
            fixed (void* pppv = &ppv)
            {
                return InteropCalls.CallI(UnsafeBasePtr, &riid, pppv, ((void**) (*(void**) UnsafeBasePtr))[14]);
            }
        }

        /// <summary>
        ///     Accesses additional services from the audio client object.
        /// </summary>
        /// <param name="riid">
        ///     The interface ID for the requested service. For a list of all available values, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370873(v=vs.85).aspx" />.
        /// </param>
        /// <returns>
        ///     A pointer into which the method writes the address of an instance of the requested interface.
        ///     Through this method, the caller obtains a counted reference to the interface. The caller is responsible for
        ///     releasing the interface, when it is no longer needed, by calling the interface's Release method.
        /// </returns>
        /// <remarks>
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370873(v=vs.85).aspx" />.
        /// </remarks>
        public IntPtr GetService(Guid riid)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetServiceNative(riid, out ptr), InterfaceName, "GetService");
            return ptr;
        }
    }
}