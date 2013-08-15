using System;
using System.Collections.Generic;
using System.Text;

namespace CSCore.Tags.ID3.Frames
{
    public class MultiStringTextFrame : TextFrame
    {
        private List<string> _strings;

        public List<string> Strings
        {
            get { return _strings ?? (_strings = new List<string>()); }
        }

        public MultiStringTextFrame(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            Encoding e;
            int index = 1;
            Strings.Clear();

            e = ID3Utils.GetEncoding(content, 0, 1);
            //int read = 0;
            //e = ID3Utils.GetEncoding2(content, 0, out read);
            //index = read;
            while (index < content.Length)
            {
                int r = 0;
                Strings.Add(ID3Utils.ReadString(content, index, -1, e, out r));
                index += r;
            }

            if (Strings.Count == 0)
                Strings.Add(String.Empty);
        }

        public override string Text
        {
            get
            {
                return Strings[0];
            }
        }
    }
}