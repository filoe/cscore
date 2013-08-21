using CSCore.Win32;
using System;
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

        public const uint FrequencyOriginal = 0;
        public const uint FrequencyMin = 100;
        public const uint FrequencyMax = 20000;

        public DirectSoundBufferBase()
        {
        }

        public DirectSoundBufferBase(IntPtr basePtr)
        {
            _basePtr = basePtr.ToPointer();
        }

        public DSBufferCaps BufferCaps
        {
            get
            {
                DSBufferCaps caps;
                DirectSoundException.Try(GetCaps(out caps), "IDirectSoundBuffer", "GetCaps");
                return caps;
            }
        }

        public DSResult GetCaps(out DSBufferCaps bufferCaps)
        {
            bufferCaps = new DSBufferCaps();
            bufferCaps.dwSize = Marshal.SizeOf(bufferCaps);
            fixed (void* ptrbuffercaps = &bufferCaps)
            {
                var result = InteropCalls.CalliMethodPtr(_basePtr, ptrbuffercaps, ((void**)(*(void**)_basePtr))[3]);
                return result;
            }
        }

        public DSResult Play(DSBPlayFlags flags)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, 0, 0, unchecked((int)flags), ((void**)(*(void**)_basePtr))[12]);
        }

        public DSResult Stop()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[18]);
        }

        public DSResult Restore()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[20]);
        }

        public DSResult Lock(int offset, int bytes, out IntPtr audioPtr1, out int audioBytes1,
                             out IntPtr audioPtr2, out int audioBytes2, DSBLock lockFlags)
        {
            fixed (void* pAudioPtr1 = &audioPtr1, pAudioPtr2 = &audioPtr2)
            {
                fixed (void* pAudioBytes1 = &audioBytes1, pAudioBytes2 = &audioBytes2)
                {
                    var result = InteropCalls.CalliMethodPtr(_basePtr, offset, bytes,
                                        pAudioPtr1, pAudioBytes1, pAudioPtr2, pAudioBytes2, unchecked((int)lockFlags),
                                        ((void**)(*(void**)_basePtr))[11]);
                    return result;
                }
            }
        }

        public DSResult Unlock(IntPtr audioPtr1, int audioBytes1, IntPtr audioPtr2, int audioBytes2)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, (void*)audioPtr1, audioBytes1, (void*)audioPtr2, audioBytes2, ((void**)(*(void**)_basePtr))[19]);
        }

        public DSResult GetCurrentPosition(out int playCursorPosition, out int writeCursorPosition)
        {
            fixed (void* pplaypos = &playCursorPosition, pwritepos = &writeCursorPosition)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, pplaypos, pwritepos, ((void**)(*(void**)_basePtr))[4]);
            }
        }

        public DSResult SetCurrentPosition(int playPosition)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, playPosition, ((void**)(*(void**)_basePtr))[13]);
        }

        public DSResult Initialize(DirectSoundBase directSound, DSBufferDescription bufferDesc)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, directSound.BasePtr.ToPointer(), &bufferDesc, ((void**)(*(void**)_basePtr))[10]);
        }

        public DSResult GetStatus(out DSBStatus status)
        {
            //int istatus;
            //void* pistatus = &istatus;
            fixed (void* pstatus = &status)
            {
                var result = InteropCalls.CalliMethodPtr(_basePtr, pstatus, ((void**)(*(void**)_basePtr))[9]);
                return result;
            }
            //status = (DSBStatus)istatus;
        }

        public DSResult SetFrequency(int frequency)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, frequency, ((void**)(*(void**)_basePtr))[17]);
        }

        public DSResult GetFrequency(out int frequency)
        {
            fixed (void* pfrequency = &frequency)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, pfrequency, ((void**)(*(void**)_basePtr))[8]);
            }
        }

        public DSResult SetPan(int pan)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, pan, ((void**)(*(void**)_basePtr))[16]);
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
                return InteropCalls.CalliMethodPtr(_basePtr, ppan, ((void**)(*(void**)_basePtr))[7]);
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
            return InteropCalls.CalliMethodPtr(_basePtr, volume, ((void**)(*(void**)_basePtr))[15]);
        }

        public DSResult SetVolume(float volume)
        {
            if (volume == 0)
                volume = 0.00001f;
            int dvolume = (int)(20 * Math.Log(volume));
            dvolume *= 100;
            dvolume = Math.Max(MinVolume, dvolume);
            dvolume = Math.Min(MaxVolume, dvolume);
            return SetVolume(dvolume);
        }

        public DSResult GetVolume(out int volume)
        {
            fixed (void* pvolume = &volume)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, pvolume, ((void**)(*(void**)_basePtr))[6]);
            }
        }

        public DSResult GetVolume(out float volume)
        {
            int dvolume;
            var result = GetVolume(out dvolume);
            dvolume /= 100;
            volume = (float)Math.Pow(10.0, (double)dvolume / 20.0);
            volume = Math.Min(1, volume);
            volume = Math.Max(0, volume);
            if (volume == 0.00001f)
                volume = 0;
            return result;
        }

        public float GetVolume()
        {
            float volume;
            DirectSoundException.Try(GetVolume(out volume), "IDirectSoundBuffer", "GetVolume");
            return volume;
        }

        public DSResult GetFormat(out WaveFormat waveFormat)
        {
            waveFormat = new WaveFormat();
            GCHandle hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            var result = InteropCalls.CalliMethodPtr(_basePtr, hWaveFormat.AddrOfPinnedObject().ToPointer(),
                            Marshal.SizeOf(waveFormat), IntPtr.Zero.ToPointer(), ((void**)(*(void**)_basePtr))[5]);
            hWaveFormat.Free();
            return result;
        }

        public DSResult SetFormat(WaveFormat waveFormat)
        {
            GCHandle hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            var result = InteropCalls.CalliMethodPtr(_basePtr, hWaveFormat.AddrOfPinnedObject().ToPointer(), ((void**)(*(void**)_basePtr))[14]);
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