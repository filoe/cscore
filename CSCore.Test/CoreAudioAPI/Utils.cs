using CSCore.CoreAudioAPI;
using CSCore.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Test.CoreAudioAPI
{
    internal static class Utils
    {
        public static void DumpCollection(MMDeviceCollection collection)
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

        public static MMDevice GetDefaultRenderDevice()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            }
        }

        public static MMDevice GetDefaultCaptureDevice()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                return enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
            }
        }

        public static AudioClient CreateDefaultRenderClient()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console))
                {
                    var audioClient = AudioClient.FromMMDevice(device);
                    Assert.IsNotNull(audioClient);
                    return audioClient;
                }
            }
        }

        public static AudioClient CreateDefaultCaptureClient()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console))
                {
                    var audioClient = AudioClient.FromMMDevice(device);
                    Assert.IsNotNull(audioClient);
                    return audioClient;
                }
            }
        }
    }
}