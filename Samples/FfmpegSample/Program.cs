using System;
using System.IO;
using System.Windows.Forms;
using CSCore;
using CSCore.Ffmpeg;
using CSCore.SoundOut;

namespace FfmpegSample
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
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
            FfmpegDecoder ffmpegDecoder = stream == null
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
    }
}
