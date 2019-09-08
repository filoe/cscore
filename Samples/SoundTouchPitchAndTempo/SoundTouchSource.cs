using CSCore;
using System;

namespace SoundTouchPitchAndTempo
{
    public class SoundTouchSource : SampleAggregatorBase
    {
        private bool _isDisposed;

        private readonly int _latency;
        private readonly float[] _sourceReadBuffer;
        private readonly float[] _soundTouchReadBuffer;
        private readonly object lockObject;

        private bool _seekRequested;

        private ISampleSource _sampleSource;
        private ISoundTouch _soundTouch;

        public SoundTouchSource(ISampleSource sampleSource, int latency)
            : this(sampleSource, latency, new SoundTouch())
        {
        }

        public SoundTouchSource(ISampleSource sampleSource, int latency, ISoundTouch soundTouch)
            : base(sampleSource)
        {
            _sampleSource = sampleSource;
            _latency = latency;
            _soundTouch = soundTouch;

            _soundTouch.SetChannels(_sampleSource.WaveFormat.Channels);
            _soundTouch.SetSampleRate(_sampleSource.WaveFormat.SampleRate);

            _sourceReadBuffer = new float[(_sampleSource.WaveFormat.SampleRate * _sampleSource.WaveFormat.Channels * (long)_latency) / 1000];
            _soundTouchReadBuffer = new float[_sourceReadBuffer.Length * 10];

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

        public void Seek()
        {
            _seekRequested = true;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            lock(lockObject)
            {
                if(_seekRequested)
                {
                    _soundTouch.Clear();
                    _seekRequested = false;
                }

                var samplesRead = 0;
                var endOfSource = false;

                while(samplesRead < count)
                {
                    if(_soundTouch.NumberOfSamples() == 0)
                    {
                        var readFromSource = _sampleSource.Read(_sourceReadBuffer, 0, _sourceReadBuffer.Length);
                        if(readFromSource == 0)
                        {
                            endOfSource = true;
                            _soundTouch.Flush();
                        }

                        _soundTouch.PutSamples(_sourceReadBuffer, readFromSource / _sampleSource.WaveFormat.Channels);
                    }

                    var desiredSampleFrames = (count - samplesRead) / _sampleSource.WaveFormat.Channels;
                    var received = _soundTouch.ReceiveSamples(_soundTouchReadBuffer, desiredSampleFrames) * _sampleSource.WaveFormat.Channels;

                    for(int n = 0; n < received; n++)
                    {
                        buffer[offset + samplesRead++] = _soundTouchReadBuffer[n];
                    }

                    if(received == 0 && endOfSource)
                    {
                        break;
                    }
                }

                return samplesRead;
            }
        }

        public new void Dispose()
        {
            base.Dispose();

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual new void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if(_isDisposed)
            {
                return;
            }

            if(isDisposing)
            {
                if(_sampleSource != null)
                {
                    _sampleSource.Dispose();
                    _sampleSource = null;
                }

                if(_soundTouch != null)
                {
                    _soundTouch.Dispose();
                    _soundTouch = null;
                }
            }

            _isDisposed = true;
        }
    }
}
