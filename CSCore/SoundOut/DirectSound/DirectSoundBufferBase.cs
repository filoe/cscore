using CSCore.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.SoundOut.DirectSound
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("279AFA85-4981-11CE-A521-0020AF0BE560")]
    public unsafe class DirectSoundBufferBase : ComObject
    {
        public const int MinVolume = -10000;
        public const int MaxVolume = 0;

        public const int PanLeft = -10000;
        public const int PanCenter = 0;
        public const int PanRight = 10000;

        public const int FrequencyOriginal = 0;
        public const int FrequencyMin = 100;
        public const int FrequencyMax = 20000;
        private const string c = "IDirectSoundBuffer";

        //assume that dv = 10.0
        private const double MinAttentuation = 9.766E-4; //(1/2)^(100/dv)

        public DirectSoundBufferBase()
        {
        }

        public DirectSoundBufferBase(IntPtr basePtr)
        {
            UnsafeBasePtr = basePtr.ToPointer();
        }

        public DSBufferCaps BufferCaps
        {
            get
            {
                DSBufferCaps caps;
                DirectSoundException.Try(GetCaps(out caps), c, "GetCaps");
                return caps;
            }
        }

        public DSBStatus Status
        {
            get
            {
                DSBStatus status;
                DirectSoundException.Try(GetStatus(out status), c, "GetStatus");
                return status;
            }
        }

        public DSResult GetCaps(out DSBufferCaps bufferCaps)
        {
            bufferCaps = new DSBufferCaps();
            bufferCaps.dwSize = Marshal.SizeOf(bufferCaps);
            fixed (void* ptrbuffercaps = &bufferCaps)
            {
                var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptrbuffercaps, ((void**)(*(void**)UnsafeBasePtr))[3]);
                return result;
            }
        }

        public void Play(DSBPlayFlags flags)
        {
            DirectSoundException.Try(PlayNative(flags), c, "Play");
        }

        public DSResult PlayNative(DSBPlayFlags flags)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, 0, 0, unchecked((int)flags), ((void**)(*(void**)UnsafeBasePtr))[12]);
        }

        public void Stop()
        {
            DirectSoundException.Try(StopNative(), c, "Stop");
        }

        public DSResult StopNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[18]);
        }

        public void Restore()
        {
            DirectSoundException.Try(RestoreNative(), c, "Restore");
        }

        public DSResult RestoreNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[20]);
        }

        public DSResult Lock(int offset, int bytes, out IntPtr audioPtr1, out int audioBytes1,
                             out IntPtr audioPtr2, out int audioBytes2, DSBLock lockFlags)
        {
            fixed (void* pAudioPtr1 = &audioPtr1, pAudioPtr2 = &audioPtr2)
            {
                fixed (void* pAudioBytes1 = &audioBytes1, pAudioBytes2 = &audioBytes2)
                {
                    var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, offset, bytes,
                                        pAudioPtr1, pAudioBytes1, pAudioPtr2, pAudioBytes2, unchecked((int)lockFlags),
                                        ((void**)(*(void**)UnsafeBasePtr))[11]);
                    return result;
                }
            }
        }

        public DSResult Unlock(IntPtr audioPtr1, int audioBytes1, IntPtr audioPtr2, int audioBytes2)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)audioPtr1, audioBytes1, (void*)audioPtr2, audioBytes2, ((void**)(*(void**)UnsafeBasePtr))[19]);
        }

        public DSResult GetCurrentPosition(out int playCursorPosition, out int writeCursorPosition)
        {
            fixed (void* pplaypos = &playCursorPosition, pwritepos = &writeCursorPosition)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pplaypos, pwritepos, ((void**)(*(void**)UnsafeBasePtr))[4]);
            }
        }

        public void SetCurrentPosition(int playPosition)
        {
            DirectSoundException.Try(SetCurrentPositionNative(playPosition), c, "SetCurrentPosition");
        }

        public DSResult SetCurrentPositionNative(int playPosition)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, playPosition, ((void**)(*(void**)UnsafeBasePtr))[13]);
        }

        public DSResult Initialize(DirectSoundBase directSound, DSBufferDescription bufferDesc)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, directSound.BasePtr.ToPointer(), &bufferDesc, ((void**)(*(void**)UnsafeBasePtr))[10]);
        }

        public DSResult GetStatus(out DSBStatus status)
        {
            //int istatus;
            //void* pistatus = &istatus;
            fixed (void* pstatus = &status)
            {
                var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, pstatus, ((void**)(*(void**)UnsafeBasePtr))[9]);
                return result;
            }
            //status = (DSBStatus)istatus;
        }

        public DSResult SetFrequency(int frequency)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, frequency, ((void**)(*(void**)UnsafeBasePtr))[17]);
        }

        public DSResult GetFrequency(out int frequency)
        {
            fixed (void* pfrequency = &frequency)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pfrequency, ((void**)(*(void**)UnsafeBasePtr))[8]);
            }
        }

        public DSResult SetPan(int pan)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pan, ((void**)(*(void**)UnsafeBasePtr))[16]);
        }

        public DSResult SetPan(float pan)
        {
            int pani = (int)(pan * Math.Abs(PanLeft));
            return SetPan(pani);
        }

        public DSResult GetPan(out int pan)
        {
            fixed (void* ppan = &pan)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ppan, ((void**)(*(void**)UnsafeBasePtr))[7]);
            }
        }

        public DSResult GetPan(out float pan)
        {
            int pani;
            var result = GetPan(out pani);
            pan = (float)pani / Math.Abs(PanLeft);
            return result;
        }

        public DSResult SetVolume(int volume)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, volume, ((void**)(*(void**)UnsafeBasePtr))[15]);
        }

        public void SetVolume(double volume)
        {
            DirectSoundException.Try(SetVolumeNative(volume), c, "SetVolume");
        }

        public DSResult SetVolumeNative(double volume)
        {
            int dwvolume = MinVolume;
            if (volume != 0)
            {
                const double dv = 10.0;
                const double z0 = 0.69314718055994529; //ln(2)

                double attenuation = MinAttentuation + volume * (1 - MinAttentuation);
                double db = dv * Math.Log(attenuation) / z0;
                dwvolume = (int)(db * 100);

                dwvolume = Math.Max(MinVolume, Math.Min(dwvolume, MaxVolume));
            }

            return SetVolume(dwvolume);
        }

        public DSResult GetVolume(out int volume)
        {
            fixed (void* pvolume = &volume)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pvolume, ((void**)(*(void**)UnsafeBasePtr))[6]);
            }
        }

        public DSResult GetVolume(out double volume)
        {
            int dwvolume;
            DSResult result = GetVolume(out dwvolume);

            if (result != DSResult.DS_OK)
            {
                volume = 0f;
            }
            else
            {
                const double z1 = 0.001; //(1/100)/(dv)
                volume = (MinAttentuation - Math.Pow(2, z1 * dwvolume)) / (MinAttentuation - 1);

                volume = Math.Min(1, Math.Max(0, volume));
            }

            return result;
        }

        public double GetVolume()
        {
            double volume;
            DirectSoundException.Try(GetVolume(out volume), "IDirectSoundBuffer", "GetVolume");
            return volume;
        }

        public DSResult GetFormat(out WaveFormat waveFormat)
        {
            waveFormat = new WaveFormat();
            GCHandle hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, hWaveFormat.AddrOfPinnedObject().ToPointer(),
                            Marshal.SizeOf(waveFormat), IntPtr.Zero.ToPointer(), ((void**)(*(void**)UnsafeBasePtr))[5]);
            hWaveFormat.Free();
            return result;
        }

        public DSResult SetFormat(WaveFormat waveFormat)
        {
            GCHandle hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, hWaveFormat.AddrOfPinnedObject().ToPointer(), ((void**)(*(void**)UnsafeBasePtr))[14]);
            hWaveFormat.Free();
            return result;
        }

        public bool IsBufferLost()
        {
            DSBStatus status;
            DirectSoundException.Try(GetStatus(out status), "IDirectSoundBuffer", "GetStatus");
            return (status & DSBStatus.BufferLost) == DSBStatus.BufferLost;
        }

        public bool Write(byte[] buffer, int offset, int count)
        {
            IntPtr ptr1, ptr2;
            int b1, b2;
            if (Lock(offset, count, out ptr1, out b1, out ptr2, out b2, DSBLock.Default) == DSResult.DS_OK)
            {
                if (ptr1 != IntPtr.Zero)
                    Marshal.Copy(buffer, 0, ptr1, b1);
                if (ptr2 != IntPtr.Zero)
                    Marshal.Copy(buffer, b1 - 1, ptr2, b2);

                return Unlock(ptr1, b1, ptr2, b2) == DSResult.DS_OK;
            }
            return false;
        }

        public bool Write(short[] buffer, int offset, int count)
        {
            int dscount = count * 2;

            IntPtr ptr1, ptr2;
            int b1, b2;
            if (Lock(offset, dscount, out ptr1, out b1, out ptr2, out b2, DSBLock.Default) == DSResult.DS_OK)
            {
                if (ptr1 != IntPtr.Zero)
                    Marshal.Copy(buffer, 0, ptr1, Math.Min(b1, count));
                if (ptr2 != IntPtr.Zero)
                    Marshal.Copy(buffer, Math.Min(b1, count) - 2, ptr2, Math.Min(b2, count));

                return Unlock(ptr1, b1, ptr2, b2) == DSResult.DS_OK;
            }
            return false;
        }
    }
}