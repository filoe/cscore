using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSCore.Codecs;

namespace CSCore.OSX.Test
{
    public static class GlobalTestConfig
    {
        //public const string TestMp3 = @"D:\Temp\test_120s_stereo.mp3";
        public const string _TestMp3 = @"D:\Temp\test_120s.mp3";
        public const string _TestWav2S = @"D:\Temp\test_2s.wav";

        public static MemoryStream TestMp3AsStream()
        {
            return new MemoryStream(Properties.Resources.test_120s_stereo);
        }
    }
}
