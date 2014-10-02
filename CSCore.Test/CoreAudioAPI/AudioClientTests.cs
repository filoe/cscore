using CSCore.CoreAudioAPI;
using CSCore.DMO;
using CSCore.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSCore.Test.CoreAudioAPI
{
    [TestClass]
    public class AudioClientTests
    {
        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void GetMixFormat()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                Debug.WriteLine(audioClient.GetMixFormat().ToString());
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void InitializeSharedMode()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void InitializeExlusiveMode()
        {
            WaveFormat waveFormat = new WaveFormat();
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Exclusive, AudioClientStreamFlags.None, 1000, 0, waveFormat, Guid.Empty);
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateAudioClient()
        {
            Utils.CreateDefaultRenderClient().Dispose();
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateRenderClient()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                using (var renderClient = AudioRenderClient.FromAudioClient(audioClient))
                {
                    Assert.IsNotNull(renderClient);
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetPadding()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                audioClient.GetCurrentPadding();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetBufferSize()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                audioClient.GetBufferSize();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetDefaultDevicePeriod()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                long n0 = audioClient.DefaultDevicePeriod;
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetMinimumDevicePeriod()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                long n0 = audioClient.MinimumDevicePeriod;
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetStreamLatency()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                long n0 = audioClient.StreamLatency;
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateSimpleAudioVolume()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                SimpleAudioVolume.FromAudioClient(audioClient).Dispose();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanSetVolumeThroughSimpleAudioVolume()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                using (var volume = SimpleAudioVolume.FromAudioClient(audioClient))
                {
                    for (float v = 0; v < 1.0; v += 0.01f)
                    {
                        volume.MasterVolume = v;
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanMuteThroughSimpleAudioVolume()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                using (var volume = SimpleAudioVolume.FromAudioClient(audioClient))
                {
                    var muted = volume.IsMuted;
                    volume.IsMuted = !muted;
                    volume.IsMuted = muted;
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void IsMixFormatSupported()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                Assert.IsTrue(IsFormatSupported(audioClient.MixFormat, AudioClientShareMode.Shared, audioClient));
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CheckVariousFormatsForSupported_SharedMode()
        {
            var sampleRates = new int[] { 8000, 11025, 16000, 22050, 32000, 44056, 44100, 47250, 48000, 50000, 50400, 88200, 96000, 176400, 192000, 352800 };
            var channels = Enumerable.Range(1, 24); //24 = 22.2 surrond sound
            var bps = new int[] { 8, 16, 24, 32 };
            List<WaveFormat> waveFormats = new List<WaveFormat>();
            foreach (var s in sampleRates)
            {
                foreach (var c in channels)
                {
                    foreach (var b in bps)
                    {
                        waveFormats.Add(new WaveFormat(s, b, c));
                        waveFormats.Add(new WaveFormatExtensible(s, b, c, b == 32 ? AudioSubTypes.IeeeFloat : AudioSubTypes.Pcm));
                    }
                }
            }

            Debug.WriteLine("---------------------------------------");
            Debug.WriteLine("Generated {0} formats.", waveFormats.Count);
            Debug.WriteLine("---------------------------------------");

            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                foreach (var format in waveFormats)
                {
                    Debug.WriteLine("Format[{0}]: {1} Hz, {2} bits/sample, {3} channels", format.GetType().FullName, format.SampleRate, format.BitsPerSample, format.Channels);
                    audioClient.IsFormatSupported(AudioClientShareMode.Shared, format);
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CheckVariousFormatsForSupported_ExclusiveMode()
        {
            var sampleRates = new int[] { 8000, 11025, 16000, 22050, 32000, 44056, 44100, 47250, 48000, 50000, 50400, 88200, 96000, 176400, 192000, 352800 };
            var channels = Enumerable.Range(1, 24); //24 = 22.2 surrond sound
            var bps = new int[] { 8, 16, 24, 32 };
            List<WaveFormat> waveFormats = new List<WaveFormat>();
            foreach (var s in sampleRates)
            {
                foreach (var c in channels)
                {
                    foreach (var b in bps)
                    {
                        waveFormats.Add(new WaveFormat(s, b, c));
                        waveFormats.Add(new WaveFormatExtensible(s, b, c, b == 32 ? AudioSubTypes.IeeeFloat : AudioSubTypes.Pcm));
                    }
                }
            }

            Debug.WriteLine("---------------------------------------");
            Debug.WriteLine("Generated {0} formats.", waveFormats.Count);
            Debug.WriteLine("---------------------------------------");

            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                foreach (var format in waveFormats)
                {
                    Debug.WriteLine("Format[{0}]: {1} Hz, {2} bits/sample, {3} channels", format.GetType().FullName, format.SampleRate, format.BitsPerSample, format.Channels);
                    audioClient.IsFormatSupported(AudioClientShareMode.Exclusive, format);
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateCaptureClientExclusive()
        {
            using (var audioClient = Utils.CreateDefaultCaptureClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.MixFormat, Guid.Empty);
                using (var captureClient = AudioCaptureClient.FromAudioClient(audioClient))
                {
                    Assert.IsNotNull(captureClient);
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateCaptureClientShared()
        {
            WaveFormat waveFormat = new WaveFormat();
            using (var audioClient = Utils.CreateDefaultCaptureClient())
            {
                audioClient.Initialize(AudioClientShareMode.Exclusive, AudioClientStreamFlags.None, 1000, 0, waveFormat, Guid.Empty);
                using (var captureClient = AudioCaptureClient.FromAudioClient(audioClient))
                {
                    Assert.IsNotNull(captureClient);
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetAudioClock()
        {
            using (var audioClient = Utils.CreateDefaultRenderClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0,
                    audioClient.GetMixFormat(), Guid.Empty);
                using (var audioClock = AudioClock.FromAudioClient(audioClient))
                {
                    Assert.IsNotNull(audioClock);
                    Debug.WriteLine("Frequency: {0}; Position: {1}", audioClock.Pu64Frequency, audioClock.Pu64Position);
                }
            }
        }

        private bool IsFormatSupported(WaveFormat waveFormat, AudioClientShareMode sharemode, AudioClient audioClient)
        {
            return audioClient.IsFormatSupported(sharemode, waveFormat);
        }
    }
}