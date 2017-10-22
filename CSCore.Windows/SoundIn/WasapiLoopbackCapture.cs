using System.Threading;
using CSCore.CoreAudioAPI;

namespace CSCore.SoundIn
{
    /// <summary>
    /// Provides audio loopback capture through Wasapi. That enables a client to capture the audio stream that is being played by a rendering endpoint device (e.g. speakers, headset, etc.).
    /// Minimum supported OS: Windows Vista (see <see cref="WasapiCapture.IsSupportedOnCurrentPlatform"/> property).
    /// Read more about loopback recording here: http://msdn.microsoft.com/en-us/library/windows/desktop/dd316551(v=vs.85).aspx.
    /// </summary>
    public class WasapiLoopbackCapture : WasapiCapture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiLoopbackCapture"/> class.
        /// </summary>
        public WasapiLoopbackCapture()
            : base(false, AudioClientShareMode.Shared)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiLoopbackCapture"/> class with the <paramref name="latency"/> specified in milliseconds.
        /// </summary>
        /// <param name="latency">The latency specified in milliseconds. The default value is 100ms.</param>
        public WasapiLoopbackCapture(int latency)
            : this(latency, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiLoopbackCapture"/> class with the <paramref name="latency"/> specified in milliseconds
        /// and the <paramref name="defaultFormat"/> to use.
        /// </summary>
        /// <param name="latency">The latency specified in milliseconds. The default value is 100ms.</param>
        /// <param name="defaultFormat">The default <see cref="WaveFormat"/> to use. 
        /// Note: The <paramref name="defaultFormat"/> is just a suggestion. If the driver does not support this format, 
        /// any other format will be picked. After calling <see cref="WasapiCapture.Initialize"/>, the <see cref="WasapiCapture.WaveFormat"/> 
        /// property will return the actually picked <see cref="WaveFormat"/>.</param>
        public WasapiLoopbackCapture(int latency, WaveFormat defaultFormat)
            : this(latency, defaultFormat, ThreadPriority.AboveNormal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiLoopbackCapture"/> class with the <paramref name="latency"/> specified in milliseconds,
        /// the <paramref name="defaultFormat"/> to use and the <see cref="ThreadPriority"/> of the internal capture thread.
        /// </summary>
        /// <param name="latency">The latency specified in milliseconds. The default value is 100ms.</param>
        /// <param name="defaultFormat">The default <see cref="WaveFormat"/> to use. 
        /// Note: The <paramref name="defaultFormat"/> is just a suggestion. If the driver does not support this format, 
        /// any other format will be picked. After calling <see cref="WasapiCapture.Initialize"/>, the <see cref="WasapiCapture.WaveFormat"/> 
        /// property will return the actually picked <see cref="WaveFormat"/>.</param>
        /// <param name="captureThreadPriority">The <see cref="ThreadPriority"/>, the internal capture thread will run on.</param>
        public WasapiLoopbackCapture(int latency, WaveFormat defaultFormat, ThreadPriority captureThreadPriority)
            : base(false, AudioClientShareMode.Shared, latency, defaultFormat, captureThreadPriority)
        {
            
        }

        /// <summary>
        /// Returns the default rendering device.
        /// </summary>
        /// <returns>Default rendering device.</returns>
        protected override MMDevice GetDefaultDevice()
        {
            return MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Console);
        }

        /// <summary>
        /// Returns the stream flags to use for the audioclient initialization.
        /// </summary>
        /// <returns>
        /// The stream flags to use for the audioclient initialization.
        /// </returns>
        protected override AudioClientStreamFlags GetStreamFlags()
        {
            return AudioClientStreamFlags.StreamFlagsLoopback;
        }
    }
}
