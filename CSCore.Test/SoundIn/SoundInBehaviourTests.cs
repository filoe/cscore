using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.SoundIn;
using System.Threading;
using System.Diagnostics;

namespace CSCore.Test.SoundIn
{
    [TestClass]
    public class SoundInBehaviourTests
    {
        [TestMethod]
        public void CanCaptureAudio()
        {
            int n = 0;
            SoundInTests((c) =>
            {
                for (int i = 0; i < 5000; i++)
                {
                    var waitHandle = new AutoResetEvent(true);

                    c.DataAvailable += (s, e) =>
                    {
                        waitHandle.Reset();
                    };

                    c.Initialize();
                    c.Start();

                    if (!waitHandle.WaitOne(2000))
                        Assert.Fail("Timeout");
                    else
                    {
                        Debug.WriteLine(n.ToString());
                        n++;
                    }

                    c.Stop();

                    waitHandle.Dispose();
                }
            });
        }

        private void SoundInTests(Action<ISoundIn> action)
        {
            foreach (var soundIn in GetSoundIns())
            {
                action(soundIn);
                soundIn.Dispose();
            }
        }

        private ISoundIn[] GetSoundIns()
        {
            return new ISoundIn[] { new WasapiCapture(true, CSCore.CoreAudioAPI.AudioClientShareMode.Shared), new WasapiLoopbackCapture() };
        }
    }
}
