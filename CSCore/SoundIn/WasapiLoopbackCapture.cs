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
