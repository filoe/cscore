using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSCore.SoundOut;
using CSCore.Codecs;
using CSCore.Streams;
using CSCore;

namespace EqualizerTest
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ISoundOut _soundOut;
        Equalizer _eq;

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
                if (_soundOut != null)
                {
                    _soundOut.Stop();
                    _soundOut.Dispose();
                    _eq.Dispose();
                    _soundOut = null;
                }

                _soundOut = new WasapiOut();//DirectSoundOut();
                var source = CodecFactory.Instance.GetCodec(ofn.FileName);
                _eq = Equalizer.Create11BandEqualizer(source);
                _soundOut.Initialize(_eq.ToWaveSource(16));
                _soundOut.Play();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_soundOut != null)
            {
                _soundOut.Dispose();
                _eq.Dispose();
            }
            base.OnClosing(e);
        }
    }
}
