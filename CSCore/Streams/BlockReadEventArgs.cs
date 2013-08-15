using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class BlockReadEventArgs<T> : EventArgs
    {
        public int Length { get; private set; }

        public T[] Data { get; private set; }

        public BlockReadEventArgs(T[] data, int length)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (length < 0)
                throw new ArgumentNullException("length");

            Data = data;
            Length = length;
        }
    }
}