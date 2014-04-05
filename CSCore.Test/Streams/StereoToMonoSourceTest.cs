using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.SoundOut;
using CSCore.Codecs;
using CSCore.Streams;
using System.Threading;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class StereoToMonoSourceTest
    {
        const string testfile = @"C:\Temp\test.mp3";

        [TestMethod]
        [TestCategory("Streams")]
        public void CanPlayStereoToMonoSource()
        {
            //in order to fix workitem 3

            var source = CodecFactory.Instance.GetCodec(testfile);
            Assert.AreEqual(2, source.WaveFormat.Channels);

            var monoSource = new StereoToMonoSource(source);
            Assert.AreEqual(1, monoSource.WaveFormat.Channels);

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
