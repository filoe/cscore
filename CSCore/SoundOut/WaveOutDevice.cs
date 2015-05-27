using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CSCore.SoundOut.MMInterop;

namespace CSCore.SoundOut
{
    /// <summary>
    /// Represents a <see cref="WaveOut"/>-device.
    /// </summary>
    public class WaveOutDevice
    {
        /// <summary>
        /// Enumerates the WaveOut devices installed on the system.
        /// </summary>
        /// <returns>A an iterator to iterate through all enumerated WaveOut devices.</returns>
        public static IEnumerable<WaveOutDevice> EnumerateDevices()
        {
            for (int i = 0; i < NativeMethods.waveOutGetNumDevs(); i++)
            {
                yield return new WaveOutDevice(i);
            }
        }

        /// <summary>
        /// Gets the default WaveOut device.
        /// </summary>
        public static WaveOutDevice DefaultDevice
        {
            get { return new WaveOutDevice(0); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveOutDevice"/> class.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        public WaveOutDevice(int deviceId)
        {
            var caps = new WaveOutCaps();
            MmException.Try(NativeMethods.waveOutGetDevCaps((IntPtr) deviceId, out caps, (uint) Marshal.SizeOf(caps)),
                "waveOutGetDevCaps");
            DeviceId = deviceId;
            Name = caps.szPname;
            DriverSupported = caps.dwSupport;
            DriverVersion = new Version(caps.vDriverVersion.HighWord(), caps.vDriverVersion.LowWord());
            SupportedFormatsFlags = caps.dwFormats;
            SupportedFormats = caps.GetSupportedFormats();
        }

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        public int DeviceId { get; private set; }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the supported functionalities of the device.
        /// </summary>
        public MmDeviceSupported DriverSupported { get; private set; }

        /// <summary>
        /// Gets the version of the driver.
        /// </summary>
        public Version DriverVersion { get; private set; }

        /// <summary>
        /// Gets the standard formats that are supported.
        /// </summary>
        public MmDeviceFormats SupportedFormatsFlags { get; private set; }

        /// <summary>
        /// Gets the supported formats.
        /// </summary>
        public WaveFormat[] SupportedFormats { get; private set; }
    }
}