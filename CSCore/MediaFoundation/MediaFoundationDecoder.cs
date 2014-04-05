using CSCore.DMO;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    public class MediaFoundationDecoder : IWaveSource
    {
        private IMFByteStream _byteStream;
        private MFSourceReader _reader;
        private WaveFormat _waveFormat;
        private Stream _stream;
        private Object _lockObj = new Object();

        private long _length;
        private long _position = 0; //could not find a possibility to find out the position
        private bool _hasFixedLength = false;

        private byte[] _decoderBuffer;
        private int _decoderBufferOffset;
        private int _decoderBufferCount;

        public MediaFoundationDecoder(string url)
        {
            if (String.IsNullOrEmpty(url))
                throw new ArgumentNullException("filename");

            _hasFixedLength = true;

            MediaFoundationCore.Startup();
            _reader = Initialize(MediaFoundationCore.CreateSourceReaderFromUrl(url));
        }

        public MediaFoundationDecoder(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");

            stream = new ComStream(stream);
            _stream = stream;
            _byteStream = MediaFoundationCore.IStreamToByteStream((IStream)stream);
            _reader = Initialize(_byteStream);
        }

        public MediaFoundationDecoder(IMFByteStream byteStream)
        {
            if (byteStream == null)
                throw new ArgumentNullException("byteStream");
            _byteStream = byteStream;
            _reader = Initialize(_byteStream);
        }

        private MFSourceReader Initialize(IMFByteStream stream)
        {
            MediaFoundationCore.Startup();
            return Initialize(MediaFoundationCore.CreateSourceReaderFromByteStream(stream, IntPtr.Zero));
        }

        private MFSourceReader Initialize(MFSourceReader reader)
        {
            MediaFoundationCore.Startup();

            try
            {
                reader.SetStreamSelection(MFInterops.MF_SOURCE_READER_ALL_STREAMS, false);
                reader.SetStreamSelection(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM, true);

                using (var mediaType = MFMediaType.CreateEmpty())
                {
                    mediaType.MajorType = MediaTypes.MediaTypeAudio;
                    mediaType.SubType = MediaTypes.MEDIATYPE_Pcm; //variable??

                    reader.SetCurrentMediaType(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM, mediaType);
                }

                using (var currentMediaType = reader.GetCurrentMediaType(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM))
                {
                    if (currentMediaType.MajorType != MediaTypes.MediaTypeAudio)
                        throw new InvalidOperationException(String.Format("Invalid Majortype set on sourcereader: {0}.", currentMediaType.MajorType.ToString()));

                    AudioEncoding encoding;
                    if (currentMediaType.SubType == MediaTypes.MEDIATYPE_Pcm)
                        encoding = AudioEncoding.Pcm;
                    else if (currentMediaType.SubType == MediaTypes.MEDIATYPE_IeeeFloat)
                        encoding = AudioEncoding.IeeeFloat;
                    else
                        throw new InvalidOperationException(String.Format("Invalid Subtype set on sourcereader: {0}.", currentMediaType.SubType.ToString()));

                    _waveFormat = new WaveFormat(currentMediaType.SampleRate, currentMediaType.BitsPerSample, currentMediaType.Channels, encoding);
                }

                reader.SetStreamSelection(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM, true);

                if (_hasFixedLength)
                    _length = GetLength(reader);

                return reader;
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        private long GetLength(MFSourceReader reader)
        {
            lock (_lockObj)
            {
                try
                {
                    if (reader == null)
                        return 0;

                    PropertyVariant value = reader.GetPresentationAttribute(MFInterops.MF_SOURCE_READER_MEDIASOURCE, MediaFoundationAttributes.MF_PD_DURATION);
                    var length = ((value.HValue) * _waveFormat.BytesPerSecond) / 10000000L;
                    value.Dispose();
                    return length;
                }
                catch (Exception)
                {
                    //if (e.Result == (int)HResult.MF_E_ATTRIBUTENOTFOUND)
                    //    return 0;
                    //throw;
                    return 0;
                }
            }
        }

        private void SetPosition(long value)
        {
            if (CanSeek)
            {
                lock (_lockObj)
                {
                    long hnsPos = (10000000L * value) / WaveFormat.BytesPerSecond;
                    var propertyVariant = PropertyVariant.CreateLong(hnsPos);
                    _reader.SetCurrentPosition(Guid.Empty, propertyVariant);
                    _decoderBufferCount = 0;
                    _decoderBufferOffset = 0;
                    _position = value;
                }
            }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length < count)
                throw new ArgumentException("Length is too small.", "buffer");

            lock (_lockObj)
            {
                int read = 0;

                if (_reader == null || _disposed)
                    return read;

                if (_decoderBufferCount > 0)
                {
                    read += CopyDecoderBuffer(buffer, offset + read, count - read);
                }

                while (read < count)
                {
                    MFSourceReaderFlag flags;
                    long timestamp;
                    int actualStreamIndex;
                    using (var sample = _reader.ReadSample(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM, 0, out actualStreamIndex, out flags, out timestamp))
                    {
                        if (flags != MFSourceReaderFlag.None)
                            break;

                        using (MFMediaBuffer mediaBuffer = sample.ConvertToContinousBuffer())
                        {
                            int maxlength, currentlength;
                            IntPtr pdata = mediaBuffer.Lock(out maxlength, out currentlength);
                            _decoderBuffer = _decoderBuffer.CheckBuffer(currentlength);
                            Marshal.Copy(pdata, _decoderBuffer, 0, currentlength);
                            _decoderBufferCount = currentlength;
                            _decoderBufferOffset = 0;

                            int tmp = CopyDecoderBuffer(buffer, offset + read, count - read);
                            read += tmp;

                            mediaBuffer.Unlock();
                        }
                    }
                }

                _position += read;

                return read;
            }
        }

        private int CopyDecoderBuffer(byte[] destBuffer, int offset, int count)
        {
            count = Math.Min(count, _decoderBufferCount);
            Array.Copy(_decoderBuffer, _decoderBufferOffset, destBuffer, offset, count);
            _decoderBufferCount -= count;
            _decoderBufferOffset += count;

            if (_decoderBufferCount == 0)
                _decoderBufferOffset = 0;

            return count;
        }

        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_lockObj)
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }
                if (_byteStream != null)
                {
                    Marshal.ReleaseComObject(_byteStream);
                    _byteStream = null;
                }
                if (_stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }
            }
        }

        ~MediaFoundationDecoder()
        {
            Dispose(false);
        }

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public long Position
        {
            get
            {
                return _position;
            }
            set
            {
                SetPosition(value);
            }
        }

        public long Length
        {
            get
            {
                if (this._hasFixedLength)
                    return _length;
                return GetLength(_reader);
            }
        }

        public bool CanSeek
        {
            get { return _reader.CanSeek; }
        }
    }
}