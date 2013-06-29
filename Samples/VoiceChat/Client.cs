using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundIn;

namespace VoiceChat
{
    public class Client
    {
        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        WaveFormat _waveFormat;
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
            protected set { _waveFormat = value; }
        }
    }
}
