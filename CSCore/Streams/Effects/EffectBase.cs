namespace CSCore.Streams.Effects
{
    public abstract class EffectBase : SampleSourceBase
    {
        public EffectBase(ISampleSource source)
            : base(source)
        {
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);

            Process(buffer, offset, read);

            return read;
        }

        protected virtual void Process(float[] buffer, int offset, int count)
        {
            unsafe
            {
                fixed (float* ptrBuffer = buffer)
                {
                    Process(ptrBuffer + offset, count);
                }
            }
        }

        protected virtual unsafe void Process(float* buffer, int count)
        {
        }
    }
}