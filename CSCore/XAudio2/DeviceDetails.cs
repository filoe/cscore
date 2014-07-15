using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Provides information about an audio device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DeviceDetails
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        private short[] _internalDeviceIdField;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        private short[] _internalDisplayNameField;
        private XAudio2DeviceRole _role;
        private WaveFormatExtensible _outputFormat;

        // ReSharper restore FieldCanBeMadeReadOnly.Local

        /// <summary>
        ///     Gets the <see cref="DeviceId" /> of the Device.
        /// </summary>
        public string DeviceId
        {
            get
            {
                fixed (void* p = &_internalDeviceIdField[0])
                {
                    return new string((char*)p);
                }
            }
        }

        /// <summary>
        ///     Gets the <see cref="DisplayName" /> of the Device.
        /// </summary>
        public string DisplayName
        {
            get
            {
                fixed (void* p = &_internalDisplayNameField[0])
                {
                    return new string((char*)p);
                }
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