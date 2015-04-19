using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test
{
    [TestClass]
    public class TimeConverterTests
    {
        [TestMethod]
        public void TimeConverterAttributeTest()
        {
            var timeConverter = TimeConverterFactory.Instance.GetTimeConverterForSource<Foo>();
            Assert.IsTrue(timeConverter is FooTimeConverter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MissingTimeConverterAttributeTest()
        {
            TimeConverterFactory.Instance.GetTimeConverterForSource<Foo1>();
        }

        [TestMethod]
        public void WaveSourceTimeConverterTest()
        {
            using (var waveSource = GlobalTestConfig.TestWav2S())
            {
                var timeConverter = TimeConverterFactory.Instance.GetTimeConverterForSource(waveSource);
                var totalMs = timeConverter.ToTimeSpan(waveSource.WaveFormat, waveSource.Length).TotalMilliseconds;
                Assert.AreEqual(2000, totalMs);
                Assert.AreEqual(waveSource.Length, timeConverter.ToRawElements(waveSource.WaveFormat, TimeSpan.FromMilliseconds(totalMs)));
            }
        }

        [TestMethod]
        public void SampleSourceTimeConverterTest()
        {
            using (var sampleSource = GlobalTestConfig.TestWav2S().ToSampleSource())
            {
                var timeConverter = TimeConverterFactory.Instance.GetTimeConverterForSource(sampleSource);
                var totalMs = timeConverter.ToTimeSpan(sampleSource.WaveFormat, sampleSource.Length).TotalMilliseconds;
                Assert.AreEqual(2000, totalMs);
                Assert.AreEqual(sampleSource.Length, timeConverter.ToRawElements(sampleSource.WaveFormat, TimeSpan.FromMilliseconds(totalMs)));
            }
        }

        private class FooTimeConverter : TimeConverter
        {
            public override long ToRawElements(WaveFormat waveFormat, TimeSpan timeSpan)
            {
                throw new NotImplementedException();
            }

            public override TimeSpan ToTimeSpan(WaveFormat waveFormat, long rawElements)
            {
                throw new NotImplementedException();
            }
        }

        private class Foo1 : IWaveSource, ISampleSource
        {
            public int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public int Read(float[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool CanSeek { get; private set; }
            public WaveFormat WaveFormat { get; private set; }
            public long Position { get; set; }
            public long Length { get; private set; }
        }

        [TimeConverterAttribute(typeof(FooTimeConverter))]
        private class Foo : IWaveSource, ISampleSource
        {
            public int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public int Read(float[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool CanSeek { get; private set; }
            public WaveFormat WaveFormat { get; private set; }
            public long Position { get; set; }
            public long Length { get; private set; }
        }
    }
}
