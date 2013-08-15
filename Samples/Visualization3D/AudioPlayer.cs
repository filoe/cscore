using CSCore;
using CSCore.Codecs;
using CSCore.DSP;
using CSCore.SoundOut;
using CSCore.Streams.Effects;
using CSCore.Streams.SampleConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visualization3D.Core.Graphics;

namespace Visualization3D
{
    public class AudioPlayer<T> : IDisposable where T : IVisualisationItem
    {
        private ISoundOut _soundOut;

        private VisualisationItemManager<T> _visualizer;
        private int _bands;

        public AudioPlayer(VisualisationItemManager<T> visualizer, int bands)
        {
            _visualizer = visualizer;
            _bands = bands;
        }

        public void StartStream(IWaveSource source)
        {
            Stop();

            FFTAggregator fftAggregator = new FFTAggregator(source, _bands);
            fftAggregator.FFTCalculated += OnFFTCalculated;
            source = fftAggregator;

            if (WasapiOut.IsSupportedOnCurrentPlatform)
            {
                _soundOut = new WasapiOut();
            }
            else
            {
                source = new SampleToPcm16(WaveToSampleBase.CreateConverter(source));
                _soundOut = new DirectSoundOut() { Latency = 100 };
            }
            _soundOut.Initialize(source);
            _soundOut.Play();
        }

        protected virtual void OnFFTCalculated(object sender, FFTCalculatedEventArgs e)
        {
            int pts = e.Data.Length / 2;

            for (int i = 0; i < pts; i++)
            {
                _visualizer.SetValue(i, (float)e.Data[i].CalculateFFTPercentage());
            }
        }

        public void Stop()
        {
            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _soundOut.WaveSource.Dispose();

                _soundOut = null;
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}