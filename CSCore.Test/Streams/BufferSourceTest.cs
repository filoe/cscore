using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.Streams;
using System.Diagnostics;

namespace CSCore.Test.Streams
{
    [TestClass]
    public class BufferSourceTest
    {
        //[Obsolete]
        //[TestMethod]
        //public void BufferSourcePositionTest()
        //{
        //    IWaveSource source0 = Codecs.CodecFactory.Instance.GetCodec(@"C:\Temp\download.mp3");
        //    IWaveSource source1 = Codecs.CodecFactory.Instance.GetCodec(@"C:\Temp\download.mp3");

        //    byte[] buffer = new byte[1000];


        //    Debug.WriteLine("Started");
        //    //source1 = new BufferSource(source1, source1.WaveFormat.BytesPerSecond * 4);
        //    int r0 = 0, r1 = 0;

        //    for (int i = 0; i < 15000; i++)
        //    {
        //        r0 += source0.Read(buffer, 0, buffer.Length);
        //        //r1 += source1.Read(buffer, 0, buffer.Length);
               
        //        if (r0 != source0.Position)
        //            Debug.WriteLine("bad");//throw new InvalidOperationException("r0 does not match");
        //        //if (r1 != source1.Position)
        //        //    throw new InvalidOperationException("r1 does not match");
        //    }

        //    source0.Dispose();
        //    source1.Dispose();
        //}
    }
}
