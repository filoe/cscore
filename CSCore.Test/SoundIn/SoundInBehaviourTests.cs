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
        [TestCategory("SoundIns")]
        public void CanCaptureAudio()
        {
            int n = 0;
            SoundInTests((c) =>
            {
                for (int i = 0; i < 500; i++)
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

        [TestMethod]
        [TestCategory("SoundIns")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsInvalidCallerThread()
        {
            bool flag = true;
            SoundInTests((c) =>
            {
                c.DataAvailable += (s, e) =>
                {
                    try
                    {
                        c.Stop();
                        flag = false;
                    }
                    catch(InvalidOperationException)
                    {
                        Debug.WriteLine("Caught expected exception for " + c.GetType().FullName);
                    }
                };

                c.Initialize();
                c.Start();
            });

            if (flag)
                throw new InvalidOperationException();
        }

        [TestMethod]
        [TestCategory("SoundIns")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ThrowsObjectDisposed()
        {
            bool flag = true;
            foreach (var c in GetSoundIns())
            {
                try
                {
                    c.Initialize();
                    c.Start();
                    Thread.Sleep(200);
                    c.Dispose();
                    c.Start();
                    flag = false;
                }
                catch(ObjectDisposedException)
                {
                }
            }

            if (flag)
                throw new ObjectDisposedException(String.Empty);
        }

        private void SoundInTests(Action<ISoundIn> action)
        {
            foreach (var soundIn in GetSoundIns())
            {
                try
                {
                    action(soundIn);
                }
                finally
                {
                    soundIn.Dispose();
                }
            }
        }

        private ISoundIn[] GetSoundIns()
        {
            return new ISoundIn[] { new WasapiCapture(true, CSCore.CoreAudioAPI.AudioClientShareMode.Shared), new WasapiLoopbackCapture() };
        }
    }
}
