using CSCore.MediaFoundation;
using CSCore.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CSCore.Test.MediaFoundation
{
    [TestClass]
    public class BasicMediaFoundationTests
    {
        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanCreateByteStreamFromIOStream()
        {
            MediaFoundationCore.Startup();
            var stream = new MemoryStream();
            var comstream = new ComStream(stream);
            var byteStream = MediaFoundationCore.IStreamToByteStream(comstream);
            Assert.IsNotNull(byteStream);
            Marshal.ReleaseComObject(byteStream);
            MediaFoundationCore.Shutdown();
        }

        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanCreateSourceReaderFromUrl()
        {
            MediaFoundationCore.Startup();
            using (var reader = MediaFoundationCore.CreateSourceReaderFromUrl(@"C:\Temp\test.mp3"))
            {
                Assert.IsNotNull(reader);
            }
            MediaFoundationCore.Shutdown();
        }

        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanCreateSourceReaderFromIOStream()
        {
            MediaFoundationCore.Startup();
            var stream = File.OpenRead(@"C:\Temp\test.mp3");
            using (var comstream = new ComStream(stream))
            {
                var byteStream = MediaFoundationCore.IStreamToByteStream(comstream);
                Assert.IsNotNull(byteStream);

                using (var reader = MediaFoundationCore.CreateSourceReaderFromByteStream(byteStream, IntPtr.Zero))
                {
                    Assert.IsNotNull(reader);
                }

                Marshal.ReleaseComObject(byteStream);
            }
            MediaFoundationCore.Shutdown();
        }

        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanEnumerateMFDecoders()
        {
            MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All);
        }
    }
}