using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSCore.Codecs;
using CSCore.SoundOut;

namespace TestProj
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var source = CodecFactory.Instance.GetCodec(@"C:\Temp\test.mp3");
            var soundOut = new WasapiOut();
            soundOut.Initialize(source);
            soundOut.Play();

            Console.ReadKey();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}