using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Visualization
{
    public class BlockReadEventArgs : EventArgs
    {
        public float[] DataLeft { get; private set; }

        public float[] DataRight { get; private set; }

        public BlockReadEventArgs(float[] dataleft, float[] dataright)
        {
            if (dataleft == null && dataright == null)
                throw new ArgumentNullException("data", "at least dataleft or dataright must not be null");
            DataLeft = dataleft;
            DataRight = dataright;
        }
    }
}