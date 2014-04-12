using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.DMO.Effects
{
    /// <summary>
    /// Base class for any DirectSoundEffect.
    /// </summary>
    public abstract class DirectSoundFXBase<T> : ComObject where T : struct
    {
        /// <summary>
        /// Default ctor for a ComObject.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundEffect interface.</param>
        public DirectSoundFXBase(IntPtr ptr)
            : base(ptr)
        {
        }

        public T Parameters
        {
            get
            {
                T parameters;
                DmoException.Try(GetAllParametersNative(out parameters), InterfaceName, "GetAllParameters");
                return parameters;
            }
            set
            {
                IntPtr p = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(T)));
                Marshal.StructureToPtr(value, p, true);
                DmoException.Try(SetAllParametersNative(p), InterfaceName, "SetAllParameters");
            }
        }

        protected abstract string InterfaceName
        {
            get;
        }

        /// <summary>
        /// The SetAllParameters method sets the effects parameters.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetAllParametersNative(IntPtr parameters)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, parameters.ToPointer(), ((void**)(*(void**)_basePtr))[3]);
        }

        /// <summary>
        /// The GetAllParameters method retrieves the effects parameters.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetAllParametersNative(out T parameters)
        {
            IntPtr p = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(T)));

            int hresult = InteropCalls.CalliMethodPtr(_basePtr, p.ToPointer(), ((void**)(*(void**)_basePtr))[4]);

            parameters = (T)Marshal.PtrToStructure(p, typeof(T));
            Marshal.FreeCoTaskMem(p);
            return hresult;
        }
    }
}
