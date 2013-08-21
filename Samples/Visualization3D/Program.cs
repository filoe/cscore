using CSCore.Codecs;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D9;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Visualization3D.Core.Graphics;

namespace Visualization3D
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            VisualizationRoot program = new VisualizationRoot() { WindowState = FormWindowState.Maximized };
            program.Text = "3D Visualisierung";
            program.Show();
            program.Run();
        }
    }
}