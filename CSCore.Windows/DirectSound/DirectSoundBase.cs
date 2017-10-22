using System;
using System.Runtime.InteropServices;
using System.Security;
using CSCore.Win32;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Used to create buffer objects, manage devices, and set up the environment.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [Guid("279AFA83-4981-11CE-A521-0020AF0BE560")]
    public unsafe class DirectSoundBase : ComObject
    {
        /// <summary>
        /// Returns a new instance of the <see cref="DirectSound"/> class.
        /// </summary>
        /// <param name="device">The device to use for the initialization.</param>
        /// <returns>The new instance of the <see cref="DirectSound"/> class.</returns>
        public static DirectSoundBase Create(Guid device)
        {
            IntPtr ptr;
            DirectSoundException.Try(NativeMethods.DirectSoundCreate(ref device, out ptr, IntPtr.Zero), "DSInterop", "DirectSoundCreate(ref Guid, out IntPtr, IntPtr)");
            return new DirectSoundBase(ptr);
        }

        /// <summary>
        /// Returns a new instance of the <see cref="DirectSound8"/> class.
        /// </summary>
        /// <param name="device">The device to use for the initialization.</param>
        /// <returns>The new instance of the <see cref="DirectSound8"/> class.</returns>
        public static DirectSound8 Create8(Guid device)
        {
            IntPtr ptr;
            DirectSoundException.Try(NativeMethods.DirectSoundCreate8(ref device, out ptr, IntPtr.Zero), "DSInterop", "DirectSoundCreate8(ref Guid, out IntPtr, IntPtr)");
            return new DirectSound8(ptr);
        }

        /// <summary>
        /// Gets the capabilities.
        /// </summary>
        public DirectSoundCapabilities Caps
        {
            get
            {
                DirectSoundCapabilities caps;
                DirectSoundException.Try(GetCapsNative(out caps), "IDirectSound", "GetCaps");
                return caps;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSoundBase"/> class.
        /// </summary>
        /// <param name="directSound">The native pointer of the DirectSound COM object.</param>
        public DirectSoundBase(IntPtr directSound)
            : base(directSound)
        {
        }

        /// <summary>
        /// Checks whether the specified <paramref name="format"/> is supported.
        /// </summary>
        /// <param name="format">The wave format.</param>
        /// <returns>A value indicating whether the specified <paramref name="format"/> is supported. If true, the <paramref name="format"/> is supported; Otherwise false.</returns>
        public bool SupportsFormat(WaveFormat format)
        {
            DirectSoundCapabilities caps = Caps;
            bool result = true;
            if (format.Channels == 2)
                result &= (caps.Flags & DSCapabilitiesFlags.SecondaryBufferStereo) == DSCapabilitiesFlags.SecondaryBufferStereo;
            else if (format.Channels == 1)
                result &= (caps.Flags & DSCapabilitiesFlags.SecondaryBufferMono) == DSCapabilitiesFlags.SecondaryBufferMono;

            if (format.BitsPerSample == 8)
                result &= (caps.Flags & DSCapabilitiesFlags.SecondaryBuffer8Bit) == DSCapabilitiesFlags.SecondaryBuffer8Bit;
            else if (format.BitsPerSample == 16)
                result &= (caps.Flags & DSCapabilitiesFlags.SecondaryBuffer16Bit) == DSCapabilitiesFlags.SecondaryBuffer16Bit;

            result &= format.IsPCM();
            return result;
        }

        /// <summary>
        /// Sets the cooperative level of the application for this sound device. 
        /// </summary>
        /// <param name="hWnd">Handle to the application window.</param>
        /// <param name="level">The requested level.</param>
        public void SetCooperativeLevel(IntPtr hWnd, DSCooperativeLevelType level)
        {
            DirectSoundException.Try(SetCooperativeLevelNative(hWnd, level), "IDirectSound8", "SetCooperativeLevel");
        }

        /// <summary>
        /// Sets the cooperative level of the application for this sound device. 
        /// </summary>
        /// <param name="hWnd">Handle to the application window.</param>
        /// <param name="level">The requested level.</param>
        /// <returns>DSResult</returns>
        public DSResult SetCooperativeLevelNative(IntPtr hWnd, DSCooperativeLevelType level)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, hWnd.ToPointer(), unchecked((int)level), ((void**)(*(void**)UnsafeBasePtr))[6]);
        }

        /// <summary>
        /// Creates a sound buffer object to manage audio samples. 
        /// </summary>
        /// <param name="bufferDesc">A <see cref="DSBufferDescription"/> structure that describes the sound buffer to create.</param>
        /// <param name="pUnkOuter">Must be <see cref="IntPtr.Zero"/>.</param>
        /// <returns>A variable that receives the IDirectSoundBuffer interface of the new buffer object.</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsound8.idirectsound8.createsoundbuffer%28v=vs.85%29.aspx"/>.</remarks>
        public IntPtr CreateSoundBuffer(DSBufferDescription bufferDesc, IntPtr pUnkOuter)
        {
            IntPtr soundBuffer;
            DirectSoundException.Try(CreateSoundBufferNative(bufferDesc, out soundBuffer, pUnkOuter),
                "IDirectSound8", "CreateSoundBuffer");
            return soundBuffer;
        }

        /// <summary>
        /// Creates a sound buffer object to manage audio samples. 
        /// </summary>
        /// <param name="bufferDesc">A <see cref="DSBufferDescription"/> structure that describes the sound buffer to create.</param>
        /// <param name="pUnkOuter">Must be <see cref="IntPtr.Zero"/>.</param>
        /// <param name="soundBuffer">A variable that receives the IDirectSoundBuffer interface of the new buffer object.</param>
        /// <returns>DSResult</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsound8.idirectsound8.createsoundbuffer%28v=vs.85%29.aspx"/>.</remarks>
        public DSResult CreateSoundBufferNative(DSBufferDescription bufferDesc, out IntPtr soundBuffer, IntPtr pUnkOuter)
        {
            fixed (void* ptrsoundbuffer = &soundBuffer)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &bufferDesc, ptrsoundbuffer, (void*)pUnkOuter, ((void**)(*(void**)UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        /// Retrieves the capabilities of the hardware device that is represented by the device object. 
        /// <seealso cref="Caps"/>
        /// </summary>
        /// <param name="caps">Receives the capabilities of this sound device.</param>
        /// <returns>DSResult</returns>
        /// <remarks>Use the <see cref="Caps"/> property instead.</remarks>
        public DSResult GetCapsNative(out DirectSoundCapabilities caps)
        {
            DirectSoundCapabilities tmp = new DirectSoundCapabilities();
            tmp.Size = Marshal.SizeOf(tmp);
            var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, &tmp, ((void**)(*(void**)UnsafeBasePtr))[4]);
            caps = tmp;
            return result;
        }

        /// <summary>
        /// Creates a new secondary buffer that shares the original buffer's memory.
        /// </summary>
        /// <typeparam name="T">Type of the buffer to duplicate.</typeparam>
        /// <param name="bufferOriginal">The buffer to duplicate.</param>
        /// <returns>The duplicated buffer.</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsound8.idirectsound8.duplicatesoundbuffer(v=vs.85).aspx"/>.</remarks>        
        public T DuplicateSoundBuffer<T>(T bufferOriginal) where T : DirectSoundBuffer
        {
            IntPtr resultPtr;
            DirectSoundException.Try(DuplicateSoundBufferNative(bufferOriginal.BasePtr, out resultPtr), "IDirectSound8",
                "DuplicateSoundBuffer");
            return (T)Activator.CreateInstance(typeof(T), resultPtr);
        }

        /// <summary>
        /// Creates a new secondary buffer that shares the original buffer's memory. 
        /// </summary>
        /// <param name="bufferOriginal">Address of the IDirectSoundBuffer or IDirectSoundBuffer8 interface of the buffer to duplicate.</param>
        /// <param name="duplicatedBuffer">Address of a variable that receives the IDirectSoundBuffer interface pointer for the new buffer.</param>
        /// <returns>DSResult</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsound8.idirectsound8.duplicatesoundbuffer(v=vs.85).aspx"/>.</remarks>
        public DSResult DuplicateSoundBufferNative(IntPtr bufferOriginal, out IntPtr duplicatedBuffer)
        {
            fixed (void* p = &duplicatedBuffer)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)bufferOriginal, p,
                    ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        /// Has no effect. See remarks.
        /// </summary>
        /// <remarks>This method was formerly used for compacting the on-board memory of ISA sound cards.</remarks>
        /// <returns>DSResult</returns>
        public DSResult CompactNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[7]);
        }

        /// <summary>
        /// Has no effect. See remarks.
        /// </summary>
        /// <remarks>This method was formerly used for compacting the on-board memory of ISA sound cards.</remarks>
        public void Compact()
        {
            DirectSoundException.Try(CompactNative(), "IDirectSound8", "Compact");
        }

        /// <summary>
        /// Retrieves the speaker configuration. 
        /// </summary>
        /// <param name="speakerConfig">Retrieves the speaker configuration.</param>
        /// <returns>DSResult</returns>
        public DSResult GetSpeakerConfigNative(out DSSpeakerConfigurations speakerConfig)
        {
            fixed (void* pspeakerConfig = &speakerConfig)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pspeakerConfig, ((void**)(*(void**)UnsafeBasePtr))[8]);
            }
        }

        /// <summary>
        /// Retrieves the speaker configuration. 
        /// </summary>
        /// <returns>The speaker configuration.</returns>
        public DSSpeakerConfigurations GetSpeakerConfig()
        {
            DSSpeakerConfigurations speakerConfig;
            DirectSoundException.Try(GetSpeakerConfigNative(out speakerConfig), "IDirectSound8", "GetSpeakerConfig");
            return speakerConfig;
        }

        /// <summary>
        /// Specifies the speaker configuration of the device. 
        /// </summary>
        /// <param name="speakerConfig">The speaker configuration.</param>
        /// <returns>DSResult</returns>
        /// <remarks>
        /// In Windows Vista and later versions of Windows, <see cref="GetSpeakerConfig"/> is a NOP. For Windows Vista and later versions, the speaker configuration is a system setting that should not be modified by an application. End users can set the speaker configuration through control panels.
        /// For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsound8.idirectsound8.setspeakerconfig(v=vs.85).aspx"/>.
        /// </remarks>
        public DSResult SetSpeakerConfigNative(DSSpeakerConfigurations speakerConfig)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (int)speakerConfig, ((void**)(*(void**)UnsafeBasePtr))[9]);
        }

        /// <summary>
        /// Specifies the speaker configuration of the device. 
        /// </summary>
        /// <param name="speakerConfig">The speaker configuration.</param>
        /// <remarks>
        /// In Windows Vista and later versions of Windows, <see cref="GetSpeakerConfig"/> is a NOP. For Windows Vista and later versions, the speaker configuration is a system setting that should not be modified by an application. End users can set the speaker configuration through control panels.
        /// For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsound8.idirectsound8.setspeakerconfig(v=vs.85).aspx"/>.
        /// </remarks>
        public void SetSpeakerConfig(DSSpeakerConfigurations speakerConfig)
        {
            DirectSoundException.Try(SetSpeakerConfigNative(speakerConfig), "IDirectSound8", "SetSpeakerConfig");
        }

        /// <summary>
        /// Initializes a device object that was created by using the CoCreateInstance function. 
        /// </summary>
        /// <param name="device">The globally unique identifier (GUID) specifying the sound driver to which this device object binds. Pass null to select the primary sound driver.</param>
        /// <returns>DSResult</returns>
        public DSResult InitializeNative(Guid? device)
        {
            Guid d = Guid.Empty;
            if (device.HasValue)
                d = device.Value;

            void* ptr = device == null ? (void*)IntPtr.Zero : &d;
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[10]);
        }

        /// <summary>
        /// Initializes a device object that was created by using the CoCreateInstance function. 
        /// </summary>
        /// <param name="device">The globally unique identifier (GUID) specifying the sound driver to which this device object binds. Pass null to select the primary sound driver.</param>
        public void Initialize(Guid? device)
        {
            DirectSoundException.Try(InitializeNative(device), "IDirectSound8", "Initialize");
        }

        /// <summary>
        /// Combines a <see cref="DSSpeakerGeometry"/> value with a <see cref="DSSpeakerConfigurations"/> value.
        /// </summary>
        /// <param name="speakerConfiguration">Must be <see cref="DSSpeakerConfigurations.Stereo"/>.</param>
        /// <param name="speakerGeometry">The <see cref="DSSpeakerGeometry"/> value to combine with the <paramref name="speakerConfiguration"/>.</param>
        /// <returns>Combination out of the <paramref name="speakerConfiguration"/> and the <paramref name="speakerGeometry"/> value.</returns>
        /// <exception cref="ArgumentException">Must be stereo.; speakerConfiguration</exception>
        public DSSpeakerConfigurations CombineSpeakerConfiguration(DSSpeakerConfigurations speakerConfiguration, DSSpeakerGeometry speakerGeometry)
        {
            if(speakerConfiguration != DSSpeakerConfigurations.Stereo)
                throw new ArgumentException("Must be stereo.", "speakerConfiguration");

            int c = (int) speakerConfiguration;
            int g = (int) speakerGeometry;
            return (DSSpeakerConfigurations) (((byte) c) | (byte) g << 16);
        }
    }
}