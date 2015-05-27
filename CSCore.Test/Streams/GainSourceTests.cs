using CSCore.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class GainSourceTests : GenericSourceTests
    {
        protected override IWaveSource GetSource()
        {
            var source = GetMockSource();
            return new GainSource(source.ToSampleSource()) {Volume = 1.2f}.ToWaveSource();
        }
    }
}