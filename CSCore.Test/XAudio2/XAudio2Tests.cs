using System;
using System.Diagnostics;
using System.Threading;
using CSCore.Codecs;
using CSCore.XAudio2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.XAudio2
{
    [TestClass]
    public class XAudio28Tests : XAudio2Tests
    {
        protected override CSCore.XAudio2.XAudio2 CreateXAudio2()
        {
            return new XAudio2_8();
        }
    }

    [TestClass]
    public class XAudio27Tests : XAudio2Tests
    {
        protected override CSCore.XAudio2.XAudio2 CreateXAudio2()
        {
            return new XAudio2_7();
        }
    }

    [TestClass]
    public abstract class XAudio2Tests
    {
        private IWaveSource _source;
        private CSCore.XAudio2.XAudio2 _xaudio2;

        [TestInitialize]
        public void Initialize()
        {
            _source = GlobalTestConfig.TestMp3();
            _xaudio2 = CreateXAudio2();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _source.Dispose();
            _xaudio2.Dispose();
        }

        [TestMethod, TestCategory("XAudio2")]
        public void CanCreateXAudio2()
        {
            //Work is done by Initialize and Cleanup method
        }

        [TestMethod, TestCategory("XAudio2")]
        public void CanCreateMasteringVoice()
        {
            using (var masteringVoice = _xaudio2.CreateMasteringVoice())
            {
            }
        }

        [TestMethod, TestCategory("XAudio2")]
        public void CanCreateSourceVoice()
        {
            using (var masteringVoice = _xaudio2.CreateMasteringVoice())
            using (var sourceVoice = _xaudio2.CreateSourceVoice(_source.WaveFormat))
            {
            }
        }

        [TestMethod, TestCategory("XAudio2")]
        public void CanSubmitSourceBuffer()
        {
            const int lengthInSeconds = 2;

            using (var masteringVoice = _xaudio2.CreateMasteringVoice())
            using (var sourceVoice = _xaudio2.CreateSourceVoice(_source.WaveFormat))
            {
                byte[] rawBuffer = new byte[_source.WaveFormat.BytesPerSecond * lengthInSeconds];
                int read = _source.Read(rawBuffer, 0, rawBuffer.Length);

                var buffer = new XAudio2Buffer(read);
                using (var stream = buffer.GetStream())
                {
                    stream.Write(rawBuffer, 0, read);
                }

                sourceVoice.SubmitSourceBuffer(buffer);                
                sourceVoice.Start();

                Thread.Sleep(lengthInSeconds * 1000 + 500);
            }
        }

        [TestMethod, TestCategory("XAudio2")]
        public void CanRegisterSourceVoiceCallback()
        {
            const int lengthInSeconds = 2;
            bool b0 = false, b1 = false;

            var callback = new VoiceCallback();

            using (var masteringVoice = _xaudio2.CreateMasteringVoice())
            using (var sourceVoice = _xaudio2.CreateSourceVoice(_source.WaveFormat, VoiceFlags.None, CSCore.XAudio2.XAudio2.DefaultFrequencyRatio, callback, null, null))
            {
                callback.BufferStart += (s,e) => b0 = true;
                callback.BufferEnd += (s,e) => b1 = true;

                byte[] rawBuffer = new byte[_source.WaveFormat.BytesPerSecond * lengthInSeconds];
                int read = _source.Read(rawBuffer, 0, rawBuffer.Length);

                var buffer = new XAudio2Buffer(read);
                using (var stream = buffer.GetStream())
                {
                    stream.Write(rawBuffer, 0, read);
                }

                sourceVoice.SubmitSourceBuffer(buffer);                
                sourceVoice.Start();

                Thread.Sleep(lengthInSeconds * 1000 + 2000);

                Assert.IsTrue(b0, "No BufferStart.");
                Assert.IsTrue(b1, "No BufferEnd.");
            }
        }

        [TestMethod, TestCategory("XAudio2")]
        public void CanRegisterEngineCallback()
        {
            const int lengthInSeconds = 2;
            bool b0 = false, b1 = false;

            XAudio2EngineCallback engineCallback = new XAudio2EngineCallback();
            engineCallback.ProcessingPassStart += (s, e) =>
                b0 = true;
            engineCallback.ProcessingPassEnd += (s, e) => 
                b1 = true;

            _xaudio2.RegisterForCallbacks(engineCallback);

            using (var masteringVoice = _xaudio2.CreateMasteringVoice())
            using (var sourceVoice = _xaudio2.CreateSourceVoice(_source.WaveFormat))
            {
                byte[] rawBuffer = new byte[_source.WaveFormat.BytesPerSecond * lengthInSeconds];
                int read = _source.Read(rawBuffer, 0, rawBuffer.Length);

                var buffer = new XAudio2Buffer(read);
                using (var stream = buffer.GetStream())
                {
                    stream.Write(rawBuffer, 0, read);
                }

                sourceVoice.SubmitSourceBuffer(buffer);
                sourceVoice.Start();

                Thread.Sleep((lengthInSeconds * 1000 + 500) / 2);

                Assert.IsTrue(b0 || b1);

                Console.WriteLine("Unregistered");
                _xaudio2.UnregisterForCallbacks(engineCallback);
                b0 = b1 = false;

                Thread.Sleep((lengthInSeconds * 1000 + 500) / 4);

                Assert.IsFalse(b0 && b1);

                Console.WriteLine("Register");
                _xaudio2.RegisterForCallbacks(engineCallback);

                Thread.Sleep((lengthInSeconds * 1000 + 500) / 4);

                Assert.IsTrue(b0 || b1);
            }

            _xaudio2.UnregisterForCallbacks(engineCallback);
        }

        [TestMethod, TestCategory("XAudio2")]
        public void CanPlayWithStreamingSourceVoice()
        {
            for (int i = 0; i < 20; i++)
            {
                using (var xaudio2 = new XAudio2_7())
                {
                    xaudio2.StartEngine();

                    using (var masteringVoice = xaudio2.CreateMasteringVoice())
                    using (var source = GlobalTestConfig.TestWav2S())
                    using (var pool = new StreamingSourceVoiceListener())
                    using (var streamingSourceVoice = StreamingSourceVoice.Create(xaudio2, source))
                    {
                        var stoppedEvent = new ManualResetEvent(false);
                        streamingSourceVoice.Stopped += (s, e) =>
                            stoppedEvent.Set();

                        streamingSourceVoice.Start();

                        pool.Add(streamingSourceVoice);

                        Debug.WriteLine("All queued.");

                        stoppedEvent.WaitOne();

                        pool.Remove(streamingSourceVoice);
                    }

                    xaudio2.StopEngine();
                }

                Debug.WriteLine("All removed.");
            }
        }

        [TestMethod, TestCategory("XAudio2")]
        public void CanSetEffect()
        {
            //TODO: Implement
        }

        [TestMethod, TestCategory("XAudio2")]
        public void CanGetEffect()
        {
            //TODO: Implement
        }

        protected abstract CSCore.XAudio2.XAudio2 CreateXAudio2();
    }
}
