using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.SoundOut.DirectSound
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("279AFA85-4981-11CE-A521-0020AF0BE560")]
    public unsafe class DirectSoundPrimaryBuffer : DirectSoundBufferBase
    {
        public DirectSoundPrimaryBuffer(DirectSoundBase directSound)
        {
            if (directSound == null) throw new ArgumentNullException("directSound");

            DSBufferDescription primaryBufferDesc = new DSBufferDescription()
            {
                dwBufferBytes = 0,
                dwFlags = DSBufferCapsFlags.DSBCAPS_PRIMARYBUFFER | DSBufferCapsFlags.DSBCAPS_CTRLVOLUME | DSBufferCapsFlags.DSBCAPS_CTRL3D,
                dwReserved = 0,
                lpwfxFormat = IntPtr.Zero,
                guid3DAlgorithm = Guid.Empty
            };
            primaryBufferDesc.dwSize = Marshal.SizeOf(primaryBufferDesc);

            _basePtr = directSound.CreateSoundBuffer(primaryBufferDesc, IntPtr.Zero).ToPointer();
        }

        public DirectSoundPrimaryBuffer(IntPtr basePtr)
            : base(basePtr)
        {
            if (basePtr == IntPtr.Zero)
                throw new ArgumentException("basePtr is Zero");
        }
    }
}