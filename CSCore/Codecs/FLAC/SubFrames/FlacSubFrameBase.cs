using System.Diagnostics;
namespace CSCore.Codecs.FLAC
{
    public class FlacSubFrameBase
    {
        public unsafe static FlacSubFrameBase GetSubFrame(FlacBitReader reader, FlacSubFrameData data, FlacFrameHeader header, int bps)
        {
            int wastedBits = 0, order = 0;

            uint x = reader.ReadBits(8);
            bool hasWastedBits = (x & 1) != 0;
            x &= 0xFE; //1111 1110

            if (hasWastedBits)
            {
                int u = (int)reader.ReadUnary();
                wastedBits = u + 1;
                bps -= wastedBits;
            }

            if ((x & 0x80) != 0)
            {
                Debug.WriteLine("Flacdecoder lost sync while reading FlacSubFrameHeader. [x & 0x80].");
                return null;
            }

            FlacSubFrameBase subFrame;

            if ((x > 2 && x < 16) ||
                 (x > 24 && x < 64))
            {
                Debug.WriteLine("Invalid FlacSubFrameHeader. [" + x.ToString("x") + "]");
                return null;
            }

            if (x == 0)
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

            if (hasWastedBits)
            {
                int* ptrDest = data.DestBuffer;
                for (int i = 0; i < header.BlockSize; i++)
                {
                    *(ptrDest++) <<= wastedBits;
                }
            }

            //System.Diagnostics.Debug.WriteLine(subFrame.GetType().Name);

            //check null removed
            subFrame.WastedBits = wastedBits;

            return subFrame;
        }

        public int WastedBits { get; protected set; }

        public FlacFrameHeader Header { get; protected set; }

        protected FlacSubFrameBase(FlacFrameHeader header)
        {
            Header = header;
        }
    }
}