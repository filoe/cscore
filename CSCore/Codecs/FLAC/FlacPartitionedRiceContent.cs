namespace CSCore.Codecs.FLAC
{
    public unsafe class FlacPartitionedRiceContent
    {
        public int[] Parameters;
        public int[] RawBits;

        private int _capByOrder = -1;

        public void UpdateSize(int po)
        {
            if (_capByOrder < po)
            {
                int size = 1 << po;
                Parameters = new int[size];
                RawBits = new int[size];

                _capByOrder = po;
            }
        }
    }
}