using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CSCore.SoundOut;
using CSCore.SoundOut.MMInterop;

namespace CSCore.SoundIn
{
    /// <summary>
    /// Represents a <see cref="WaveIn"/>-device.
    /// </summary>
    public class WaveInDevice
    {
        /// <summary>
        /// Enumerates the WaveIn devices installed on the system.
        /// </summary>
        /// <returns>A an iterator to iterate through all enumerated WaveIn devices.</returns>
        public static IEnumerable<WaveInDevice> EnumerateDevices()
        {
            for (int i = 0; i < NativeMethods.waveInGetNumDevs(); i++)
            {
                yield return new WaveInDevice(i);
            }
        }

        /// <summary>
        /// Gets the default WaveOut device.
        /// </summary>
        public static WaveInDevice DefaultDevice
        {
            get { return new WaveInDevice(0); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveInDevice"/> class.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        public WaveInDevice(int deviceId)
        {
            var caps = new WaveInCaps();
            MmException.Try(
                NativeMethods.waveInGetDevCaps((IntPtr) deviceId, out caps, (uint) Marshal.SizeOf(caps)),
                "waveInGetDevCaps");

            DeviceId = deviceId;
            Name = caps.Name;
            DriverVersion = new Version(caps.DriverVersion.HighWord(), caps.DriverVersion.LowWord());
            SupportedFormatsFlags = caps.Formats;
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