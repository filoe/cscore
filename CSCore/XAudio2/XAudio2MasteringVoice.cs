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
        public XAudio2MasteringVoice(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Gets the channel mask for this voice.
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
        ///     Returns the channel mask for this voice.
        /// </summary>
        /// <param name="channelMask">
        ///     Returns the channel mask for this voice. This corresponds to the
        ///     <see cref="WaveFormatExtensible.ChannelMask" /> member of the <see cref="WaveFormatExtensible" /> class.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelMaskNative(out ChannelMask channelMask)
        {
            fixed (void* p = &channelMask)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[19]);
            }
        }
    }
}