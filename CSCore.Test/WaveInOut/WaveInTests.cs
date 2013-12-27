using CSCore.SoundIn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSCore.Test.WaveInOut
{
    [TestClass]
    public class WaveInTests
    {
        [TestMethod]
        [TestCategory("WaveIn")]
        public void CanEnumerateWaveInDevices()
        {
            foreach (var device in WaveIn.Devices)
            {
                Debug.WriteLine("{0};{1};{2}", device.Name, device.Channels, device.DriverVersion);
            }
        }

        [TestMethod]
        [TestCategory("WaveIn")]
        public void CanCreateWaveInDevice()
        {
            using (WaveIn waveIn = new WaveIn())
            {
                waveIn.Initialize();
                waveIn.Start();
            }
        }
    }
}