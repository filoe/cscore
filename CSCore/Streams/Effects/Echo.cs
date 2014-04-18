using System;
using System.Collections.Generic;

namespace CSCore.Streams.Effects
{
    [Obsolete("Use DmoEchoEffect instead.")]
    public class Echo : EffectBase
    {
        private const float maxdelay = 10f;
        private float delay = 1.0f; //second
        private float decay = 0.45f;

        private Queue<float> backBuffer;

        public Echo(ISampleSource source)
            : base(source)
        {
            backBuffer = new Queue<float>();
        }

        protected override unsafe void Process(float* buffer, int count)
        {
            int delaySamples = DelayToSamples(delay);

            for (int i = 0; i < count; i++)
            {
                if (backBuffer.Count >= delaySamples)
                {
                    buffer[i] = (buffer[i] * (decay)) + backBuffer.Dequeue() * (1 - decay);
                }
                backBuffer.Enqueue(buffer[i]);
            }
        }

        private int DelayToSamples(float delay)
        {
            int delaySamples = (int)(delay * WaveFormat.SampleRate * WaveFormat.Channels);
            delaySamples -= (delaySamples % (WaveFormat.Channels));
            return delaySamples;
        }
    }
}