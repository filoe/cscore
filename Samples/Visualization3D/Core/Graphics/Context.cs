using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D9;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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