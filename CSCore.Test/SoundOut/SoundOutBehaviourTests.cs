using System;
using CSCore.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.SoundOut;
using System.Threading;
using System.Diagnostics;
using CSCore.Streams;

namespace CSCore.Test.SoundOut
{
    /// <summary>
    /// NOTE: All these tests may throw an CoreAudioAPIException with the errorcode AUDCLNT_E_CPUUSAGE_EXCEEDED (0x88890017).
    /// Read more about it here: http://msdn.microsoft.com/en-us/library/windows/desktop/dd370875%28v=vs.85%29.aspx
    /// TODO: Fix AUDCLNT_E_CPUUSAGE_EXCEEDED
    /// </summary>
    [TestClass]
    public abstract class SoundOutBehaviourTests
    {
        const int BasicIterationCount =30;
        const int TimeOut = 60000 * 2 * BasicIterationCount;// / 50

        private ISoundOut _soundOut;
        protected abstract ISoundOut CreateSoundOut();

        [TestInitialize]
        public virtual void OnTestInitialize()
        {
            _soundOut = CreateSoundOut();
            if (_soundOut == null)
                throw new Exception("No valid soundout.");
            Debug.WriteLine(_soundOut.GetType().FullName);
        }

        [TestCleanup]
        public virtual void OnTestCleanup()
        {
            _soundOut.Dispose();
            _soundOut = null;
        }


        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        public virtual void CanReinitializeSoundOut()
        {
            using (var source = GetWaveSource())
            {
                CanReinitializeSoundOutTestInternal(_soundOut, source);
            }
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        public void CanHandleEof()
        {
            using (var source = GetWaveSource())
            {
                CanHandleEOFTestInternal(_soundOut, source);
            }
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        public void CanHandleEofOnReplay()
        {
            using (var source = GetWaveSource())
            {
                CanHandleEOFOnReplayTestInternal(_soundOut, source);
            }
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        public void PlayPauseResumeBehaviourTest()
        {
            using (var source = GetLoopingWaveSource())
            {
                PlayPauseResumeBehaviourTestInternal(_soundOut, source);
            }
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        public void VolumeResetTest()
        {
            using (var source = GetLoopingWaveSource())
            {
                for (int i = 0; i < BasicIterationCount; i++)
                    VolumeResetTestInternal(_soundOut, source);
            }
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        public void DisposeAfterConstructionTest()
        {
            _soundOut.Dispose();
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        public void PlaybackStoppedEventTest()
        {
            using (var source = GetWaveSource())
            {
                PlaybackStoppedEventTestInternal(_soundOut, source);
            }
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ThrowsObjectDisposedException()
        {
            using (var source = GetLoopingWaveSource())
            {
                _soundOut.Initialize(source);
                _soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, _soundOut.PlaybackState);
                _soundOut.Dispose();
                Assert.AreEqual(PlaybackState.Stopped, _soundOut.PlaybackState);
                _soundOut.Stop();
            }
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsInvalidCallerThread()
        {
            Exception exception = null;
            using (var source = new NotificationSource(GetLoopingWaveSource().ToSampleSource()))
            {
                using (var waitHandle = new ManualResetEvent(false))
                {
                    _soundOut.Initialize(source.ToWaveSource());
                    //the play method sometimes won't cause an InvalidOperationException
                    //for example the waveout class will read from the source which fires up the BlockRead event BUT not on the playbackthread.
                    _soundOut.Play();
                    source.BlockRead += (s, e) =>
                    {
                        try
                        {
                            _soundOut.Pause(); //hopefully throws InvalidOperationException
                        }
                        catch (InvalidOperationException ex)
                        {
                            exception = ex;
                        }
                        finally
                        {
// ReSharper disable once AccessToDisposedClosure
                            waitHandle.Set();
                        }
                    };
                    waitHandle.WaitOne();

                    _soundOut.Stop();
                }
            }
            if (exception != null)
                throw exception;
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        public void SoundOutBehaviour()
        {
            using (var source = new DisposableSource(GetLoopingWaveSource()))
            {
                Assert.AreEqual(PlaybackState.Stopped, _soundOut.PlaybackState);
                _soundOut.Initialize(source);
                Assert.AreEqual(PlaybackState.Stopped, _soundOut.PlaybackState);
                _soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, _soundOut.PlaybackState);
                Thread.Sleep(20);
                _soundOut.Pause();
                Assert.AreEqual(PlaybackState.Paused, _soundOut.PlaybackState);
                Thread.Sleep(20);
                _soundOut.Resume();
                Assert.AreEqual(PlaybackState.Playing, _soundOut.PlaybackState);
                Thread.Sleep(20);
                _soundOut.Dispose();

                Assert.IsFalse(source.IsDisposed, "{0} disposed source.", _soundOut.GetType().FullName);
            }
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        public void FastPlayStopSoundOutTest()
        {
            using (var source = GetLoopingWaveSource())
            {
                PlayStopTestInternal(_soundOut, source);
            }
        }

        [TestMethod]
        [TestCategory("SoundOuts")]
        [Timeout(2 * TimeOut)]
        public void DontTouchWaveSourceTest()
        {
            using (var source = new DisposableSource(GetWaveSource()))
            {
                _soundOut.Initialize(source);
                _soundOut.Play();

                Thread.Sleep(100);

                Assert.AreEqual(PlaybackState.Playing, _soundOut.PlaybackState);

                _soundOut.Dispose();

                Assert.IsFalse(source.IsDisposed);
            }
        }

        private void CanHandleEOFOnReplayTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            soundOut.Initialize(source);
            soundOut.Stopped += (s, e) => waitHandle.Set();

            for (int i = 0; i < BasicIterationCount; i++)
            {
                soundOut.Play();
                waitHandle.WaitOne();
                Assert.AreEqual(source.Length, source.Position, "Source is not EOF");
                Assert.AreEqual(PlaybackState.Stopped, soundOut.PlaybackState);
                source.Position = 0;
            }
        }

        private void CanReinitializeSoundOutTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            for (int i = 0; i < BasicIterationCount; i++)
            {
                Debug.WriteLine(soundOut.ToString());

                soundOut.Initialize(source);
                soundOut.Play();

                Thread.Sleep(100);

                soundOut.Stop();
                source.Position = 0;
            }
        }

        private void CanHandleEOFTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            int sourceLength = (int)source.GetLength().TotalMilliseconds;
            Debug.WriteLine(soundOut.GetType().FullName);
            for (int i = 0; i < BasicIterationCount; i++)
            {
                soundOut.Initialize(source);
                soundOut.Play();

                Thread.Sleep(sourceLength + 500);
                Assert.AreEqual(source.Length, source.Position, "Source is not EOF");

                soundOut.Stop();
                source.Position = 0;

                soundOut.Initialize(source);
                soundOut.Play();

                Thread.Sleep(sourceLength + 500);
                Assert.AreEqual(source.Length, source.Position, "Source is not EOF");

                soundOut.Pause();
                soundOut.Resume();

                Thread.Sleep(10);

                soundOut.Stop();
                source.Position = 0;

                soundOut.Initialize(source);
                soundOut.Play();

                Thread.Sleep(sourceLength + 1000);
                Assert.AreEqual(source.Length, source.Position, "Source is not EOF");
                Assert.AreEqual(PlaybackState.Stopped, soundOut.PlaybackState);

                source.Position = 0;
                soundOut.Play();

                Thread.Sleep(sourceLength + 500);
                Assert.AreEqual(source.Length, source.Position, "Source is not EOF");
            }
        }

        private void PlayPauseResumeBehaviourTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            for (int i = 0; i < BasicIterationCount; i++)
            {
                soundOut.Initialize(source);
                //--

                Assert.AreEqual(PlaybackState.Stopped, soundOut.PlaybackState);

                soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);

                Thread.Sleep(50);

                soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);

                soundOut.Pause();
                Assert.AreEqual(PlaybackState.Paused, soundOut.PlaybackState);

                soundOut.Pause();
                Assert.AreEqual(PlaybackState.Paused, soundOut.PlaybackState);

                soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);

                soundOut.Pause();
                Assert.AreEqual(PlaybackState.Paused, soundOut.PlaybackState);

                soundOut.Resume();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);
                //--
                Thread.Sleep(250);
                //--
                soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);

                Thread.Sleep(50);

                soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);

