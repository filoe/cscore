using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Blob
    {
        public int Length;
        public IntPtr Data;

        public byte[] GetData()
        {
            byte[] data = new byte[Length];
            Marshal.Copy(Data, data, 0, data.Length);
            return data;
        }

        public string GetString(Encoding encoding)
        {
            return encoding.GetString(GetData());
        }
    }
}