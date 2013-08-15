namespace CSCore
{
    public interface IWaveAggregator : IWaveSource
    {
        IWaveSource BaseStream { get; }
    }
}