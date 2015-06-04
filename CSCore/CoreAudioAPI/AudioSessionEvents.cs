using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides notifications of session-related events such as changes in the volume level, display name, and session state.
    /// For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd368289(v=vs.85).aspx"/>.
    /// </summary>
    [Guid("24918ACC-64B3-37C1-8CA9-74A66E9957A8")]
    public sealed class AudioSessionEvents : IAudioSessionEvents
    {
        /// <summary>
        /// Occurs when the display name for the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionDisplayNameChangedEventArgs> DisplayNameChanged;

        /// <summary>
        /// Occurs when the display icon for the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionIconPathChangedEventArgs> IconPathChanged;

        /// <summary>
        /// Occurs when the volume level or muting state of the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionSimpleVolumeChangedEventArgs> SimpleVolumeChanged;

        /// <summary>
        /// Occurs when the volume level of an audio channel in the session submix has changed.
        /// </summary>
        public event EventHandler<AudioSessionChannelVolumeChangedEventArgs> ChannelVolumeChanged;

        /// <summary>
        /// Occurs when the grouping parameter for the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionGroupingParamChangedEventArgs> GroupingParamChanged;

        /// <summary>
        /// Occurs when the stream-activity state of the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Occurs when the session has been disconnected.
        /// </summary>
        public event EventHandler<AudioSessionDisconnectedEventArgs> SessionDisconnected;

        /// <summary>
        /// Notifies the client that the display name for the session has changed.
        /// </summary>
        /// <param name="newDisplayName">The new display name for the session. </param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns>HRESULT</returns>
        int IAudioSessionEvents.OnDisplayNameChanged(string newDisplayName, ref Guid eventContext)
        {
            if (DisplayNameChanged != null)
                DisplayNameChanged(this, new AudioSessionDisplayNameChangedEventArgs(newDisplayName, eventContext));
            return (int) Win32.HResult.S_OK;
        }

        /// <summary>
        /// Notifies the client that the display icon for the session has changed.
        /// </summary>
        /// <param name="newIconPath">The path for the new display icon for the session.</param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns>HRESULT</returns>
        int IAudioSessionEvents.OnIconPathChanged(string newIconPath, ref Guid eventContext)
        {
            if (IconPathChanged != null)
                IconPathChanged(this, new AudioSessionIconPathChangedEventArgs(newIconPath, eventContext));
            return (int) Win32.HResult.S_OK;
        }

        /// <summary>
        /// Notifies the client that the volume level or muting state of the audio session has changed.
        /// </summary>
        /// <param name="newVolume">The new volume level for the audio session. This parameter is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume (no attenuation).</param>
        /// <param name="newMute">The new muting state. If TRUE, muting is enabled. If FALSE, muting is disabled.</param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns>HRESULT</returns>
        int IAudioSessionEvents.OnSimpleVolumeChanged(float newVolume, bool newMute, ref Guid eventContext)
        {
            if (SimpleVolumeChanged != null)
                SimpleVolumeChanged(this, new AudioSessionSimpleVolumeChangedEventArgs(newVolume, newMute, eventContext));
            return (int) Win32.HResult.S_OK;
        }

        /// <summary>
        /// Notifies the client that the volume level of an audio channel in the session submix has changed.
        /// </summary>
        /// <param name="channelCount">The number of channels in the session submix.</param>
        /// <param name="newChannelVolumeArray">An array of volume levels. Each element is a value of type float that specifies the volume level for a particular channel. Each volume level is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume (no attenuation). The number of elements in the array is specified by the ChannelCount parameter.</param>
        /// <param name="changedChannel">The number of the channel whose volume level changed.</param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns></returns>
        int IAudioSessionEvents.OnChannelVolumeChanged(int channelCount, float[] newChannelVolumeArray,
            int changedChannel, ref Guid eventContext)
        {
            if (ChannelVolumeChanged != null)
            {
                ChannelVolumeChanged(this,
                    new AudioSessionChannelVolumeChangedEventArgs(channelCount, newChannelVolumeArray, changedChannel,
                        eventContext));
            }
            return (int) Win32.HResult.S_OK;
        }

        /// <summary>
        /// Notifies the client that the grouping parameter for the session has changed.
        /// </summary>
        /// <param name="newGroupingParam">The new grouping parameter for the session. This parameter points to a grouping-parameter GUID.</param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns>HRESULT</returns>
        int IAudioSessionEvents.OnGroupingParamChanged(ref Guid newGroupingParam, ref Guid eventContext)
        {
            if (GroupingParamChanged != null)
                GroupingParamChanged(this, new AudioSessionGroupingParamChangedEventArgs(newGroupingParam, eventContext));
            return (int) Win32.HResult.S_OK;
        }

        /// <summary>
        /// Notifies the client that the stream-activity state of the session has changed.
        /// </summary>
        /// <param name="newState">The new session state.</param>
        /// <returns>HRESULT</returns>
        int IAudioSessionEvents.OnStateChanged(AudioSessionState newState)
        {
            if (StateChanged != null)
                StateChanged(this, new AudioSessionStateChangedEventArgs(newState));
            return (int) Win32.HResult.S_OK;
        }

        /// <summary>
        /// Notifies the client that the audio session has been disconnected.
        /// </summary>
        /// <param name="disconnectReason">The reason that the audio session was disconnected.</param>
        /// <returns>HRESULT</returns>
        int IAudioSessionEvents.OnSessionDisconnected(AudioSessionDisconnectReason disconnectReason)
        {
            if (SessionDisconnected != null)
                SessionDisconnected(this, new AudioSessionDisconnectedEventArgs(disconnectReason));
            return (int) Win32.HResult.S_OK;
        }
    }
}