using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Direct3D;
using SharpDX.Windows;
using System.Diagnostics;
using System.Windows.Forms;
using CSCore.Codecs;

using Visualization3D.Core.Graphics;

namespace Visualization3D
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            CSCore.Utils.Logger.LogDispatcher.Loggers.Add(new CSCore.Utils.Logger.ConsoleLogger());

            VisualizationRoot program = new VisualizationRoot() { WindowState = FormWindowState.Maximized };
            program.Text = "3D Visualisierung";
            program.Show();
            program.Run();
        }
    }
}
