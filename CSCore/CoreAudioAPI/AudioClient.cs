using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Wrapper of the IAudioClient-Interface. For more details see http://msdn.microsoft.com/en-us/library/windows/desktop/dd370865(v=vs.85).aspx
    /// </summary>
    [Guid("1CB9AD4C-DBFA-4c32-B178-C2F568A703B2")]
    public class AudioClient : ComObject
    {
        public static readonly Guid IID_IAudioClient = new Guid("1CB9AD4C-DBFA-4c32-B178-C2F568A703B2");
        const string c = "IAudioClient";

        public static AudioClient FromMMDevice(MMDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            IntPtr ptr;
            int result = device.Activate(IID_IAudioClient, ExecutionContext.CLSCTX_ALL, IntPtr.Zero, out ptr);
            CoreAudioAPIException.Try(result, "IMMDevice", "Activate");

            return new AudioClient(ptr);
        }

        public AudioClient(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// The Initialize method initializes the audio stream.
        /// </summary>
        public unsafe int InitializeInternal(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, long hnsBufferDuration, long hnsPeriodicity,
                              WaveFormat waveFormat, Guid audioSessionGuid)
        {
            var hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            int result = -1;
            result = InteropCalls.CallI(_basePtr, shareMode, streamFlags, hnsBufferDuration, hnsPeriodicity, hWaveFormat.AddrOfPinnedObject().ToPointer(), audioSessionGuid, ((void**)(*(void**)_basePtr))[3]);
            hWaveFormat.Free();
            return result;
        }

        /// <summary>
        /// The Initialize method initializes the audio stream.
        /// </summary>
        public void Initialize(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, long hnsBufferDuration, long hnsPeriodicity, WaveFormat waveFormat, Guid audioSessionGuid)
        {
            CoreAudioAPIException.Try(InitializeInternal(shareMode, streamFlags, hnsBufferDuration, hnsPeriodicity, waveFormat, audioSessionGuid), c, "Initialize");
        }

        /// <summary>
        /// The GetBufferSize method retrieves the size (maximum capacity) of the endpoint buffer.
        /// </summary>
        /// <remarks>The length is expressed as the number of audio frames the buffer can hold. The size in bytes of an audio frame is calculated as the number of channels in the stream multiplied by the sample size per channel. For example, the frame size is four bytes for a stereo (2-channel) stream with 16-bit samples.</remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetBufferSize(out UInt32 bufferFramesCount)
        {
            fixed (void* pbfc = &bufferFramesCount)
            {
                return InteropCalls.CallI(_basePtr, pbfc, ((void**)(*(void**)_basePtr))[4]);
            }
        }

        /// <summary>
        /// The GetBufferSize method retrieves the size (maximum capacity) of the endpoint buffer.
        /// </summary>
        /// /// <remarks>The length is expressed as the number of audio frames the buffer can hold. The size in bytes of an audio frame is calculated as the number of channels in the stream multiplied by the sample size per channel. For example, the frame size is four bytes for a stereo (2-channel) stream with 16-bit samples.</remarks>
        public int GetBufferSize()
        {
            uint bufferSize;
            CoreAudioAPIException.Try(GetBufferSize(out bufferSize), c, "GetBufferSize");
            return (int)bufferSize;
        }

        /// <summary>
        /// The GetStreamLatency method retrieves the maximum latency for the current stream and can be called any time after the stream has been initialized.
        /// </summary>
        /// <remarks>Rendering clients can use this latency value to compute the minimum amount of data that they can write during any single processing pass. To write less than this minimum is to risk introducing glitches into the audio stream. For more information, see IAudioRenderClient::GetBuffer.</remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetStreamLatency(out int hnsLatency)
        {
            fixed (void* pl = &hnsLatency)
            {
                return InteropCalls.CallI(_basePtr, pl, ((void**)(*(void**)_basePtr))[5]);
            }
        }

        public int GetStreamLatency()
        {
            int latency = -1;
            CoreAudioAPIException.Try(GetStreamLatency(out latency), c, "GetStreamLatency");
            return latency;
        }

        /// <summary>
        /// The GetCurrentPadding method retrieves the number of frames of padding in the endpoint buffer.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetCurrentPadding(out UInt32 numPaddingFrames)
        {
            fixed (void* pndf = &numPaddingFrames)
            {
                return InteropCalls.CallI(_basePtr, pndf, ((void**)(*(void**)_basePtr))[6]);
            }
        }

        public int GetCurrentPadding()
        {
            uint padding;
            CoreAudioAPIException.Try(GetCurrentPadding(out padding), c, "GetCurrentPadding");
            return (int)padding;
        }

        /// <summary>
        /// The IsFormatSupportedInternal method indicates whether the audio endpoint device supports a particular stream format.
        /// </summary>
        /// <returns>For exclusive mode, IsFormatSupportedInternal returns S_OK if the audio endpoint device supports the caller-specified format, or it returns AUDCLNT_E_UNSUPPORTED_FORMAT if the device does not support the format. The ppClosestMatch parameter can be NULL. If it is not NULL, the method writes NULL to *ppClosestMatch. 
        /// For shared mode, if the audio engine supports the caller-specified format, IsFormatSupportedInternal sets *ppClosestMatch to NULL and returns S_OK. If the audio engine does not support the caller-specified format but does support a similar format, the method retrieves the similar format through the ppClosestMatch parameter and returns S_FALSE. If the audio engine does not support the caller-specified format or any similar format, the method sets *ppClosestMatch to NULL and returns AUDCLNT_E_UNSUPPORTED_FORMAT.</returns>
        public unsafe int IsFormatSupportedInternal(AudioClientShareMode shareMode, WaveFormat waveFormat, out WaveFormatExtensible closestMatch)
        {
            closestMatch = null;
            var hClosestMatch = GCHandle.Alloc(closestMatch, GCHandleType.Pinned);
            var hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);

            IntPtr pclosestmatch = hClosestMatch.AddrOfPinnedObject();

            var result = InteropCalls.CallI(_basePtr, shareMode, hWaveFormat.AddrOfPinnedObject().ToPointer(),
                &pclosestmatch, ((void**)(*(void**)_basePtr))[7]); 

            hWaveFormat.Free();
            hClosestMatch.Free();

            return result;
        }

        /// <summary>
        /// Checks whether the audio endpoint device supports a particular stream format. 
        /// </summary>
        public bool IsFormatSupported(AudioClientShareMode shareMode, WaveFormat waveFormat, out WaveFormatExtensible closestMatch)
        {
            int result = IsFormatSupportedInternal(shareMode, waveFormat, out closestMatch);
            switch (result)
            {
                case 0x0:
                    return true;
                case 0x1:
                case unchecked((int)0x88890008):
                    return false;
                default:
                    CoreAudioAPIException.Try(result, c, "IsFormatSupported");
                    return false;
            }
        }

        public bool IsFormatSupported(AudioClientShareMode shareMode, WaveFormat waveFormat)
        {
            WaveFormatExtensible tmp;
            return IsFormatSupported(shareMode, waveFormat, out tmp);
        }

        /// <summary>
        /// The GetMixFormat method retrieves the stream format that the audio engine uses for its internal processing of shared-mode streams.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>HRESULT</returns>
        public unsafe int GetMixFormat(out WaveFormat deviceFormat)
        {
            IntPtr pdeviceFormat = IntPtr.Zero;
            int result = -1;
            if((result = InteropCalls.CallI(_basePtr, &pdeviceFormat, ((void**)(*(void**)_basePtr))[8])) == 0 && pdeviceFormat != IntPtr.Zero)
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

        public WaveFormat GetMixFormat()
        {
            WaveFormat waveFormat;
            CoreAudioAPIException.Try(GetMixFormat(out waveFormat), c, "GetMixFormat");
            return waveFormat;
        }

        /// <summary>
        /// The GetDevicePeriod method retrieves the length of the periodic interval separating successive processing passes by the audio engine on the data in the endpoint buffer.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetDevicePeriod(out long hnsDefaultDevicePeriod, out long hnsMinimumDevicePeriod)
        {
            fixed (void* ddp = &hnsDefaultDevicePeriod, mdp = &hnsMinimumDevicePeriod)
            {
                return InteropCalls.CallI(_basePtr, ddp, mdp, ((void**)(*(void**)_basePtr))[9]);
            }
        }

        /// <summary>
        /// The Start method starts the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int Start()
        {
            return InteropCalls.CallI(_basePtr, ((void**)(*(void**)_basePtr))[10]);
        }

        /// <summary>
        /// The Stop method stops the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int Stop()
        {
            return InteropCalls.CallI(_basePtr, ((void**)(*(void**)_basePtr))[11]);
        }

        /// <summary>
        /// The Reset method resets the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int Reset()
        {
            return InteropCalls.CallI(_basePtr, ((void**)(*(void**)_basePtr))[12]);
        }

        /// <summary>
        /// The SetEventHandle method sets the event handle that the system signals when an audio buffer is ready to be processed by the client.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetEventHandleInternal(IntPtr handle)
        {
            return InteropCalls.CallI(_basePtr, handle.ToPointer(), ((void**)(*(void**)_basePtr))[13]);
        }

        /// <summary>
        /// The SetEventHandle method sets the event handle that the system signals when an audio buffer is ready to be processed by the client.
        /// </summary>
        public void SetEventHandle(IntPtr handle)
        {
            CoreAudioAPIException.Try(SetEventHandleInternal(handle), c, "SetEventHandle");
        }

        /// <summary>
        /// The GetService method accesses additional services from the audio client object.
        /// Fore more details see http://msdn.microsoft.com/en-us/library/windows/desktop/dd370873(v=vs.85).aspx
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetServiceInternal(Guid riid, out IntPtr ppv)
        {
            fixed(void* pppv = &ppv){
                return InteropCalls.CallI(_basePtr, ((void*)&riid), pppv, ((void**)(*(void**)_basePtr))[14]);
            }
        }

        /// <summary>
        /// The GetService method accesses additional services from the audio client object.
        /// Fore more details see http://msdn.microsoft.com/en-us/library/windows/desktop/dd370873(v=vs.85).aspx
        /// </summary>
        public IntPtr GetService(Guid riid)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetServiceInternal(riid, out ptr), c, "GetService");
            return ptr;
        }
    }

    /// <summary>
    /// For details see: http://msdn.microsoft.com/en-us/library/windows/desktop/dd370791(v=vs.85).aspx and http://msdn.microsoft.com/en-us/library/windows/desktop/dd370789(v=vs.85).aspx
    /// </summary>
    [Flags]
    public enum AudioClientStreamFlags
    {
        None = 0x0,
        StreamFlags_CrossProcess = 0x00010000,
        StreamFlags_Loopback = 0x00020000,
        StreamFlags_EventCallback = 0x00040000,
        StreamFlags_NoPersist = 0x00080000,
        /// <summary>
        /// Supported since Windows 7
        /// </summary>
        StreamFlags_RateAdjust = 0x00100000,
        SessionFlags_ExpireWhenUnowned = 0x10000000,
        SessionFlags_DisplayHide = 0x20000000,
        SessionFlags_Display_HideWhenExpired = 0x40000000
    }
}
