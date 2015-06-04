using CSCore.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class FadeInOutTests : GenericSourceTests
    {
        protected override IWaveSource GetSource()
        {
            var source = GetMockSource();
            return new FadeInOut(source.ToSampleSource()).ToWaveSource();
        }
    }
}