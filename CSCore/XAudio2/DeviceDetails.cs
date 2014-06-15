using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    /// Provides information about an audio device. 
    /// </summary>
    public partial struct DeviceDetails
    {
        /// <summary>
        /// Gets the <see cref="DisplayName"/> of the Device.
        /// </summary>
        public string DisplayName
        {
            unsafe get
            {
                fixed (void* p = &_internalDisplayNameField0)
                {
                    return Marshal.PtrToStringUni(new IntPtr(p), 256).TrimEnd();
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="DeviceId"/> of the Device.
        /// </summary>
        public string DeviceId
        {
            unsafe get
            {
                fixed (void* p = &_internalDeviceIdField0)
                {
                    return Marshal.PtrToStringUni(new IntPtr(p), 256).TrimEnd();
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="XAudio2DeviceRole"/> of the Device.
        /// </summary>
        public XAudio2DeviceRole Role
        {
            get { return _role; }
        }

        /// <summary>
        /// Gets the <see cref="OutputFormat"/> of the Device.
        /// </summary>
        public WaveFormatExtensible OutputFormat
        {
            get { return _outputFormat; }
        }
    }
}