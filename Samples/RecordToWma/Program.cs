using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.MediaFoundation;
using CSCore.SoundIn;
using CSCore.Streams;

namespace RecordToWma
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var wasapiCapture = new WasapiLoopbackCapture())
            {
                wasapiCapture.Initialize();
                var wasapiCaptureSource = new SoundInSource(wasapiCapture);
                using(var stereoSource = wasapiCaptureSource.ToStereo())
                {
                    using (var writer = MediaFoundationEncoder.CreateWMAEncoder(stereoSource.WaveFormat, "output.wma"))
                    {
                        byte[] buffer = new byte[stereoSource.WaveFormat.BytesPerSecond];
                        wasapiCaptureSource.DataAvailable += (s, e) =>
                        {
                            int read = stereoSource.Read(buffer, 0, buffer.Length);
                            writer.Write(buffer, 0, read);
                        };

                        wasapiCapture.Start();

                        Console.ReadKey();

                        wasapiCapture.Stop();
                    }
                }
            }
        }
    }
}
