using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Provides information about an audio device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DeviceDetails
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        private short[] internalDeviceIdField;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        private short[] internalDisplayNameField;

        
        //private string internalDeviceIdField;
        //private string internalDisplayNameField;
        private XAudio2DeviceRole _role;
        private WaveFormatExtensible _outputFormat;

        /// <summary>
        ///     Gets the <see cref="DeviceId" /> of the Device.
        /// </summary>
        public unsafe string DeviceId
        {
            get
            {
                /*fixed (void* p = &_internalDeviceIdField0)
                {
                    return Marshal.PtrToStringUni(new IntPtr(p), 256).TrimEnd();
                }*/
                fixed (void* p = &internalDeviceIdField[0])
                {
                    return new string((char*)p);
                }
                //return internalDisplayNameField;
            }
        }

        /// <summary>
        ///     Gets the <see cref="DisplayName" /> of the Device.
        /// </summary>
        public unsafe string DisplayName
        {
            get
            {
                /*fixed (void* p = &_internalDisplayNameField0)
                {
                    return Marshal.PtrToStringUni(new IntPtr(p), 256).TrimEnd();
                }*/
                fixed (void* p = &internalDisplayNameField[0])
                {
                    return new string((char*)p);
                }
                //return new string(internalDisplayNameField);
                //return internalDeviceIdField;
            }
        }

        /// <summary>
        ///     Gets the <see cref="XAudio2DeviceRole" /> of the Device.
        /// </summary>
        public XAudio2DeviceRole Role
        {
            get { return _role; }
        }

        /// <summary>
        ///     Gets the <see cref="OutputFormat" /> of the Device.
        /// </summary>
        public WaveFormatExtensible OutputFormat
        {
            get { return _outputFormat; }
        }
    }
}