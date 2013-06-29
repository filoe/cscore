using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.SoundOut;
using CSCore.DSP;
using CSCore;
using CSCore.Streams;

namespace VoiceChat
{
    public class AudioOut
    {
        BufferingStream _buffer;
        ISoundOut _soundOut;
        Client _client;
        WaveFormat _waveFormat;

        public AudioOut(Client client)
        {
            _waveFormat = client.WaveFormat;
            _buffer = new BufferingStream(_waveFormat);
        }

        public void Initialize()
        {
            _client.DataAvailable += _client_DataAvailable;

            _soundOut = new DirectSoundOut();
            _soundOut.Initialize(_buffer);
            _soundOut.Play();
        }

        void _client_DataAvailable(object sender, CSCore.SoundIn.DataAvailableEventArgs e)
        {
            _buffer.Write(e.Data, 0, e.ByteCount);
        }
    }
}
