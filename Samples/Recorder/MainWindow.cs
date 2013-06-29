using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using CSCore.SoundOut;
using CSCore;
using CSCore.DSP;
using System.IO;
using System.Runtime.InteropServices;

namespace Recorder
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        WaveInCaps _selectedDevice;

        WaveIn _waveIn;
        WaveWriter _writer;
        ISoundOut _soundOut;

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
            if (deviceslist.SelectedItems.Count <= 0) //nur zur Sicherheit
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "WAV (*.wav)|*.wav";
            sfd.Title = "Speichern";
            sfd.FileName = String.Empty;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _waveIn = new WaveInEvent(new CSCore.WaveFormat(44100, 16, _selectedDevice.Channels));
                _waveIn.Device = deviceslist.SelectedItems[0].Index;
                _writer = new WaveWriter(sfd.FileName, _waveIn.WaveFormat);

                _waveIn.DataAvailable += OnNewData;

                _waveIn.Initialize();
                _waveIn.Start();

                btnStart.Enabled = false;
                chbOutput.Enabled = true;
                btnStop.Enabled = true;
            }
        }

        private void OnNewData(object sender, DataAvailableEventArgs e)
        {
            _writer.Write(e.Data, 0, e.ByteCount);
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
            chbOutput.Enabled = false;
            chbOutput.Checked = false;
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

        private void chbOutput_CheckedChanged(object sender, EventArgs e)
        {
            if (_waveIn != null && chbOutput.Checked)
            {
                if (_soundOut == null)
                {
                    if (WasapiOut.IsSupportedOnCurrentPlatform)
                    {
                        _soundOut = new WasapiOut();
                    }
                    else
                    {
                        _soundOut = new WaveOutWindow();
                    }
                    IWaveSource source = new CSCore.Streams.WaveInSource(_waveIn);
                    var peak = new PeakMeter(source);
                    peak.PeakCalculated += MainWindow_BlockRead;
                    source = peak.ToWaveSource(16);
                    _soundOut.Initialize(source);
                    _soundOut.Play();
                }
                else
                {
                    _soundOut.Volume = 1f;
                }
            }
            else if (chbOutput.Checked == false && _soundOut != null && _soundOut.PlaybackState == PlaybackState.Playing)
            {
                _soundOut.Volume = 0f;
            }
        }

        void MainWindow_BlockRead(object sender, PeakCalculatedEventArgs e)
        {
            peakLeft.Value = (int)(e.MaxLeftPeak * peakLeft.Maximum);
            peakRight.Value = (int)(e.MaxRightPeak * peakRight.Maximum);
        }
    }
}
