using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CSCoreDemo.Model;
using CSCore.Codecs;
using System.Diagnostics;

namespace CSCoreDemo.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        TagsViewModel _tagViewModel;
        public TagsViewModel TagViewModel
        {
            get { return _tagViewModel ?? (_tagViewModel = new TagsViewModel()); }
            set { SetProperty(value, ref _tagViewModel, () => TagViewModel); }
        }

        SoundModificationViewModel _soundModificationViewModel;
        public SoundModificationViewModel SoundModificationViewModel
        {
            get { return _soundModificationViewModel ?? (_soundModificationViewModel = new SoundModificationViewModel()); }
            set { SetProperty(value, ref _soundModificationViewModel, () => SoundModificationViewModel); }
        }

        VisualizationViewModel _visualizationViewModel;
        public VisualizationViewModel VisualizationViewModel
        {
            get { return _visualizationViewModel ?? (_visualizationViewModel = new VisualizationViewModel()); }
            set { SetProperty(value, ref _visualizationViewModel, () => VisualizationViewModel); }
        }

        AudioPlayer _audioPlayer;
        public AudioPlayer AudioPlayer
        {
            get
            {
                if (_audioPlayer == null)
                {
                    _audioPlayer = new AudioPlayer();
                    _audioPlayer.Updated += OnAudioPlayerUpdated;
                }
                return _audioPlayer;
            }
        }

        private void OnAudioPlayerUpdated(object sender, EventArgs e)
        {
            if (UpdatePosition)
            {
                OnPropertyChanged(() => Position);
                OnPropertyChanged(() => Length);
            }
            else
            {
                Debug.WriteLine("cancel");
            }
        }

        List<SoundOutType> _soundOutTypes;
        public List<SoundOutType> SoundOutTypes
        {
            get 
            {
                if (_soundOutTypes == null)
                {
                    if(CSCore.SoundOut.WasapiOut.IsSupportedOnCurrentPlatform)
                        _soundOutTypes = new List<SoundOutType>(new SoundOutType[] { SoundOutType.WaveOut, SoundOutType.DirectSound, SoundOutType.Wasapi });
                    else
                        _soundOutTypes = new List<SoundOutType>(new SoundOutType[] { SoundOutType.WaveOut, SoundOutType.DirectSound });
                }
                return _soundOutTypes; 
            }
            set { SetProperty(value, ref _soundOutTypes, () => SoundOutTypes); }
        }

        SoundOutType _soundOutType = SoundOutType.DirectSound;
        public SoundOutType SelectedSoundOutType
        {
            get { return _soundOutType; }
            set 
            {
                AudioPlayer.SetupAudioPlayer(value);
                OnPropertyChanged(() => Devices);
                SetProperty(value, ref _soundOutType, () => SelectedSoundOutType);
                Device = Devices.FirstOrDefault();
            }
        }

        public IEnumerable<SoundOutDevice> Devices
        {
            get { return AudioPlayer.Devices; }
        }

        SoundOutDevice _device;
        public SoundOutDevice Device
        {
            get { return _device; }
            set
            {
                AudioPlayer.Device = value;
                SetProperty(value, ref _device, () => Device);
            }
        }

        ICommand _openfileCommand;
        public ICommand OpenFileCommand
        {
            get { return _openfileCommand ?? new AutoDelegateCommand((c) => OpenFile(), (c) => CanOpenFile()); }
            set { SetProperty(value, ref _openfileCommand, () => OpenFileCommand); }
        }

        ICommand _playCommand;
        public ICommand PlayCommand
        {
            get { return _playCommand ?? (_playCommand = new AutoDelegateCommand((c) => Play(), (c) => CanPlay())); }
            set { SetProperty(value, ref _playCommand, () => PlayCommand); }
        }

        ICommand _pauseCommand;
        public ICommand PauseCommand
        {
            get { return _pauseCommand ?? (_pauseCommand = new AutoDelegateCommand((c) => Pause(), (c) => CanPause())); }
            set { SetProperty(value, ref _pauseCommand, () => PauseCommand); }
        }

        ICommand _stopCommand;
        public ICommand StopCommand
        {
            get { return _stopCommand ?? (_stopCommand = new AutoDelegateCommand((c) => Stop(), (c) => CanStop())); }
            set { SetProperty(value, ref _stopCommand, () => StopCommand); }
        }

        public TimeSpan Position
        {
            get { return AudioPlayer.Position; }
            set 
            {     
                AudioPlayer.Position = value;
                OnPropertyChanged(() => Position);            
            }
        }

        public TimeSpan Length
        {
            get { return AudioPlayer.Length; }
        }

        bool updatePosition = true;
        public bool UpdatePosition
        {
            get { return updatePosition; }
            set { SetProperty(value, ref updatePosition, () => UpdatePosition); }
        }

        public MainViewModel()
        {
            SelectedSoundOutType = SoundOutType.DirectSound;
        }

        public void OpenFile()
        {
            var ofn = new Microsoft.Win32.OpenFileDialog();
            ofn.Filter = CodecFactory.SupportedFilesFilterEN;
            if (ofn.ShowDialog().Value)
            {
                AudioPlayer.OpenFile(ofn.FileName, (s) => VisualizationViewModel.InitializeVisualization(s));
                TagViewModel.LoadTags(ofn.FileName);
            }
        }

        public void Play()
        {
            AudioPlayer.Play();
        }

        public void Pause()
        {
            AudioPlayer.Pause();
        }

        public void Stop()
        {
            AudioPlayer.Stop();
        }

        public bool CanOpenFile()
        {
            return AudioPlayer.CanOpenFile;
        }

        public bool CanPlay()
        {
            return AudioPlayer.CanPlay;
        }

        public bool CanPause()
        {
            return AudioPlayer.CanPause;
        }

        public bool CanStop()
        {
            return AudioPlayer.CanStop;
        }

        public void OnClosing(object s, System.ComponentModel.CancelEventArgs e)
        {
            AudioPlayer.Dispose();
        }
    }
}
