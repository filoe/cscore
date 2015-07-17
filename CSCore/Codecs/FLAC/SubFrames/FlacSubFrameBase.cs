using System;
using System.Diagnostics;
// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    internal class FlacSubFrameBase
    {
        public unsafe static FlacSubFrameBase GetSubFrame(FlacBitReader reader, FlacSubFrameData data, FlacFrameHeader header, int bitsPerSample)
        {
            int wastedBits = 0, order;

            uint firstByte = reader.ReadBits(8);

            if ((firstByte & 0x80) != 0) //Zero bit padding, to prevent sync-fooling string of 1s
            {
                Debug.WriteLine("Flacdecoder subframe-header got no zero-bit padding.");
                return null;
            }

            bool hasWastedBits = (firstByte & 1) != 0; //Wasted bits-per-sample' flag
            if (hasWastedBits)
            {
                int k = (int)reader.ReadUnary();
                wastedBits = k + 1; //"k-1" follows -> add 1
                bitsPerSample -= wastedBits;
            }

            FlacSubFrameBase subFrame;
            var subframeType = (firstByte & 0x7E) >> 1; //0111 1110

            if (subframeType == 0) //000000
            {
                subFrame = new FlacSubFrameConstant(reader, header, data, bitsPerSample);
            }
            else if (subframeType == 1) //000001
            {
                subFrame = new FlacSubFrameVerbatim(reader, header, data, bitsPerSample);
            }
            else if ((subframeType & 0x20) != 0) //100000 = 0x20
            {
                order = (int)(subframeType & 0x1F) + 1;
                subFrame = new FlacSubFrameLPC(reader, header, data, bitsPerSample, order);
            }
            else if ((subframeType & 0x08) != 0) //001000 = 0x08
            {
                order = (int) (subframeType & 0x07);
                if (order > 4) return null;
                subFrame = new FlacSubFrameFixed(reader, header, data, bitsPerSample, order);
            }
            else
            {
                Debug.WriteLine(String.Format("Invalid Flac-SubframeType. SubframeType: 0x{0:x}.", subframeType));
                return null;
            }

            if (hasWastedBits)
            {
                int* destination = data.DestinationBuffer;
                for (int i = 0; i < header.BlockSize; i++)
                {
                    *(destination++) <<= wastedBits;
                }
            }

#if FLAC_DEBUG
            subFrame.WastedBits = wastedBits;
#endif
            return subFrame;
        }

#if FLAC_DEBUG
        public int WastedBits { get; private set; }

        public FlacFrameHeader Header { get; private set; }
#endif

        protected FlacSubFrameBase(FlacFrameHeader header)
        {
#if FLAC_DEBUG
            Header = header;
#endif
        }
    }
}