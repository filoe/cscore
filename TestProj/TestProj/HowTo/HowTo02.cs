using CSCore;
using CSCore.Codecs;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProj.HowTo
{
    public class HowTo02
    {
        public IWaveSource CreateAWaveSourceFromFile(string filename)
        {
            try
            {
                return CodecFactory.Instance.GetCodec(filename);
            }
            catch(Exception)
            {
                //Any Exception occurred.
                //Maybe the fileformat is not supported or the file could not be found.
                //Handle the Exception here:
                //...

                throw;
            }
        }

        public IWaveSource CreateASineSource()
        {
            double frequency = 1200;
            double amplitude = 0.6;
            double phase = 0.0;

            //Create a ISampleSource
            ISampleSource sampleSource = new SineGenerator(frequency, amplitude, phase);

            //Convert the ISampleSource into a IWaveSource
            return sampleSource.ToWaveSource();
        }
    }
}
