using System;
using System.ComponentModel;
using System.Drawing;

namespace CSCore.Tags.ID3.Frames
{
    public class PictureFrame : Frame
    {
        public string MimeType { get; private set; }

        public PictureFormat Format { get; set; }

        public string Description { get; private set; }

        internal byte[] RawData { get; private set; }

        private Image _image;

        /// <summary>
        /// WARNING: If MimeType equals "-->" the picture will be downloaded from the web.
        /// Use GetURL() the get the url to the picture. If not, data, contained by the frame will
        /// be used.
        /// </summary>
        public Image Image
        {
            get { return _image ?? (_image = DecodeImage()); }
        }

        private Image DecodeImage()
        {
            return ID3Utils.DecodeImage(RawData, MimeType);
        }

        private ID3Version _version;

        public PictureFrame(FrameHeader header, ID3Version version)
            : base(header)
        {
            if (version == ID3Version.ID3v1)
                throw new InvalidEnumArgumentException("version", (int)version, typeof(ID3Version));
            _version = version;
        }

        protected override void Decode(byte[] content)
        {
            int offset = 1;
            if (content.Length < 3)
                throw new ID3Exception("Invalid contentlength id=0.");//id -> for debugging

            int read;
            if (_version == ID3Version.ID3v2_2)
            {
                //MimeType = ID3Utils.ReadString(content, offset, 3, ID3Utils.Iso88591, out read);
                MimeType = ID3Utils.ReadString(content, offset, 3, ID3Utils.Iso88591, out read);
                offset += 3;
            }
            else
            {
                MimeType = ID3Utils.ReadString(content, 1, -1, ID3Utils.Iso88591, out read);
                offset += read;
            }

            if (content.Length < offset)
                throw new ID3Exception("Invalid contentlength id=1.");

            if (!Enum.IsDefined(typeof(PictureFormat), content[offset]))
                throw new ID3Exception("Invalid pictureformat: 0x{0}", content[offset].ToString("x"));
            Format = (PictureFormat)content[offset++];

            if (content.Length < offset)
                throw new ID3Exception("Invalid contentlength id=2.");
            var descenc = ID3Utils.GetEncoding(content, 0, 2);
            Description = ID3Utils.ReadString(content, offset, -1, descenc, out read);
            offset += read;

            if (content.Length < offset)
                throw new ID3Exception("Invalid contentlength id=3.");
            RawData = new byte[content.Length - offset];
            Array.Copy(content, offset, RawData, 0, RawData.Length);
        }

        public string GetURL()
        {
            return ID3Utils.GetURL(RawData, MimeType);
        }
    }

    public enum PictureFormat : byte
    {
        None = 0x0,
        Icon32x32,
        OtherIcon = 0x02,
        CoverFront = 0x03,
        CoverBack = 0x04,
        LeafletPage = 0x05,
        Media = 0x06,
        LeadArtist = 0x07,
        Artist = 0x08,
        Conductor = 0x09,
        Band = 0x0A,
        Composer = 0x0B,
        Lyricist = 0x0C,
        RecordingLocation = 0x0D,
        DuringRecording = 0x0E,
        DuringPerformance = 0x0F,
        MovieCapture = 0x10,
        BrightColoredFish = 0x11,
        Illustration = 0x12,
        LogoArtist = 0x13,
        LogoPublisher = 0x14
    }
}