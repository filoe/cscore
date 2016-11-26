using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;

namespace EndpointAudioMeterSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            ((MainViewModel)DataContext).Dispose();
        }
    }

    public sealed class MainViewModel : NotifyPropertyChangedBase, IDisposable
    {
        private ObservableCollection<MMDevice> _devices;
        private MMDevice _selectedDevice;
        private readonly MMDeviceEnumerator _deviceEnumerator;
        private readonly MMNotificationClient _notificationClient;
        private AudioMeterModel _audioMeter;

        public ObservableCollection<MMDevice> Devices
        {
            get { return _devices ?? (_devices = new ObservableCollection<MMDevice>()); }
        }

        public MMDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                AudioMeter.Endpoint = value;
                OnPropertyChanged();
            }
        }

        public AudioMeterModel AudioMeter
        {
            get { return _audioMeter ?? (_audioMeter = new AudioMeterModel()); }
        }

        public MainViewModel()
        {
            _deviceEnumerator = new MMDeviceEnumerator();
            _notificationClient = new MMNotificationClient(_deviceEnumerator);
            _notificationClient.DeviceAdded += (s, e) => UpdateDevices();
            _notificationClient.DeviceRemoved += (s, e) => UpdateDevices();
            _notificationClient.DevicePropertyChanged += (s, e) => UpdateDevices();

            UpdateDevices();
        }

        private void UpdateDevices()
        {
            Devices.Clear();
            foreach (var device in _deviceEnumerator.EnumAudioEndpoints(DataFlow.All, DeviceState.Active))
            {
                Devices.Add(device);
            }
        }

        public void Dispose()
        {
            if (!_deviceEnumerator.IsDisposed)
            {
                _deviceEnumerator.Dispose();
            }

            if (_audioMeter != null)
            {
                _audioMeter.Dispose();
                _audioMeter = null;
            }
        }
    }

    public sealed class AudioMeterModel : NotifyPropertyChangedBase, IDisposable
    {
        private AudioMeterInformation _audioMeterInformation;

        private MMDevice _endpoint;
        private ObservableCollection<AudioMeterItem> _items;
        private readonly DispatcherTimer _timer;

        private WasapiCapture _dummyCapture;

        public MMDevice Endpoint
        {
            get { return _endpoint;}
            set
            {
                _endpoint = value;
                EnableCaptureEndpoint();

                if (_endpoint != null)
                {
                    _audioMeterInformation = AudioMeterInformation.FromDevice(_endpoint);
                }

                Items = null;
            }
        }

        public ObservableCollection<AudioMeterItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public AudioMeterModel()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(30),
                IsEnabled = true
            };
            _timer.Tick += (s, e) => UpdateItems();
        }

        private void UpdateItems()
        {
            if (_audioMeterInformation == null)
                return;
            
            CreateItems();

            var values = _audioMeterInformation.GetChannelsPeakValues();
            _items[0].Value = _audioMeterInformation.PeakValue;
            for (int i = 0; i < values.Length; i++)
            {
                _items[i + 1].Value = values[i];
            }
        }

        private void CreateItems()
        {
            if (Items == null)
            {
                Items = new ObservableCollection<AudioMeterItem> {new AudioMeterItem("MasterPeakValue")};
                for (int i = 0; i < _audioMeterInformation.MeteringChannelCount; i++)
                {
                    Items.Add(new AudioMeterItem(String.Format("Channel {0}", i + 1)));
                }
            }
        }

        private void EnableCaptureEndpoint()
        {
            if (_dummyCapture != null)
            {
                _dummyCapture.Dispose();
                _dummyCapture = null;
            }

            if (Endpoint != null && Endpoint.DataFlow == DataFlow.Capture)
            {
                _dummyCapture = new WasapiCapture(true, AudioClientShareMode.Shared, 250) {Device = Endpoint};
                _dummyCapture.Initialize();
                _dummyCapture.Start();
            }
        }

        public class AudioMeterItem : NotifyPropertyChangedBase
        {
            private string _name;
            private float _value;

            public AudioMeterItem(string name)
            {
                Name = name;
            }

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public float Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    OnPropertyChanged();
                }
            
            }
        }

        public void Dispose()
        {
            if (_dummyCapture != null)
            {
                _dummyCapture.Dispose();
                _dummyCapture = null;
            }
        }
    }

    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        protected void OnPropertyChanged([CallerMemberNameAttribute] string propertyName = null)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
