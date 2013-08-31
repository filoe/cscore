using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    public class DevicePropertyChangedEventArgs : DeviceNotificationEventArgs
    {
        public PropertyKey PropertyKey { get; private set; }

        public DevicePropertyChangedEventArgs(string deviceID, PropertyKey propertyKey)
            : base(deviceID)
        {
        }
    }
}
