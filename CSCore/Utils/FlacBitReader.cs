using System;
using CSCore.Utils;

namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// This method is based on the CUETools.NET BitReader (see http://sourceforge.net/p/cuetoolsnet/code/ci/default/tree/CUETools.Codecs/BitReader.cs)
    /// The author "Grigory Chudov" explicitly gave the permission to use the source as part of the cscore source code which got licensed under the ms-pl.
    /// </summary>
    internal class FlacBitReader : BitReader
    {
        internal static readonly byte[] UnaryTable =
        {
            8, 7, 6, 6, 5, 5, 5, 5, 4, 4, 4, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        public FlacBitReader(byte[] buffer, int offset)
            : base(buffer, offset)
        {
        }

        public unsafe FlacBitReader(byte* buffer, int offset)
            : base(buffer, offset)
        {
        }

        public uint ReadUnary()
        {
            uint result = 0;
            uint unaryindicator = Cache >> 24;

            while (unaryindicator == 0)
            {
                SeekBits(8);
                result += 8;
                unaryindicator = Cache >> 24;
            }

            result += UnaryTable[unaryindicator];
            SeekBits((int)(result & 7) + 1);
            return result;
        }

        public int ReadUnarySigned()
        {
            var value = ReadUnary();
            return (int)(value >> 1 ^ -((int)(value & 1)));
        }

        #region utf8

        public bool ReadUTF8_64Signed(out long result)
        {
            ulong r;
            var returnValue = ReadUTF8_64(out r);
            result = (long) r;
            return returnValue;
        }

        public bool ReadUTF8_64(out ulong result)
        {
            uint x = ReadBits(8);
            ulong v;
            int i;

            if ((x & 0x80) == 0)
            {
                v = x;
                i = 0;
            }
            else if ((x & 0xC0) != 0 && (x & 0x20) == 0)
            {
                v = x & 0x1F;
                i = 1;
            }
            else if ((x & 0xE0) != 0 && (x & 0x10) == 0) /* 1110xxxx */
            {
                v = x & 0x0F;
                i = 2;
            }
            else if ((x & 0xF0) != 0 && (x & 0x08) == 0) /* 11110xxx */
            {
                v = x & 0x07;
                i = 3;
            }
            else if ((x & 0xF8) != 0 && (x & 0x04) == 0) /* 111110xx */
            {
                v = x & 0x03;
                i = 4;
            }
            else if ((x & 0xFC) != 0 && (x & 0x02) == 0) /* 1111110x */
            {
                v = x & 0x01;
                i = 5;
            }
            else if ((x & 0xFE) != 0 && (x & 0x01) == 0)
            {
                v = 0;
                i = 6;
            }
            else
            {
                result = ulong.MaxValue;
                return false;
            }

            for (; i != 0; i--)
            {
                x = ReadBits(8);
                if ((x & 0xC0) != 0x80)
                {
                    result = ulong.MaxValue;
                    return false;
                }

                v <<= 6;
                v |= (x & 0x3F);
            }

            result = v;
            return true;
        }

        public bool ReadUTF8_32Signed(out int result)
        {
            uint r;
            var returnValue = ReadUTF8_32(out r);
            result = (int) r;
            return returnValue;
        }

        public bool ReadUTF8_32(out uint result)
        {
            uint v, x;
            int i;

            x = ReadBits(8);
            if ((x & 0x80) == 0)
            {
                v = x;
                i = 0;
            }
            else if ((x & 0xC0) != 0 && (x & 0x20) == 0)
            {
                v = x & 0x1F;
                i = 1;
            }
            else if ((x & 0xE0) != 0 && (x & 0x10) == 0) /* 1110xxxx */
            {
                v = x & 0x0F;
                i = 2;
            }
            else if ((x & 0xF0) != 0 && (x & 0x08) == 0) /* 11110xxx */
            {
                v = x & 0x07;
                i = 3;
            }
            else if ((x & 0xF8) != 0 && (x & 0x04) == 0) /* 111110xx */
            {
                v = x & 0x03;
                i = 4;
            }
            else if ((x & 0xFC) != 0 && (x & 0x02) == 0) /* 1111110x */
            {
                v = x & 0x01;
                i = 5;
            }
            else
            {
                result = uint.MaxValue;
                return false;
            }

            for (; i != 0; i--)
            {
                x = ReadBits(8);
                if ((x & 0xC0) != 0x80)
                {
                    result = uint.MaxValue;
                    return false;
                }

                v <<= 6;
                v |= (x & 0x3F);
            }

            result = v;
            return true;
        }

        #endregion utf8
    }
}