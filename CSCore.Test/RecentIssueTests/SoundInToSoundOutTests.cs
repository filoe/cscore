using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.SoundOut;
using System.Threading;

namespace CSCore.Test.RecentIssueTests
{
    [TestClass]
    public class SoundInToSoundOutTests
    {
        [TestMethod]
        public void SoundInToSoundOutTest_Wasapi()
        {
            for (int i = 0; i < 10; i++)
            {
                var waveIn = new WasapiCapture();
                waveIn.Initialize();
                waveIn.Start();

                var waveInToSource = new SoundInSource(waveIn) { FillWithZeros = true };

                var soundOut = new WasapiOut();
                soundOut.Initialize(waveInToSource);
                soundOut.Play();

                Thread.Sleep(2000);

                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);

                soundOut.Dispose();
                waveIn.Dispose();
            }
        }
    }
}
