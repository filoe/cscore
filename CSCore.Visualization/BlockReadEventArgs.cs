using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Visualization
{
    public class BlockReadEventArgs : EventArgs
    {
        public float[] Data { get; private set; }

        public BlockReadEventArgs(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("Length of data is zero");
            Data = data;
        }
    }
}
