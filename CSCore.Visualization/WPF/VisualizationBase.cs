using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CSCore.Utils;
using CSCore.DSP;

namespace CSCore.Visualization.WPF
{
    public abstract class VisualizationBase : ContentControl, IVisualization
    {
        public bool EnableRendering
        {
            get { return (bool)GetValue(EnableRenderingProperty); }
            set { SetValue(EnableRenderingProperty, value); }
        }

        public static readonly DependencyProperty EnableRenderingProperty =
            DependencyProperty.Register("EnableRendering", typeof(bool), typeof(VisualizationBase), new PropertyMetadata(true));

        FPSTimer _timer;
        protected FPSTimer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public VisualizationBase()
        {
            _timer = new FPSTimer(60);
            _timer.Start();
        }

        protected virtual bool ValidateTimer()
        {
            return _timer.Update();
        }
    }
}
