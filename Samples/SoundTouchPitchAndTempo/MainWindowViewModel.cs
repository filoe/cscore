using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace SoundTouchPitchAndTempo
{
    public interface IMainWindowViewModel : IDisposable
    {
        ICommand OpenCommand { get; set; }
        ICommand PlayCommand { get; set; }
        ICommand StopCommand { get; set; }

        ICommand TempoUpCommand { get; set; }
        ICommand TempoCenterCommand { get; set; }
        ICommand TempoDownCommand { get; set; }

        ICommand PitchUpCommand { get; set; }
        ICommand PitchCenterCommand { get; set; }
        ICommand PitchDownCommand { get; set; }

        int TempoSliderValue { get; set; }
        int PitchSliderValue { get; set; }
        string TempoValue { get; }
        string PitchValue { get; }

        void PositionSliderMouseDown();
        void PositionSliderMouseUp();

        void Close();
    }

    public class MainWindowViewModel : PropertyChangedBase, IMainWindowViewModel
    {
        private bool _isDisposed;

        private ISoundOut _soundOut;
        private SoundTouchSource _soundTouchSource;

        private bool _stopPositionSliderUpdate;
        private DispatcherTimer _updateTimer;

        public ICommand OpenCommand { get; set; }
        public ICommand PlayCommand { get; set; }
        public ICommand StopCommand { get; set; }

        public ICommand TempoUpCommand { get; set; }
        public ICommand TempoCenterCommand { get; set; }
        public ICommand TempoDownCommand { get; set; }

        public ICommand PitchUpCommand { get; set; }
        public ICommand PitchCenterCommand { get; set; }
        public ICommand PitchDownCommand { get; set; }

        private int _tempoSliderValue;
        public int TempoSliderValue
        {
            get
            {
                return _tempoSliderValue;
            }
            set
            {
                _tempoSliderValue = value;
                OnPropertyChanged();
                OnPropertyChanged("TempoValue");

                if(_soundTouchSource != null)
                {
                    _soundTouchSource.SetTempo(value);
                }
            }
        }

        private int _pitchSliderValue;
        public int PitchSliderValue
        {
            get
            {
                return _pitchSliderValue;
            }
            set
            {
                _pitchSliderValue = value;
                OnPropertyChanged();
                OnPropertyChanged("PitchValue");

                if(_soundTouchSource != null)
                {
                    _soundTouchSource.SetPitch(value / 2.0f);
                }
            }
        }

        public string TempoValue
        {
            get
            {
                return $"{TempoSliderValue}%";
            }
        }

        public string PitchValue
        {
            get
            {
                return $"{PitchSliderValue / 2.0f}";
            }
        }

        public int PositionMaximum
        {
            get
            {
                return 1000;
            }
        }

        private int _positionValue;
        public int PositionValue
        {
            get
            {
                return _positionValue;
            }
            set
            {
                _positionValue = value;
                OnPropertyChanged();

                if(_stopPositionSliderUpdate)
                {
                    PositionSliderValueChanged(PositionValue, PositionMaximum);
                }
            }
        }

        public MainWindowViewModel()
        {
            _updateTimer = new DispatcherTimer();
            _updateTimer.Tick += UpdateTimerTick;
            _updateTimer.Interval = TimeSpan.FromMilliseconds(1);

            OpenCommand = new Command(OpenHandler);
            PlayCommand = new Command(PlayHandler);
            StopCommand = new Command(StopHandler);

            TempoUpCommand = new Command(TempoUpHandler);
            TempoCenterCommand = new Command(TempoCenterHandler);
            TempoDownCommand = new Command(TempoDownHandler);

            PitchUpCommand = new Command(PitchUpHandler);
            PitchCenterCommand = new Command(PitchCenterHandler);
            PitchDownCommand = new Command(PitchDownHandler);

            TempoSliderValue = 0;
            PitchSliderValue = 0;
        }

        public void PositionSliderMouseDown()
        {
            _stopPositionSliderUpdate = true;
        }

        public void PositionSliderMouseUp()
        {
            _stopPositionSliderUpdate = false;
            PositionSliderValueChanged(PositionValue, PositionMaximum);
        }

        public void Close()
        {
            Dispose();
        }

        private void OpenHandler()
        {
            var fileName = OpenFileDialog("MP3 Files|*.mp3");
            if(string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var waveSource = CodecFactory.Instance.GetCodec(fileName)
                .ToSampleSource()
                .AppendSource(x => new SoundTouchSource(x, 50), out _soundTouchSource)
                .ToWaveSource();

            _soundOut = new WasapiOut();
            _soundOut.Initialize(waveSource);

            TempoSliderValue = 0;
            PitchSliderValue = 0;
        }

        private void PositionSliderValueChanged(int value, int maximum)
        {
            var percent = value / (double)maximum;
            TimeSpan position = TimeSpan.FromMilliseconds(Math.Round(_soundOut.WaveSource.GetLength().TotalMilliseconds * percent));
            _soundTouchSource.SetPosition(position);
            _soundTouchSource.Seek();
        }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            var total = _soundOut.WaveSource.GetLength();
            var current = _soundTouchSource.GetPosition();

            if(!_stopPositionSliderUpdate)
            {
                var percent = total != TimeSpan.Zero
                    ? current.TotalMilliseconds / total.TotalMilliseconds * PositionMaximum
                    : 0;

                PositionValue = (int)percent;
            }

            if(current >= total)
            {
                StopHandler();
            }
        }

        private void PlayHandler()
        {
            _soundOut.Play();
            _updateTimer.IsEnabled = true;
            PositionValue = 0;
        }

        private void StopHandler()
        {
            _soundOut.Stop();
            _updateTimer.IsEnabled = false;
            PositionValue = 0;
        }

        private string OpenFileDialog(string filter, string initialDirectory = "")
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter,
                InitialDirectory = initialDirectory
            };

            return openFileDialog.ShowDialog() == DialogResult.OK
                ? openFileDialog.FileName
                : string.Empty;
        }

        private void TempoUpHandler()
        {
            if(TempoSliderValue == 52)
            {
                return;
            }

            TempoSliderValue += 1;
        }

        private void TempoCenterHandler()
        {
            TempoSliderValue = 0;
        }

        private void TempoDownHandler()
        {
            if(TempoSliderValue == -52)
            {
                return;
            }

            TempoSliderValue -= 1;
        }

        private void PitchUpHandler()
        {
            if(PitchSliderValue == 12)
            {
                return;
            }

            PitchSliderValue += 1;
        }

        private void PitchCenterHandler()
        {
            PitchSliderValue = 0;
        }

        private void PitchDownHandler()
        {
            if(PitchSliderValue == -12)
            {
                return;
            }

            PitchSliderValue -= 1;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        private void Dispose(bool isDisposing)
        {
            if(_isDisposed)
            {
                return;
            }

            if(isDisposing)
            {
                StopHandler();

                if(_updateTimer != null)
                {
                    _updateTimer.Stop();
                    _updateTimer = null;
                }

                if(_soundTouchSource != null)
                {
                    _soundTouchSource.Dispose();
                    _soundTouchSource = null;
                }

                if(_soundOut != null)
                {
                    _soundOut.Dispose();
                    _soundOut = null;
                }
            }

            _isDisposed = true;
        }
    }
}