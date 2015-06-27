using System;
using System.IO;
using CSCore.Codecs.AIFF;
using CSCore.Test.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class AiffDecoderTests : GenericSourceTests
    {
        protected override IWaveSource GetSource()
        {
            return new AiffReader(new MemoryStream(Resources.aiff_50s_sine));
        }
    }
}
