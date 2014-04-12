using CSCore.Win32;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.SoundOut.DirectSound
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("6825A449-7524-4D82-920F-50E36AB3AB1E")]
    public unsafe class DirectSoundSecondaryBuffer : DirectSoundBufferBase
    {
        public DirectSoundSecondaryBuffer(DirectSoundBase directSound, WaveFormat waveFormat, int bufferSize)
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
            if ((bufferDesc.dwFlags & DSBufferCapsFlags.DSBCAPS_PRIMARYBUFFER) == DSBufferCapsFlags.DSBCAPS_PRIMARYBUFFER)
                throw new ArgumentException("Don t set the PRIMARYBUFFER flag for creating a secondarybuffer.", "bufferDesc");

            _basePtr = directSound.CreateSoundBuffer(bufferDesc, IntPtr.Zero).ToPointer();
        }

        public DSResult GetObjectInPath(Guid guidObject, int index, Guid guidInterface, out IntPtr @object)
        {
            fixed (void* ptrEffect = &@object)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, &guidObject, index, &guidInterface, ptrEffect, ((void**)(*(void**)_basePtr))[23]);
            }
        }
    }
}