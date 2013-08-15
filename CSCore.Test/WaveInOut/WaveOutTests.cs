using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.SampleConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace CSCore.Test.WaveInOut
{
    [TestClass]
    public class WaveOutTests
    {
        [TestMethod]
        [TestCategory("WaveOut")]
        public void CanEnumerateWaveOutDevices()
        {
            foreach (var device in WaveOut.GetDevices())
            {
                Debug.WriteLine("{0};{1};{2}", device.szPname, device.wChannels, device.vDriverVersion);
            }
        }

        [TestMethod]
        [TestCategory("WaveOut")]
        public void CanCreateWaveOutDevice()
        {
            WaveOut waveOut = new WaveOut();
            var source = new SineGenerator().ToWaveSource(16);
            waveOut.Initialize(source);
            waveOut.Dispose();
        }
    }
}