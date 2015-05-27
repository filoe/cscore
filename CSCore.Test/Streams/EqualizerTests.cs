using CSCore.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class EqualizerTests : GenericSourceTests
    {
        protected override IWaveSource GetSource()
        {
            var source = GetMockSource();
            return Equalizer.Create10BandEqualizer(source.ToSampleSource()).ToWaveSource();
        }
    }
}