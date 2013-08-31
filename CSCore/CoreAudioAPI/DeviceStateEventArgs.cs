using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    public class DeviceStateChangedEventArgs : DeviceNotificationEventArgs
    {
        public DeviceState DeviceState { get; private set; }

        public DeviceStateChangedEventArgs(string deviceID, DeviceState deviceState)
            : base(deviceID)
        {
            DeviceState = deviceState;
        }
    }
}
