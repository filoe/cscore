using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="IAudioSessionEvents"/> interface provides notifications of session-related events such as changes in the volume level, display name, and session state.
    /// </summary>
    [ComImport]
    [Guid("24918ACC-64B3-37C1-8CA9-74A66E9957A8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IAudioSessionEvents
    {
        /// <summary>
        /// Notifies the client that the display name for the session has changed.
        /// </summary>
        /// <param name="newDisplayName">The new display name for the session. </param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns>HRESULT</returns>
        int OnDisplayNameChanged([In, MarshalAs(UnmanagedType.LPWStr)] string newDisplayName, [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the display icon for the session has changed.
        /// </summary>
        /// <param name="newIconPath">The path for the new display icon for the session.</param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns>HRESULT</returns>
        int OnIconPathChanged([In, MarshalAs(UnmanagedType.LPWStr)] string newIconPath, [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the volume level or muting state of the audio session has changed.
        /// </summary>
        /// <param name="newVolume">The new volume level for the audio session. This parameter is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume (no attenuation).</param>
        /// <param name="newMute">The new muting state. If TRUE, muting is enabled. If FALSE, muting is disabled.</param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns>HRESULT</returns>
        int OnSimpleVolumeChanged([In] float newVolume, [In, MarshalAs(UnmanagedType.Bool)] bool newMute,
            [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the volume level of an audio channel in the session submix has changed.
        /// </summary>
        /// <param name="channelCount">The number of channels in the session submix.</param>
        /// <param name="newChannelVolumeArray">An array of volume levels. Each element is a value of type float that specifies the volume level for a particular channel. Each volume level is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume (no attenuation). The number of elements in the array is specified by the ChannelCount parameter.</param>
        /// <param name="changedChannel">The number of the channel whose volume level changed.</param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns>HRESULT</returns>
        int OnChannelVolumeChanged([In] int channelCount, [In] float[] newChannelVolumeArray, [In] int changedChannel,
            [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the grouping parameter for the session has changed.
        /// </summary>
        /// <param name="newGroupingParam">The new grouping parameter for the session. This parameter points to a grouping-parameter GUID.</param>
        /// <param name="eventContext">The event context value.</param>
        /// <returns>HRESULT</returns>
        int OnGroupingParamChanged([In] ref Guid newGroupingParam, [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the stream-activity state of the session has changed.
        /// </summary>
        /// <param name="newState">The new session state.</param>
        /// <returns>HRESULT</returns>
        int OnStateChanged([In] AudioSessionState newState);

        /// <summary>
        /// Notifies the client that the audio session has been disconnected.
        /// </summary>
        /// <param name="disconnectReason">The reason that the audio session was disconnected.</param>
        /// <returns>HRESULT</returns>
        int OnSessionDisconnected([In] AudioSessionDisconnectReason disconnectReason);
    }
}