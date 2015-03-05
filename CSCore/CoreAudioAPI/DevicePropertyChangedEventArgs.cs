using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="MMNotificationClient.DevicePropertyChanged"/> event.
    /// </summary>
    public class DevicePropertyChangedEventArgs : DeviceNotificationEventArgs
    {
        /// <summary>
        /// Gets the <see cref="Win32.PropertyKey"/> that specifies the changed property.
        /// </summary>
        public PropertyKey PropertyKey { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevicePropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <param name="propertyKey">The <see cref="Win32.PropertyKey"/> that specifies the changed property.</param>
        public DevicePropertyChangedEventArgs(string deviceId, PropertyKey propertyKey)
            : base(deviceId)
        {
            PropertyKey = propertyKey;
        }
    }
}