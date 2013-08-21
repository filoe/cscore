using System.Diagnostics;
namespace CSCore.Codecs.FLAC
{
    public class FlacSubFrameBase
    {
        public unsafe static FlacSubFrameBase GetSubFrame(FlacBitReader reader, FlacSubFrameData data, FlacFrameHeader header, int bps)
        {
            uint x;
            int wastedBits = 0, totalBits = 0, order = 0;

            x = reader.ReadBits(8);
            bool haswastedBits = (x & 1) != 0;
            x &= 0xFE; //1111 1110

            if (haswastedBits)
            {
                int u = (int)reader.ReadUnary();
                wastedBits = u + 1;
                bps -= wastedBits;
                totalBits = bps;
            }

            if ((x & 0x80) != 0)
            {
                Debug.WriteLine("Flacdecoder lost sync while reading FlacSubFrameHeader. [x & 0x80].");
                return null;
            }

            FlacSubFrameBase subFrame = null;

            if ((x > 2 && x < 16) ||
                 (x > 24 && x < 64))
            {
                Debug.WriteLine("Invalid FlacSubFrameHeader. [" + x.ToString("x") + "]");
                return null;
            }
            else if (x == 0)
            {
                subFrame = new FlacSubFrameConstant(reader, header, data, bps);
            }
            else if (x == 2)
            {
                //verbatim
                subFrame = new FlacSubFrameVerbatim(reader, header, data, bps);
            }
            else if (x >= 16 && x <= 24)
            {
                //fixed
                order = (int)((x >> 1) & 7);
                subFrame = new FlacSubFrameFixed(reader, header, data, bps, order);
            }
            else if (x >= 64)
            {
                //lpc
                order = (int)(((x >> 1) & 31) + 1);
                subFrame = new FlacSubFrameLPC(reader, header, data, bps, order);
            }
            else
            {
                Debug.WriteLine("Invalid Flac-SubframeType: x = " + x + ".");
                return null;
            }

            if (haswastedBits)
            {
                int* ptrDest = data.destBuffer;
                for (int i = 0; i < header.BlockSize; i++)
                {
                    *(ptrDest++) <<= wastedBits;
                }
            }

            //System.Diagnostics.Debug.WriteLine(subFrame.GetType().Name);

            if (subFrame != null)
                subFrame.WastedBits = wastedBits;
            else
                Debug.WriteLine("Unknown error while reading FlacSubFrameHeader");

            return subFrame;
        }

        protected int _wastedBits;

        public int WastedBits
        {
            get { return _wastedBits; }
            protected set { _wastedBits = value; }
        }

        protected FlacFrameHeader _header;

        public FlacFrameHeader Header
        {
            get { return _header; }
            protected set { _header = value; }
        }

        protected FlacSubFrameBase(FlacFrameHeader header)
        {
            Header = header;
        }
    }
}