using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.Utils
{
    [TestClass]
    public class ILUtilsTests
    {
        [TestMethod]
        public void MemoryCopyTest()
        {
            Random random = new Random();
            int[] source = new int[random.Next(100000, 200000)];
            int[] destination = new int[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                source[i] = random.Next();
            }

            ILUtils.MemoryCopy(destination, source, sizeof(int) * source.Length);

            CollectionAssert.AreEqual(source, destination, "Something went wrong while copying the memory from source to destination.");
        }

        [TestMethod]
        public unsafe void MemoryCopyPointerTest()
        {
            Random random = new Random();

            int length = random.Next(100000, 200000);

            int* source = stackalloc int[length];
            int* destination = stackalloc int[length];

            for (int i = 0; i < length; i++)
            {
                source[i] = random.Next();
            }

            ILUtils.MemoryCopy(destination, source, sizeof(int) * length);

            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(source[i], destination[i], "Something went wrong while copying the memory from source to destination.");
            }
        }
    }
}
