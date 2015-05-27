using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.DSP;
using CSCore.SoundOut;
using CSCore.Streams;
using WinformsVisualization.Visualization;

namespace WinformsVisualization
{
    public partial class Form1 : Form
    {
        private ISoundOut _soundOut;
        private IWaveSource _source;
        private LineSpectrum _lineSpectrum;
        private VoicePrint3DSpectrum _voicePrint3DSpectrum;

        private readonly Bitmap _bitmap = new Bitmap(2000, 600);
        private int _xpos;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = CodecFactory.SupportedFilesFilterEn,
                Title = "Select a file..."
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stop();

                const FftSize fftSize = FftSize.Fft4096;

                IWaveSource source = CodecFactory.Instance.GetCodec(openFileDialog.FileName);

                var spectrumProvider = new BasicSpectrumProvider(source.WaveFormat.Channels,
                    source.WaveFormat.SampleRate, fftSize);
                _lineSpectrum = new LineSpectrum(fftSize)
                {
                    SpectrumProvider = spectrumProvider,
                    UseAverage = true,
                    BarCount = 50,
                    BarSpacing = 2,
                    IsXLogScale = true,
                    ScalingStrategy = ScalingStrategy.Sqrt
                };
                _voicePrint3DSpectrum = new VoicePrint3DSpectrum(fftSize)
                {
                    SpectrumProvider = spectrumProvider,
                    UseAverage = true,
                    PointCount = 200,
                    IsXLogScale = true,
                    ScalingStrategy = ScalingStrategy.Sqrt
                };

                var notificationSource = new SingleBlockNotificationStream(source.ToSampleSource());
                notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

                _source = notificationSource.ToWaveSource(16);

                _soundOut = new WasapiOut();
                _soundOut.Initialize(_source.ToMono());
                _soundOut.Play();

                timer1.Start();

                propertyGridTop.SelectedObject = _lineSpectrum;
                propertyGridBottom.SelectedObject = _voicePrint3DSpectrum;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Stop();
        }

        private void Stop()
        {
            timer1.Stop();

            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GenerateLineSpectrum();
            GenerateVoice3DPrintSpectrum();   
        }

        private void GenerateLineSpectrum()
        {
            Image image = pictureBoxTop.Image;
            var newImage = _lineSpectrum.CreateSpectrumLine(pictureBoxTop.Size, Color.Green, Color.Red, Color.Black, true);
            if (newImage != null)
            {
                pictureBoxTop.Image = newImage;
                if (image != null)
                    image.Dispose();
            }
        }

        private void GenerateVoice3DPrintSpectrum()
        {
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                pictureBoxBottom.Image = null;
                if (_voicePrint3DSpectrum.CreateVoicePrint3D(g, new RectangleF(0, 0, _bitmap.Width, _bitmap.Height),
                    _xpos, Color.Black, 3))
                {
                    _xpos += 3;
                    if (_xpos >= _bitmap.Width)
                        _xpos = 0;
                }
                pictureBoxBottom.Image = _bitmap;
            }
        }
    }
}
