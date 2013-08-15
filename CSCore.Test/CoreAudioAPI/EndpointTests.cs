using CSCore.CoreAudioAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSCore.Test.CoreAudioAPI
{
    [TestClass]
    public class EndpointTests
    {
        [TestMethod]
        [TestCategory("CoreAudioAPI.Endpoint")]
        public void CanCreateAudioEndpointVolume()
        {
            using (var device = Utils.GetDefaultRenderDevice())
            using (var endpointVolume = AudioEndpointVolume.FromDevice(device))
            {
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI.Endpoint")]
        public void CanGetAudioEndpointVolume()
        {
            using (var device = Utils.GetDefaultRenderDevice())
            using (var endpointVolume = AudioEndpointVolume.FromDevice(device))
            {
                var volume = endpointVolume.GetMasterVolumeLevelScalar();
                Debug.WriteLine("Volume: {0}", volume);
                endpointVolume.SetMasterVolumeLevelScalar(0.5f, Guid.Empty);
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI.Endpoint")]
        public void CanGetAudioEndpointVolumeChannelCount()
        {
            using (var device = Utils.GetDefaultRenderDevice())
            using (var endpointVolume = AudioEndpointVolume.FromDevice(device))
            {
                Debug.WriteLine(endpointVolume.GetChannelCount());
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI.Endpoint")]
        public void CanCreateAudioEndpointVolumeNotification()
        {
            using (var device = Utils.GetDefaultRenderDevice())
            using (var endpointVolume = AudioEndpointVolume.FromDevice(device))
            {
                AudioEndpointVolumeCallback callback = new AudioEndpointVolumeCallback();
                callback.NotifyRecived += (s, e) =>
                {
                    Debug.WriteLine("Notification1");
                    //Debug.Assert(e.Channels == endpointVolume.ChannelCount);
                };
                endpointVolume.RegisterControlChangeNotify(callback);

                var vol = endpointVolume.GetChannelVolumeLevelScalar(0);
                endpointVolume.SetChannelVolumeLevelScalar(0, 1f, Guid.Empty);
                endpointVolume.SetChannelVolumeLevelScalar(0, vol, Guid.Empty);

                endpointVolume.UnregisterControlChangeNotify(callback);
                System.Threading.Thread.Sleep(1000);
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI.Endpoint")]
        public void CanCreateAudioMeterInformation()
        {
            using (var device = Utils.GetDefaultRenderDevice())
            using (var meter = AudioMeterInformation.FromDevice(device))
            {
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI.Endpoint")]
        public void CanGetAudioMeterInformationPeakValue()
        {
            using (var device = Utils.GetDefaultRenderDevice())
            using (var meter = AudioMeterInformation.FromDevice(device))
            {
                Console.WriteLine(meter.PeakValue);
            }
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI.Endpoint")]
        public void CanGetAudioMeterInformationChannelsPeaks()
        {
            using (var device = Utils.GetDefaultRenderDevice())
            using (var meter = AudioMeterInformation.FromDevice(device))
            {
                for (int i = 0; i < meter.MeteringChannelCount; i++)
                {
                    Console.WriteLine(meter[i]);
                }
            }
        }
    }
}