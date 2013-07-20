using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.CoreAudioAPI;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace CSCore.Test
{
    [TestClass]
    public class CoreAudioAPI
    {
        [TestMethod]
        [TestCategory("CoreAudioAPI.EndpointVolume")]
        public void CanCreateAudioEndpointVolume()
        {
            using (var device = GetDefaultRenderDevice())
            using (var endpointVolume = AudioEndpointVolume.FromDevice(device))
            {
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI.EndpointVolume")]
        public void CanGetAudioEndpointVolume()
        {
            using (var device = GetDefaultRenderDevice())
            using (var endpointVolume = AudioEndpointVolume.FromDevice(device))
            {
                var volume = endpointVolume.GetMasterVolumeLevelScalar();
                Debug.WriteLine("Volume: {0}", volume);
                endpointVolume.SetMasterVolumeLevelScalar(0.5f, Guid.Empty);
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI.EndpointVolume")]
        public void CanGetAudioEndpointVolumeChannelCount()
        {
            using (var device = GetDefaultRenderDevice())
            using (var endpointVolume = AudioEndpointVolume.FromDevice(device))
            {
                Debug.WriteLine(endpointVolume.GetChannelCount());
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI.EndpointVolume")]
        public void CanCreateAudioEndpointVolumeNotification()
        {
            using (var device = GetDefaultRenderDevice())
            using (var endpointVolume = AudioEndpointVolume.FromDevice(device))
            {
                AudioEndpointVolumeCallback callback = new AudioEndpointVolumeCallback();
                callback.NotifyRecived += (s, e) =>
                {
                    Debug.Assert(e.Channels == endpointVolume.ChannelCount);
                };
                endpointVolume.RegisterControlChangeNotify(callback);
                var result = endpointVolume.SetChannelVolumeLevelScalarNative(1, 1f, Guid.Empty);
                System.Threading.Thread.Sleep(1000);
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateDeviceNotificationEvent()
        {
            using(var enumerator = new MMDeviceEnumerator())
            using (var notification = new MMNotificationClient(enumerator))
            {
                notification.DefaultDeviceChanged += (s, e) => { };
                notification.DeviceAdded += (s, e) => { };
                notification.DevicePropertyChanged += (s, e) => { };
                notification.DeviceRemoved += (s, e) => { };
                notification.DeviceStateChanged += (s, e) => { };
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanAllEnumerateDevices()
        {
            using (var enumerator = new MMDeviceEnumerator())
            using (var collection = enumerator.EnumAudioEndpoints(DataFlow.All, DeviceState.All))
            {

                foreach (var item in collection)
                {
                    Debug.WriteLine(item.ToString());
                    item.Dispose();
                }

                enumerator.Dispose();
                collection.Dispose();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanEnumerateRenderDevices()
        {
            using (var enumerator = new MMDeviceEnumerator())
            using (var collection = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.All))
            {

                foreach (var item in collection)
                {
                    Debug.WriteLine(item.ToString());
                    item.Dispose();
                }

                enumerator.Dispose();
                collection.Dispose();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanEnumerateCaptureDevices()
        {
            using (var enumerator = new MMDeviceEnumerator())
            using (var collection = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.All))
            {

                foreach (var item in collection)
                {
                    Debug.WriteLine(item.ToString());
                    item.Dispose();
                }

                enumerator.Dispose();
                collection.Dispose();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void GetDefaultRenderEndpoint()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console))
                {
                    Debug.WriteLine(device.ToString());
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void GetDefaultCaptureEndpoint()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console))
                {
                    Debug.WriteLine(device.ToString());
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanAccessPropertyStore()
        {
            using (var enumerator = new MMDeviceEnumerator())
            using (var collection = enumerator.EnumAudioEndpoints(DataFlow.All, DeviceState.All))
            {

                DumpCollection(collection);

                enumerator.Dispose();
                collection.Dispose();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void GetMixFormat()
        {
            using (var audioClient = CreateAudioClient())
            {
                Debug.WriteLine(audioClient.GetMixFormat().ToString());
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void InitializeSharedMode()
        {
            using (var audioClient = CreateAudioClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void InitializeExlusiveMode()
        {
            WaveFormat waveFormat = new WaveFormat();
            using (var audioClient = CreateAudioClient())
            {
                audioClient.Initialize(AudioClientShareMode.Exclusive, AudioClientStreamFlags.None, 1000, 0, waveFormat, Guid.Empty);
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateAudioClient()
        {
            CreateAudioClient().Dispose();
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateRenderClient()
        {
            using (var audioClient = CreateAudioClient())
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
            using (var audioClient = CreateAudioClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                audioClient.GetCurrentPadding();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetBufferSize()
        {
            using (var audioClient = CreateAudioClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                audioClient.GetBufferSize();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetDefaultDevicePeriod()
        {
            using (var audioClient = CreateAudioClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                long n0 = audioClient.DefaultDevicePeriod;
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetMinimumDevicePeriod()
        {
            using (var audioClient = CreateAudioClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                long n0 = audioClient.MinimumDevicePeriod;
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanGetStreamLatency()
        {
            using (var audioClient = CreateAudioClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                long n0 = audioClient.StreamLatency;
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateSimpleAudioVolume()
        {
            using (var audioClient = CreateAudioClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                SimpleAudioVolume.FromAudioClient(audioClient).Dispose();
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanSetVolumeThroughSimpleAudioVolume()
        {
            using (var audioClient = CreateAudioClient())
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
            using (var audioClient = CreateAudioClient())
            {
                audioClient.Initialize(AudioClientShareMode.Shared, AudioClientStreamFlags.None, 1000, 0, audioClient.GetMixFormat(), Guid.Empty);
                using (var volume = SimpleAudioVolume.FromAudioClient(audioClient))
                {
                    volume.IsMuted = true;
                    volume.IsMuted = false;
                }
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void IsMixFormatSupported()
        {
            using (var audioClient = CreateAudioClient())
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
            var bps = new int[]{ 8, 16, 24, 32 };
            List<WaveFormat> waveFormats = new List<WaveFormat>();
            foreach (var s in sampleRates)
            {
                foreach (var c in channels)
                {
                    foreach (var b in bps)
                    {
                        waveFormats.Add(new WaveFormat(s, b, c));
                        waveFormats.Add(new WaveFormatExtensible(s, b, c, b == 32 ? DMO.MediaTypes.MEDIASUBTYPE_IEEE_FLOAT : DMO.MediaTypes.MEDIASUBTYPE_PCM));
                    }
                }
            }

            Debug.WriteLine("---------------------------------------");
            Debug.WriteLine("Generated {0} formats.", waveFormats.Count);
            Debug.WriteLine("---------------------------------------");

            using (var audioClient = CreateAudioClient())
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
                        waveFormats.Add(new WaveFormatExtensible(s, b, c, b == 32 ? DMO.MediaTypes.MEDIASUBTYPE_IEEE_FLOAT : DMO.MediaTypes.MEDIASUBTYPE_PCM));
                    }
                }
            }

            Debug.WriteLine("---------------------------------------");
            Debug.WriteLine("Generated {0} formats.", waveFormats.Count);
            Debug.WriteLine("---------------------------------------");

            using (var audioClient = CreateAudioClient())
            {
                foreach (var format in waveFormats)
                {
                    Debug.WriteLine("Format[{0}]: {1} Hz, {2} bits/sample, {3} channels", format.GetType().FullName, format.SampleRate, format.BitsPerSample, format.Channels);
                    audioClient.IsFormatSupported(AudioClientShareMode.Exclusive, format);
                }
            }
        }

        private bool IsFormatSupported(WaveFormat waveFormat, AudioClientShareMode sharemode)
        {
            using (var audioClient = CreateAudioClient())
            {
                return IsFormatSupported(waveFormat, sharemode, audioClient);
            }
        }

        private bool IsFormatSupported(WaveFormat waveFormat, AudioClientShareMode sharemode, AudioClient audioClient)
        {
            return audioClient.IsFormatSupported(sharemode, waveFormat);
        }

        private void DumpCollection(MMDeviceCollection collection)
        {
            foreach (var dev in collection)
            {
                Console.WriteLine("Name: {0}", dev.PropertyStore[PropertyStore.FriendlyName]);
                Console.WriteLine("Desc: {0}", dev.PropertyStore[PropertyStore.DeviceDesc]);
                Console.WriteLine("-----------------------------------------------");
                foreach (var item in dev.PropertyStore)
                {
                    try
                    {
                        Console.WriteLine("Key: {0}\nValue: {1}\n\n", item.Key.PropertyID, item.Value.GetValue());
                    }
                    catch (Exception)
                    {
                    }
                }

                dev.Dispose();
            }
        }

        private MMDevice GetDefaultRenderDevice()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            }
        }

        private AudioClient CreateAudioClient()
        {
            var enumerator = new MMDeviceEnumerator();
            var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            var audioClient = AudioClient.FromMMDevice(device);
            Assert.IsNotNull(audioClient);
            device.Dispose();
            enumerator.Dispose();
            return audioClient;
        }
    }
}
