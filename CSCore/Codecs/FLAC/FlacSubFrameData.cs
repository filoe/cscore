using System;

namespace CSCore.Codecs.FLAC
{
    [CLSCompliant(false)]
    public unsafe class FlacSubFrameData
    {
        public int* DestBuffer;
        public int* ResidualBuffer;

        private FlacPartitionedRiceContent _content;

        public FlacPartitionedRiceContent Content
        {
            get { return _content ?? (_content = new FlacPartitionedRiceContent()); }
            set { _content = value; }
        }
    }
}