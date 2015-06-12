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
#pragma warning disable 612
            foreach (var device in WaveInDevice.EnumerateDevices())
#pragma warning restore 612
            {
                Debug.WriteLine("{0};{1}", device.Name, device.DriverVersion);
            }
        }

        [TestMethod]
        [TestCategory("WaveIn")]
        public void CanCreateWaveInDevice()
        {
#pragma warning disable 612
            using (WaveIn waveIn = new WaveIn())
#pragma warning restore 612
            {
                waveIn.Initialize();
                waveIn.Start();
            }
        }
    }
}