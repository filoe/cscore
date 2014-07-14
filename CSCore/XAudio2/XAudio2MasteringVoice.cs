using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     A mastering voice is used to represent the audio output device.
    /// </summary>
    public class XAudio2MasteringVoice : XAudio2Voice
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2MasteringVoice" /> class.
        /// </summary>
        /// <param name="ptr">Native pointer of the <see cref="XAudio2MasteringVoice" /> object.</param>
        /// <param name="version">The <see cref="XAudio2Version"/> to use.</param>        
        public XAudio2MasteringVoice(IntPtr ptr, XAudio2Version version)
            : base(ptr, version)
        {
        }

        /// <summary>
        ///     <b>XAudio2.8 only:</b> Gets the channel mask for this voice.
        /// </summary>
        public ChannelMask ChannelMask
        {
            get
            {
                ChannelMask value;
                XAudio2Exception.Try(GetChannelMaskNative(out value), "IXAudio2MasteringVoice", "GetChannelMask");
                return value;
            }
        }

        /// <summary>
        ///     <b>XAudio2.8 only:</b> Returns the channel mask for this voice.
        /// </summary>
        /// <param name="channelMask">
        ///     Returns the channel mask for this voice. This corresponds to the
        ///     <see cref="WaveFormatExtensible.ChannelMask" /> member of the <see cref="WaveFormatExtensible" /> class.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelMaskNative(out ChannelMask channelMask)
        {
            if(Version != XAudio2Version.XAudio2_8)
                throw new InvalidOperationException("The Channelmask of a mastering voice is only available using XAudio2.8.");

            fixed (void* p = &channelMask)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p, ((void**)(*(void**)UnsafeBasePtr))[19]);
            }
        }
    }
}