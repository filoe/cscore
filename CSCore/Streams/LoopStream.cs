using System;

namespace CSCore.Streams
{
    public class LoopStream : WaveAggregatorBase
    {
        public event EventHandler StreamFinished;

        public LoopStream(IWaveSource source)
            : base(source)
        {
        }

        bool _enalbeLoop = true;
        public bool EnableLoop
        {
            get { return _enalbeLoop; }
            set { _enalbeLoop = value; }
        }

        bool raised = false;

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            while (read < count)
            {
                int r = base.Read(buffer, offset + read, count - read);
                if (r == 0)
                {
                    if (StreamFinished != null && !raised)
                    {
                        StreamFinished(this, new EventArgs());
                        raised = true;
                    }
                    if (_enalbeLoop)
                        _baseStream.Position = 0;
                    else break;
                }
                raised = false;
                read += r;
            }
            return read;
        }
    }
}
