using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     A submix voice is used primarily for performance improvements and effects processing.
    /// </summary>
    public class XAudio2SubmixVoice : XAudio2Voice
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2SubmixVoice" /> class.
        /// </summary>
        /// <param name="ptr">Native pointer of the <see cref="XAudio2SubmixVoice" /> object.</param>
        /// <param name="version">The <see cref="XAudio2Version"/> to use.</param>
        public XAudio2SubmixVoice(IntPtr ptr, XAudio2Version version)
            : base(ptr, version)
        {
        }
    }
}