using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CSCore.MediaFoundation;
using System.Threading;

namespace CSCore.Test.MediaFoundation
{
    [TestClass]
    public class MediafoundationEncoderTests
    {
        const string testfile = GlobalTestConfig.testWav2s;

        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanEncodeToAAC()
        {
            string targetfilename = Path.ChangeExtension(testfile, "test.mp4");
            if (File.Exists(targetfilename))
                File.Delete(targetfilename);

            using (var source = Codecs.CodecFactory.Instance.GetCodec(testfile))
            {
                using (var encoder = MediaFoundationEncoder.CreateAACEncoder(source.WaveFormat, targetfilename))
                {
                    MediaFoundationEncoder.EncodeWholeSource(encoder, source);
                }
            }

            //cleanup
            File.Delete(targetfilename);
        }
    }
}
