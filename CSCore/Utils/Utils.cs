using System;

namespace CSCore.Utils
{
    public static class Utils
    {
        public static unsafe float ConvertToSample(byte* buffer, int bitsPerSample, bool autoIncrementPtr = true, bool mindown = true)
        {
            float value;
            if (bitsPerSample == 8)
                value = CSMath.Bit8ToFloat(buffer, mindown);
            else if (bitsPerSample == 16)
                value = CSMath.Bit16ToFloat(buffer, mindown);
            else if (bitsPerSample == 24)
                value = CSMath.Bit24ToFloat(buffer, mindown);
            else if (bitsPerSample == 32)
                value = CSMath.Bit32ToFloat(buffer, mindown);
            else
                throw new ArgumentOutOfRangeException("bitsPerSample");

            if (autoIncrementPtr)
                buffer += (bitsPerSample / 8);

            return value;
        }

        public static void SampleToBuffer(byte[] buffer, ref int index, double sample, int bitsPerSample)
        {
            int i = index;

            if (bitsPerSample == 8)
                buffer[index] = (byte)sample;
            else if (bitsPerSample == 16)
            {
                buffer[index++] = (byte)((short)sample & 0x00FF);
                buffer[index] = (byte)(((short)sample & 0xFF00) >> 8);
            }
            else if (bitsPerSample == 24)
            {
                buffer[index++] = (byte)((short)sample & 0x0000FF);
                buffer[index++] = (byte)(((short)sample & 0x00FF00) >> 8);
                buffer[index] = (byte)(((short)sample & 0xFF0000) >> 16);
            }
            else if (bitsPerSample == 32)
            {
                buffer[index++] = (byte)((short)sample &    0x000000FF);
                buffer[index++] = (byte)(((short)sample &   0x0000FF00) >> 8);
                buffer[index++] = (byte)(((short)sample &   0x00FF0000) >> 16);
                buffer[index++] = (byte)(((short)sample &   0xFF000000) >> 24);
            }

            index = index - i + 1;
        }

#if DEBUG
        internal static unsafe void DumpPtr(int* i, int count)
        {
            for (int n = 0; n < count; n++)
            {
                System.Diagnostics.Debug.WriteLine(n + " " + *(i++));
            }
        }
#endif
    }
}
