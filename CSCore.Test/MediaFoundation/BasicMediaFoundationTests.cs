using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.MediaFoundation;
using CSCore.Win32;
using System.IO;

namespace CSCore.Test.MediaFoundation
{
    [TestClass]
    public class BasicMediaFoundationTests
    {
        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanCreateByteStreamFromIOStream()
        {
            var stream = new MemoryStream();
            var comstream = new ComStream(stream);
            var byteStream = MFInterops.IStreamToByteStream(comstream);
            Assert.IsNotNull(byteStream);
        }
    }
}
