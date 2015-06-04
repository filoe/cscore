using System;
using CSCore.CoreAudioAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.SoundIn;
using System.Threading;
using System.Diagnostics;

namespace CSCore.Test.SoundIn
{
    [TestClass]
    public abstract class SoundInBehaviourTests
    {
        private ISoundIn _soundIn;
        protected abstract ISoundIn CreateSoundIn();

        [TestInitialize]
        public virtual void OnTestInitialize()
        {
            _soundIn = CreateSoundIn();
            if (_soundIn == null)
                throw new Exception("No valid soundin.");
            Debug.WriteLine(_soundIn.GetType().FullName);
        }

        [TestCleanup]
        public virtual void OnTestCleanup()
        {
            _soundIn.Dispose();
            _soundIn = null;
        }

        [TestMethod]
        [TestCategory("SoundIns")]
        public virtual void CanCaptureAudio()
        {
            const int runs = 10;
            int i = 0;
            using (var waitHandle = new AutoResetEvent(false))
            {
                for (; i < runs; i++)
                {
                    _soundIn.DataAvailable += (s, e) =>
                    {
// ReSharper disable once AccessToDisposedClosure
                        waitHandle.Set();
                    };

                    _soundIn.Initialize();
                    Assert.AreEqual(RecordingState.Stopped, _soundIn.RecordingState);

                    _soundIn.Start();
                    Assert.AreEqual(RecordingState.Recording, _soundIn.RecordingState);

                    if (!waitHandle.WaitOne(Debugger.IsAttached ? Timeout.Infinite : 2000))
                        Assert.Fail("Timeout");

                    _soundIn.Stop();
                    Assert.AreEqual(RecordingState.Stopped, _soundIn.RecordingState);
                }
            }

            Assert.AreEqual(runs, i);
        }

        [TestMethod]
        [TestCategory("SoundIns")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsInvalidCallerThread()
        {
            Exception exception = null;
            using (var waitHandle = new AutoResetEvent(false))
            {
                _soundIn.DataAvailable += (s, e) =>
                {
                    try
                    {
                        _soundIn.Stop();
                    }
                    catch (InvalidOperationException ex)
                    {
                        Debug.WriteLine("Caught expected exception for " + _soundIn.GetType().FullName);
                        exception = ex;
                    }
                    finally
                    {
// ReSharper disable once AccessToDisposedClosure
                        waitHandle.Set();
                    }
                };

                _soundIn.Initialize();
                _soundIn.Start();
                waitHandle.WaitOne();
            }
            if (exception != null)
                throw exception;
        }

        [TestMethod]
        [TestCategory("SoundIns")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ThrowsObjectDisposed()
        {
            _soundIn.Initialize();
            _soundIn.Start();
            Thread.Sleep(200);
            _soundIn.Dispose();
            _soundIn.Start();
        }
    }
}
