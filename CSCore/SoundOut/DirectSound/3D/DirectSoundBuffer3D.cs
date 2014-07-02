using CSCore.Win32;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.SoundOut.DirectSound
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("279AFA86-4981-11CE-A521-0020AF0BE560")]
    public unsafe class DirectSoundBuffer3D : ComObject
    {
        public static DirectSoundBuffer3D FromBuffer(DirectSoundBufferBase buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return buffer.QueryInterface<DirectSoundBuffer3D>();
        }

        public DirectSoundBuffer3D(IntPtr ptr)
            : base(ptr)
        {
        }

        public DSResult GetAllParameters(out DSBuffer3DSettings dsBuffer3DSettings)
        {
            fixed (void* psettings = &dsBuffer3DSettings)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, psettings, ((void**)(*(void**)UnsafeBasePtr))[3]);
            }
        }

        public DSResult GetConeAngles(out int insideConeAngle, out int outsideConeAngle)
        {
            fixed (void* pinside = &insideConeAngle, poutside = &outsideConeAngle)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pinside, poutside, ((void**)(*(void**)UnsafeBasePtr))[4]);
            }
        }

        public DSResult GetConeOrientation(out D3DVector orientation)
        {
            fixed (void* porientation = &orientation)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, porientation, ((void**)(*(void**)UnsafeBasePtr))[5]);
            }
        }

        public DSResult GetConeOutsideVolume(out int coneOutsideVolume)
        {
            fixed (void* pconeOutsideVolume = &coneOutsideVolume)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pconeOutsideVolume, ((void**)(*(void**)UnsafeBasePtr))[6]);
            }
        }

        public DSResult GetMaxDistance(out float maxDistance)
        {
            fixed (void* pmaxDistance = &maxDistance)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pmaxDistance, ((void**)(*(void**)UnsafeBasePtr))[7]);
            }
        }

        public DSResult GetMinDistance(out float mixDistance)
        {
            fixed (void* pminDistance = &mixDistance)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pminDistance, ((void**)(*(void**)UnsafeBasePtr))[8]);
            }
        }

        public DSResult GetMode(out DSMode3D mode)
        {
            fixed (void* pmode = &mode)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pmode, ((void**)(*(void**)UnsafeBasePtr))[9]);
            }
        }

        public DSResult GetPosition(out D3DVector position)
        {
            fixed (void* pposition = &position)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pposition, ((void**)(*(void**)UnsafeBasePtr))[10]);
            }
        }

        public DSResult GetVelocity(out D3DVector velocity)
        {
            fixed (void* pvelocity = &velocity)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pvelocity, ((void**)(*(void**)UnsafeBasePtr))[11]);
            }
        }

        public DSResult SetAllParameters(DSBuffer3DSettings settings, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &settings, unchecked((int)applyMode), ((void**)(*(void**)UnsafeBasePtr))[12]);
        }

        public DSResult SetConeAngles(int insideConeAngle, int outsideConeAngle, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, insideConeAngle, outsideConeAngle, unchecked((int)applyMode), ((void**)(*(void**)UnsafeBasePtr))[13]);
        }

        public DSResult SetConeOrientation(D3DVector coneOrientation, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return SetConeOrientation(coneOrientation.X, coneOrientation.Y, coneOrientation.Z, applyMode);
        }

        public DSResult SetConeOrientation(float x, float y, float z, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, x, y, z, unchecked((int)applyMode), ((void**)(*(void**)UnsafeBasePtr))[14]);
        }

        public DSResult SetConeOutsideVolume(int coneOutsideVolume, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, coneOutsideVolume, unchecked((int)applyMode), ((void**)(*(void**)UnsafeBasePtr))[15]);
        }

        public DSResult SetMaxDistance(float maxDistance, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, maxDistance, unchecked((int)applyMode), ((void**)(*(void**)UnsafeBasePtr))[16]);
        }

        public DSResult SetMinDistance(float minDistance, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, minDistance, unchecked((int)applyMode), ((void**)(*(void**)UnsafeBasePtr))[17]);
        }

        public DSResult SetMode(DSMode3D mode, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, unchecked((int)mode), unchecked((int)applyMode), ((void**)(*(void**)UnsafeBasePtr))[18]);
        }

        public DSResult SetPosition(D3DVector position, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return SetPosition(position.X, position.Y, position.Z, applyMode);
        }

        public DSResult SetPosition(float x, float y, float z, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, x, y, z, unchecked((int)applyMode), ((void**)(*(void**)UnsafeBasePtr))[19]);
        }

        public DSResult SetVelocity(D3DVector velocity, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return SetVelocity(velocity.X, velocity.Y, velocity.Z, applyMode);
        }

        public DSResult SetVelocity(float x, float y, float z, DS3DApplyMode applyMode = DS3DApplyMode.Immediate)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, x, y, z, unchecked((int)applyMode), ((void**)(*(void**)UnsafeBasePtr))[20]);
        }
    }
}