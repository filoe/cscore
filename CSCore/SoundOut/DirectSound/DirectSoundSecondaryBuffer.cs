using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.SoundOut.DirectSound
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("6825A449-7524-4D82-920F-50E36AB3AB1E")]
    public unsafe class DirectSoundSecondaryBuffer : DirectSoundBufferBase
    {      
        public DirectSoundSecondaryBuffer(DirectSoundBase directSound, WaveFormat waveFormat, int bufferSize, bool controlEffects = false)
        {
            if (directSound == null) throw new ArgumentNullException("directSound");

            DSBufferDescription secondaryBufferDesc = new DSBufferDescription()
            {
                dwBufferBytes = (uint)bufferSize * 2,
                dwFlags = DSBufferCapsFlags.DSBCAPS_CTRLFREQUENCY | DSBufferCapsFlags.DSBCAPS_CTRLPAN |
                          DSBufferCapsFlags.DSBCAPS_CTRLVOLUME | DSBufferCapsFlags.DSBCAPS_CTRLPOSITIONNOTIFY |
                          DSBufferCapsFlags.DSBCAPS_GETCURRENTPOSITION2 | DSBufferCapsFlags.DSBCAPS_GLOBALFOCUS |
                          DSBufferCapsFlags.DSBCAPS_STICKYFOCUS,
                dwReserved = 0,
                guid3DAlgorithm = Guid.Empty
            };

            if (controlEffects)
                secondaryBufferDesc.dwFlags |= DSBufferCapsFlags.DSBCAPS_CTRLFX;

            secondaryBufferDesc.dwSize = Marshal.SizeOf(secondaryBufferDesc);
            GCHandle hWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned);
            secondaryBufferDesc.lpwfxFormat = hWaveFormat.AddrOfPinnedObject();

            Create(directSound, secondaryBufferDesc);

            hWaveFormat.Free();
        }

        public DirectSoundSecondaryBuffer(DirectSoundBase directSound, DSBufferDescription bufferDesc)
        {
            Create(directSound, bufferDesc);
        }

        public DirectSoundSecondaryBuffer(IntPtr basePtr)
            : base(basePtr)
        {
            if (basePtr == IntPtr.Zero)
                throw new ArgumentException("basePtr is Zero");
        }

        private void Create(DirectSoundBase directSound, DSBufferDescription bufferDesc)
        {
            IntPtr handle;
            if (bufferDesc.dwFlags.HasFlag(DSBufferCapsFlags.DSBCAPS_PRIMARYBUFFER))
                throw new ArgumentException("Don t set the PRIMARYBUFFER flag for creating a secondarybuffer.", "bufferDesc");
            var result = directSound.CreateSoundBuffer(bufferDesc, out handle, IntPtr.Zero);
            DirectSoundException.Try(result, "IDirectSound", "CreateSoundBuffer");

            _basePtr = handle.ToPointer();
        }

        public DSFXResult[] SetFX(params DSEffectDesc[] effects)
        {
            DSFXResult[] results;
            SetFX(effects.Length, effects, out results);
            return results;
        }

        public void SetFX(int effectsCount, DSEffectDesc[] effects, out DSFXResult[] results)
        {
            if (effectsCount <= 0 || effectsCount > effects.Length)
                throw new ArgumentOutOfRangeException("effectCount");

            results = new DSFXResult[effectsCount];

            fixed (void* peffects = &effects[0], presults = &results[0])
            {
                var result = InteropCalls.CalliMethodPtr(_basePtr, effectsCount, peffects, presults, ((void**)(*(void**)_basePtr))[21]);
                DirectSoundException.Try(result, "IDirectSoundBuffer8", "SetFX");
            }
        }

        public T GetFX<T>(int index) where T : ComObject
        {
            DSResult result;
            var t = GetFX<T>(index, out result);
            DirectSoundException.Try(result, "IDirectSoundBuffer8", "GetObjectInPath");
            return t;
        }

        public T GetFX<T>(int index, out DSResult result) where T : ComObject
        {
            IntPtr ptr;
            result = GetObjectInPath(DSInterop.AllObjects, index, typeof(T).GUID, out ptr);
            return (T)Activator.CreateInstance(typeof(T), ptr);
        }

        public DSResult GetObjectInPath(Guid guidObject, int index, Guid guidInterface, out IntPtr @object)
        {
            fixed (void* ptrEffect = &@object)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, &guidObject, index, &guidInterface, ptrEffect, ((void**)(*(void**)_basePtr))[23]);
            }
        }

        public DSResult AcquireResources(int flags, int effectsCount, out DSFXResult[] results)
        {
            results = new DSFXResult[effectsCount];
            fixed (void* presults = results)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, flags, effectsCount, presults, ((void**)(*(void**)_basePtr))[22]);
            }
        }
    }
}
