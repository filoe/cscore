using System;
using System.Runtime.InteropServices;
using System.Security;
using CSCore.Utils;
using CSCore.Win32;

namespace CSCore.XAudio2.X3DAudio
{
    /// <summary>
    /// Provides access to the X3DAudio functions.
    /// </summary>
    public sealed class X3DAudioCore : IDisposable
    {
// ReSharper disable once InconsistentNaming
        private X3DAudioCalculateDelegate _calculateDelegate;
        private IntPtr _hModule;
        private X3DAudioHandle _handle;

        private X3DAudioInitializeDelegate _initializeDelegate;

        /// <summary>
        ///     Initializes a new instance of the <see cref="X3DAudioCore" /> class.
        /// </summary>
        /// <param name="channelMask">Assignment of channels to speaker positions. This value must not be zero.</param>
        public X3DAudioCore(ChannelMask channelMask)
            : this(channelMask, 343.5f)
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="X3DAudioCore" /> class.
        /// </summary>
        /// <param name="speedOfSound">
        ///     Speed of sound, in user-defined world units per second. Use this value only for doppler
        ///     calculations. It must be greater than or equal to zero.
        /// </param>
        /// <param name="channelMask">Assignment of channels to speaker positions. This value must not be zero.</param>
        public X3DAudioCore(ChannelMask channelMask, float speedOfSound)
        {
            LoadX3DAudio();

            X3DAudioInitialize(channelMask, speedOfSound, out _handle);
        }

        private void LoadX3DAudio()
        {
            _hModule = Win32.NativeMethods.LoadLibrary("X3DAudio1_7.dll");
            if (_hModule == IntPtr.Zero)
                _hModule = Win32.NativeMethods.LoadLibrary("X3DAudio2_8.dll");

            if (_hModule == IntPtr.Zero)
                throw new NotSupportedException("No supported X3DAudio version could be found.");

            _initializeDelegate = GetUnmanagedProc<X3DAudioInitializeDelegate>(_hModule, "X3DAudioInitialize");
            _calculateDelegate = GetUnmanagedProc<X3DAudioCalculateDelegate>(_hModule, "X3DAudioCalculate");

            if (_initializeDelegate == null || _calculateDelegate == null)
            {
                _initializeDelegate = null;
                _calculateDelegate = null;
                Win32.NativeMethods.FreeLibrary(_hModule);
                throw new Exception("Could not load X3DAudio functions.");
            }
        }


