using CSCore.DirectSound;
using CSCore.SoundOut;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;

namespace CSCore.Test.DirectSound
{
    [TestClass]
    public class DirectSoundTests
    {
        [TestMethod]
        [TestCategory("DirectSound")]
        public void EnumerateDirectSoundDeviceTest()
        {
            var devices = DirectSoundDeviceEnumerator.EnumerateDevices();
            foreach (var device in devices)
            {
                Debug.WriteLine(device.ToString());
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void OpenDirectSoundDevice()
        {
            DirectSoundOut dsoundOut = new DirectSoundOut();
            dsoundOut.Initialize(new CSCore.Streams.SineGenerator().ToWaveSource(16));
            dsoundOut.Dispose();
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanCreateDirectSound()
        {
            DirectSoundBase.Create((Guid)DirectSoundDevice.DefaultDevice).Dispose();
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanCreateDirectSound8()
        {
            DirectSound8.Create8((Guid)DirectSoundDevice.DefaultDevice).Dispose();
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanSetPriorityCooperativeLevel()
        {
            using (var dsound = CreateDirectSound8())
            {
                dsound.SetCooperativeLevel(DSUtils.GetDesktopWindow(), DSCooperativeLevelType.Priority);
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanSetNormalCooperativeLevel()
        {
            using (var dsound = CreateDirectSound8())
            {
                dsound.SetCooperativeLevel(DSUtils.GetDesktopWindow(), DSCooperativeLevelType.Normal);
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanSetExclusiveCooperativeLevel()
        {
            using (var dsound = CreateDirectSound8())
            {
                dsound.SetCooperativeLevel(DSUtils.GetDesktopWindow(), DSCooperativeLevelType.Exclusive);
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanSetWritePrimaryCooperativeLevel()
        {
            using (var dsound = CreateDirectSound8())
            {
                dsound.SetCooperativeLevel(DSUtils.GetDesktopWindow(), DSCooperativeLevelType.WritePrimary);
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanGetCaps()
        {
            using (var dsound = CreateDirectSound8())
            {
                dsound.SetCooperativeLevel(DSUtils.GetDesktopWindow(), DSCooperativeLevelType.Normal);
                var caps = dsound.Caps;
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanCheckForSupportedFormat()
        {
            using (var dsound = CreateDirectSound8())
            {
                dsound.SetCooperativeLevel(DSUtils.GetDesktopWindow(), DSCooperativeLevelType.Normal);
                dsound.SupportsFormat(new WaveFormat());
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanCreatePrimaryBuffer()
        {
            using (var dsound = CreateDirectSound8())
            {
                dsound.SetCooperativeLevel(DSUtils.GetDesktopWindow(), DSCooperativeLevelType.Normal);
                new DirectSoundPrimaryBuffer(dsound).Dispose();
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanCreateSecondaryBuffer()
        {
            using (var dsound = CreateDirectSound8())
            {
                dsound.SetCooperativeLevel(DSUtils.GetDesktopWindow(), DSCooperativeLevelType.Normal);
                new DirectSoundPrimaryBuffer(dsound).Dispose();
                WaveFormat waveFormat = new WaveFormat();
                new DirectSoundSecondaryBuffer(dsound, waveFormat, (int)waveFormat.MillisecondsToBytes(100)).Dispose();
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanPlayBuffers()
        {
            using (var dsound = CreateDirectSound8())
            {
                dsound.SetCooperativeLevel(DSUtils.GetDesktopWindow(), DSCooperativeLevelType.Normal);
                WaveFormat waveFormat = new WaveFormat(44100, 16, 2);
                using (var primaryBuffer = new DirectSoundPrimaryBuffer(dsound))
                using (var secondaryBuffer = new DirectSoundSecondaryBuffer(dsound, waveFormat, (int)waveFormat.MillisecondsToBytes(10000)))
                {
                    primaryBuffer.Play(DSBPlayFlags.Looping);
                    var caps = secondaryBuffer.BufferCaps;

                    var data = GenerateData(caps.BufferBytes / 2, waveFormat);

                    if (secondaryBuffer.Write(data, 0, data.Length))
                    {
                        secondaryBuffer.Play(DSBPlayFlags.Looping);
                    }
                    else
                    {
                        Assert.Fail("Could not write data.");
                    }
                    Thread.Sleep(1);
                }
            }
        }

        private DirectSound8 CreateDirectSound8()
        {
            return DirectSound8.Create8((Guid)DirectSoundDevice.DefaultDevice);
        }

        private short[] GenerateData(int bufferSize, WaveFormat waveFormat)
        {
            int samples = bufferSize / waveFormat.BlockAlign;
            short[] data = new short[2 * samples];
            int dataIndex = 0;
            for (int i = 0; i < samples; i++)
            {
                double vibrato = Math.Cos(2 * Math.PI * 10.0 * i / waveFormat.SampleRate);
                short value = (short)(Math.Cos(2 * Math.PI * (220.0 + 4.0 * vibrato) * i / waveFormat.SampleRate) * 16384); // Not too loud
                data[dataIndex++] = value;
                data[dataIndex++] = value;
            }

            return data;
        }
    }
}