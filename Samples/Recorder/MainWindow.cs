using CSCore;
using CSCore.Codecs.WAV;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recorder
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private WaveInCaps _selectedDevice;

        private WaveIn _waveIn;
        private WaveWriter _writer;
        private ISoundOut _soundOut;

        private IWaveSource _source;
        private byte[] _writerBuffer;

        private int _peakUpdateCounter = 0;
        private int _left, _right;

        private void deviceslist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deviceslist.SelectedItems.Count > 0)
            {
                _selectedDevice = (WaveInCaps)deviceslist.SelectedItems[0].Tag;
                btnStart.Enabled = true;
            }
            else
            {
                btnStart.Enabled = false;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (deviceslist.SelectedItems.Count <= 0)
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "WAV (*.wav)|*.wav";
            sfd.Title = "Speichern";
            sfd.FileName = String.Empty;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _waveIn = new WaveInEvent(new WaveFormat(44100, 16, _selectedDevice.Channels));
                _waveIn.Device = deviceslist.SelectedItems[0].Index;

                _waveIn.Initialize();
                _waveIn.Start();

                var waveInToSource = new SoundInSource(_waveIn);

                _source = waveInToSource;
                var notifyStream = new SingleBlockNotificationStream(_source);
                notifyStream.SingleBlockRead += OnNotifyStream_SingleBlockRead;

                _source = notifyStream.ToWaveSource(16);
                _writerBuffer = new byte[_source.WaveFormat.BytesPerSecond];

                _writer = new WaveWriter(File.OpenWrite(sfd.FileName), _source.WaveFormat);
                waveInToSource.DataAvailable += OnNewData;

                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
        }

        private void OnNotifyStream_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            _left = Math.Max((int)(Math.Abs(e.Left) * 10000), _left);
            _right = Math.Max((int)(Math.Abs(e.Right) * 10000), _right);

            if (++_peakUpdateCounter >= _waveIn.WaveFormat.SampleRate / 20)
            {
                Invoke(new MethodInvoker(() =>
                {
                    peakLeft.Value = _left;
                    peakRight.Value = _right;
                    _peakUpdateCounter = _left = _right = 0;
                }));
            }
        }

        private void OnNewData(object sender, DataAvailableEventArgs e)
        {
            int read = 0;
            while ((read = _source.Read(_writerBuffer, 0, _writerBuffer.Length)) > 0)
            {
                _writer.Write(_writerBuffer, 0, read);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_soundOut != null)
                _soundOut.Dispose();

            _waveIn.Dispose();
            _writer.Dispose();

            _waveIn = null;
            _writer = null;
            _soundOut = null;

            btnStart.Enabled = deviceslist.SelectedItems.Count > 0;
            btnStop.Enabled = false;
        }

        private void btnRefreshDevices_Click(object sender, EventArgs e)
        {
            deviceslist.Items.Clear();
            foreach (var device in WaveIn.Devices)
            {
                var item = new ListViewItem(device.Name);
                item.Tag = device;
                item.SubItems.Add(device.Channels.ToString());
                item.SubItems.Add(device.DriverVersion.ToString());
                deviceslist.Items.Add(item);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_waveIn != null || _writer != null)
            {
                e.Cancel = true;
                MessageBox.Show("Aufnahme zuerst beenden.");
            }
        }
    }
}