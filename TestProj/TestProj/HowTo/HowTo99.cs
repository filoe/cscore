using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProj.HowTo
{
    public static class HowTo99
    {
        public static void Main(string[] args)
        {
            CustomDevicePlayback(@"C:\Temp\test.mp3");
        }

        #region test

        public static void SimpleWaveOutPlayback(string filename)
        {
            using (var source = CodecFactory.Instance.GetCodec(filename))
            using (var soundOut = new WaveOut())
            {
                soundOut.Initialize(source);
                soundOut.Play();

                Console.ReadKey();
            }
        }

        public static void SimpleWasapiPlayback(string filename)
        {
            using (var source = CodecFactory.Instance.GetCodec(filename))
            {
                ISoundOut soundOut;
                if (WasapiOut.IsSupportedOnCurrentPlatform)
                {
                    soundOut = new WasapiOut();
                }
                else
                {
                    soundOut = new DirectSoundOut();
                }

                soundOut.Initialize(source);
                soundOut.Play();

                Console.ReadKey();

                soundOut.Stop();
            }
        }

        public static void CustomDevicePlayback(string filename)
        {
            if (!WasapiOut.IsSupportedOnCurrentPlatform)
            {
                Console.WriteLine("Wasapi not supported.");
                return;
            }

            using (var soundOut = SelectWasapiPlaybackDevice())
            {
                var source = CodecFactory.Instance.GetCodec(filename);
                var notificationSource = new NotificationSource(source);
                notificationSource.BlockRead += (s, e) =>
                {
                    var info = String.Format("Playbackposition: {0}/{1}", source.GetPosition().ToString(@"hh\:mm\:ss"), source.GetLength().ToString(@"hh\:mm\:ss"));
                    Console.CursorLeft = 0;
                    Console.Write(info);
                };

                //convert samplesource back to wavesource
                source = notificationSource.ToWaveSource(source.WaveFormat.BitsPerSample);

                soundOut.Initialize(source);
                soundOut.Play();

                Console.ReadKey();

                soundOut.Stop();
                soundOut.WaveSource.Dispose();
                soundOut.Dispose();
            }
        }

        public static WasapiOut SelectWasapiPlaybackDevice()
        {
            using (MMDeviceEnumerator enumerator = new MMDeviceEnumerator())
            using (MMDeviceCollection devices = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    Console.WriteLine("{0}: {1}", i.ToString("00"), devices[i].ToString());
                }

                int deviceIndex = -1;

                do
                {
                    Console.Write("Select device: ");
                }
                while (!Int32.TryParse(Console.ReadLine(), out deviceIndex)
                       || deviceIndex < 0 || deviceIndex >= devices.Count);

                Console.WriteLine();

                WasapiOut soundOut = new WasapiOut();
                soundOut.Device = devices[deviceIndex];

                return soundOut;
            }
        }

        #endregion test
    }
}