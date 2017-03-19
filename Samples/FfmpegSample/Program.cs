using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.Ffmpeg;
using CSCore.SoundOut;
using CSCore.Streams;

namespace FfmpegSample
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                //seems like while a debugger is attached the ffmpeg log does not appear in the console
                FfmpegUtils.LogToDefaultLogger = false;
                FfmpegUtils.FfmpegLogReceived += (s, e) =>
                {
                    Console.Error.Write(e.Message);
                };
            }

            EnumerateSupportedFormats();

            const string DefaultStream =
                @"http://stream.srg-ssr.ch/m/rsj/aacp_96";

            Console.WriteLine("01 - File");
            Console.WriteLine("02 - Stream");
            Console.WriteLine("99 - Test-Stream");

            int choice;
            do
            {
                Int32.TryParse(Console.ReadLine(), out choice);
            } while (choice != 1 && choice != 2 && choice != 99);

            Stream stream = null;
            string url = null;

            if (choice == 1)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                stream = File.OpenRead(openFileDialog.FileName);
            }
            else if (choice == 2)
            {
                Console.WriteLine("Enter a stream url:");
                url = Console.ReadLine();
            }
            else
            {
                url = DefaultStream;
            }

            //we could also easily pass the filename as url
            //but since we want to test the decoding of System.IO.Stream, we
            //pass a FileStream as argument.
            IWaveSource ffmpegDecoder = stream == null
                ? new FfmpegDecoder(url)
                : new FfmpegDecoder(stream);

            using (ffmpegDecoder)
            using (var wasapiOut = new WasapiOut())
            {
                wasapiOut.Initialize(ffmpegDecoder);
                wasapiOut.Play();

                Console.ReadKey();
            }
        }

        private static void EnumerateSupportedFormats()
        {
            Console.BufferHeight = 1500;

            foreach (var format in FfmpegUtils.GetInputFormats())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(format.Name);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(format.LongName);
                Console.ResetColor();

                string extensions = String.Empty;
                if (format.FileExtensions.Count > 0)
                    extensions = format.FileExtensions.Aggregate((x, y) => x + ", " + y);
                Console.WriteLine("Extensions: " + extensions);
                Console.WriteLine("Codecs");
                foreach (var supportedCodec in format.Codecs)
                {
                    Console.WriteLine(" -" + supportedCodec);
                }
            }
        }
    }
}
