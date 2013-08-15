using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace CSCore.Visualization.WPF
{
    public abstract class FFTVisualizationBase : VisualizationBase, IFFTVisualization, IDisposable
    {
        private Mutex _mutex;

        public FFTVisualizationBase()
        {
            _mutex = new Mutex();
        }

        public FFTDataProvider DataProvider
        {
            get
            {
                return (FFTDataProvider)GetValue(DataProviderProperty);
            }
            set
            {
                //if (value == null)
                //    throw new ArgumentNullException("value");

                if (DataProvider != null)
                {
                    DataProvider.FFTCalculated -= Update;
                }

                SetValue(DataProviderProperty, value);
                if (value != null)
                    DataProvider.FFTCalculated += Update;
            }
        }

        public static readonly DependencyProperty DataProviderProperty =
            DependencyProperty.Register("DataProvider", typeof(FFTDataProvider), typeof(FFTVisualizationBase), new PropertyMetadata(null));

        public void Update(object sender, DSP.FFTCalculatedEventArgs e)
        {
            if (!ValidateTimer())
                return;

            double[] values = new double[e.Data.Length];
            for (int i = 0; i < e.Data.Length; i++)
            {
                values[i] = e.Data[i].CalculateFFTPercentage();
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_mutex.WaitOne(10) == false)
                    return;
                OnUpdate(values);
                _mutex.ReleaseMutex();
            })/*, System.Windows.Threading.DispatcherPriority.Render*/);
        }

        protected abstract void OnUpdate(double[] values);

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == DataProviderProperty)
                DataProvider = e.NewValue as FFTDataProvider;
        }

        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_mutex != null)
            {
                _mutex.Dispose();
                _mutex = null;
            }
        }

        ~FFTVisualizationBase()
        {
            Dispose(false);
        }
    }
}