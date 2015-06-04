using CSCore.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class CachedSoundSourceTests : GenericSourceTests
    {
        private IWaveSource _source;

        protected override IWaveSource GetSource()
        {
            _source = GetMockSource();
            return new CachedSoundSource(_source);
        }

        [TestMethod, TestCategory("Streams")]
        public void IsSourceCachedTest()
        {
            var cachedSoundSource = (CachedSoundSource) SourceToTest;
            //use a delta of blockalgin because some decoders may cause bugs
            Assert.AreEqual(_source.Length, cachedSoundSource.Length, cachedSoundSource.WaveFormat.BlockAlign * 10);
        }
    }
}