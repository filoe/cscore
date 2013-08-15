namespace CSCore
{
    public interface IWaveSource : IWaveStream
    {
        int Read(byte[] buffer, int offset, int count);
    }
}