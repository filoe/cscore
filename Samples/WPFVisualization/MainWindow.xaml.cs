using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.MP3;
using CSCore.DSP;
using CSCore.SoundOut;
using CSCore.SoundOut.DirectSound;
using CSCore.Streams;
using CSCore.Streams.SampleConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFVisualisation
{
    public partial class MainWindow : Window
    {
        private ISoundOut _soundOut;
        private DispatcherTimer _timer;

        private IWaveSource _audioSource;
        private GainSource _gainSource;

        private double _gain = 1;

        public double Gain
        {
            get
            {
                return _gain;
            }
            set
            {
                _gain = value;
                if (_gainSource != null)
                {
                    _gainSource.Gain = (float)value;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CSCore.Context.Current.CreateDefaultLogger();
        }

        private void OnOpenFile(object sender, RoutedEventArgs e)
        {
            var ofn = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = CodecFactory.SupportedFilesFilterEN,
                Title = "Select file"
            };

            if (ofn.ShowDialog().GetValueOrDefault())
            {
                openmenu.IsEnabled = false;
                ShowBufferedIndicator(false);
                OpenSource(CodecFactory.Instance.GetCodec(ofn.FileName));
                openmenu.IsEnabled = true;
            }
        }

        private void OnOpenStream(object sender, RoutedEventArgs e)
        {
            StreamURLSelector streamSelector = new StreamURLSelector();
            if (streamSelector.ShowDialog().GetValueOrDefault())
            {
                openmenu.IsEnabled = false;
                Stop();
                var stream = new Mp3WebStream(streamSelector.Value, true);
                stream.ConnectionCreated += (s, args) =>
                {
                    if (args.Success)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            ShowBufferedIndicator(true);
                            OpenSource(stream);
                        }));
                    }
                    else
                        MessageBox.Show("Connecting to server failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    Dispatcher.Invoke(new Action(() => { openmenu.IsEnabled = true; }));
                };
            }
        }

        private void OpenSource(IWaveSource source)
        {
            try
            {
                Stop(); //if playing -> stop playback
                _audioSource = source;

                _gainSource = new GainSource(source) { Gain = (float)this.Gain };

                source = SetupVisualization(_gainSource.ToWaveSource(16));

                if (WasapiOut.IsSupportedOnCurrentPlatform) // > Vista
                {
                    _soundOut = new WasapiOut(false, CSCore.CoreAudioAPI.AudioClientShareMode.Shared, 100);
                }
                else // < Vista
                {
                    _soundOut = new DirectSoundOut() { Latency = 100 };
                }

                _soundOut.Initialize(source);
                _soundOut.Stopped += OnPlaybackStopped;
                _soundOut.Play();
            }
            catch (CSCore.CoreAudioAPI.CoreAudioAPIException ex)
            {
                MessageBox.Show("Unknown CoreAudioAPI-error: 0x" + ex.ErrorCode.ToString("x"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Stop();
            }
        }

        private void OnPlaybackStopped(object sender, EventArgs e)
        {
            Context.Current.Logger.Debug("Playback stopped");
        }

        private void Stop()
        {
            if (_soundOut != null)
            {
                var soundOut = _soundOut;
                _soundOut = null;
                soundOut.Stop();
                soundOut.Dispose();
                if (soundOut.WaveSource != null)
                    soundOut.WaveSource.Dispose();
            }
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Stop();
        }

        private void ShowBufferedIndicator(bool show)
        {
            //display the amount of buffered data
            if (show)
            {
                if (_timer == null || !_timer.IsEnabled)
                {
                    _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
                    _timer.Tick += OnTimerTick;
                }

                _timer.Start();
                bufferedDataIndicator.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                if (_timer != null)
                    _timer.Stop();
                bufferedDataIndicator.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            //update the amount of buffered data
            var stream = _audioSource as Mp3WebStream;
            if (stream != null)
            {
                bufferedDataIndicator.Maximum = stream.BufferSize;
                bufferedDataIndicator.Value = stream.BufferedBytes;
            }
        }

        private IWaveSource SetupVisualization(IWaveSource source)
        {
            //setup for FFTVisualization
            source = new CSCore.Visualization.FFTDataProvider(source) { Bands = 512 }; //using 512 bands by default
            //apply data provider
            spectrum.DataProvider = source as CSCore.Visualization.FFTDataProvider;
            peak.DataProvider = source as CSCore.Visualization.FFTDataProvider;

            //setup for SampleVisalization
            var sdp = new CSCore.Visualization.SampleDataProvider(source);
            //apply data provider
            waveform.DataProvider = sdp;

            //convert back to raw data
            source = sdp.ToWaveSource(16);
            return source;
        }
    }
}