using System;

namespace CSCore.Test.Utils
{
    internal class TrimmedWaveSource : WaveAggregatorBase
    {
        private long _readBytes;
        private readonly long _lengthInBytes;

        public TrimmedWaveSource(IWaveSource source, TimeSpan length)
            : base(source)
        {
            _lengthInBytes = source.GetRawElements(length);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            _readBytes += read;
            if (_readBytes > _lengthInBytes - 1)
            {
                read -= (int) (_readBytes - (_lengthInBytes - 1));
                _readBytes = _lengthInBytes - 1;
            }

            return read;
        }

        public override long Length
        {
            get { return _lengthInBytes; }
        }

        public override long Position
        {
            get { return Math.Min(base.Position, _lengthInBytes); }
            set
            {
                if (value > _lengthInBytes - 1)
                    throw new ArgumentOutOfRangeException("value");
                base.Position = value;
            }
        }
    }
}