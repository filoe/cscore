using CSCore.ACM;
using System;
using System.Diagnostics;
using System.IO;

namespace CSCore.Codecs.MP3
{
    public class MP3Stream : IWaveSource, IDisposable
    {
        private MP3Frame _frame = null;
        private FrameInfoCollection _frameInfoCollection;
        private AcmConverter _converter;
        private Stream _stream;
        private object _lockObject = new object();

        private int _overflows;
        private int _bufferoffset;

        private int _sampleRate = 0;
        private long _dataStartIndex = 0;
        private long _dataLength = 0;
        private double _bitRate = 0.0;

        private long _position;

        //int _bytesPerSample = 0; //BitsPerSample / 8 * Channel
        private byte[] _pcmDstBuffer;
        private byte[] _frameBuffer;

        private const short SamplesPerFrame = 1152;

        public MP3Stream(Stream stream, bool scanStream)
            : this(stream, scanStream, 0)
        {
        }

        public MP3Stream(Stream stream, bool scanStream, int lengthOffset)
        {
            int frameLength = 0;
            if (scanStream)
                _frameInfoCollection = new FrameInfoCollection();
            else
                _frameInfoCollection = null;

            _dataStartIndex = stream.Position;
            do
            {
                _frame = MP3Frame.FromStream(stream);
                if (_frame == null && stream.IsEndOfStream())
                    throw new FormatException("Stream is no MP3-stream. No MP3-Frame was found.");
            } while (_frame == null && !stream.IsEndOfStream());

            frameLength = _frame.FrameLength;
            _sampleRate = _frame.SampleRate;
            XingHeader = XingHeader.FromFrame(_frame); //The first frame can contain a Xingheader
            if (XingHeader != null)
            {
                //Todo: dataInitPosition = stream.Position
                _dataStartIndex = stream.Position;
            }

            _dataLength = stream.Length - _dataStartIndex - lengthOffset;

            if (scanStream)
            {
                stream.Position = _dataStartIndex;
                PreScanFile(stream);
                CanSeek = true;
            }
            else
            {
                CanSeek = false;
            }

            stream.Position = _dataStartIndex;

            /*
             * bytes * 8 (8bits perbyte) / ms = totalbits / totalms = bits per ms
             */
            if (scanStream)
            {
                _bitRate = ((_dataLength * 8.0) / ((double)_frameInfoCollection.TotalSamples / (double)_sampleRate));
            }
            else
            {
                _bitRate = ((_frame.BitRate) / 1);
            }
            MP3Format = new MP3Format(_sampleRate, _frame.ChannelCount, frameLength, (int)Math.Round(_bitRate));
            _converter = new AcmConverter(MP3Format);
            WaveFormat = _converter.DestinationFormat;

            //_bytesPerSample = (WaveFormat.BitsPerSample / 8 * WaveFormat.Channels);
            _pcmDstBuffer = new byte[SamplesPerFrame * WaveFormat.BytesPerBlock * 2];

            stream.Position = _dataStartIndex;
            _stream = stream;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (IsEOF) 
                return 0;

            try
            {
                int read = 0;
                lock (_lockObject)
                {
                    read += CheckForOverflows(buffer, ref offset, count);
                    if (read == count)
                        return read; //if we've already read enough -> exit

                    while (read < count)
                    {
                        try
                        {
                            _frame = ReadNextMP3Frame();
                            if (_frame == null) 
                            { 
                                IsEOF = true; 
                                break; 
                            }

                            int converted = _converter.Convert(_frameBuffer, _frame.FrameLength, _pcmDstBuffer, 0);
                            int BTCC = Math.Min(count - read, converted);

                            Array.Copy(_pcmDstBuffer, 0, buffer, offset, BTCC);
                            offset += BTCC; 
                            read += BTCC;

                            /*
                             * If there are any overflows -> store them in a
                             * buffer to use it next time.
                             */
                            _overflows = ((converted > BTCC) ? (converted - BTCC) : 0);
                            _bufferoffset = ((converted > BTCC) ? (BTCC) : 0);
                        }
                        catch (Exception ex)
                        {
                            Debugger.Break();
                            Debug.WriteLine("Mp3Stream::Read: " + ex.ToString());
                        }
                    }

                    _position += read;

                    return read;
                }
            }
            catch (EndOfStreamException)
            {
                return -1;
            }
        }

