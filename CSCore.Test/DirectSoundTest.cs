using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using CSCore.SoundOut;
using CSCore.SoundOut.DirectSound;

namespace CSCore.Test
{
    [TestClass]
    public class DirectSoundTest
    {
        [TestMethod]
        [TestCategory("DirectSound")]
        public void EnumerateDirectSoundDeviceTest()
        {
            var devices = DirectSoundDevice.EnumerateDevices();
            foreach(var device in devices)
            {
                Debug.WriteLine(device.ToString());
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void OpenDirectSoundDevice()
        {
            DirectSoundOut dsoundOut = new DirectSoundOut();
            dsoundOut.Initialize(new Streams.SineGenerator().ToWaveSource(16));
            dsoundOut.Dispose();
        }
    }
}
