using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    public class DeviceNotificationEventArgs : EventArgs
    {
        public string DeviceID { get; private set; }

        public DeviceNotificationEventArgs(string deviceID)
        {
            if (String.IsNullOrEmpty(deviceID))
                throw new ArgumentNullException("deviceID");
            DeviceID = deviceID;
        }
    }
}
