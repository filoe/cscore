using System;
using CSCore.Codecs;
using CSCore.Codecs.AAC;
using CSCore.Codecs.MP3;

namespace CSCore.OSXCoreAudio
{
    /// <summary>
    ///     Utility class for the Core Audio APIs on OSX
    /// </summary>
    public static class OSXAudio
    {
        /// <summary>
        ///     Gets whether the Core Audio APIs are supported - currently just checks we are running on MacOSX
        /// </summary>
        /// <value>Whether Core Audio is supported</value>
        public static bool IsSupported { get { return Utils.PlatformDetection.RunningPlatform() == Utils.Platform.MacOSX;}}

        /// <summary>
        ///     Register the OSX Core Audio codecs to the default factory
        ///     Note that the default action for urls will require downloading the entire stream into a memory stream first
        /// </summary>
        /// <returns>The codecs.</returns>
        public static void RegisterCodecs()
        {
            RegisterCodecs(CodecFactory.Instance);
        }

        /// <summary>
        ///     Register the OSX Core Audio codecs
        ///     Note that the default action for urls will require downloading the entire stream into a memory stream first
        /// </summary>
        /// <param name="factory">The factory to register codecs to</param>
        public static void RegisterCodecs(CodecFactory factory)
        {
            if (!IsSupported) return;

            // Register MP3
            factory.Register("mp3", new CodecFactoryEntry(s => new MP3DecoderOSX(s),"mp3", "mpeg3"));

            // Register AAC
            factory.Register("aac", new CodecFactoryEntry(s => new AACDecoderOSX(s), "aac", "mp4", "m4a"));

            // Register default codec
            factory.RegisterDefaultCodec((url, isWebURL) => 
            {
                if (isWebURL) return new OSXAudioDecoder(new Uri(url));
                return new OSXAudioDecoder(url); 
            });
        }
    }
}

