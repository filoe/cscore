using CSCore.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class LoopStreamTests : GenericSourceTests
    {
        protected override IWaveSource GetSource()
        {
            var source = GetMockSource();
            return new LoopStream(source);
        }

        [TestMethod, TestCategory("Streams")]
        public override void ReadToEndTest()
        {
            long totalRead = 0;
            ReadToEndTestInternal((read, source) =>
            {
                totalRead += read;
                return totalRead <= source.Length || source.Position >= totalRead; //return false as the position got reseted. 
            });
        }

        [TestMethod, TestCategory("Streams")]
        public override void ReadToEndAndPositionAdjustmentTest()
        {
            long totalRead = 0;
            ReadToEndAndPositionAdjustmentTestInternal((read, source) =>
            {
                totalRead += read;
                var result = totalRead <= source.Length || source.Position >= totalRead; //return false as the position got reseted.
                if (!result)
                    source.Position = source.Length; //prevent error where position != length 
                return result;
            });
        }

        [TestMethod, TestCategory("Streams")]
        public void StreamFinishedEventGetsFiredTest()
        {
            bool eventFired = false;
            var loopSource = SourceToTest as LoopStream;
            Assert.IsNotNull(loopSource, "loopSource is null.");

            loopSource.StreamFinished += (s, e) =>
            {
                eventFired = true;
            };

            ReadToEndTest();

            Assert.IsTrue(eventFired, "StreamFinished-event was not fired.");
        }
    }
}