using System;
using System.Diagnostics;
using CSCore.Codecs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Streams
{
    [TestClass]
    public abstract class GenericSourceTests
    {
        private IWaveSource _sourceToTest;

        protected IWaveSource SourceToTest
        {
            get { return _sourceToTest; }
        }

        [TestInitialize]
        public virtual void OnInitialize()
        {
            PrintCurrentTestClass();

            _sourceToTest = GetSource();
            if(SourceToTest == null)
                throw new InvalidOperationException("No valid source.");
        }

        protected abstract IWaveSource GetSource();

        [TestCleanup]
        public virtual void OnCleanup()
        {
            PrintCurrentTestClass();

            SourceToTest.Dispose();
            _sourceToTest = null;
        }

        [TestMethod, TestCategory("Streams")]
        public virtual void ReadToEndTest()
        {
            PrintCurrentTestClass();

            ReadToEndTestInternal((read, source) => read > 0);
        }

        [TestMethod, TestCategory("Streams")]
        public virtual void ReadToEndAndPositionAdjustmentTest()
        {
            PrintCurrentTestClass();

            if (!SourceToTest.CanSeek)
                throw new InvalidOperationException("Specified source does not support seeking.");

            ReadToEndAndPositionAdjustmentTestInternal((read, source) => read > 0);
        }

        [TestMethod, TestCategory("Streams")]
        public virtual void CanSeekTest()
        {
            PrintCurrentTestClass();

            if(!SourceToTest.CanSeek)
                throw new InvalidOperationException("Specified source does not support seeking.");

            byte[] buffer = new byte[SourceToTest.WaveFormat.BytesPerSecond];

            long startPosition = SourceToTest.Position;
            int read = SourceToTest.Read(buffer, 0, buffer.Length);
            Assert.AreEqual(startPosition + read, SourceToTest.Position);

            SourceToTest.Position = SourceToTest.Length / 2;
            Assert.AreEqual(SourceToTest.Length / 2, SourceToTest.Position, "Position does not match.");
            startPosition = SourceToTest.Position;

            read = SourceToTest.Read(buffer, 0, buffer.Length);
            Assert.AreEqual(startPosition + read, SourceToTest.Position);

            SourceToTest.Position = 0;
        }

        [TestMethod, TestCategory("Streams")]
        public virtual void CanGetWaveFormat()
        {
            PrintCurrentTestClass();

            var waveFormat = SourceToTest.WaveFormat;
            Assert.IsNotNull(waveFormat, "WaveFormat of {0} is null.", SourceToTest.GetType().FullName);
        }


        protected virtual void ReadToEndTestInternal(Func<int, IWaveSource, bool> abortReadingFunc)
        {
            PrintCurrentTestClass();

            if (abortReadingFunc == null)
                throw new ArgumentNullException("abortReadingFunc");

            byte[] buffer = new byte[SourceToTest.WaveFormat.BytesPerSecond];

            while (abortReadingFunc((SourceToTest.Read(buffer, 0, buffer.Length)), SourceToTest))
            {
            }
        }

        protected virtual void ReadToEndAndPositionAdjustmentTestInternal(Func<int, IWaveSource, bool> abortReadingFunc)
        {
            PrintCurrentTestClass();

            if (abortReadingFunc == null)
                throw new ArgumentNullException("abortReadingFunc");

            byte[] buffer = new byte[SourceToTest.WaveFormat.BytesPerSecond];
            int read;
            long totalRead = 0, startPosition = 0;

            if (TestSeeking)
            {
                startPosition = SourceToTest.Position;
            }

            while (abortReadingFunc((read = SourceToTest.Read(buffer, 0, buffer.Length)), SourceToTest))
            {
                if (TestSeeking)
                {
                    totalRead += read;
                    Assert.AreEqual(totalRead + startPosition, SourceToTest.Position, SourceToTest.WaveFormat.BlockAlign * 10, "Position was not adjusted as expected.");
                }
                Debug.WriteLine("Read: {0}, total read: {1}", read, totalRead);
            }

            if (TestSeeking)
            {
                //use 10*blockalign as delta because for example the media foundation decoder can be a little bit imprecise. 
                Assert.AreEqual(SourceToTest.Position, SourceToTest.Length, SourceToTest.WaveFormat.BlockAlign * 10,
                    "Could not read to the end.");
            }
        }

        public bool TestSeeking
        {
            get { return SourceToTest.CanSeek; }
        }

        public static IWaveSource GetMockSource()
        {
            return GlobalTestConfig.TestMp3();
        }

        private void PrintCurrentTestClass()
        {
            Debug.WriteLine(GetType().FullName);
        }
    }
}
