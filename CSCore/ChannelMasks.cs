using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore
{

    /// <summary>
    /// Defines common channelmasks.
    /// </summary>
    public static class ChannelMasks
    {
        /// <summary>
        /// Mono.
        /// </summary>
        public const ChannelMask MonoMask = ChannelMask.SpeakerFrontCenter;
        /// <summary>
        /// Stereo.
        /// </summary>
        public const ChannelMask StereoMask = ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight;
        /// <summary>
        /// 5.1 surround with rear speakers.
        /// </summary>
        public const ChannelMask FiveDotOneWithRearMask = ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight | ChannelMask.SpeakerFrontCenter | ChannelMask.SpeakerLowFrequency | ChannelMask.SpeakerBackLeft | ChannelMask.SpeakerBackRight;
        /// <summary>
        /// 5.1 surround with side speakers.
        /// </summary>
        public const ChannelMask FiveDotOneWithSideMask = ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight | ChannelMask.SpeakerFrontCenter | ChannelMask.SpeakerLowFrequency | ChannelMask.SpeakerSideLeft | ChannelMask.SpeakerSideRight;
        /// <summary>
        /// 7.1 surround.
        /// </summary>
        public const ChannelMask SevenDotOneMask = ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight | ChannelMask.SpeakerFrontCenter | ChannelMask.SpeakerLowFrequency | ChannelMask.SpeakerSideLeft | ChannelMask.SpeakerSideRight | ChannelMask.SpeakerBackLeft | ChannelMask.SpeakerBackRight;
    }
}
