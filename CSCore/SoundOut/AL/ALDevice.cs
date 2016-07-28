using System;
using System.Diagnostics;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace CSCore.SoundOut.AL
{
    /// <summary>
    /// Represents an OpenAL Device.
    /// </summary>
    public class ALDevice : IDisposable
    {
        private static ALDevice[] _devices;

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// Gets the device handle.
        /// </summary>
        public IntPtr DeviceHandle
        {
            get
            {
                if (_deviceHandle == IntPtr.Zero)
                {
                    _deviceHandle = ALInterops.alcOpenDevice(Name);
                    if (_deviceHandle == IntPtr.Zero)
                        throw new ALException(String.Format("Could not open device \"{0}\".", Name));
                }
                return _deviceHandle;
            }
        }

        private IntPtr _deviceHandle;

        /// <summary>
        /// Initializes a new ALDevice class
        /// </summary>
        internal ALDevice(string deviceName)
        {
            Name = deviceName;
        }

        /// <summary>
        /// Enumerates all OpenAL devices.
        /// </summary>
        /// <returns>An array containing all found OpenAL devices.</returns>
        public static ALDevice[] EnumerateALDevices()
        {
            var deviceNames = ALInterops.GetALDeviceNames();
            var devices = deviceNames.Select(deviceName => new ALDevice(deviceName));

            if (_devices == null)
            {
                _devices = devices.ToArray();
            }
            else
            {
                //devices which were and are still present in _devices
                var result = _devices.Where(x => devices.Any(d => d.Name == x.Name)).ToList();

                //add new devices which were not present in _devices
                result.AddRange(devices.Where(x => result.All(d => x.Name != d.Name)));
                _devices = result.ToArray();
            }

            return _devices;
        }

        /// <summary>
        /// Gets the default playback device.
        /// </summary>
        public static ALDevice DefaultDevice
        {
            get { return EnumerateALDevices().FirstOrDefault(); }
        }

        /// <summary>
        /// Disposes the openal device
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the openal device
        /// </summary>
        /// <param name="disposing">The disposing state</param>
        protected void Dispose(bool disposing)
        {
            if (_deviceHandle != IntPtr.Zero)
            {
                if (!ALInterops.alcCloseDevice(_deviceHandle))
                {
                    Debug.WriteLine("Failed to close ALDevice. Check whether there are still some active Contexts or Buffers.");
                }
                _deviceHandle = IntPtr.Zero;
            }
        }
    }
}
