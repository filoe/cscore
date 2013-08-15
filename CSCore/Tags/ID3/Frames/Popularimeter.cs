namespace CSCore.Tags.ID3.Frames
{
    public class Popularimeter : Frame
    {
        public string UserEmail { get; private set; }

        /// <summary>
        /// Range from 1(worst) to 255(best). Zero -> Rating disabled.
        /// </summary>
        public byte Rating { get; private set; }

        /// <summary>
        /// - 1 -> ommit the counter. Default length is 4 byte. If 4 byte is not enough to hold the
        ///   number, a byte will be added(up to 8 bytes total).
        /// </summary>
        public long PlayedCounter { get; private set; }

        public Popularimeter(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            int offset = 0;
            int read;

            UserEmail = ID3Utils.ReadString(content, 0, -1, ID3Utils.Iso88591, out read);
            offset += read;

            Rating = content[offset];
            offset++;

            if (offset < content.Length)
            {
                int pos = 0;
                for (int i = offset; i < content.Length; i++)
                {
                    PlayedCounter |= ((uint)(content[i] << pos)); //cast to uint to fix warning CS0675
                    pos += 8;
                }
            }
        }
    }
}