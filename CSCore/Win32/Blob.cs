using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Win32
{
    /// <summary>
    /// Blob
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Blob
    {
        /// <summary>
        /// Number of bytes stored in the blob.
        /// </summary>
        public int Length;

        /// <summary>
        /// Pointer to a byte array which stores the data.
        /// </summary>
        public IntPtr Data;

        /// <summary>
        /// Returns the data stored in the <see cref="Blob"/>.
        /// </summary>
        /// <returns>The data stored in the <see cref="Blob"/></returns>
        public byte[] GetData()
        {
            byte[] data = new byte[Length];
            Marshal.Copy(Data, data, 0, data.Length);
            return data;
        }

        /// <summary>
        /// Converts the data stored in the <see cref="Blob"/> based on an <paramref name="encoding"/> to a string and returns the string.
        /// </summary>
        /// <param name="encoding">Encoding used to convert the data to a string.</param>
        /// <returns>String of the stored data.</returns>
        public string GetString(Encoding encoding)
        {
            return encoding.GetString(GetData());
        }
    }
}