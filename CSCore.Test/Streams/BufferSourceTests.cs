using CSCore.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class BufferSourceTests : GenericSourceTests
    {
        protected override IWaveSource GetSource()
        {
            var source = GetMockSource();
            return new BufferSource(source, source.WaveFormat.BytesPerSecond * 2);
        }
    }
}