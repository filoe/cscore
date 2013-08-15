using System;

namespace CSCore.Utils
{
    partial class CSMath
    {
        public abstract class CRCBase<T> where T : struct
        {
            protected readonly int tableSize = 256;
            protected ushort[] crc_table;

            protected void CalcTable(int bits)
            {
                if (bits != 8 && bits != 16)
                    throw new ArgumentOutOfRangeException("bits has to be 8 or 16");
                int polySumm = bits == 8 ? 0x07 : 0x8005;
                int bitmask = bits == 8 ? 0x00FF : 0xFFFF;
                crc_table = new ushort[tableSize];

                int poly = (ushort)(polySumm + (1 << bits));
                for (int i = 0; i < crc_table.Length; i++)
                {
                    int crc = i;
                    for (int n = 0; n < bits; n++)
                    {
                        if ((crc & (1 << (bits - 1))) != 0)
                            crc = ((crc << 1)
                                ^ poly);
                        else crc = crc << 1;
                    }
                    crc_table[i] = (UInt16)(crc & bitmask);
                }
            }

            public abstract T CalcCheckSum(byte[] buffer, int offset, int count);
        }
    }
}