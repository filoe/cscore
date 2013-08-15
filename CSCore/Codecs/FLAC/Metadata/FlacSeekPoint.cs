namespace CSCore.Codecs.FLAC
{
    public class FlacSeekPoint
    {
        public long Number { get; private set; }

        public long Offset { get; private set; }

        public int FrameSize { get; private set; }

        public FlacSeekPoint()
        {
        }

        public FlacSeekPoint(long number, long offset, int frameSize)
        {
            Number = number;
            Offset = offset;
            FrameSize = frameSize;
        }
    }
}