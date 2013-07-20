
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.Utils;
using System.Collections.Generic;

namespace CSCore.Test
{
    [TestClass]
    public class BitReaderTest
    {
        [TestMethod]
        [TestCategory("Utils")]
        public void RandomBitReaderTest()
        {
            Random rand = new Random();
            byte[] buffer = new byte[50];
            rand.NextBytes(buffer);

            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    BitReader reader = new BitReader(ptr, 0);
                    reader.ReadBit();
                    reader.ReadBits(10);
                    reader.ReadBits(15);
                    reader.SeekBits(11);
                    reader.ReadInt64();
                    reader.ReadUInt32();
                    reader.ReadUInt16();

                    reader.Dispose();
                }
            }
        }

        public unsafe void RandomBitReaderTest0()
        {
            Random rand = new Random();
            byte[] buffer = new byte[rand.Next(50, 100)];
            rand.NextBytes(buffer);

            fixed (byte* ptr = buffer)
            {
                BitReader reader = new BitReader(ptr, 0);



                reader.Dispose();
            }
        }
    }
}
