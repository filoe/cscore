namespace CSCore.Utils
{
    public partial class CSMath
    {
        /// <summary>
        /// http: //flac.sourceforge.net/format.html#frame_footer
        /// CRC-16 (polynomial = x^16 + x^15 + x^2 + x^0, initialized with 0) of everything before
        /// the crc, back to and including the frame header sync code
        /// </summary>
        public class CRC16 : CRCBase<ushort>
        {
            public CRC16()
            {
                CalcTable(16);
            }

            public override unsafe ushort CalcCheckSum(byte[] buffer, int offset, int count)
            {
                fixed (byte* pBuffer = buffer)
                {
                    return CalcCheckSum(pBuffer + offset, count);
                }
            }

            public unsafe ushort CalcCheckSum(byte* buffer, int count)
            {
                int res = 0;
                fixed (ushort* pTable = crc_table)
                {
                    for (int i = count; i > 0; i--)
                        res = ((res << 8) ^
                            pTable[(res >> 8) ^ *(buffer++)]);
                }
                return (ushort)res;
            }
        }
    }
}