namespace CSCore.Codecs.FLAC
{
    public unsafe class FlacPartitionedRiceContent
    {
        public int[] parameters;
        public int[] raw_bits;

        private int capByOrder = -1;

        public void UpdateSize(int po)
        {
            if (capByOrder < po)
            {
                int size = 1 << po;
                parameters = new int[size];
                raw_bits = new int[size];

                capByOrder = po;
            }
        }
    }
}