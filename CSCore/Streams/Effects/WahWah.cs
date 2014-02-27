/**********************************************************************

  Audacity: A Digital Audio Editor

  Wahwah.cpp

  Effect programming:
  Nasca Octavian Paul (Paul Nasca)

  UI programming:
  Dominic Mazzoni (with the help of wxDesigner)
  Vaughan Johnson (Preview)

*******************************************************************/

using System;

namespace CSCore.Streams.Effects
{
    public class WahWah : EffectBase
    {
        public WahWah(ISampleSource source)
            : base(source)
        {
            freq = 1.5f;
            startphase = 0;
            depth = 0.7f;
            freqofs = 0.3f;
            res = 2.5f;

            //setup:
            lfoskip = (float)(freq * 2 * Math.PI / WaveFormat.SampleRate);
            _skipcount = 0;
            xn1 = 0;
            xn2 = 0;
            yn1 = 0;
            yn2 = 0;
            b0 = 0;
            b1 = 0;
            b2 = 0;
            a0 = 0;
            a1 = 0;
            a2 = 0;

            _phase = startphase;
            //todo
        }

        private ulong _skipcount;
        private float _phase, lfoskip;
        private float xn1, xn2, yn1, yn2;
        private float b0, b1, b2, a0, a1, a2;

        private float freq, startphase;
        private float depth, freqofs, res;

        private const int lfoskipsamples = 30;

        protected override unsafe void Process(float* buffer, int count)
        {
            int channels = WaveFormat.Channels;

            float frequency, omega, sn, cs, alpha;
            float input, output;

            int channel = 1;

            for (int i = 0; i < count; i++)
            {
                if (channel == 2)
                    _phase += (float)Math.PI;

                input = buffer[i];
                if ((_skipcount++) % lfoskipsamples == 0)
                {
                    frequency = (float)(1 + Math.Cos(_skipcount * lfoskip + _phase)) / 2;
                    frequency = frequency * depth * (1 - freqofs) + freqofs;
                    frequency = (float)Math.Exp((frequency - 1) * 6);

                    omega = (float)(Math.PI * frequency);
                    sn = (float)Math.Sin(omega);
                    cs = (float)Math.Cos(omega);
                    alpha = (float)(sn / (2 * res));

                    b0 = (float)((1 - cs) / 2);
                    b1 = (float)((1 - cs));
                    b2 = (float)((1 - cs) / 2);

                    a0 = 1 + alpha;
                    a1 = -2 * cs;
                    a2 = 1 - alpha;
                }

                output = (b0 * input + b1 * xn1 + b2 * xn2 - a1 * yn1 - a2 * yn2) / a0;
                xn2 = xn1;
                xn1 = input;
                yn2 = yn1;
                yn1 = output;

                if (output > 1.0f)
                    output = 1.0f;
                else if (output < -1.0f)
                    output = -1.0f;

                buffer[i] = (float)output;

                if (channel == 2)
                {
                    _phase -= (float)Math.PI;
                    channel++;
                    if (channel > channels)
                        channel = 1;
                }
            }
        }
    }
}