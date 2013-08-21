using System;
using System.Runtime.InteropServices;

namespace CSCore.ACM
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = 128)]
    public class NativeAcmHeader
    {
        public int cbStruct;
        public AcmHeaderSate fdwStatus = 0;
        public IntPtr userData;
        public IntPtr inputBufferPointer;
        public int inputBufferLength;
        public int inputBufferLengthUsed;
        public IntPtr inputUserData;
        public IntPtr outputBufferPointer;
        public int outputBufferLength;
        public int outputBufferLengthUsed = 0;
        public IntPtr outputUserData;
    }
}