                soundOut.Pause();
                Assert.AreEqual(PlaybackState.Paused, soundOut.PlaybackState);
                soundOut.Pause();
                Assert.AreEqual(PlaybackState.Paused, soundOut.PlaybackState);

                soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);

                soundOut.Pause();
                Assert.AreEqual(PlaybackState.Paused, soundOut.PlaybackState);

                soundOut.Resume();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);
                //--

                soundOut.Stop();
                Assert.AreEqual(PlaybackState.Stopped, soundOut.PlaybackState);

                soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);

                Thread.Sleep(10);

                soundOut.Stop();
                Assert.AreEqual(PlaybackState.Stopped, soundOut.PlaybackState);

                soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);

                soundOut.Pause();
                Assert.AreEqual(PlaybackState.Paused, soundOut.PlaybackState);

                Thread.Sleep(50);

                //--
                soundOut.Stop();
                Assert.AreEqual(PlaybackState.Stopped, soundOut.PlaybackState);
                source.Position = 0;
            }
        }

        private void VolumeResetTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            soundOut.Initialize(source);
            soundOut.Play();
            soundOut.Volume = 0.5f;
            Assert.AreEqual(0.5f, Math.Round(soundOut.Volume, 3)); //round => directsound may causes problems because of db => lin calculations.
            Thread.Sleep(50);
            soundOut.Stop();
            soundOut.Initialize(source);
            soundOut.Play();
            Assert.AreEqual(1.0f, soundOut.Volume);
            soundOut.Stop();
        }

        private void PlaybackStoppedEventTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            for (int i = 0; i < BasicIterationCount; i++)
            {
                bool raised = false;
                soundOut.Stopped += (s, e) => raised = true;

                //1. wait for the event on end of stream
                soundOut.Initialize(source);
                soundOut.Play();

                Thread.Sleep((int)(source.GetLength().TotalMilliseconds + 50));
                while (soundOut.PlaybackState != PlaybackState.Stopped) 
                    Thread.Sleep(10);

                soundOut.Initialize(source); //the playbackstate may be stopped but event was not fired. initialize will wait until it gets fired.

                source.Position = 0;
                Assert.IsTrue(raised);
                raised = false;

                //2. event on Stop()
                soundOut.Play();

                Thread.Sleep(50);
                soundOut.Stop();

                Assert.IsTrue(raised);
            }
        }

        private void PlayStopTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            for (int i = 0; i < BasicIterationCount; i++)
            {
                soundOut.Initialize(source);
                soundOut.Play();
                soundOut.Stop();
                soundOut.Initialize(source);
                soundOut.Play();
                soundOut.Stop();

                soundOut.Initialize(source);
                soundOut.Play();
                soundOut.Stop();
                soundOut.Initialize(source);
                soundOut.Play();
                soundOut.Stop();

                soundOut.Initialize(source);
                soundOut.Play();
                soundOut.Stop();
                soundOut.Initialize(source);
                soundOut.Play();
                soundOut.Stop();
            }
        }

        private IWaveSource GetWaveSource()
        {
            return GlobalTestConfig.TestWav2S();
        }

        private IWaveSource GetLoopingWaveSource()
        {
            return new LoopStream(GetWaveSource());
        }
    }
}
