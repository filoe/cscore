using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     XAudio2CriticalErrorEventArgs
    /// </summary>
    public class XAudio2CriticalErrorEventArgs : EventArgs
    {
        private readonly int _hresult;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2CriticalErrorEventArgs" /> class.
        /// </summary>
        /// <param name="hresult">Errorcode</param>
        public XAudio2CriticalErrorEventArgs(int hresult)
        {
            _hresult = hresult;
        }

        /// <summary>
        ///     Errorcode
        /// </summary>
        public int HResult
        {
            get { return _hresult; }
        }
    }
}