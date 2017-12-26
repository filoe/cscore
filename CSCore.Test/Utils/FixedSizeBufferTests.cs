using System;
using System.Linq;
using CSCore.Utils.Buffer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Utils
{
    [TestClass]
    public class FixedSizeBufferTests
    {
        [TestMethod]
        public void FixedBufferCanReadWrite()
        {
            int length = 100;
            int readOffset = 10;

            FixedSizeBuffer<int> fixedSizeBuffer = new FixedSizeBuffer<int>(length);
            var numbers = Enumerable.Range(0, length).ToArray();
            var readData = new int[numbers.Length + readOffset];

            int written = fixedSizeBuffer.Write(numbers, 0, numbers.Length);
            Assert.AreEqual(numbers.Length, written);
            Assert.AreEqual(fixedSizeBuffer.Buffered, numbers.Length);

            fixedSizeBuffer.Read(readData, readOffset, length);
            Assert.AreEqual(fixedSizeBuffer.Buffered, 0);

            Array.Copy(readData, readOffset, readData, 0, length);
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(readData[i], numbers[i]);
            }
        }

        [TestMethod]
        public void FixedBufferCanOverwrite()
        {
            int dataLength = 100;
            int bufferLength = 30;
            int readOffset = 10;

            FixedSizeBuffer<int> fixedSizeBuffer = new FixedSizeBuffer<int>(bufferLength);
            var numbers = Enumerable.Range(0, dataLength).ToArray();
            var readData = new int[numbers.Length + readOffset];

            int written = fixedSizeBuffer.Write(numbers, 0, numbers.Length);
            Assert.AreEqual(numbers.Length, written);
            Assert.AreEqual(fixedSizeBuffer.Buffered, bufferLength);

            int read = fixedSizeBuffer.Read(readData, readOffset, readData.Length);
            Assert.AreEqual(read, bufferLength);
            Assert.AreEqual(fixedSizeBuffer.Buffered, 0);

            Array.Copy(readData, readOffset, readData, 0, dataLength);
            Array.Copy(numbers, dataLength - bufferLength, numbers, 0, bufferLength);
            for (int i = 0; i < bufferLength; i++)
            {
                Assert.AreEqual(readData[i], numbers[i]);
            }
        }

        [TestMethod]
        public void FixedBufferCanAdjustReadOffsetOnOverwrite()
        {
            int dataLength = 100;
            int bufferLength = 30;
            int readOffset = 10;

            int writeInitial = 10;
            int readInitial = 5;

            FixedSizeBuffer<int> fixedSizeBuffer = new FixedSizeBuffer<int>(bufferLength);
            var numbers = Enumerable.Range(0, dataLength).ToArray();
            var readData = new int[numbers.Length + readOffset];

            fixedSizeBuffer.Write(Enumerable.Range(5000, writeInitial).ToArray(), 0, writeInitial);
            fixedSizeBuffer.Read(readData, 0, readInitial);
            Array.Clear(readData, 0, readData.Length);

            int written = fixedSizeBuffer.Write(numbers, 0, numbers.Length);
            Assert.AreEqual(numbers.Length, written);
            Assert.AreEqual(fixedSizeBuffer.Buffered, bufferLength);

            int read = fixedSizeBuffer.Read(readData, readOffset, readData.Length);
            Assert.AreEqual(read, bufferLength);
            Assert.AreEqual(fixedSizeBuffer.Buffered, 0);

            Array.Copy(readData, readOffset, readData, 0, dataLength);
            Array.Copy(numbers, dataLength - bufferLength, numbers, 0, bufferLength);
            for (int i = 0; i < bufferLength; i++)
            {
                Assert.AreEqual(readData[i], numbers[i]);
            }
        }
    }
}
