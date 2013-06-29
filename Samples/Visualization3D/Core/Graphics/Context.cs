using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Direct3D;
using SharpDX.Windows;
using System.Diagnostics;
using Visualization3D.Core.Input;

namespace Visualization3D.Core.Graphics
{
    public class Context
    {
        public Device Device { get { return DeviceManager.Device; } }
        public DeviceManager DeviceManager { get; set; }
        public InputManager Input { get; set; }
    }
}
