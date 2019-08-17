using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace CSCore.SoundOut.AL
{
    [CLSCompliant(false)]
    internal class ALInterops
    {
        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alGetString(int name);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcGetString([In] IntPtr device, int name);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern sbyte alcIsExtensionPresent([In] IntPtr device, string extensionName);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern sbyte alIsExtensionPresent(string extensionName);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcCaptureStart(IntPtr device);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcCaptureStop(IntPtr device);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcCaptureSamples(IntPtr device, IntPtr buffer, int numSamples);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcCaptureOpenDevice(string deviceName, uint frequency, ALFormat format,
            int bufferSize);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcCaptureCloseDevice(IntPtr device);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcOpenDevice(string deviceName);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alcCloseDevice(IntPtr handle);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcCreateContext(IntPtr device, IntPtr attrlist);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alcMakeContextCurrent(IntPtr context);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcGetContextsDevice(IntPtr context);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcGetCurrentContext();

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcDestroyContext(IntPtr context);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetSourcei(uint sourceId, ALSourceParameters param, out int value);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcePlay(uint sourceId);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcePause(uint sourceId);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceStop(uint sourceId);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceQueueBuffers(uint sourceId, int number, uint[] bufferIDs);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceUnqueueBuffers(uint sourceId, int buffers, uint[] buffersDequeued);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGenSources(int count, uint[] sources);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alDeleteSources(int count, uint[] sources);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetSourcef(uint sourceId, ALSourceParameters param, out float value);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetSource3f(uint sourceId, ALSourceParameters param, out float val1,
            out float val2, out float val3);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcef(uint sourceId, ALSourceParameters param, float value);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcefv(uint sourceId, ALSourceParameters param, float[] value);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSource3f(uint sourceId, ALSourceParameters param, float val1, float val2,
            float val3);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcei(uint sourceId, ALSourceParameters param, float val1);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGenBuffers(int count, uint[] bufferIDs);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alBufferData(uint bufferId, ALFormat format, byte[] data, int byteSize,
            uint frequency);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alDeleteBuffers(int numBuffers, uint[] bufferIDs);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alListenerf(ALSourceParameters param, float val);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alListenerfv(ALSourceParameters param, float[] val);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alListener3f(ALSourceParameters param, float val1, float val2, float val3);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetListener3f(ALSourceParameters param, out float val1, out float val2,
            out float val3);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetListenerf(ALSourceParameters param, out float val);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetListenerfv(ALSourceParameters param, float[] val);

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ALErrorCode alGetError();

        [DllImport("openal32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ALErrorCode alcGetError(IntPtr handle);

        public const int DeviceSpecifier = 0x1005;

        public const int AllDevicesSpecifier = 0x1013;

        internal static string[] GetALDeviceNames()
        {
            var strings = new string[0];
            if (IsExtensionPresent("ALC_ENUMERATE_ALL_EXT"))
            {
                strings =
                    ReadStringsFromMemory(alcGetString(IntPtr.Zero,
                        AllDevicesSpecifier));
            }
            else if (IsExtensionPresent("ALC_ENUMERATION_EXT"))
            {
                strings =
                    ReadStringsFromMemory(alcGetString(IntPtr.Zero, DeviceSpecifier));
            }

            return strings;
        }

        internal static string[] ReadStringsFromMemory(IntPtr location)
        {
            var strings = new List<string>();

            bool lastNull = false;
            int i = -1;
            byte c;
            while (!((c = Marshal.ReadByte(location, ++i)) == '\0' && lastNull))
            {
                if (c == '\0')
                {
                    lastNull = true;

                    strings.Add(Marshal.PtrToStringAnsi(location, i));
                    location = new IntPtr((long)location + i + 1);
                    i = -1;
                }
                else
                    lastNull = false;
            }

            return strings.ToArray();
        }

        internal static bool IsExtensionPresent(string extension)
        {
            sbyte result = extension.StartsWith("ALC")
                ? alcIsExtensionPresent(IntPtr.Zero, extension)
                : alIsExtensionPresent(extension);

            return result == 1;
        }

        internal static bool IsSupported()
        {
            try
            {
                alIsExtensionPresent("TEST_ONLY");
                return true;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
        }
    }
}
