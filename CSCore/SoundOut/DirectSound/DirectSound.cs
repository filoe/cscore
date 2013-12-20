using CSCore.Win32;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.SoundOut.DirectSound
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("279AFA83-4981-11CE-A521-0020AF0BE560")]
    public unsafe class DirectSoundBase : ComObject
    {
        public static DirectSoundBase Create(Guid device)
        {
            IntPtr ptr;
            DirectSoundException.Try(NativeMethods.DirectSoundCreate(ref device, out ptr, IntPtr.Zero), "DSInterop", "DirectSoundCreate(ref Guid, out IntPtr, IntPtr)");
            return new DirectSoundBase(ptr);
        }

        public static DirectSound8 Create8(Guid device)
        {
            IntPtr ptr;
            DirectSoundException.Try(NativeMethods.DirectSoundCreate8(ref device, out ptr, IntPtr.Zero), "DSInterop", "DirectSoundCreate8(ref Guid, out IntPtr, IntPtr)");
            return new DirectSound8(ptr);
        }

        public DirectSoundCapabilities Caps
        {
            get
            {
                DirectSoundCapabilities caps;
                DirectSoundException.Try(GetCaps(out caps), "IDirectSound", "GetCaps");
                return caps;
            }
        }

        public DirectSoundBase(IntPtr directSound)
            : base(directSound)
        {
        }

        public bool SupportsFormat(WaveFormat format)
        {
            DirectSoundCapabilities caps;
            DirectSoundException.Try(GetCaps(out caps), "IDirectSound", "GetCaps");
            bool result = true;
            if (format.Channels == 2)
                result &= (caps.Flags & DSCapabilitiesFlags.SecondaryBufferStereo) == DSCapabilitiesFlags.SecondaryBufferStereo;
            else if (format.Channels == 1)
                result &= (caps.Flags & DSCapabilitiesFlags.SecondaryBufferMono) == DSCapabilitiesFlags.SecondaryBufferMono;
            else result &= false;

            if (format.BitsPerSample == 8)
                result &= (caps.Flags & DSCapabilitiesFlags.SecondaryBuffer8Bit) == DSCapabilitiesFlags.SecondaryBuffer8Bit;
            else if (format.BitsPerSample == 16)
                result &= (caps.Flags & DSCapabilitiesFlags.SecondaryBuffer16Bit) == DSCapabilitiesFlags.SecondaryBuffer16Bit;
            else 
                result &= false;

            result &= format.WaveFormatTag == AudioEncoding.Pcm;
            return result;
        }

        public void SetCooperativeLevel(IntPtr hWnd, DSCooperativeLevelType level)
        {
            DirectSoundException.Try(SetCooperativeLevelNative(hWnd, level), "IDirectSound8", "SetCooperativeLevel");
        }

        public DSResult SetCooperativeLevelNative(IntPtr hWnd, DSCooperativeLevelType level)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, hWnd.ToPointer(), unchecked((int)level), ((void**)(*(void**)_basePtr))[6]);
        }

        public IntPtr CreateSoundBuffer(DSBufferDescription bufferDesc, IntPtr pUnkOuter)
        {
            IntPtr soundBuffer;
            DirectSoundException.Try(CreateSoundBufferNative(bufferDesc, out soundBuffer, pUnkOuter),
                "IDirectSound8", "CreateSoundBuffer");
            return soundBuffer;
        }

        public DSResult CreateSoundBufferNative(DSBufferDescription bufferDesc, out IntPtr soundBuffer, IntPtr pUnkOuter)
        {
            fixed (void* ptrsoundbuffer = &soundBuffer)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, &bufferDesc, ptrsoundbuffer, (void*)pUnkOuter, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        public DSResult GetCaps(out DirectSoundCapabilities caps)
        {
            DirectSoundCapabilities tmp = new DirectSoundCapabilities();
            tmp.Size = Marshal.SizeOf(tmp);
            var result = InteropCalls.CalliMethodPtr(_basePtr, &tmp, ((void**)(*(void**)_basePtr))[4]);
            caps = tmp;
            return result;
        }

        public DSResult DuplicateSoundBuffer<T>(T bufferOriginal, out T duplicatedBuffer) where T : DirectSoundBufferBase
        {
            IntPtr resultPtr;
            var result = InteropCalls.CalliMethodPtr(_basePtr, bufferOriginal.BasePtr.ToPointer(), &resultPtr, ((void**)(*(void**)_basePtr))[5]);
            duplicatedBuffer = (T)Activator.CreateInstance(typeof(T), resultPtr);
            return result;
        }

        public DSResult Compact()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[7]);
        }

        public DSResult GetSpeakerConfig(out int speakerConfig)
        {
            fixed (void* pspeakerConfig = &speakerConfig)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, pspeakerConfig, ((void**)(*(void**)_basePtr))[8]);
            }
        }

        public DSResult SetSpeakerConfig(int speakerConfig)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, speakerConfig, ((void**)(*(void**)_basePtr))[9]);
        }

        public DSResult Initialize(Guid device)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &device, ((void**)(*(void**)_basePtr))[10]);
        }
    }
}