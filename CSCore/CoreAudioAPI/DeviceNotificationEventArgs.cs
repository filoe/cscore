using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides basic data for all device notification events.
    /// </summary>
    public class DeviceNotificationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the device id that identifies the audio endpoint device.
        /// </summary>
        public string DeviceId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceNotificationEventArgs"/> class.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        public DeviceNotificationEventArgs(string deviceId)
        {
            if (String.IsNullOrEmpty(deviceId))
                throw new ArgumentNullException("deviceId");
            DeviceId = deviceId;
        }


        /// <summary>
        /// Tries the get device associated with the <see cref="DeviceId"/>.
        /// </summary>
        /// <param name="device">The device associated with the <see cref="DeviceId"/>. If the return value is <c>false</c>, the <paramref name="device"/> will be null.</param>
        /// <returns><c>true</c> if the associated device be successfully retrieved; false otherwise.</returns>
        public bool TryGetDevice(out MMDevice device)
        {
            try
            {
                using (var deviceEnumerator = new MMDeviceEnumerator())
                {
                    device = deviceEnumerator.GetDevice(DeviceId);
                }
                return true;
            }
            catch (Exception)
            {
                device = null;
            }

            return false;
        }
    }
}