using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CSCore.MediaFoundation;
using System.Threading;

namespace CSCore.Test.MediaFoundation
{
    [TestClass]
    public class MediafoundationEncoderTests
    {
        private string _targetfilename;

        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize]
        public void OnInitialize()
        {
            string extension =
                ((OutputFileExtensionAttribute)
                    GetType()
                        .GetMethod(TestContext.TestName)
                        .GetCustomAttributes(typeof (OutputFileExtensionAttribute), false)
                        .First()).Extension;

            Debug.WriteLine("Extension: " + extension);

            _targetfilename = Path.ChangeExtension(Path.GetTempFileName(), extension);
        }

        [TestCleanup]
        public void OnCleanup()
        {
            if(File.Exists(_targetfilename))
                File.Delete(_targetfilename);
        }



        [TestMethod]
        [TestCategory("MediaFoundation")]
        [OutputFileExtension(Extension = "mp4")]
        public void CanEncodeToAAC()
        {
            using (var source = GlobalTestConfig.TestWav2S())
            {
                using (var encoder = MediaFoundationEncoder.CreateAACEncoder(source.WaveFormat, _targetfilename))
                {
                    MediaFoundationEncoder.EncodeWholeSource(encoder, source);
                }
            }
        }


        [TestMethod]
        [TestCategory("MediaFoundation")]
        [OutputFileExtension(Extension = "wma")]
        public void CanEncodeToWMA()
        {
            using (var source = GlobalTestConfig.TestWav2S())
            {
                using (var encoder = MediaFoundationEncoder.CreateWMAEncoder(source.WaveFormat, _targetfilename))
                {
                    MediaFoundationEncoder.EncodeWholeSource(encoder, source);
                }
            }
        }

        [TestMethod]
        [TestCategory("MediaFoundation")]
        [OutputFileExtension(Extension = "mp3")]
        public void CanEncodeToMP3()
        {
            using (var source = GlobalTestConfig.TestWav2S())
            {
                using (var encoder = MediaFoundationEncoder.CreateMP3Encoder(source.WaveFormat, _targetfilename))
                {
                    MediaFoundationEncoder.EncodeWholeSource(encoder, source);
                }
            }
        }

        private class OutputFileExtensionAttribute : Attribute
        {
            public string Extension { get; set; }
        }
    }
}
