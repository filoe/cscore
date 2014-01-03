using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.SoundIn;

namespace CSCore.Test.SoundIn
{
    [TestClass]
    public class SoundInBehaviourTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            
        }

        private void SoundInTests(Action<ISoundIn> action)
        {
            foreach (var soundIn in GetSoundIns())
            {
                action(soundIn);
            }
        }

        private ISoundIn[] GetSoundIns()
        {
            return new ISoundIn[] { new WaveIn(), new WasapiCapture(true, CSCore.CoreAudioAPI.AudioClientShareMode.Shared) };
        }
    }
}