        private int CheckForOverflows(byte[] buffer, ref int offset, int count)
        {
            if (_overflows != 0)
            {
                int result = 0;
                int BTCC = Math.Min(count, _overflows);
                Array.Copy(_pcmDstBuffer, _bufferoffset, buffer, offset, BTCC);

                _overflows -= BTCC;
                _bufferoffset = _overflows == 0 ? 0 : (_bufferoffset + BTCC);
                result += BTCC;
                offset += BTCC;

                return result;
            }
            else
            {
                return 0;
            }
        }

        private MP3Frame ReadNextMP3Frame()
        {
            MP3Frame frame = MP3Frame.FromStream(_stream, ref _frameBuffer);
            if (frame != null && _frameInfoCollection != null)
                _frameInfoCollection.PlaybackIndex++;

            return frame;
        }

        public long Position
        {
            get
            {
                /*if (CanSeek)
                {
                    if (_frameInfoCollection.PlaybackIndex < _frameInfoCollection.Count)
                    {
                        int frameIndex = _frameInfoCollection.PlaybackIndex;
                        int result = ((_frameInfoCollection[frameIndex].SampleIndex) * WaveFormat.BytesPerBlock) + _bufferoffset;
                        return result;
                    }
                    else
                    {
                        return Length;
                    }
                }
                
                return -1;*/
                lock (_lockObject)
                {
                    return _position;
                }
            }
            set
            {
                if (!CanSeek)
                    return;

                lock (_lockObject)
                {
                    value = Math.Min(value, Length);
                    value = (value > 0) ? value : 0;

                    long n = value / WaveFormat.BytesPerBlock;

                    for (int i = 0; i < _frameInfoCollection.Count; i++)
                    {
                        if ((value / WaveFormat.BytesPerBlock) <= _frameInfoCollection[i].SampleIndex)
                        {
                            _stream.Position = _frameInfoCollection[i].StreamPosition;
                            _frameInfoCollection.PlaybackIndex = i;

                            _position = _frameInfoCollection[i].SampleIndex * WaveFormat.BlockAlign;

                            if (_stream.Position < _stream.Length)
                                IsEOF = false;

                            break;
                        }
                    }

                    _bufferoffset = 0; 
                    _overflows = 0;
                }
            }
        }

        private void PreScanFile(Stream stream)
        {
            while (_frameInfoCollection.AddFromMP3Stream(stream)) ;
        }

        public XingHeader XingHeader { get; private set; }

        public Boolean CanSeek { get; private set; }

        public MP3Format MP3Format { get; private set; }

        public WaveFormat WaveFormat { get; private set; }

        public Boolean IsEOF { get; private set; }

        public long Length
        {
            get
            {
                return CanSeek ? (_frameInfoCollection.TotalSamples * WaveFormat.BytesPerBlock) : -1;
            }
        }

        internal double BitRate { get { return _bitRate; } }

        internal long DataStartIndex { get { return _dataStartIndex; } }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            lock (_lockObject)
            {
                if (disposing)
                {
                    if (_converter != null)
                    {
                        _converter.Dispose();
                        _converter = null;
                    }
                    if (_stream != null)
                    {
                        _stream.Dispose();
                        _stream = null;
                    }
                    if (_frameInfoCollection != null)
                    {
                        _frameInfoCollection.Dispose();
                        _frameInfoCollection = null;
                    }
                    _pcmDstBuffer = null;
                    _frame = null;
                }
            }
        }

        ~MP3Stream()
        {
            Dispose(false);
        }
    }
}