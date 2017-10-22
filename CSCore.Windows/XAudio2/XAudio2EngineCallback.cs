using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     XAudio2EngineCallback
    /// </summary>
    [ComVisible(true)]
    public sealed class XAudio2EngineCallback : IXAudio2EngineCallback
    {
        void IXAudio2EngineCallback.OnProcessingPassStart()
        {
            EventHandler handler = this.ProcessingPassStart;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        void IXAudio2EngineCallback.OnProcessingPassEnd()
        {
            EventHandler handler = this.ProcessingPassEnd;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        void IXAudio2EngineCallback.OnCriticalError(int error)
        {
            EventHandler<XAudio2CriticalErrorEventArgs> handler = this.CriticalError;
            if (handler != null)
                handler(this, new XAudio2CriticalErrorEventArgs(error));
        }

        /// <summary>
        ///     Fired by XAudio2 just before an audio processing pass begins.
        /// </summary>
        public event EventHandler ProcessingPassStart;

        /// <summary>
        ///     Fired by XAudio2 just after an audio processing pass ends.
        /// </summary>
        public event EventHandler ProcessingPassEnd;

        /// <summary>
        ///     Fired if a critical system error occurs that requires XAudio2 to be closed down and restarted.
        /// </summary>
        public event EventHandler<XAudio2CriticalErrorEventArgs> CriticalError;
    }
}