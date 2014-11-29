/* libFLAC - Free Lossless Audio Codec library
 * Copyright (C) 2000,2001,2002,2003,2004,2005,2006,2007,2008,2009  Josh Coalson
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * - Redistributions of source code must retain the above copyright
 * notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above copyright
 * notice, this list of conditions and the following disclaimer in the
 * documentation and/or other materials provided with the distribution.
 *
 * - Neither the name of the Xiph.org Foundation nor the names of its
 * contributors may be used to endorse or promote products derived from
 * this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE FOUNDATION OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using CSCore.Utils;

namespace CSCore.Codecs.FLAC
{
    public class FlacBitReader : BitReader
    {
        /*unary table from codeproject*/

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

        [CLSCompliant(false)]
        public unsafe FlacBitReader(byte* buffer, int offset)
            : base(buffer, offset)
        {
        }

        [CLSCompliant(false)]
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

        [CLSCompliant(false)]
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

        [CLSCompliant(false)]
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