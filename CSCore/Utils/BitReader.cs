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
using System.Runtime.InteropServices;

namespace CSCore.Utils
{
    public unsafe class BitReader : IDisposable
    {
        private readonly byte* _storedBuffer;
        private int _bitoffset;
        private byte* _buffer;
        private uint _cache;
        private GCHandle _hBuffer;
        private int _position;

        public BitReader(byte[] buffer, int offset)
        {
            if (buffer == null || buffer.Length <= 0)
                throw new ArgumentException("buffer is null or has no elements", "buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");

            _hBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            _buffer = _storedBuffer = (byte*) _hBuffer.AddrOfPinnedObject().ToPointer() + offset;

            _cache = PeekCache();
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="offset">offset in bits</param>
        [CLSCompliant(false)]        
        public BitReader(byte* buffer, int offset)
        {
            if (new IntPtr(buffer) == IntPtr.Zero)
                throw new ArgumentNullException("buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");

            int byteoffset = offset / 8;

            _buffer = _storedBuffer = buffer + byteoffset;
            _bitoffset = offset % 8;

            _cache = PeekCache();
        }

        [CLSCompliant(false)]
        protected internal uint Cache
        {
            get { return _cache; }
        }

        protected internal int CacheSigned
        {
            get { return (int) Cache; }
        }

        [CLSCompliant(false)]
        public byte* Buffer
        {
            get { return _storedBuffer; }
        }

        public IntPtr BufferPtr
        {
            get { return new IntPtr(Buffer);}
        }

        public int Position
        {
            get { return _position; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private uint PeekCache()
        {
            unchecked
            {
                byte* ptr = _buffer;
                uint result = *(ptr++);
                result = (result << 8) + *(ptr++);
                result = (result << 8) + *(ptr++);
                result = (result << 8) + *(ptr++);

                return result << _bitoffset;
            }
        }

        public void SeekBytes(int bytes)
        {
            if (bytes <= 0)
                throw new ArgumentOutOfRangeException("bytes");

            SeekBits(bytes * 8);
        }

        public void SeekBits(int bits)
        {
            if (bits <= 0)
                throw new ArgumentOutOfRangeException("bits");

            int tmp = _bitoffset + bits;
            _buffer += tmp >> 3; //skip bytes
            _bitoffset = tmp & 7; //bitoverflow -> max 7 bit

            _cache = PeekCache();

            _position += tmp >> 3;
        }

        [CLSCompliant(false)]
        public uint ReadBits(int bits)
        {
            if (bits <= 0 || bits > 32)
                throw new ArgumentOutOfRangeException("bits", "bits has to be a value between 1 and 32");

            uint result = _cache >> 32 - bits;
            if (bits <= 24)
            {
                SeekBits(bits);
                return result;
            }

            SeekBits(24);
            result |= _cache >> 56 - bits;
            SeekBits(bits - 24);

            return result;
        }

        public int ReadBitsSigned(int bits)
        {
            if (bits <= 0 || bits > 32)
                throw new ArgumentOutOfRangeException("bits", "bits has to be a value between 1 and 32");

            var result = (int) ReadBits(bits);
            result <<= (32 - bits);
            result >>= (32 - bits);
            return result;
        }

        [CLSCompliant(false)]
        public ulong ReadBits64(int bits)
        {
            if (bits <= 0 || bits > 64)
                throw new ArgumentOutOfRangeException("bits", "bits has to be a value between 1 and 32");

            ulong result = ReadBits(Math.Min(24, bits));
            if (bits <= 24)
                return result;

            bits -= 24;
            result = (result << bits) | ReadBits(Math.Min(24, bits));
            if (bits <= 24)
                return result;

            bits -= 24;
            return (result << bits) | ReadBits(bits);
        }

        public long ReadBits64Signed(int bits)
        {
            if (bits <= 0 || bits > 64)
                throw new ArgumentOutOfRangeException("bits", "bits has to be a value between 1 and 32");

            var result = (long) ReadBits64(bits);
            result <<= (64 - bits);
            result >>= (64 - bits);
            return result;
        }

        public Int16 ReadInt16()
        {
            return (Int16) ReadBitsSigned(16);
        }

        [CLSCompliant(false)]
        public UInt16 ReadUInt16()
        {
            return (UInt16) ReadBits(16);
        }

        public Int32 ReadInt32()
        {
            return ReadBitsSigned(32);
        }

        [CLSCompliant(false)]

        public UInt32 ReadUInt32()
        {
            return ReadBits(32);
        }

        [CLSCompliant(false)]
        public UInt64 ReadUInt64()
        {
            return ReadBits64(64);
        }

        public Int64 ReadInt64()
        {
            return ReadBits64Signed(64);
        }

        public bool ReadBit()
        {
            return ReadBitI() == 1;
        }

        public int ReadBitI()
        {
            uint result = _cache >> 31;
            SeekBits(1);
            return (int) result;
        }

        public void Flush()
        {
            if (_bitoffset > 0 && _bitoffset <= 8)
                SeekBits(8 - _bitoffset);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_hBuffer.IsAllocated)
                _hBuffer.Free();
        }

        ~BitReader()
        {
            Dispose(false);
        }
    }
}