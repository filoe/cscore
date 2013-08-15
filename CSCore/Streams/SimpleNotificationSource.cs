using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class SimpleNotificationSource : SampleSourceBase
    {
        public EventHandler DataRead;

        public SimpleNotificationSource(IWaveStream source)
            : base(source)
        {
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);

            if (DataRead != null)
                DataRead(this, EventArgs.Empty);

            return read;
        }
    }
}