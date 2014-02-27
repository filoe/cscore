using System;

namespace CSCore.Streams
{
    /// <summary>
    /// A Stream which can be used for endless looping.
    /// </summary>
    public class LoopStream : WaveAggregatorBase
    {
        public event EventHandler StreamFinished;

        public LoopStream(IWaveSource source)
            : base(source)
        {
        }

        private bool _enalbeLoop = true;

        /// <summary>
        /// Gets or sets whether looping is enabled.
        /// </summary>
        public bool EnableLoop
        {
            get { return _enalbeLoop; }
            set { _enalbeLoop = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            while (read < count)
            {
                int r = base.Read(buffer, offset + read, count - read);
                if (r == 0)
                {
                    if (StreamFinished != null)
                    {
                        StreamFinished(this, EventArgs.Empty);
                    }
                    if (EnableLoop)
                    {
                        Position = 0;
                    }
                    else
                    {
                        break;
                    }
                }
                read += r;
            }
            return read;
        }
    }
}