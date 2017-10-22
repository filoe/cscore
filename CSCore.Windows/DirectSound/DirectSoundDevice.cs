using System;
using System.Collections.Generic;
using System.Linq;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Represents a directsound-device.
    /// </summary>
    public class DirectSoundDevice
    {
        /// <summary>
        /// The guid of the default playback device.
        /// </summary>
        public static readonly Guid DefaultPlaybackGuid = new Guid("DEF00000-9C6D-47ED-AAF1-4DDA8F2B5C03");

        /// <summary>
        /// Gets the default playback device.
        /// </summary>
        public static DirectSoundDevice DefaultDevice
        {
            get
            {
                var devices = DirectSoundDeviceEnumerator.EnumerateDevices();
                var defaultDevice = devices.FirstOrDefault(x => x.Guid == DefaultPlaybackGuid);
                return defaultDevice ?? (devices.FirstOrDefault());
            }
        }

        /// <summary>
        /// Enumerates all directsound-devices. Use the <see cref="DirectSoundDeviceEnumerator.EnumerateDevices"/> method instead.
        /// </summary>
        /// <returns>A list, containing all enumerated directsound-devices.</returns>
        [Obsolete("Use the DirectSoundDeviceEnumerator.EnumerateDevices method instead.")]
        public static List<DirectSoundDevice> EnumerateDevices()
        {
            return DirectSoundDeviceEnumerator.EnumerateDevices().ToList();
        }

        /// <summary>
        /// Gets the textual description of the DirectSound device.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the module name of the DirectSound driver corresponding to this device.
        /// </summary>
        public string Module { get; private set; }

        /// <summary>
        /// The <see cref="System.Guid"/> that identifies the device being enumerated.
        /// </summary>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSoundDevice"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="module">The module.</param>
        /// <param name="guid">The unique identifier.</param>
        public DirectSoundDevice(string description, string module, Guid guid)
        {
            Description = description;
            Module = module;
            Guid = guid;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DirectSoundDevice"/> to <see cref="Guid"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>
        /// The <see cref="Guid"/> of the <paramref name="device"/>.
        /// </returns>
        public static explicit operator Guid(DirectSoundDevice device)
        {
            return device.Guid;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Description;
        }
    }
}