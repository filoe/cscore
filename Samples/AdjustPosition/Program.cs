using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;

namespace AdjustPosition
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = CodecFactory.SupportedFilesFilterEn;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var source1 = CodecFactory.Instance.GetCodec(openFileDialog.FileName))
                {
                    Debug.Assert(source1.CanSeek, "Source does not support seeking.");

                    using (var soundOut = new WaveOut(150))
                    {
                        var source = source1.ToSampleSource();
                        soundOut.Initialize(source.ToWaveSource());
                        soundOut.Play();

                        Console.WriteLine("Press any key to skip half the track.");
                        Console.ReadKey();

                        source.Position = source.Length / 2;

                        while (true)
                        {
                            IAudioSource s = source1;
                            var str = String.Format(@"New position: {0:mm\:ss\.f}/{1:mm\:ss\.f}",
                                TimeConverterFactory.Instance.GetTimeConverterForSource(s)
                                    .ToTimeSpan(s.WaveFormat, s.Position),
                                TimeConverterFactory.Instance.GetTimeConverterForSource(s)
                                    .ToTimeSpan(s.WaveFormat, s.Length));
                            str += String.Concat(Enumerable.Repeat(" ", Console.BufferWidth - 1 - str.Length));
                            Console.Write(str);
                            Console.SetCursorPosition(0, Console.CursorTop);
                        }
                    }
                }
            }
        }
    }
}
