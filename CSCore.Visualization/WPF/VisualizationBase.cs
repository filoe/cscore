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
        bool _enablerendering = true;
        public virtual bool EnableRendering
        {
            get { return _enablerendering; }
            set 
            {
                if (_enablerendering != value)
                {
                    _enablerendering = value;
                    OnEnableRenderingChanged(value);
                }
            }
        }

        FPSTimer _timer;
        protected FPSTimer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public VisualizationBase()
        {
            _timer = new FPSTimer(1000 / 60);
        }

        protected virtual void OnEnableRenderingChanged(bool newvalue)
        {

        }

        protected virtual bool ValidateTimer()
        {
            return _timer.Update();
        }
    }
}
