using System;
using System.Runtime.InteropServices;

namespace CSCore.Utils.Buffer
{
    public static class BufferUtils
    {
        public static T[] CheckBuffer<T>(T[] buffer, int size)
        {
            if (buffer == null || buffer.Length < size)
                return new T[size];
            return buffer;
        }

        public unsafe static T[] CreateArray<T>(void* source, int length)
        {
            var type = typeof(T);
            var sizeInBytes = Marshal.SizeOf(typeof(T));

            T[] output = new T[length];

            if (type.IsPrimitive)
            {
                // Make sure the array won't be moved around by the GC
                var handle = GCHandle.Alloc(output, GCHandleType.Pinned);

                var destination = (byte*)handle.AddrOfPinnedObject().ToPointer();
                var byteLength = length * sizeInBytes;

                // There are faster ways to do this, particularly by using wider types or by
                // handling special lengths.
                for (int i = 0; i < byteLength; i++)
                    destination[i] = ((byte*)source)[i];

                handle.Free();
            }
            else if (type.IsValueType)
            {
                if (!type.IsLayoutSequential && !type.IsExplicitLayout)
                {
                    throw new InvalidOperationException(string.Format("{0} does not define a StructLayout attribute", type));
                }

                IntPtr sourcePtr = new IntPtr(source);

                for (int i = 0; i < length; i++)
                {
                    IntPtr p = new IntPtr((byte*)source + i * sizeInBytes);

                    output[i] = (T)System.Runtime.InteropServices.Marshal.PtrToStructure(p, typeof(T));
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("{0} is not supported", type));
            }

            return output;
        }
    }
}