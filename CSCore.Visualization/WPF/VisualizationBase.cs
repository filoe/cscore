using CSCore.DSP;
using CSCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSCore.Visualization.WPF
{
    public abstract class VisualizationBase : ContentControl, IVisualization
    {
        bool enableRendering = true;
        public bool EnableRendering
        {
            get { return (bool)GetValue(EnableRenderingProperty); }
            set { SetValue(EnableRenderingProperty, value); enableRendering = value; }
        }

        public static readonly DependencyProperty EnableRenderingProperty =
            DependencyProperty.Register("EnableRendering", typeof(bool), typeof(VisualizationBase), new PropertyMetadata(true));

        private FPSTimer _timer;

        protected FPSTimer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public VisualizationBase()
        {
            _timer = new FPSTimer(60);
            _timer.Start();
            EnableRendering = true;
        }

        protected virtual bool ValidateTimer()
        {
            bool flag = false;
            if (_timer.Update())
            {
                Dispatcher.Invoke(new Action(() => flag = EnableRendering));
            }
            return flag;
        }
    }
}