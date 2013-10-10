using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class SingleBlockNotificationStream : SampleSourceBase
    {
        public event EventHandler<SingleBlockReadEventArgs> SingleBlockRead;

        public SingleBlockNotificationStream(IWaveStream source)
            : base(source)
        {
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            if (read != 0 && SingleBlockRead != null)
            {
                int channels = WaveFormat.Channels;
                for (int n = 0; n < read; n += channels)
                {
                    SingleBlockRead(this, new SingleBlockReadEventArgs(buffer, offset + n, channels));
                }
            }

            return read;
        }
    }
}
