using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.MediaFoundation;
using CSCore.SoundIn;

namespace RecordToWma
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var wasapiCapture = new WasapiCapture())
            {
                wasapiCapture.Initialize();

                using (var writer = MediaFoundationEncoder.CreateWMAEncoder(wasapiCapture.WaveFormat, "output.wma"))
                {
                    wasapiCapture.DataAvailable += (s, e) =>
                    {
                        writer.Write(e.Data, e.Offset, e.ByteCount);
                    };

                    wasapiCapture.Start();

                    Console.ReadKey();

                    wasapiCapture.Stop();
                }
            }
        }
    }
}
