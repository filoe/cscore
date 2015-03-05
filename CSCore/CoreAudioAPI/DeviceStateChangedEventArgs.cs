using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="MMNotificationClient.DeviceStateChanged"/> event.
    /// </summary>
    public class DeviceStateChangedEventArgs : DeviceNotificationEventArgs
    {
        /// <summary>
        /// Gets the new state of the endpoint device. 
        /// </summary>
        public DeviceState DeviceState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <param name="deviceState">The new state of the endpoint device. </param>
        public DeviceStateChangedEventArgs(string deviceId, DeviceState deviceState)
            : base(deviceId)
        {
            DeviceState = deviceState;
        }
    }
}