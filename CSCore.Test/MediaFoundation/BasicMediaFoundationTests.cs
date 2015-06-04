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
            var stream = new MemoryStream();
            var comstream = new ComStream(stream);
            using (var byteStream = MediaFoundationCore.IStreamToByteStream(comstream))
            {
                Assert.IsNotNull(byteStream);
            }
        }

        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanCreateSourceReaderFromUrl()
        {
            var filename = Path.ChangeExtension(Path.GetTempFileName(), "mp3");
            try
            {
                using (var stream = GlobalTestConfig.TestMp3AsStream())
                {
                    File.WriteAllBytes(filename, stream.ToArray());
                    using (var reader = MediaFoundationCore.CreateSourceReaderFromUrl(filename))
                    {
                        Assert.IsNotNull(reader);
                    }
                }
            }
            finally
            {
                File.Delete(filename);
            }
        }

        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanCreateSourceReaderFromIOStream()
        {
            using(var stream = GlobalTestConfig.TestMp3AsStream())
            using (var comstream = new ComStream(stream))
            {
                using (var byteStream = MediaFoundationCore.IStreamToByteStream(comstream))
                {
                    Assert.IsNotNull(byteStream);

                    using (
                        var reader = MediaFoundationCore.CreateSourceReaderFromByteStream(byteStream.BasePtr,
                            IntPtr.Zero))
                    {
                        Assert.IsNotNull(reader);
                    }
                }
            }
        }

        //will only run on windows 7 and above
        [TestMethod]
        [TestCategory("MediaFoundation")]
        public void CanEnumerateMFDecodersEx()
        {
            var arr =
                MFTEnumerator.EnumerateTransformsEx(MFTCategories.AudioDecoder, MFTEnumFlags.All)
                    .Select(x => x[MediaFoundationAttributes.MFT_TRANSFORM_CLSID_Attribute])
                    .ToArray();
        }

        [TestMethod]
        public void CanEnumerateMFDecoders()
        {
            MFTEnumerator.EnumerateTransforms(MFTCategories.AudioDecoder);
        }
    }
}