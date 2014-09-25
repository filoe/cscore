using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSCore;
using CSCore.SoundOut;
using NVorbis;

namespace NVorbisIntegration
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Vorbis file (*.ogg)|*.ogg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = openFileDialog.OpenFile())
                {
                    var source = new NVorbisSource(stream);
                    using (WasapiOut soundOut = new WasapiOut())
                    {
                        soundOut.Initialize(source.ToWaveSource());
                        soundOut.Play();

                        Console.ReadKey();
                        soundOut.Stop();
                    }
                }
            }
        }
    }

    public sealed class NVorbisSource : ISampleSource
    {
        private readonly Stream _stream;
        private readonly VorbisReader _vorbisReader;

        private readonly WaveFormat _waveFormat;
        private bool _disposed;

        public NVorbisSource(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if(!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");
            _stream = stream;
            _vorbisReader = new VorbisReader(stream, false);
            _waveFormat = new WaveFormat(_vorbisReader.SampleRate, 32, _vorbisReader.Channels, AudioEncoding.IeeeFloat);
        }

        public bool CanSeek { get { return _stream.CanSeek; } }

        public WaveFormat WaveFormat { get { return _waveFormat; } }

        public long Position
        {
            get { return CanSeek ? _vorbisReader.DecodedPosition : 0; }
            set
            {
                if(CanSeek)
                    _vorbisReader.DecodedPosition = value;
                else 
                    throw new InvalidOperationException("NVorbisSource is not seekable.");
            }
        }

        public long Length { get { return CanSeek ? _vorbisReader.TotalSamples : 0; } }

        public int Read(float[] buffer, int offset, int count)
        {
            return _vorbisReader.ReadSamples(buffer, offset, count);
        }

        public void Dispose()
        {
            if(!_disposed)
                _vorbisReader.Dispose();
            else
                throw new ObjectDisposedException("NVorbisSource");
            _disposed = true;
        }
    }
}
