using System.Runtime.InteropServices;

namespace CSCore.Utils.Buffer
{
    [StructLayout(LayoutKind.Explicit, Pack=2)]
    public class UnsafeBuffer
    {
        [FieldOffset(0)]
        long tmp = 0;

        [FieldOffset(8)]
        byte[] _byteBuffer;
        [FieldOffset(8)]
        short[] _shortBuffer;
        [FieldOffset(8)]
        float[] _floatBuffer;
        [FieldOffset(8)]
        int[] _intBuffer;

        public UnsafeBuffer()
        {

        }

        public UnsafeBuffer(byte[] byteBuffer)
        {
            _byteBuffer = byteBuffer;
        }

        public byte[] ByteBuffer
        {
            get { return _byteBuffer; }
            set { _byteBuffer = value; }
        }

        public short[] ShortBuffer
        {
            get { return _shortBuffer; }
            set { _shortBuffer = value; }
        }

        public float[] FloatBuffer
        {
            get { return _floatBuffer; }
            set { _floatBuffer = value; }
        }

        public int[] IntBuffer
        {
            get { return _intBuffer; }
            set { _intBuffer = value; }
        }
    }
}
