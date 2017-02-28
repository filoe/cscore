using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

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
            {
                var sessionControl = new AudioSessionControl(newSession);
                //make sure, the reference can be used within our application
                //in order to prevent the instance from being released, add a reference
                //if it won't be used in our application (eventhandlers and so on), the
                //destructor will release the instance.
                ((IUnknown) sessionControl).AddRef(); 
                SessionCreated(this, new SessionCreatedEventArgs(sessionControl));
            }
            return (int) HResult.S_OK;
        }
    }
}