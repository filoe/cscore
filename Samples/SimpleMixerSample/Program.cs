using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.DSP;
using CSCore.SoundOut;
using CSCore.Streams;

namespace SimpleMixerSample
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine(
                "This example will mix one audio file with \n"+
                "two sine waves (300Hz and 700Hz).\n"+
                "The 300Hz sine wave will play only on the right\n" +
                "channel and the 700Hz sine wave only on the left channel."+
                "\n\n\nPlease select the audio file!\n"
                );

            IWaveSource fileWaveSource = null;

            do
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    Title = "Select any file to mix into",
                    Filter = CodecFactory.SupportedFilesFilterEn
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        fileWaveSource = CodecFactory.Instance.GetCodec(openFileDialog.FileName);
                    }
                    catch
                    {
                    }
                }

            } while (fileWaveSource == null);


            const int mixerSampleRate = 44100; //44.1kHz

            var mixer = new SimpleMixer(2, mixerSampleRate) //output: stereo, 44,1kHz
            {
                FillWithZeros = true,
                DivideResult = true //you may play around with this
            };

            var monoToLeftOnlyChannelMatrix = new ChannelMatrix(ChannelMask.SpeakerFrontCenter,
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight);
            var monoToRightOnlyChannelMatrix = new ChannelMatrix(ChannelMask.SpeakerFrontCenter,
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight);

            /*
            * Set the channel conversion matrix. 
            * The y-axis specifies the input. This in only one channel since the SineGenerator only uses one channel. 
            * The x-axis specifies the output. There we have to use two channels since we want stereo output. 
            * The first value on the x-axis specifies the volume of the left channel, the second value 
            * on the x-axis specifies the volume of the right channel.
            * 
            * If we take look at the left only channel conversion matrix, we can see that we are mapping one channel (y-axis)
            * to two channels (x-axis). The left channel receives a volume of 1.0 (which means 100%) and the right channel 
            * receives a volume of 0.0 (which means 0.0% -> muted). 
            */
            monoToLeftOnlyChannelMatrix.SetMatrix(
                new[,]
                {
                    {1.0f, 0.0f}
                });

            monoToRightOnlyChannelMatrix.SetMatrix(
                new[,]
                {
                    {0.0f, 1.0f}
                });

            VolumeSource volumeSource1, volumeSource2;

            //Add any sound track.
            mixer.AddSource(
                fileWaveSource
                .ChangeSampleRate(mixerSampleRate)
                .ToStereo()
                .ToSampleSource());

            //Add a 700Hz sine with a amplitude of 0.5 which plays only on the left channel.
            mixer.AddSource(
                new SineGenerator(700, 0.5, 0).ToWaveSource()
                .AppendSource(x => new DmoChannelResampler(x, monoToLeftOnlyChannelMatrix, mixerSampleRate))
                .AppendSource(x => new VolumeSource(x.ToSampleSource()), out volumeSource1));

            //Add a 300Hz sine with a amplitude of 0.5 which plays only on the right channel.
            mixer.AddSource(
                new SineGenerator(300, 0.5, 0).ToWaveSource()
                .AppendSource(x => new DmoChannelResampler(x, monoToRightOnlyChannelMatrix, mixerSampleRate))
                .AppendSource(x => new VolumeSource(x.ToSampleSource()), out volumeSource2));

            //Initialize the soundout with the mixer. 
            var soundOut = new WasapiOut() { Latency = 200 }; //better use a quite high latency
            soundOut.Initialize(mixer.ToWaveSource());
            soundOut.Play();

            //adjust the volume of the input signals (default value is 100%):
            volumeSource1.Volume = 0.5f; //set the volume of the 700Hz sine to 50%
            volumeSource2.Volume = 0.7f; //set the volume of the 300Hz sine to 70%

            Console.ReadKey();

            mixer.Dispose();
            soundOut.Dispose();
        }
    }
}
