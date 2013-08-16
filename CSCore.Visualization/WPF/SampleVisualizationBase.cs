using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace CSCore.Visualization.WPF
{
    public abstract class SampleVisualizationBase : VisualizationBase, ISampleVisualization, IDisposable
    {
        private Mutex _mutex;

        public SampleVisualizationBase()
        {
            _mutex = new Mutex();
        }

        public SampleDataProvider DataProvider
        {
            get
            {
                return (SampleDataProvider)GetValue(DataProviderProperty);
            }
            set
            {
                //if (value == null)
                //    throw new ArgumentNullException("value");

                if (DataProvider != null)
                {
                    DataProvider.BlockRead -= Update;
                }

                SetValue(DataProviderProperty, value);
                if (value != null)
                    DataProvider.BlockRead += Update;
            }
        }

        public static readonly DependencyProperty DataProviderProperty =
            DependencyProperty.Register("DataProvider", typeof(SampleDataProvider), typeof(SampleVisualizationBase), new PropertyMetadata(null));

        public void Update(object sender, BlockReadEventArgs e)
        {
            if (!ValidateTimer())
                return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_mutex.WaitOne(10) == false)
                    return;
                OnUpdate(e.DataLeft, e.DataRight);
                _mutex.ReleaseMutex();
            })/*, System.Windows.Threading.DispatcherPriority.Render*/);
        }

        protected abstract void OnUpdate(float[] left, float[] right);

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == DataProviderProperty)
                DataProvider = e.NewValue as SampleDataProvider;
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
            //only available in .net 4.0+
            //if (_mutex != null)
            //{
            //    _mutex.Dispose();
            //    _mutex = null;
            //}
        }

        ~SampleVisualizationBase()
        {
            Dispose(false);
        }
    }
}