using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using CSCore.SoundOut;
using CSCore.SoundOut.DirectSound;
using System.Threading;

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
            DirectSoundOut1 dsoundOut = new DirectSoundOut1();
            dsoundOut.Initialize(new Streams.SineGenerator().ToWaveSource(16));
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
                DirectSoundException.Try(dsound.SetCooperativeLevel(DSInterop.DirectSoundUtils.GetDesktopWindow(), DSCooperativeLevelType.DSSCL_PRIORITY),
                    "IDirectSound8", "SetCooperativeLevel");
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanSetNormalCooperativeLevel()
        {
            using (var dsound = CreateDirectSound8())
            {
                DirectSoundException.Try(dsound.SetCooperativeLevel(DSInterop.DirectSoundUtils.GetDesktopWindow(), DSCooperativeLevelType.DSSCL_NORMAL),
                    "IDirectSound8", "SetCooperativeLevel");
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanSetExclusiveCooperativeLevel()
        {
            using (var dsound = CreateDirectSound8())
            {
                DirectSoundException.Try(dsound.SetCooperativeLevel(DSInterop.DirectSoundUtils.GetDesktopWindow(), DSCooperativeLevelType.DSSCL_EXCLUSIVE),
                    "IDirectSound8", "SetCooperativeLevel");
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanSetWritePrimaryCooperativeLevel()
        {
            using (var dsound = CreateDirectSound8())
            {
                DirectSoundException.Try(dsound.SetCooperativeLevel(DSInterop.DirectSoundUtils.GetDesktopWindow(), DSCooperativeLevelType.DSSCL_WRITEPRIMARY),
                    "IDirectSound8", "SetCooperativeLevel");
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanGetCaps()
        {
            using (var dsound = CreateDirectSound8())
            {
                DirectSoundException.Try(dsound.SetCooperativeLevel(DSInterop.DirectSoundUtils.GetDesktopWindow(), DSCooperativeLevelType.DSSCL_NORMAL),
                    "IDirectSound8", "SetCooperativeLevel");
                var caps = dsound.Caps;
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanCheckForSupportedFormat()
        {
            using (var dsound = CreateDirectSound8())
            {
                DirectSoundException.Try(dsound.SetCooperativeLevel(DSInterop.DirectSoundUtils.GetDesktopWindow(), DSCooperativeLevelType.DSSCL_NORMAL),
                    "IDirectSound8", "SetCooperativeLevel");
                dsound.SupportsFormat(new WaveFormat());
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanCreatePrimaryBuffer()
        {
            using (var dsound = CreateDirectSound8())
            {
                DirectSoundException.Try(dsound.SetCooperativeLevel(DSInterop.DirectSoundUtils.GetDesktopWindow(), DSCooperativeLevelType.DSSCL_NORMAL),
                    "IDirectSound8", "SetCooperativeLevel");
                new DirectSoundPrimaryBuffer(dsound).Dispose();
            }
        }

        [TestMethod]
        [TestCategory("DirectSound")]
        public void CanCreateSecondaryBuffer()
        {
            using (var dsound = CreateDirectSound8())
            {
                DirectSoundException.Try(dsound.SetCooperativeLevel(DSInterop.DirectSoundUtils.GetDesktopWindow(), DSCooperativeLevelType.DSSCL_NORMAL),
                    "IDirectSound8", "SetCooperativeLevel");
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
                DirectSoundException.Try(dsound.SetCooperativeLevel(DSInterop.DirectSoundUtils.GetDesktopWindow(), DSCooperativeLevelType.DSSCL_NORMAL),
                    "IDirectSound8", "SetCooperativeLevel");
                WaveFormat waveFormat = new WaveFormat(44100, 16, 2);
                using (var primaryBuffer = new DirectSoundPrimaryBuffer(dsound))
                using (var secondaryBuffer = new DirectSoundSecondaryBuffer(dsound, waveFormat, (int)waveFormat.MillisecondsToBytes(10000), true))
                {
                    primaryBuffer.Play(DSBPlayFlags.DSBPLAY_LOOPING);
                    var caps = secondaryBuffer.BufferCaps;

                    var data = GenerateData(caps.dwBufferBytes / 2, waveFormat);

                    //try to set an echo effect
                    secondaryBuffer.SetFX(DSEchoEffect.GetDefaultDescription());
                    using (var echo = secondaryBuffer.GetFX<DSEchoEffect>(0))
                    {
                        echo.Feedback = DSEchoEffect.FeedbackMax;
                        echo.LeftDelay = 1000;
                        echo.RightDelay = 500;

                        if (secondaryBuffer.Write(data, 0, data.Length))
                        {
                            secondaryBuffer.Play(DSBPlayFlags.DSBPLAY_LOOPING);
                        }
                        else
                        {
                            Debug.Assert(false, "Could not write data.");
                        }
                        Thread.Sleep(1);
                    }
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
