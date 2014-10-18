using System;
using System.Windows.Forms;
using CSCore.Codecs;
using CSCore.XAudio2;

namespace XAudio2Playback
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = CodecFactory.SupportedFilesFilterEn;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var source = CodecFactory.Instance.GetCodec(openFileDialog.FileName))
                using (var xaudio2 = XAudio2.CreateXAudio2())
                using (var masteringVoice = xaudio2.CreateMasteringVoice()) //ALWAYS create at least one masteringVoice.
                using (var streamingSourceVoice = StreamingSourceVoice.Create(xaudio2, source))
                {
                    StreamingSourceVoiceListener.Default.Add(streamingSourceVoice); //add the streamingSourceVoice to the default sourcevoicelistener which processes the data requests.
                    streamingSourceVoice.Start();

                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();

                    StreamingSourceVoiceListener.Default.Remove(streamingSourceVoice);
                    streamingSourceVoice.Stop();
                }
            }
        }
    }
}
