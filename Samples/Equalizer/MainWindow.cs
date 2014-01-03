using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore.Streams;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace EqualizerTest
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ISoundOut _soundOut;
        private Equalizer _eq;

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_eq != null)
            {
                var trackbar = sender as TrackBar;
                float value = (float)(((double)trackbar.Value / (double)trackbar.Maximum) * 15);
                _eq.SampleFilters[Int32.Parse((string)trackbar.Tag)].SetGain(value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofn = new OpenFileDialog();
            ofn.Filter = CodecFactory.SupportedFilesFilterEN;
            if (ofn.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Stop();

                if (WasapiOut.IsSupportedOnCurrentPlatform)
                {
                    _soundOut = new WasapiOut();
                }
                else
                {
                    _soundOut = new DirectSoundOut();
                }
                var source = CodecFactory.Instance.GetCodec(ofn.FileName);
                source = new LoopStream(source) { EnableLoop = false };
                (source as LoopStream).StreamFinished += (s, args) => Stop();

                _eq = Equalizer.Create10BandEqualizer(source);
                _soundOut.Initialize(_eq.ToWaveSource(16));
                _soundOut.Play();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Stop();
            base.OnClosing(e);
        }

        private void Stop()
        {
            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _eq.Dispose();
                _soundOut = null;
            }
        }
    }
}