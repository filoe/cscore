using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.Codecs;
using CSCore.SoundOut;
using System.Threading;
using CSCore.Streams;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class MonoToStereoSourceTest
    {
        const string testfile = @"C:\Temp\test.mp3";

        [TestMethod] 
        [TestCategory("Streams")]
        public void CanPlayMonoToStereoSourceTest()
        {
            var source = new StereoToMonoSource(CodecFactory.Instance.GetCodec(testfile));
            Assert.AreEqual(1, source.WaveFormat.Channels);

            var monoSource = new MonoToStereoSource(source);
            Assert.AreEqual(2, monoSource.WaveFormat.Channels);

            ISoundOut soundOut;
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                soundOut = new WasapiOut();
            else
                soundOut = new DirectSoundOut();

            soundOut.Initialize(monoSource.ToWaveSource(16));
            soundOut.Play();

            Thread.Sleep((int)Math.Min(source.GetMilliseconds(source.Length), 60000));

            soundOut.Dispose();
        }
    }
}
