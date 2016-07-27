using System;
using System.Windows.Forms;
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore.SoundOut.AL;

namespace OpenALMultiContext
{
    public partial class Form1 : Form
    {
        private ALSoundOut _alSoundOut1, _alSoundOut2;

        public Form1()
        {
            InitializeComponent();

            MessageBox.Show("Note: This sample won't work on Windows.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Play(ref _alSoundOut1, comboBox1.SelectedItem as ALDevice);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Play(ref _alSoundOut2, comboBox2.SelectedItem as ALDevice);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = ALDevice.EnumerateALDevices();
            comboBox2.DataSource = ALDevice.EnumerateALDevices();
        }

        private void Play(ref ALSoundOut soundOut, ALDevice device)
        {
            if (device == null)
                return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = CodecFactory.SupportedFilesFilterEn;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                if (soundOut != null)
                {
                    soundOut.Stop();
                    soundOut.Dispose();
                }

                soundOut = new ALSoundOut(300);
                soundOut.Device = device;
                soundOut.Initialize(CodecFactory.Instance.GetCodec(openFileDialog.FileName));
                soundOut.Play();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_alSoundOut1 != null)
                _alSoundOut1.Dispose();

            if(_alSoundOut2 != null)
                _alSoundOut2.Dispose();
        }
    }
}
