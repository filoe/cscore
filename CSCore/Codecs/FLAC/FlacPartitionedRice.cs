using System;

namespace CSCore.Codecs.FLAC
{
    public class FlacPartitionedRice
    {
        public int PartitionOrder { get; private set; }

        public FlacPartitionedRiceContent Content { get; private set; }

        public FlacEntropyCoding CodingMethod { get; private set; }

        public FlacPartitionedRice(int partitionOrder, FlacEntropyCoding codingMethod, FlacPartitionedRiceContent content)
        {
            PartitionOrder = partitionOrder;
            CodingMethod = codingMethod;
            Content = content;
        }

        public unsafe bool ProcessResidual(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int order)
        {
            data.Content.UpdateSize(PartitionOrder);

            int porder = PartitionOrder;
            FlacEntropyCoding codingMethod = CodingMethod;

            int psize = header.BlockSize >> porder;
            int res_cnt = psize - order;

            int ricelength = 4 + (int)codingMethod; //4bit = RICE I | 5bit = RICE II

            //residual
            int j = order;
            int* r = data.residualBuffer + j;

            int partitioncount = 1 << porder;

            for (int p = 0; p < partitioncount; p++)
            {
                if (p == 1) res_cnt = psize;
                int n = Math.Min(res_cnt, header.BlockSize - j);

                int k = Content.parameters[p] = (int)reader.ReadBits(ricelength);
                if (k == (1 << ricelength) - 1)
                {
                    k = (int)reader.ReadBits(5);
                    for (int i = n; i > 0; i--)
                    {
                        *(r) = reader.ReadBitsSigned((int)k);
                    }
                }
                else
                {
                    ReadFlacRiceBlock(reader, n, (int)k, r);
                    r += n;
                }
                j += n;
            }

            return true;
        }

        private unsafe void ReadFlacRiceBlock(FlacBitReader reader, int nvals, int riceParameter, int* ptrDest)
        {
            fixed (byte* putable = FlacBitReader.UnaryTable)
            {
                uint mask = (1u << riceParameter) - 1;
                if (riceParameter == 0)
                {
                    for (int i = 0; i < nvals; i++)
                    {
                        *(ptrDest++) = reader.ReadUnarySigned();
                    }
                }
                else
                {
                    for (int i = 0; i < nvals; i++)
                    {
                        uint bits = putable[reader.Cache >> 24];
                        uint msbs = bits;

                        while (bits == 8)
                        {
                            reader.SeekBits(8);
                            bits = putable[reader.Cache >> 24];
                            msbs += bits;
                        }

                        uint uval = 0;
                        if (riceParameter <= 16)
                        {
                            int btsk = riceParameter + (int)bits + 1;
                            uval = (msbs << riceParameter) | ((reader.Cache >> (32 - btsk)) & mask);
                            reader.SeekBits(btsk);
                        }
                        else
                        {
                            reader.SeekBits((int)(msbs & 7) + 1);
                            uval = (msbs << riceParameter) | ((reader.Cache >> (32 - riceParameter)));
                            reader.SeekBits(riceParameter);
                        }
                        *(ptrDest++) = (int)(uval >> 1 ^ -(int)(uval & 1));
                    }
                }
            }
        }
    }
}