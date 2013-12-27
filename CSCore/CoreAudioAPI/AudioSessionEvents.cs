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

    /// <summary>
    /// AudioSessionEventContextEventArgs
    /// </summary>
    public abstract class AudioSessionEventContextEventArgs : EventArgs
    {
        /// <summary>
        /// The event context value.
        /// </summary>
        public Guid EventContext { get; private set; }

        public AudioSessionEventContextEventArgs(Guid eventContext)
        {
            EventContext = eventContext;
        }
    }

    /// <summary>
    /// AudioSessionDisplayNameChangedEventArgs
    /// </summary>
    public class AudioSessionDisplayNameChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The new display name for the session.
        /// </summary>
        public string NewDisplayName { get; private set; }

        public AudioSessionDisplayNameChangedEventArgs(string newDisplayName, Guid eventContext)
            : base(eventContext)
        {
            NewDisplayName = newDisplayName;
        }
    }

    /// <summary>
    /// AudioSessionIconPathChangedEventArgs
    /// </summary>
    public class AudioSessionIconPathChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The path for the new display icon for the session.
        /// </summary>
        public string NewIconPath { get; private set; }

        public AudioSessionIconPathChangedEventArgs(string newIconPath, Guid eventContext)
            : base(eventContext)
        {
            NewIconPath = newIconPath;
        }
    }

    /// <summary>
    /// AudioSessionSimpleVolumeChanged
    /// </summary>
    public class AudioSessionSimpleVolumeChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The new volume level for the audio session. This parameter is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume (no attenuation).
        /// </summary>
        public float NewVolume { get; private set; }

        /// <summary>
        /// The new muting state. If true, muting is enabled. If false, muting is disabled.
        /// </summary>
        public bool IsMuted { get; private set; }

        public AudioSessionSimpleVolumeChangedEventArgs(float newVolume, bool isMuted, Guid eventContext)
            : base(eventContext)
        {
            if (newVolume < 0 || newVolume > 1)
                throw new ArgumentOutOfRangeException("newVolume");
            NewVolume = newVolume;
            IsMuted = isMuted;
        }
    }

    /// <summary>
    /// AudioSessionChannelVolumeChanged
    /// </summary>
    public class AudioSessionChannelVolumeChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The channel count. This parameter specifies the number of audio channels in the session submix.
        /// </summary>
        public int ChannelCount { get; private set; }

        /// <summary>
        /// Each volume level is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume.
        /// </summary>
        public float[] ChannelVolumes { get; private set; }

        /// <summary>
        /// Use this value as an index into the <see cref="ChannelVolumes"/>.
        /// If the session submix contains n channels, the channels are numbered from 0 to n– 1. If more than one channel might have changed, the value of ChangedChannel is (DWORD)(–1).
        /// </summary>
        public int ChangedChannel { get; private set; }

        /// <summary>
        /// Returns the volume of the specified channel. <see cref="ChannelVolumes"/>
        /// </summary>
        /// <param name="channelIndex"></param>
        /// <returns>Volume level of the specified channelIndex in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume.</returns>
        public float this[int channelIndex]
        {
            get { return ChannelVolumes[channelIndex]; }
        }

        public AudioSessionChannelVolumeChangedEventArgs(int channelCount, float[] channelVolumes, int changedChannel, Guid eventContext)
            : base(eventContext)
        {
            if (channelVolumes == null)
                throw new ArgumentNullException("channelVolumes");

            if (channelCount < 0 || channelCount != channelVolumes.Length)
                throw new ArgumentOutOfRangeException("channelCount");

            ChannelCount = channelCount;
            ChannelVolumes = channelVolumes;
            ChangedChannel = changedChannel;
        }
    }

    /// <summary>
    /// AudioSessionGroupingParamChangedEventArgs
    /// </summary>
    public class AudioSessionGroupingParamChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The new grouping parameter for the session.
        /// </summary>
        public Guid NewGroupingParam { get; private set; }

        public AudioSessionGroupingParamChangedEventArgs(Guid newGroupingParam, Guid eventContext)
            : base(eventContext)
        {
            NewGroupingParam = newGroupingParam;
        }
    }

    /// <summary>
    /// AudioSessionStateChanged
    /// </summary>
    public class AudioSessionStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The new session state.
        /// </summary>
        public AudioSessionState NewState { get; private set; }

        public AudioSessionStateChangedEventArgs(AudioSessionState newState)
        {
            NewState = newState;
        }
    }

    /// <summary>
    /// AudioSessionDisconnectedEventArgs
    /// </summary>
    public class AudioSessionDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// The reason that the audio session was disconnected.
        /// </summary>
        public AudioSessionDisconnectReason DisconnectReason { get; private set; }

        public AudioSessionDisconnectedEventArgs(AudioSessionDisconnectReason disconnectReason)
        {
            DisconnectReason = disconnectReason;
        }
    }
}