using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.SoundOut.DirectSound
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("C50A7E93-F395-4834-9EF6-7FA99DE50966")]
    public unsafe class DirectSound8 : DirectSoundBase
    {
        public DirectSound8(IntPtr directSound)
            : base(directSound)
        {
        }

        public DSResult VerifyCertification(out DSCertification certified)
        {
            certified = DSCertification.Uncertified;
            fixed (void* pcertified = &certified)
            {
                var result = InteropCalls.CalliMethodPtr(_basePtr, pcertified, ((void**)(*(void**)_basePtr))[11]);
                return result;
            }
        }
    }
}