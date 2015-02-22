namespace CSCore.Utils
{
    /// <summary>
    /// This class is based on the CUETools.NET project (see http://sourceforge.net/p/cuetoolsnet/)
    /// The author "Grigory Chudov" explicitly gave the permission to use the source as part of the cscore source code which got licensed under the ms-pl.
    /// </summary>
    internal class CRC16 : CRCBase<ushort>
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
                {
                    res = ((res << 8) ^
                           pTable[(res >> 8) ^ *(buffer++)]);
                }
            }
            return (ushort) res;
        }
    }
}