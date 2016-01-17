using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;

namespace AudioPlayerSample
{
    public partial class Form1 : Form
    {
        private readonly MusicPlayer _musicPlayer = new MusicPlayer();
        private bool _stopSliderUpdate;
        private readonly ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>(); 
        
        public Form1()
        {
            InitializeComponent();
            components = new Container();
            components.Add(_musicPlayer);
            _musicPlayer.PlaybackStopped += (s, args) =>
            {
                //WasapiOut uses SynchronizationContext.Post to raise the event
                //There might be already a new WasapiOut-instance in the background when the async Post method brings the PlaybackStopped-Event to us.
                if(_musicPlayer.PlaybackState != PlaybackState.Stopped)
                    btnPlay.Enabled = btnStop.Enabled = btnPause.Enabled = false;
            };
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = CodecFactory.SupportedFilesFilterEn
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _musicPlayer.Open(openFileDialog.FileName, (MMDevice)comboBox1.SelectedItem);
                    trackbarVolume.Value = _musicPlayer.Volume;

                    btnPlay.Enabled = true;
                    btnPause.Enabled = btnStop.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open file: " + ex.Message);
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if(_musicPlayer.PlaybackState != PlaybackState.Playing)
            {
                _musicPlayer.Play();
                btnPlay.Enabled = false;
                btnPause.Enabled = btnStop.Enabled = true;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if(_musicPlayer.PlaybackState == PlaybackState.Playing)
            {
                _musicPlayer.Pause();
                btnPause.Enabled = false;
                btnPlay.Enabled = btnStop.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if(_musicPlayer.PlaybackState != PlaybackState.Stopped)
            {
                _musicPlayer.Stop();
                btnPlay.Enabled = btnStop.Enabled = btnPause.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan position = _musicPlayer.Position;
            TimeSpan length = _musicPlayer.Length;
            if (position > length)
                length = position;

            lblPosition.Text = String.Format(@"{0:mm\:ss} / {1:mm\:ss}", position, length);

            if (!_stopSliderUpdate &&
                length != TimeSpan.Zero && position != TimeSpan.Zero)
            {
                double perc = position.TotalMilliseconds / length.TotalMilliseconds * trackBar1.Maximum;
                trackBar1.Value = (int)perc;
            }
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _stopSliderUpdate = true;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _stopSliderUpdate = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_stopSliderUpdate)
            {
                double perc = trackBar1.Value / (double)trackBar1.Maximum;
                TimeSpan position = TimeSpan.FromMilliseconds(_musicPlayer.Length.TotalMilliseconds * perc);
                _musicPlayer.Position = position;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (
                    var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in mmdeviceCollection)
                    {
                        _devices.Add(device);
                    }
                }
            }

            comboBox1.DataSource = _devices;
            comboBox1.DisplayMember = "FriendlyName";
            comboBox1.ValueMember = "DeviceID";
        }

        private void trackbarVolume_ValueChanged(object sender, EventArgs e)
        {
            _musicPlayer.Volume = trackbarVolume.Value;
        }
    }
}
