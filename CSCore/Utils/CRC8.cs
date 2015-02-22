namespace CSCore.Utils
{
    /// <summary>
    /// This class is based on the CUETools.NET project (see http://sourceforge.net/p/cuetoolsnet/)
    /// The author "Grigory Chudov" explicitly gave the permission to use the source as part of the cscore source code which got licensed under the ms-pl.
    /// </summary>
    internal class CRC8 : CRCBase<byte>
    {
        private static CRC8 _instance;

        public CRC8()
        {
            CalcTable(8);
        }

        public static CRC8 Instance
        {
            get { return _instance ?? (_instance = new CRC8()); }
        }

        public override byte CalcCheckSum(byte[] buffer, int offset, int count)
        {
            int res = 0;
            for (int i = offset; i < offset + count; i++)
            {
                res = crc_table[res ^ buffer[i]];
            }

            return (byte) res;
        }

        public unsafe byte CalcCheckSum(byte* buffer, int offset, int count)
        {
            //byte[] buff = new byte[count];
            //System.Runtime.InteropServices.Marshal.Copy(new IntPtr(buffer), buff, offset, count);
            //return CalcCheckSum(buff, 0, buff.Length);
            int res = 0;
            for (int i = offset; i < offset + count; i++)
            {
                res = crc_table[res ^ buffer[i]];
            }
            return (byte) res;
        }
    }
}