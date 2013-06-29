using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.OGG
{
    public class CRC32
    {
        static uint[] _crcTable;
        const uint _crcPoly = 0x04C11DB7;

        static CRC32()
        {
            _crcTable = new uint[256]; ;
            for (uint i = 0; i < _crcTable.Length; i++)
            {
                uint s = i << 24;
                for (int n = 0; n < 8; ++n)
                {
                    s = (s << 1) ^ (s >= (1U << 31) ? _crcPoly : 0);
                }
                _crcTable[i] = s;
            }
        }

        uint _crc = 0;

        public uint Value { get { return _crc; } }

        public void Add(byte value)
        {
            _crc = (_crc << 8) ^ _crcTable[value ^ (_crc >> 24)];
        }

        public unsafe void Add(byte* pbuffer, int count)
        {
            byte* p = pbuffer;
            for (int i = 0; i < count; i++)
            {
                Add(*(p++));
            }
        }

        public void Add(byte[] buffer, int offset, int count)
        {
            unsafe
            {
                fixed (byte* ptrBuffer = buffer)
                {
                    Add(ptrBuffer + offset, count);
                }
            }
        }
    }
}
