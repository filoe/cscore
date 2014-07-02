using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.DMO.Effects
{
    /// <summary>
    ///     Base class for any DirectSoundEffect.
    /// </summary>
    public abstract class DirectSoundFXBase<T> : ComObject where T : struct
    {
        /// <summary>
        ///     Default ctor for a ComObject.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundEffect interface.</param>
        public DirectSoundFXBase(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Gets or sets the Parameters of the Effect.
        /// </summary>
        public T Parameters
        {
            get
            {
                T parameters;
                DmoException.Try(GetAllParametersNative(out parameters), InterfaceName, "GetAllParameters");
                return parameters;
            }
            set { DmoException.Try(SetAllParametersNative(value), InterfaceName, "SetAllParameters"); }
        }

        /// <summary>
        ///     Gets the COM-Interface name of the Effect interface. Used for generating error messages.
        /// </summary>
        protected abstract string InterfaceName { get; }

        /// <summary>
        ///     The SetAllParameters method sets the effects parameters.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetAllParametersNative(T parameters)
        {
            IntPtr p = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (T)));
            try
            {
                Marshal.StructureToPtr(parameters, p, true);
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, p.ToPointer(), ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
            finally
            {
                Marshal.FreeCoTaskMem(p);
            }
        }

        /// <summary>
        ///     The GetAllParameters method retrieves the effects parameters.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetAllParametersNative(out T parameters)
        {
            IntPtr p = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (T)));
            try
            {
                int hresult = InteropCalls.CalliMethodPtr(UnsafeBasePtr, p.ToPointer(), ((void**) (*(void**) UnsafeBasePtr))[4]);

                parameters = (T) Marshal.PtrToStructure(p, typeof (T));
                return hresult;
            }
            finally
            {
                Marshal.FreeCoTaskMem(p);
            }
        }
    }
}