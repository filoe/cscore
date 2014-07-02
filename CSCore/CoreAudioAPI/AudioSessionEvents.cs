using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionEvents.
    /// Fore more information take a look at: http://msdn.microsoft.com/en-us/library/windows/desktop/dd368289(v=vs.85).aspx
    /// </summary>
    [Guid("24918ACC-64B3-37C1-8CA9-74A66E9957A8")]
    public class AudioSessionEvents : IAudioSessionEvents
    {
        /// <summary>
        /// Notifies the client that the display name for the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionDisplayNameChangedEventArgs> DisplayNameChanged;

        /// <summary>
        /// Notifies the client that the display icon for the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionIconPathChangedEventArgs> IconPathChanged;

        /// <summary>
        /// Notifies the client that the volume level or muting state of the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionSimpleVolumeChangedEventArgs> SimpleVolumeChanged;

        /// <summary>
        /// Notifies the client that the volume level of an audio channel in the session submix has changed.
        /// </summary>
        public event EventHandler<AudioSessionChannelVolumeChangedEventArgs> ChannelVolumeChanged;

        /// <summary>
        /// Notifies the client that the grouping parameter for the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionGroupingParamChangedEventArgs> GroupingParamChanged;

        /// <summary>
        /// Notifies the client that the stream-activity state of the session has changed.
        /// </summary>
        public event EventHandler<AudioSessionStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Notifies the client that the session has been disconnected.
        /// </summary>
        public event EventHandler<AudioSessionDisconnectedEventArgs> SessionDisconnected;

        int IAudioSessionEvents.OnDisplayNameChanged(string newDisplayName, ref Guid eventContext)
        {
            if (DisplayNameChanged != null)
                DisplayNameChanged(this, new AudioSessionDisplayNameChangedEventArgs(newDisplayName, eventContext));
            return (int)Win32.HResult.S_OK;
        }

        int IAudioSessionEvents.OnIconPathChanged(string newIconPath, ref Guid eventContext)
        {
            if (IconPathChanged != null)
                IconPathChanged(this, new AudioSessionIconPathChangedEventArgs(newIconPath, eventContext));
            return (int)Win32.HResult.S_OK;
        }

        int IAudioSessionEvents.OnSimpleVolumeChanged(float newVolume, bool newMute, ref Guid eventContext)
        {
            if (SimpleVolumeChanged != null)
                SimpleVolumeChanged(this, new AudioSessionSimpleVolumeChangedEventArgs(newVolume, newMute, eventContext));
            return (int)Win32.HResult.S_OK;
        }

        int IAudioSessionEvents.OnChannelVolumeChanged(int channelCount, float[] newChannelVolumeArray, int changedChannel, ref Guid eventContext)
        {
            if (ChannelVolumeChanged != null)
                ChannelVolumeChanged(this, new AudioSessionChannelVolumeChangedEventArgs(channelCount, newChannelVolumeArray, changedChannel, eventContext));
            return (int)Win32.HResult.S_OK;
        }

        int IAudioSessionEvents.OnGroupingParamChanged(ref Guid newGroupingParam, ref Guid eventContext)
        {
            if (GroupingParamChanged != null)
                GroupingParamChanged(this, new AudioSessionGroupingParamChangedEventArgs(newGroupingParam, eventContext));
            return (int)Win32.HResult.S_OK;
        }

        int IAudioSessionEvents.OnStateChanged(AudioSessionState newState)
        {
            if (StateChanged != null)
                StateChanged(this, new AudioSessionStateChangedEventArgs(newState));
            return (int)Win32.HResult.S_OK;
        }

        int IAudioSessionEvents.OnSessionDisconnected(AudioSessionDisconnectReason disconnectReason)
        {
            if (SessionDisconnected != null)
                SessionDisconnected(this, new AudioSessionDisconnectedEventArgs(disconnectReason));
            return (int)Win32.HResult.S_OK;
        }
    }
}