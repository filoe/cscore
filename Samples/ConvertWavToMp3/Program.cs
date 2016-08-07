using System;
using System.IO;
using System.Linq;
using CSCore;
using CSCore.Codecs;
using CSCore.MediaFoundation;

namespace ConvertWavToMp3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || !File.Exists(args[0]) ||
                // ReSharper disable once PossibleNullReferenceException
                !Path.GetExtension(args[0]).Equals(".wav", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var supportedFormats = MediaFoundationEncoder.GetEncoderMediaTypes(AudioSubTypes.MpegLayer3);
            if (!supportedFormats.Any())
            {
                Console.WriteLine("The current platform does not support mp3 encoding.");
                return;
            }

            IWaveSource source;
            try
            {
                source = CodecFactory.Instance.GetCodec(args[0]);

                if (
                    supportedFormats.All(
                        x => x.SampleRate != source.WaveFormat.SampleRate && x.Channels == source.WaveFormat.Channels))
                {
                    //the encoder does not support the input sample rate -> convert it to any supported samplerate
                    //choose the best sample rate with stereo (in order to make simple, we always use stereo in this sample)
                    int sampleRate =
                        supportedFormats.OrderBy(x => Math.Abs(source.WaveFormat.SampleRate - x.SampleRate))
                            .First(x => x.Channels == source.WaveFormat.Channels)
                            .SampleRate;

                    Console.WriteLine("Samplerate {0} -> {1}", source.WaveFormat.SampleRate, sampleRate);
                    Console.WriteLine("Channels {0} -> {1}", source.WaveFormat.Channels, 2);
                    source = source.ChangeSampleRate(sampleRate);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Format not supported.");
                return;
            }

            using (source)
            {
                using (var encoder = MediaFoundationEncoder.CreateMP3Encoder(source.WaveFormat, "output.mp3"))
                {
                    byte[] buffer = new byte[source.WaveFormat.BytesPerSecond];
                    int read;
                    while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        encoder.Write(buffer, 0, read);

                        Console.CursorLeft = 0;
                        Console.Write("{0:P}/{1:P}", (double) source.Position / source.Length, 1);
                    }
                }
            }
        }
    }
}
