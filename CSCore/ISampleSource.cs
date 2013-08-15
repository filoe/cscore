namespace CSCore
{
    public interface ISampleSource : IWaveStream
    {
        int Read(float[] buffer, int offset, int count);
    }
}