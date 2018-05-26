using System;

namespace CSCore.Streams
{
    public class SoundTouchSource : WaveAggregatorBase
    {
        private byte[] _bytebuffer = new byte[4096];
        private float[] _floatbuffer = new float[1024];
        private bool _endReached = false;
        private readonly object lockObject;

        private IWaveSource _waveSource;
        private ISoundTouch _soundTouch;

        public SoundTouchSource(IWaveSource waveSource)
            : this(waveSource, new SoundTouch())
        {
        }

        public SoundTouchSource(IWaveSource waveSource, ISoundTouch soundTouch)
            : base(waveSource)
        {
            _waveSource = waveSource;

            _soundTouch = soundTouch;
            _soundTouch.SetChannels((uint)_waveSource.WaveFormat.Channels);
            _soundTouch.SetSampleRate((uint)_waveSource.WaveFormat.SampleRate);

            lockObject = new object();
        }

        public void SetPitch(float pitch)
        {
            if(pitch > 6.0f || pitch < -6.0f)
            {
                pitch = 0.0f;
            }

            _soundTouch.SetPitchSemiTones(pitch);
        }

        public void SetTempo(float tempo)
        {
            if(tempo > 52.0f || tempo < -52.0f)
            {
                tempo = 0.0f;
            }

            _soundTouch.SetTempoChange(tempo);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock(lockObject)
            {
                try
                {
                    while(_soundTouch.NumberOfSamples() < count)
                    {
                        var bytesRead = _waveSource.Read(_bytebuffer, offset, _bytebuffer.Length);
                        if(bytesRead == 0)
                        {
                            if(_endReached == false)
                            {
                                _endReached = true;
                                _soundTouch.Flush();
                            }

                            break;
                        }

                        Buffer.BlockCopy(_bytebuffer, 0, _floatbuffer, 0, bytesRead);
                        _soundTouch.PutSamples(_floatbuffer, (uint)(bytesRead / 8));
                    }

                    if(_floatbuffer.Length < count / 4)
                    {
                        _floatbuffer = new float[count / 4];
                    }

                    var numberOfSamples = (int)_soundTouch.ReceiveSamples(_floatbuffer, (uint)(count / 8));
                    Buffer.BlockCopy(_floatbuffer, 0, buffer, offset, numberOfSamples * 8);

                    return numberOfSamples * 8;
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}
