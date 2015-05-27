using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSCore.Codecs;

namespace CSCore.Test
{
    public static class GlobalTestConfig
    {
        //public const string TestMp3 = @"D:\Temp\test_120s_stereo.mp3";
        public const string _TestMp3 = @"D:\Temp\test_120s.mp3";
        public const string _TestWav2S = @"D:\Temp\test_2s.wav";

        public static IWaveSource TestMp3()
        {
            return CodecFactory.Instance.GetCodec(TestMp3AsStream(), "mp3");
        }

        public static MemoryStream TestMp3AsStream()
        {
            return new MemoryStream(Properties.Resources.test_120s_stereo);
        }

        public static IWaveSource TestWav2S()
        {
            return CodecFactory.Instance.GetCodec(Properties.Resources.test_2s, "wave");
        }
    }
}
