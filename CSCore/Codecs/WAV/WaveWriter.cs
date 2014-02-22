using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CSCore.Codecs.WAV
{
    public class WaveWriter : IDisposable, IWritable
    {
        public static void WriteToFile(string filename, IWaveSource source, bool deleteIfExists, int maxlength = -1)
        {
            if (deleteIfExists && File.Exists(filename))
                File.Delete(filename);

            int read = 0;
            int r = 0;
            byte[] buffer = new byte[source.WaveFormat.BytesPerSecond];
            using (var w = new WaveWriter(filename, source.WaveFormat))
            {
                while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    w.Write(buffer, 0, read);
                    r += read;
                    if (maxlength != -1 && r > maxlength)
                        break;
                }
            }
        }
            

        private Stream _stream;
        private BinaryWriter _writer;

        private WaveFormat _waveFormat;

        private long _waveStartPosition;
        private int _dataLength;

        public WaveWriter(string fileName, WaveFormat waveFormat)
            : this(File.OpenWrite(fileName), waveFormat)
        {
        }

        public WaveWriter(Stream stream, WaveFormat waveFormat)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (!stream.CanWrite) throw new ArgumentException("stream not writeable");
            if (!stream.CanSeek) throw new ArgumentException("stream not seekable");

            _stream = stream;
            _waveStartPosition = stream.Position;
            _writer = new BinaryWriter(stream);
            for (int i = 0; i < 44; i++)
            {
                _writer.Write((byte)0);
            }
            _waveFormat = waveFormat;

            WriteHeader();
        }

        public void WriteSample(float sample)
        {
            if (_waveFormat.IsPCM())
            {
                switch (_waveFormat.BitsPerSample)
                {
                    case 8:
                        Write((byte)(byte.MaxValue * sample)); break;
                    case 16:
                        Write((short)sample); break;
                    case 24:
                        byte[] buffer = BitConverter.GetBytes((int)(int.MaxValue * sample));
                        Write(new byte[] { buffer[0], buffer[1], buffer[2] }, 0, 3);
                        break;

                    default:
                        throw new InvalidOperationException("Invalid Waveformat", new InvalidOperationException("Invalid BitsPerSample while using PCM encoding."));
                }
            }
            else if (_waveFormat.WaveFormatTag == AudioEncoding.Extensible && _waveFormat.BitsPerSample == 32)
            {
                Write(UInt16.MaxValue * (int)sample);
            }
            else if (_waveFormat.WaveFormatTag == AudioEncoding.IeeeFloat)
            {
                Write(sample);
            }
            else
            {
                throw new InvalidOperationException("Invalid Waveformat: Waveformat has to be PCM[8, 16, 24, 32];IeeeFloat[32]");
            }
        }

        public void WriteSamples(float[] samples, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
                WriteSample(samples[i]);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
            _dataLength += count;
        }

        public void Write(byte value)
        {
            _writer.Write(value);
            _dataLength++;
        }

        public void Write(short value)
        {
            _writer.Write(value);
            _dataLength += 2;
        }

        public void Write(int value)
        {
            _writer.Write(value);
            _dataLength += 4;
        }

        public void Write(float value)
        {
            _writer.Write(value);
            _dataLength += 4;
        }

        private void WriteHeader()
        {
            _writer.Flush();

            long currentPosition = _stream.Position;
            _stream.Position = _waveStartPosition;

            WriteRiffHeader();
            WriteFMTChunk();
            WriteDataChunk();

            _stream.Position = currentPosition;
        }

        private void WriteRiffHeader()
        {
            _writer.Write(Encoding.UTF8.GetBytes("RIFF"));
            _writer.Write((int)(_stream.Length - 8));
            _writer.Write(Encoding.UTF8.GetBytes("WAVE"));
        }

        private void WriteFMTChunk()
        {
            var tag = _waveFormat.WaveFormatTag;
            if (tag == AudioEncoding.Extensible)
            {
                tag = DMO.MediaTypes.EncodingFromMediaType((_waveFormat as WaveFormatExtensible).SubFormat);
            }

            _writer.Write(Encoding.UTF8.GetBytes("fmt "));
            _writer.Write(16);
            _writer.Write((short)tag);
            _writer.Write(_waveFormat.Channels);
            _writer.Write(_waveFormat.SampleRate);
            _writer.Write(_waveFormat.BytesPerSecond);
            _writer.Write((short)_waveFormat.BlockAlign);
            _writer.Write(_waveFormat.BitsPerSample);
        }

        private void WriteDataChunk()
        {
            _writer.Write(Encoding.UTF8.GetBytes("data"));
            _writer.Write(_dataLength);
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                try
                {
                    WriteHeader();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("WaveWriter::Dispose: " + ex.ToString());
                }
                finally
                {
                    _writer.Close();
                    _writer = null;
                    _stream = null;
                }
            }
            _disposed = true;
        }

        ~WaveWriter()
        {
            Dispose(false);
        }
    }
}