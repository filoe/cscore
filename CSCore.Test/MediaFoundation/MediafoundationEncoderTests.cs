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
        const string testfile = @"C:\Temp\test.mp3";

        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanEncodeToMP3()
        {
            string targetfilename = Path.ChangeExtension(testfile, "test.mp3");
            if (File.Exists(targetfilename))
                File.Delete(targetfilename);

            using (var source = Codecs.CodecFactory.Instance.GetCodec(testfile))
            {
                using (var encoder = MediaFoundationEncoder.CreateMP3Encoder(source.WaveFormat, targetfilename))
                {
                    MediaFoundationEncoder.EncodeWholeSource(encoder, source);
                    //Thread.Sleep(5000);
                }
            }
        }

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
        }

        //TODO: Why does this work, but crashes test-executionengine??
        //[TestMethod]
        //[TestCategory("MediaFoundation")]
        //public void CanEncodeToWMA()
        //{
        //    string targetfilename = Path.ChangeExtension(testfile, "test.wma");
        //    if (File.Exists(targetfilename))
        //        File.Delete(targetfilename);

        //    using (var source = Codecs.CodecFactory.Instance.GetCodec(testfile))
        //    {
        //        using (var encoder = MediaFoundationEncoder.CreateWMAEncoder(source.WaveFormat, targetfilename))
        //        {
        //            MediaFoundationEncoder.EncodeWholeSource(encoder, source);
        //        }
        //    }
        //}
    }
}
