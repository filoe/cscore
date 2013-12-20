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
    public class MMDevicesTests
    {
        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void CanCreateDeviceNotificationEvent()
        {
            using (var enumerator = new MMDeviceEnumerator())
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
        public void CanEnumerateAllDevices()
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
                Utils.DumpCollection(collection);

                enumerator.Dispose();
                collection.Dispose();
            }
        }
    }
}