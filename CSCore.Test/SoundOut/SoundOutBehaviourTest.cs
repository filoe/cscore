using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.SoundOut;
using System.Threading;
using CSCore.Codecs;
using System.Diagnostics;
using CSCore.Streams;

namespace CSCore.Test.SoundOut
{
    /// <summary>
    /// NOTE: All these tests may throw an CoreAudioAPIException with the errorcode AUDCLNT_E_CPUUSAGE_EXCEEDED (0x88890017).
    /// Why that happens, you can read here: http://msdn.microsoft.com/en-us/library/windows/desktop/dd370875%28v=vs.85%29.aspx
    /// TODO: Fix AUDCLNT_E_CPUUSAGE_EXCEEDED
    /// </summary>
    [TestClass]
    public class SoundOutBehaviourTest
    {
        const int basic_iteration_count = 100;
        const int MinuteMilliseconds = 60000;

        [TestMethod]
        [Timeout(2 * MinuteMilliseconds)]
        public void CanReinitializeSoundOut()
        {
            SoundOutTests((soundOut, source) =>
            {
                CanReinitializeSoundOutTestInternal(soundOut, source);
            });
        }

        [TestMethod]
        [Timeout(2 * MinuteMilliseconds)]
        public void CanHandleEOF()
        {
            SoundOutTests((soundOut, source) =>
            {
                CanHandleEOFTestInternal(soundOut, source);
            });
        }

        [TestMethod]
        public void PlayPauseResumeBehaviourTest()
        {
            SoundOutTests((soundOut, source) =>
            {
                PlayPauseResumeBehaviourTestInternal(soundOut, source);
            }, true);
        }

        [TestMethod]
        [Timeout(2 * MinuteMilliseconds)]
        public void VolumeResetTest()
        {
            SoundOutTests((soundOut, source) =>
            {
                VolumeResetTestInternal(soundOut, source);
            });
        }

        [TestMethod]
        [Timeout(2 * MinuteMilliseconds)]
        public void PlaybackStoppedEventTest()
        {
            SoundOutTests((soundOut, source) =>
            {
                PlaybackStoppedEventTestInternal(soundOut, source);
            });
        }

        [TestMethod]
        [Timeout(2 * MinuteMilliseconds)]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ThrowsObjectDisposedException()
        {
            foreach (var soundOut in GetSoundOuts())
            {
                soundOut.Initialize(new SineGenerator().ToWaveSource(16));
                soundOut.Play();
                Assert.AreEqual(PlaybackState.Playing, soundOut.PlaybackState);
                soundOut.Dispose();
                Assert.AreEqual(PlaybackState.Stopped, soundOut.PlaybackState);
                soundOut.Stop();
            }
        }

        [TestMethod]
        [Timeout(2 * MinuteMilliseconds)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsInvalidCallerThread()
        {
            bool flag = true;
            foreach (var soundOut in GetSoundOuts())
            {
                var source = new DSP.FFTAggregator(new SineGenerator().ToWaveSource(16));
                source.FFTCalculated += (s, e) =>
                {
                    try
                    {
                        soundOut.Pause(); //hopefully throws exception
                    }
                    catch (InvalidOperationException)
                    {
                        Debug.WriteLine("Invalid behaviour in {0}.", soundOut.GetType().FullName);
                        flag &= false;
                    }
                };
                soundOut.Initialize(source);
                soundOut.Play();

                Thread.Sleep(200);

                soundOut.Stop();

                soundOut.Dispose();
            }

            if (!flag)
                throw new InvalidOperationException("InvalidOperationException was thrown inside audio render queue");
        }

        [TestMethod]
        [Timeout(2 * MinuteMilliseconds)]
        public void SoundOutSourceBehaviour()
        {
            foreach (var soundOut in GetSoundOuts())
            {
                var source = GetWaveSource();
                source = new Utils.DisposableSource(source);

                soundOut.Initialize(source);
                soundOut.Play();
                Thread.Sleep(20);
                soundOut.Pause();
                Thread.Sleep(20);
                soundOut.Resume();
                Thread.Sleep(20);
                soundOut.Dispose();

                Assert.IsFalse(((Utils.DisposableSource)source).IsDisposed, "{0} disposed source.", soundOut.GetType().FullName);
            }
        }

        private void SoundOutTests(Action<ISoundOut, IWaveSource> action, bool loopStream = false)
        {
            IWaveSource source = GetWaveSource();
            if (loopStream)
            {
                if (!(source is LoopStream))
                    source = new LoopStream(source);
            }

            ISoundOut[] soundOuts = GetSoundOuts();
            foreach (var soundOut in soundOuts)
            {
                action(soundOut, source);
                soundOut.Dispose();
            }

            source.Dispose();
        }

        private ISoundOut[] GetSoundOuts()
        {
            return new ISoundOut[]{ new WasapiOut(), new DirectSoundOut() };
        }

        private IWaveSource GetWaveSource()
        {
            return CodecFactory.Instance.GetCodec(@"C:\Temp\200msSin.wav");
        }

        private void CanReinitializeSoundOutTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            for (int i = 0; i < basic_iteration_count; i++)
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
            for (int i = 0; i < basic_iteration_count; i++)
            {
                soundOut.Initialize(source);
                soundOut.Play();

                Thread.Sleep(250);

                soundOut.Stop();
                source.Position = 0;

                soundOut.Initialize(source);
                soundOut.Play();

                Thread.Sleep(250);

                soundOut.Pause();
                soundOut.Resume();

                Thread.Sleep(10);

                soundOut.Stop();
            }
        }

        private void PlayPauseResumeBehaviourTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            for (int i = 0; i < basic_iteration_count; i++)
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
                source.Position = 0;
            }
        }

        private void VolumeResetTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            source = new CSCore.Streams.LoopStream(source);

            soundOut.Initialize(source);
            soundOut.Play();
            soundOut.Volume = 0.5f;
            Assert.AreEqual(0.5f, Math.Round(soundOut.Volume, 3)); //round => directsound may causes problems because of db => lin calculations.
            Thread.Sleep(50);
            soundOut.Stop();
            soundOut.Initialize(source);
            soundOut.Play();
            Assert.AreEqual(1.0f, soundOut.Volume);
        }

        private void PlaybackStoppedEventTestInternal(ISoundOut soundOut, IWaveSource source)
        {
            for (int i = 0; i < 100; i++)
            {
                bool raised = false;
                soundOut.Stopped += (s, e) => raised = true;

                soundOut.Initialize(source);
                soundOut.Play();

                Thread.Sleep((int)(source.GetLength().TotalMilliseconds + 50));
                while (soundOut.PlaybackState != PlaybackState.Stopped) ;

                soundOut.Initialize(source); //the playbackstate may be stopped but event was not fired. initialize will wait until it gets fired.

                source.Position = 0;
                Assert.IsTrue(raised);
                raised = false; //check for event on eof

                soundOut.Play();

                Thread.Sleep(50);
                soundOut.Stop();

                Assert.IsTrue(raised);
            }
        }
    }
}
