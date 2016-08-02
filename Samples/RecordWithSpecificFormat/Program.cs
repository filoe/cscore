using System;
using System.Diagnostics;
using System.Linq;
using CSCore;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.Streams;

namespace RecordWithSpecificFormat
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            //choose the capture mode
            Console.WriteLine("Select capturing mode:");
            Console.WriteLine("- 1: Capture");
            Console.WriteLine("- 2: LoopbackCapture");

            CaptureMode captureMode = (CaptureMode)ReadInteger(1, 2);
            DataFlow dataFlow = captureMode == CaptureMode.Capture ? DataFlow.Capture : DataFlow.Render;

            //---
            
            //select the device:
            var devices = MMDeviceEnumerator.EnumerateDevices(dataFlow, DeviceState.Active);
            if (!devices.Any())
            {
                Console.WriteLine("No devices found.");
                return;
            }

            Console.WriteLine("Select device:");
            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine("- {0:#00}: {1}", i, devices[i].FriendlyName);
            }
            int selectedDeviceIndex = ReadInteger(Enumerable.Range(0, devices.Count).ToArray());
            var device = devices[selectedDeviceIndex];

            //--- choose format
            Console.WriteLine("Enter sample rate:");
            int sampleRate;
            do
            {
                sampleRate = ReadInteger();
                if (sampleRate >= 100 && sampleRate <= 200000)
                    break;
                Console.WriteLine("Must be between 1kHz and 200kHz.");
            } while (true);

            Console.WriteLine("Choose bits per sample (8, 16, 24 or 32):");
            int bitsPerSample = ReadInteger(8, 16, 24, 32);

            //note: this sample does not support multi channel formats like surround 5.1,...
            //if this is required, the DmoChannelResampler class can be used
            Console.WriteLine("Choose number of channels (1, 2):");
            int channels = ReadInteger(1, 2);

            //---

            //start recording

            //create a new soundIn instance
            using (WasapiCapture soundIn = captureMode == CaptureMode.Capture
                ? new WasapiCapture()
                : new WasapiLoopbackCapture())
            {
                //optional: set some properties 
                soundIn.Device = device;
                //...

                //initialize the soundIn instance
                soundIn.Initialize();

                //create a SoundSource around the the soundIn instance
                //this SoundSource will provide data, captured by the soundIn instance
                SoundInSource soundInSource = new SoundInSource(soundIn) {FillWithZeros = false};

                //create a source, that converts the data provided by the
                //soundInSource to any other format
                //in this case the "Fluent"-extension methods are being used
                IWaveSource convertedSource = soundInSource
                    .ChangeSampleRate(sampleRate) // sample rate
                    .ToSampleSource()
                    .ToWaveSource(bitsPerSample); //bits per sample

                //channels...
                using (convertedSource = channels == 1 ? convertedSource.ToMono() : convertedSource.ToStereo())
                {

                    //create a new wavefile
                    using (WaveWriter waveWriter = new WaveWriter("out.wav", convertedSource.WaveFormat))
                    {

                        //register an event handler for the DataAvailable event of 
                        //the soundInSource
                        //Important: use the DataAvailable of the SoundInSource
                        //If you use the DataAvailable event of the ISoundIn itself
                        //the data recorded by that event might won't be available at the
                        //soundInSource yet
                        soundInSource.DataAvailable += (s, e) =>
                        {
                            //read data from the converedSource
                            //important: don't use the e.Data here
                            //the e.Data contains the raw data provided by the 
                            //soundInSource which won't have your target format
                            byte[] buffer = new byte[convertedSource.WaveFormat.BytesPerSecond / 2];
                            int read;

                            //keep reading as long as we still get some data
                            //if you're using such a loop, make sure that soundInSource.FillWithZeros is set to false
                            while ((read = convertedSource.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                //write the read data to a file
                                // ReSharper disable once AccessToDisposedClosure
                                waveWriter.Write(buffer, 0, read);
                            }
                        };

                        //we've set everything we need -> start capturing data
                        soundIn.Start();

                        Console.WriteLine("Capturing started ... press any key to stop.");
                        Console.ReadKey();

                        soundIn.Stop();
                    }
                }
            }

            Process.Start("out.wav");
        }

        private static int ReadInteger(params int[] validValues)
        {
            int value;

            do
            {
                value = ReadInteger();
                if (validValues == null || validValues.Any(x => x == value))
                    return value;
                Console.WriteLine("Invalid value");
            } while (true);
        }

        private static int ReadInteger()
        {
            int value;
            while (!Int32.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Invalid value");
            }
            return value;
        }

        enum CaptureMode
        {
            Capture = 1,
            // ReSharper disable once UnusedMember.Local
            LoopbackCapture = 2
        }
    }
}
