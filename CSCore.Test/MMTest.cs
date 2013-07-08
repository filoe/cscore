using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.SoundOut;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Streams.SampleConverter;

namespace CSCore.Test
{
    [TestClass]
    public class MMTest
    {
        [TestMethod]
        [TestCategory("WaveOut")]
        public void EnumerateWaveOutDevices()
        {
            foreach (var device in WaveOut.GetDevices())
            {
                Debug.WriteLine("{0};{1};{2}", device.szPname, device.wChannels, device.vDriverVersion);
            }
        }

        [TestMethod]
        [TestCategory("WaveIn")]
        public void EnumerateWaveInDevices()
        {
            foreach (var device in WaveIn.Devices)
            {
                Debug.WriteLine("{0};{1};{2}", device.Name, device.Channels, device.DriverVersion);
            }
        }

        [TestMethod]
        [TestCategory("WaveOut")]
        public void CreateWaveOutDevice()
        {
            WaveOut waveOut = new WaveOut();
            var source = new SineGenerator().ToWaveSource(16);
            waveOut.Initialize(source);
            waveOut.Dispose();
        }

        [TestMethod]
        [TestCategory("WaveIn")]
        public void CreateWaveInDevice()
        {
            WaveIn waveIn = new WaveIn();
            waveIn.Initialize();
            waveIn.Dispose();
        }
    }
}
