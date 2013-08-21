using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCoreDemo.Model
{
    public class SoundOutDevice
    {
        public string FriendlyName { get; private set; }

        public object NativeDevice { get; private set; }

        public SoundOutDevice(string friendlyName, object nativeDevice)
        {
            FriendlyName = friendlyName;
            NativeDevice = nativeDevice;
        }

        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
