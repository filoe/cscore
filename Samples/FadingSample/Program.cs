using System;
using System.Threading;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore.Streams;

namespace FadingSample
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select a file to play",
                Filter = CodecFactory.SupportedFilesFilterEn
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            using (var soundOut = new WasapiOut() { Latency = 50 })
            {
                using (var fadeInOut = CodecFactory.Instance.GetCodec(openFileDialog.FileName)
                    .ToSampleSource()
                    .AppendSource(x => new FadeInOut(x)))
                {
                    var linearFadeStrategy = new LinearFadeStrategy();
                    var eventHandle = new AutoResetEvent(false);
                    linearFadeStrategy.FadingFinished += (s, e) => eventHandle.Set();
                    fadeInOut.FadeStrategy = linearFadeStrategy;

                    soundOut.Initialize(fadeInOut.ToWaveSource());
                    soundOut.Play();

                    while (true)
                    {
                        Console.Write("Enter the target volume: ");
                        float to;
                        if (!Single.TryParse(Console.ReadLine(), out to) ||
                            to > 1 || to < 0)
                        {
                            Console.WriteLine("Invalid value.");
                            continue;
                        }

                        linearFadeStrategy.StartFading(0.3f, to, 5000); //fade from the current volume to the entered volume over a duration of 3000ms

                        do
                        {
                            ClearCurrentConsoleLine();
                            Console.WriteLine(linearFadeStrategy.CurrentVolume);
                            Console.CursorTop--;
                        } while (!eventHandle.WaitOne(50));

                        ClearCurrentConsoleLine();
                        Console.WriteLine(linearFadeStrategy.CurrentVolume);
                    }
                }
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
