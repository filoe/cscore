using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="AudioSessionNotification"/> object provides notification when an audio session is created.
    /// For more information, <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370969(v=vs.85).aspx"/>.
    /// </summary>
    [Guid("641DD20B-4D41-49CC-ABA3-174B9477BB08")]
    public sealed class AudioSessionNotification : IAudioSessionNotification
    {
        /// <summary>
        /// Occurs when the audio session has been created.
        /// </summary>
        public event EventHandler<SessionCreatedEventArgs> SessionCreated;

        /// <summary>
        /// Notifies the registered processes that the audio session has been created.
        /// </summary>
        /// <param name="newSession">Pointer to the <see cref="AudioSessionControl"/> object of the audio session that was created.</param>
        /// <returns>HRESULT</returns>
        int IAudioSessionNotification.OnSessionCreated([In] IntPtr newSession)
        {
            if (SessionCreated != null)
                SessionCreated(this, new SessionCreatedEventArgs(new AudioSessionControl(newSession)));
            return (int) Win32.HResult.S_OK;
        }
    }
}