        /// <summary>
        ///     Calculates DSP settings with respect to 3D parameters.
        /// </summary>
        /// <param name="listener">Represents the point of reception.</param>
        /// <param name="emitter">Represents the sound source.</param>
        /// <param name="flags">Bitwise combination of <see cref="CalculateFlags" /> specifying which 3D parameters to calculate.</param>
        /// <param name="settings">
        ///     Instance of the <see cref="DspSettings" /> class that receives the calculation results.
        /// </param>
        public unsafe void X3DAudioCalculate(Listener listener, Emitter emitter, CalculateFlags flags,
            DspSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (listener == null)
                throw new ArgumentNullException("listener");
            if (emitter == null)
                throw new ArgumentNullException("emitter");
            if ((int) flags == 0)
                throw new ArgumentOutOfRangeException("flags");

            if(emitter.ChannelCount > 1 && emitter.ChannelAzimuths == null)
                throw new ArgumentException("No ChannelAzimuths set for the specified emitter. The ChannelAzimuths property must not be null if the ChannelCount of the emitter is bigger than 1.");

            DspSettings.DspSettingsNative nativeSettings = settings.NativeInstance;
            Listener.ListenerNative nativeListener = listener.NativeInstance;
            Emitter.EmitterNative nativeEmitter = emitter.NativeInstance;

            try
            {
                #region setup listener

                //setup listener:
                Cone listenerCone = listener.Cone.HasValue ? listener.Cone.Value : default(Cone);
                IntPtr listenerConePtr = listener.Cone.HasValue ? (IntPtr) (&listenerCone) : IntPtr.Zero;
                nativeListener.ConePtr = listenerConePtr;

                #endregion

                #region setup emitter

                //setup emitter
                IntPtr channelAzimuthsPtr = IntPtr.Zero;
                if (emitter.ChannelAzimuths != null && emitter.ChannelAzimuths.Length > 0 && emitter.ChannelCount > 0)
                {
                    const int sizeOfFloat = sizeof (float);
                    int channelAzimuthsSize = sizeOfFloat *
                                              Math.Min(emitter.ChannelCount, emitter.ChannelAzimuths.Length);
                    channelAzimuthsPtr = Marshal.AllocHGlobal(channelAzimuthsSize);
                    ILUtils.WriteToMemory(channelAzimuthsPtr, emitter.ChannelAzimuths, 0,
                        channelAzimuthsSize / sizeOfFloat);
                }

                Cone emitterCone = emitter.Cone.HasValue ? emitter.Cone.Value : default(Cone);
                IntPtr emitterConePtr = emitter.Cone.HasValue ? (IntPtr) (&emitterCone) : IntPtr.Zero;

                nativeEmitter.ChannelAzimuthsPtr = channelAzimuthsPtr;
                nativeEmitter.ConePtr = emitterConePtr;
                nativeEmitter.LFECurvePtr = CurveNative.AllocMemoryAndBuildCurve(emitter.LowFrequencyEffectCurve);
                nativeEmitter.LPFDirectCurvePtr = CurveNative.AllocMemoryAndBuildCurve(emitter.LowPassFilterDirectCurve);
                nativeEmitter.LPFReverbCurvePtr = CurveNative.AllocMemoryAndBuildCurve(emitter.LowPassFilterReverbCurve);
                nativeEmitter.ReverbCurvePtr = CurveNative.AllocMemoryAndBuildCurve(emitter.ReverbCurve);
                nativeEmitter.VolumeCurvePtr = CurveNative.AllocMemoryAndBuildCurve(emitter.VolumeCurve);

                #endregion

                #region setup settings

                //setup settings
                fixed (void* pmc = settings.MatrixCoefficients, pdt = settings.DelayTimes)
                {
                    nativeSettings.MatrixCoefficientsPtr = new IntPtr(pmc);
                    nativeSettings.DelayTimesPtr = new IntPtr(pdt);

                    #endregion

                    fixed (void* p = &_handle)
                    {
                        X3DAudioCalculate(new IntPtr(p), (IntPtr) (&nativeListener),
                            (IntPtr) (&nativeEmitter), flags,
                            new IntPtr(&nativeSettings));
                    }

                    settings.NativeInstance = nativeSettings;
                }
            }
            finally
            {
                nativeEmitter.FreeMemory();
            }
        }

        private void X3DAudioCalculate(IntPtr instance, IntPtr listener, IntPtr emitter, CalculateFlags flags,
            IntPtr dspSettingsPtr)
        {
            _calculateDelegate(instance, listener, emitter, (int) flags, dspSettingsPtr);
        }

        private unsafe void X3DAudioInitialize(ChannelMask speakerChannelMask, float speedOfSound,
            out X3DAudioHandle handle)
        {
            handle = default(X3DAudioHandle);
            fixed (void* p = &handle)
            {
                _initializeDelegate((int) speakerChannelMask, speedOfSound, (IntPtr) p);
            }
        }

        private TDelegate GetUnmanagedProc<TDelegate>(IntPtr hModule, string procName)
            where TDelegate : class
        {
            IntPtr procAddress = Win32.NativeMethods.GetProcAddress(hModule, procName);
            if (procAddress == IntPtr.Zero)
                return null;

            return (TDelegate) ((object) Marshal.GetDelegateForFunctionPointer(procAddress, typeof (TDelegate)));
        }

        /// <summary>
        ///     Disposes the <see cref="X3DAudioCore" /> instance.
        /// </summary>
        public void Dispose()
        {
            _calculateDelegate = null;
            _initializeDelegate = null;

            if (_hModule != IntPtr.Zero)
            {
                Win32.NativeMethods.FreeLibrary(_hModule);
                _hModule = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Destructor which calls <see cref="Dispose()"/>.
        /// </summary>
        ~X3DAudioCore()
        {
            Dispose();
        }

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //msdn tells me that this should be __stdcall. But according to the header files __cdecl is used instead of __stdcall.
        private delegate void X3DAudioCalculateDelegate(IntPtr arg0, IntPtr arg1, IntPtr arg2, int arg3, IntPtr arg4);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //msdn tells me that this should be __stdcall. But according to the header files __cdecl is used instead of __stdcall.
        private delegate void X3DAudioInitializeDelegate(int arg0, float arg1, IntPtr arg2);

        /// <summary>
        ///     X3DAUDIO_HANDLE is an opaque data structure. Because the operating system doesn't allocate any additional storage
        ///     for the 3D audio instance handle, you don't need to free or close it.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 20)]
        private struct X3DAudioHandle
        {
        }
    }
}