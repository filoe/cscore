namespace CSCore.Utils
{
    public partial class CSMath
    {
        /// <summary>
        /// http: //flac.sourceforge.net/format.html#frame_header
        /// CRC-8 (polynomial = x^8 + x^2 + x^1 + x^0, initialized with 0) of everything before the
        /// crc, including the sync code
        /// </summary>
        public class CRC8 : CRCBase<byte>
        {
            private static CRC8 _instance;

            public static CRC8 Instance
            {
                get { return _instance ?? (_instance = new CRC8()); }
            }

            public CRC8()
            {
                CalcTable(8);
            }

            public override byte CalcCheckSum(byte[] buffer, int offset, int count)
            {
                int res = 0;
                for (int i = offset; i < offset + count; i++)
                    res = crc_table[res ^ buffer[i]];

                return (byte)res;
            }

            public unsafe byte CalcCheckSum(byte* buffer, int offset, int count)
            {
                //byte[] buff = new byte[count];
                //System.Runtime.InteropServices.Marshal.Copy(new IntPtr(buffer), buff, offset, count);
                //return CalcCheckSum(buff, 0, buff.Length);
                int res = 0;
                for (int i = offset; i < offset + count; i++)
                    res = crc_table[res ^ buffer[i]];
                return (byte)res;
            }
        }
    }
}