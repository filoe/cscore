using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Enables a client to create and initialize an audio stream between an audio application and the audio engine (for a shared-mode stream) or the hardware buffer of an audio endpoint device (for an exclusive-mode stream). For more details see
    /// <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370865(v=vs.85).aspx"/>.
    /// </summary>
    [Guid("1CB9AD4C-DBFA-4c32-B178-C2F568A703B2")]
    public class AudioClient : ComObject
    {
        /// <summary>
        /// IID of the IAudioClient-interface.
        /// </summary>
// ReSharper disable once InconsistentNaming
        public static readonly Guid IID_IAudioClient = new Guid("1CB9AD4C-DBFA-4c32-B178-C2F568A703B2");

        private const string InterfaceName = "IAudioClient";

        /// <summary>
        /// Creates a new <see cref="AudioClient"/> instance.
        /// </summary>
        /// <param name="device">Device which should be used to create the <see cref="AudioClient"/> instance.</param>
        /// <returns><see cref="AudioClient"/> instance.</returns>
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
        /// Gets the default interval between periodic processing passes by the audio engine. The time is expressed in 100-nanosecond units.
        /// </summary>
        public long DefaultDevicePeriod
        {
            get
            {
                long n0, n1;
                CoreAudioAPIException.Try(GetDevicePeriod(out n0, out n1), InterfaceName, "GetDevicePeriod");
                return n0;
            }
        }

        /// <summary>
        /// Gets the minimum interval between periodic processing passes by the audio endpoint device. The time is expressed in 100-nanosecond units.
        /// </summary>
        public long MinimumDevicePeriod
        {
            get
            {
                long n0, n1;
                CoreAudioAPIException.Try(GetDevicePeriod(out n0, out n1), InterfaceName, "GetDevicePeriod");
                return n1;
            }
        }

        /// <summary>
        /// Gets the maximum capacity of the endpoint buffer.
        /// </summary>
        public int BufferSize
        {
            get { return GetBufferSize(); }
        }

        /// <summary>
        /// Gets the number of frames of padding in the endpoint buffer.
        /// </summary>
        public int CurrentPadding
        {
            get { return GetCurrentPadding(); }
        }

        /// <summary>
        /// Gets the stream format that the audio engine uses for its
        /// internal processing of shared-mode streams.
        /// </summary>
        public WaveFormat MixFormat
        {
            get { return GetMixFormat(); }
        }

        /// <summary>
        /// Gets the maximum latency for the current stream and can
        /// be called any time after the stream has been initialized.
        /// </summary>
        public long StreamLatency
        {
            get { return GetStreamLatency(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioClient"/> class.
        /// </summary>
        /// <param name="ptr">Native pointer.</param>
        /// <remarks>Use the <see cref="FromMMDevice"/> method to create a new <see cref="AudioClient"/> instance.</remarks>
        public AudioClient(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// The Initialize method initializes the audio stream.
        /// </summary>
        public unsafe int InitializeNative(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, long hnsBufferDuration, long hnsPeriodicity,
                              WaveFormat waveFormat, Guid audioSessionGuid)
        {
            var hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            int result = -1;
            result = InteropCalls.CallI(UnsafeBasePtr, shareMode, streamFlags, hnsBufferDuration, hnsPeriodicity, hWaveFormat.AddrOfPinnedObject().ToPointer(), audioSessionGuid, ((void**)(*(void**)UnsafeBasePtr))[3]);
            hWaveFormat.Free();
            return result;
        }

        /// <summary>
        /// The Initialize method initializes the audio stream.
        /// </summary>
        public void Initialize(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, long hnsBufferDuration, long hnsPeriodicity, WaveFormat waveFormat, Guid audioSessionGuid)
        {
            CoreAudioAPIException.Try(InitializeNative(shareMode, streamFlags, hnsBufferDuration, hnsPeriodicity, waveFormat, audioSessionGuid), InterfaceName, "Initialize");
        }

        /// <summary>
        /// The GetBufferSize method retrieves the size (maximum capacity) of the endpoint buffer.
        /// </summary>
        /// <remarks>
        /// The length is expressed as the number of audio frames the buffer can hold. The size in
        /// bytes of an audio frame is calculated as the number of channels in the stream multiplied
        /// by the sample size per channel. For example, the frame size is four bytes for a stereo
        /// (2-channel) stream with 16-bit samples.
        /// </remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetBufferSize(out Int32 bufferFramesCount)
        {
            fixed (void* pbfc = &bufferFramesCount)
            {
                return InteropCalls.CallI(UnsafeBasePtr, pbfc, ((void**)(*(void**)UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        /// The GetBufferSize method retrieves the size (maximum capacity) of the endpoint buffer.
        /// </summary>
        /// ///
        /// <remarks>
        /// The length is expressed as the number of audio frames the buffer can hold. The size in
        /// bytes of an audio frame is calculated as the number of channels in the stream multiplied
        /// by the sample size per channel. For example, the frame size is four bytes for a stereo
        /// (2-channel) stream with 16-bit samples.
        /// </remarks>
        public int GetBufferSize()
        {
            int bufferSize;
            CoreAudioAPIException.Try(GetBufferSize(out bufferSize), InterfaceName, "GetBufferSize");
            return bufferSize;
        }

        /// <summary>
        /// The GetStreamLatency method retrieves the maximum latency for the current stream and can
        /// be called any time after the stream has been initialized.
        /// </summary>
        /// <remarks>
        /// Rendering clients can use this latency value to compute the minimum amount of data that
        /// they can write during any single processing pass. To write less than this minimum is to
        /// risk introducing glitches into the audio stream. For more information, see
        /// IAudioRenderClient::GetBuffer.
        /// </remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetStreamLatency(out long hnsLatency)
        {
            fixed (void* pl = &hnsLatency)
            {
                return InteropCalls.CallI(UnsafeBasePtr, pl, ((void**)(*(void**)UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        /// The GetStreamLatency method retrieves the maximum latency for the current stream and can
        /// be called any time after the stream has been initialized.
        /// </summary>
        /// <remarks>
        /// Rendering clients can use this latency value to compute the minimum amount of data that
        /// they can write during any single processing pass. To write less than this minimum is to
        /// risk introducing glitches into the audio stream. For more information, see
        /// IAudioRenderClient::GetBuffer.
        /// </remarks>
        public long GetStreamLatency()
        {
            long latency = -1;
            CoreAudioAPIException.Try(GetStreamLatency(out latency), InterfaceName, "GetStreamLatency");
            return latency;
        }

        /// <summary>
        /// The GetCurrentPadding method retrieves the number of frames of padding in the endpoint
        /// buffer.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetCurrentPaddingNative(out Int32 numPaddingFrames)
        {
            fixed (void* pndf = &numPaddingFrames)
            {
                return InteropCalls.CallI(UnsafeBasePtr, pndf, ((void**)(*(void**)UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        /// The GetCurrentPadding method retrieves the number of frames of padding in the endpoint
        /// buffer.
        /// </summary>
        public int GetCurrentPadding()
        {
            int padding;
            CoreAudioAPIException.Try(GetCurrentPaddingNative(out padding), InterfaceName, "GetCurrentPadding");
            return padding;
        }

        /// <summary>
        /// The IsFormatSupportedInternal method indicates whether the audio endpoint device
        /// supports a particular stream format.
        /// </summary>
        /// <returns>For exclusive mode, IsFormatSupportedInternal returns S_OK if the audio
        /// endpoint device supports the caller-specified format, or it returns
        /// AUDCLNT_E_UNSUPPORTED_FORMAT if the device does not support the format. The
        /// ppClosestMatch parameter can be NULL. If it is not NULL, the method writes NULL to
        /// *ppClosestMatch. For shared mode, if the audio engine supports the caller-specified
        /// format, IsFormatSupportedInternal sets *ppClosestMatch to NULL and returns S_OK. If the
        /// audio engine does not support the caller-specified format but does support a similar
        /// format, the method retrieves the similar format through the ppClosestMatch parameter and
        /// returns S_FALSE. If the audio engine does not support the caller-specified format or any
        /// similar format, the method sets *ppClosestMatch to NULL and returns
        /// AUDCLNT_E_UNSUPPORTED_FORMAT.</returns>
        public unsafe int IsFormatSupportedNative(AudioClientShareMode shareMode, WaveFormat waveFormat, out WaveFormat closestMatch)
        {
            closestMatch = null;
            IntPtr pclosestMatch = IntPtr.Zero;
            var hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
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
                        closestMatch =
                            (WaveFormatExtensible) Marshal.PtrToStructure(pclosestMatch, typeof (WaveFormatExtensible));
                }
            }
            finally
            {
                hWaveFormat.Free();
                if(pclosestMatch != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(pclosestMatch);
            }

            return result;
        }

        /// <summary>
        /// Checks whether the audio endpoint device supports a particular stream format.
        /// </summary>
        public bool IsFormatSupported(AudioClientShareMode shareMode, WaveFormat waveFormat, out WaveFormat closestMatch)
        {
            int result = IsFormatSupportedNative(shareMode, waveFormat, out closestMatch);
            switch (result)
            {
                case 0x0:
                    return true;

                case 0x1:
                case unchecked((int)0x88890008):
                    return false;

                default:
                    CoreAudioAPIException.Try(result, InterfaceName, "IsFormatSupported");
                    return false;
            }
        }

        /// <summary>
        /// Checks whether the audio endpoint device supports a particular stream format.
        /// </summary>
        /// <param name="shareMode"></param>
        /// <param name="waveFormat"></param>
        /// <returns></returns>
        public bool IsFormatSupported(AudioClientShareMode shareMode, WaveFormat waveFormat)
        {
            WaveFormat tmp;
            return IsFormatSupported(shareMode, waveFormat, out tmp);
        }

        /// <summary>
        /// The GetMixFormat method retrieves the stream format that the audio engine uses for its
        /// internal processing of shared-mode streams.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetMixFormat(out WaveFormat deviceFormat)
        {
            IntPtr pdeviceFormat = IntPtr.Zero;
            int result = -1;
            if ((result = InteropCalls.CallI(UnsafeBasePtr, &pdeviceFormat, ((void**)(*(void**)UnsafeBasePtr))[8])) == 0 && pdeviceFormat != IntPtr.Zero)
            {
                deviceFormat = Marshal.PtrToStructure(pdeviceFormat, typeof(WaveFormat)) as WaveFormat;
                if (deviceFormat != null && deviceFormat.WaveFormatTag == AudioEncoding.Extensible)
                    deviceFormat = Marshal.PtrToStructure(pdeviceFormat, typeof(WaveFormatExtensible)) as WaveFormatExtensible;

                Marshal.FreeCoTaskMem(pdeviceFormat);
                return result;
            }
            else
            {
                deviceFormat = null;
                return result;
            }
        }

        /// <summary>
        /// The GetMixFormat method retrieves the stream format that the audio engine uses for its
        /// internal processing of shared-mode streams.
        /// </summary>
        public WaveFormat GetMixFormat()
        {
            WaveFormat waveFormat;
            CoreAudioAPIException.Try(GetMixFormat(out waveFormat), InterfaceName, "GetMixFormat");
            return waveFormat;
        }

        /// <summary>
        /// The GetDevicePeriod method retrieves the length of the periodic interval separating
        /// successive processing passes by the audio engine on the data in the endpoint buffer.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetDevicePeriod(out long hnsDefaultDevicePeriod, out long hnsMinimumDevicePeriod)
        {
            fixed (void* ddp = &hnsDefaultDevicePeriod, mdp = &hnsMinimumDevicePeriod)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ddp, mdp, ((void**)(*(void**)UnsafeBasePtr))[9]);
            }
        }

        /// <summary>
        /// The Start method starts the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int StartNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[10]);
        }

        /// <summary>
        /// The Start method starts the audio stream.
        /// </summary>
        public void Start()
        {
            CoreAudioAPIException.Try(StartNative(), InterfaceName, "Start");
        }

        /// <summary>
        /// The Stop method stops the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int StopNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[11]);
        }

        /// <summary>
        /// The Stop method stops the audio stream.
        /// </summary>
        public void Stop()
        {
            CoreAudioAPIException.Try(StopNative(), InterfaceName, "Stop");
        }

        /// <summary>
        /// The Reset method resets the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int ResetNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[12]);
        }

        /// <summary>
        /// The Reset method resets the audio stream.
        /// </summary>
        public void Reset()
        {
            CoreAudioAPIException.Try(ResetNative(), InterfaceName, "Reset");
        }

        /// <summary>
        /// The SetEventHandle method sets the event handle that the system signals when an audio
        /// buffer is ready to be processed by the client.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetEventHandleNative(IntPtr handle)
        {
            return InteropCalls.CallI(UnsafeBasePtr, handle.ToPointer(), ((void**)(*(void**)UnsafeBasePtr))[13]);
        }

        /// <summary>
        /// The SetEventHandle method sets the event handle that the system signals when an audio
        /// buffer is ready to be processed by the client.
        /// </summary>
        public void SetEventHandle(IntPtr handle)
        {
            CoreAudioAPIException.Try(SetEventHandleNative(handle), InterfaceName, "SetEventHandle");
        }

        /// <summary>
        /// The GetService method accesses additional services from the audio client object. Fore
        /// more details see
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd370873(v=vs.85).aspx
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetServiceNative(Guid riid, out IntPtr ppv)
        {
            fixed (void* pppv = &ppv)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ((void*)&riid), pppv, ((void**)(*(void**)UnsafeBasePtr))[14]);
            }
        }

        /// <summary>
        /// The GetService method accesses additional services from the audio client object. Fore
        /// more details see
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd370873(v=vs.85).aspx
        /// </summary>
        /// <remarks>
        /// For a few services, there are already existing classes with static
        /// "FromAudioClient"-Methods like AudioRenderClient::FromAudioClient.
        /// </remarks>
        public IntPtr GetService(Guid riid)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetServiceNative(riid, out ptr), InterfaceName, "GetService");
            return ptr;
        }
    }
}