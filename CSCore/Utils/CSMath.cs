using CSCore.SoundOut.DirectSound;
using System;

namespace CSCore.Utils
{
    /// <summary>
    /// Fasst Mathematische Operationen zusammen die hin und wieder Verwendung finden
    /// </summary>
    public static partial class CSMath
    {
        public const int TwoPow32By2 = 2147483647;
        public const int TwoPow24By2 = 8388608;
        public const int TwoPow16By2 = 32768;
        public const int TwoPow8By2 = 128;

        public static double GetExponent(double number, double baseNumber)
        {
            return Math.Log(number, baseNumber);
        }

        public static float Bit24ToFloat(byte[] buffer, int i, bool minDown)
        {
            //byte 3 << 16 , byte 2 << 8 byte 1 , 8388608f = 2^24/2
            return (((sbyte)buffer[i + 2] << 16) |
                        (buffer[i + 1] << 8) |
                        buffer[i]) / (minDown ? 8388608f : 1.0f);
        }

        public static unsafe float Bit24ToFloat(byte* bufferPtr, bool minDown)
        {
            return (((sbyte)*(bufferPtr + 2) << 16) |
                        (*(bufferPtr + 1) << 8) |
                          *bufferPtr) / (minDown ? 8388608f : 1.0f);
        }

        public static float Bit32ToFloat(byte[] buffer, int i, bool minDown)
        {
            return BitConverter.ToInt32(buffer, i) / (minDown ? 2147483648f : 1.0f);
        }

        public static unsafe float Bit32ToFloat(byte* bufferPtr, bool minDown)
        {
            int i = (int)(*(bufferPtr + 3) << 24 | *(bufferPtr + 2) << 16 |
                      *(bufferPtr + 1) << 8 | *bufferPtr);
            return i / (minDown ? 2147483648f : 1.0f);
        }

        public static float Bit16ToFloat(byte[] buffer, int i, bool minDown)
        {
            return BitConverter.ToInt16(buffer, i) / (minDown ? 32768f : 1.0f);
        }

        public static unsafe float Bit16ToFloat(byte* bufferPtr, bool minDown)
        {
            //*(((short*)bufferPtr));
            //[byte1][byte2]
            short i = (short)(*(bufferPtr + 1) << 8 | *bufferPtr);
            return i / (minDown ? 32768f : 1.0f);
        }

        public static float Bit8ToFloat(byte[] buffer, int i, bool minDown)
        {
            return buffer[i] / (minDown ? (128.0f - 1.0f) : 1.0f);
        }

        public static unsafe float Bit8ToFloat(byte* bufferPtr, bool minDown)
        {
            return *bufferPtr / (minDown ? (128.0f - 1.0f) : 1.0f);
        }

        public static int FloatToDirectSoundVolume(float volume)
        {
            int result = (int)(volume * Math.Abs(DirectSoundSecondaryBuffer.MinVolume)) - Math.Abs(DirectSoundSecondaryBuffer.MinVolume);
            return result;
        }

        public static float DirectSoundVolumeToFloat(int dxvolume)
        {
            float result = 1 - ((float)dxvolume / DirectSoundSecondaryBuffer.MinVolume);
            return result;
        }

        public static uint FloatToWaveOutVolume(float volume)
        {
            return (uint)(volume * 0xFFFF) + ((uint)(volume * 0xFFFF) << 16);
        }

        public static uint FloatToWaveOutVolume(float left, float right)
        {
            return (uint)(left * 0xFFFF) + ((uint)(right * 0xFFFF) << 16);
        }

        public static float IntToWaveOutVolume(uint volume)
        {
            uint left, right;
            HightLowConverterUInt32 u = new HightLowConverterUInt32(volume);
            left = u.High;
            right = u.Low;
            return (float)(((right + left) / 2) * (1.0 / 0xFFFF));
        }

        public static void IntToWaveOutVolume(uint volume, out float left, out float right)
        {
            left = volume.HighWord() * (1.0f / 0xFFFF);
            right = volume.LowWord() * (1.0f / 0xFFFF);
        }

        public static double CalcDecibel(float volume)
        {
            return 20 * Math.Log10(volume);
        }

        internal static int ILog(int i)
        {
            int result = 0;
            while ((i >>= 1) != 0)
            {
                result++;
            }
            return result;
        }
    }
}