using CSCore.Compression.ACM;
using System;
using System.IO;

namespace CSCore.Codecs.MP3
{
    public class Mp3Stream : IWaveSource, IDisposable
    {
        Mp3Frame frame = null;
        FrameInfoCollection frameInfoCollection;
        AcmConverter _converter;
        Stream _stream;
        object lockObject = new object();

        int overflows;
        int bufferoffset;

        int sampleRate = 0;
        long dataStartIndex = 0;
        long dataLength = 0;
        double bitRate = 0.0; //double um möglichst wenig Rundungsfehler zu erzeugen
        int bytesPerSample = 0; //BitsPerSample / 8 * Channel
        byte[] pcmDstBuffer; //Buffer in den die konvertierten pcm bytes kommen
        byte[] _frameBuffer;

        const short SamplesPerFrame = 1152;

        public Mp3Stream(Stream stream, bool scanStream) : this(stream, scanStream, 0) { }
        public Mp3Stream(Stream stream, bool scanStream, int lengthOffset)
        {
            int frameLength = 0;
            if (scanStream)
                frameInfoCollection = new FrameInfoCollection();
            else
                frameInfoCollection = null;

            dataStartIndex = stream.Position;
            do
            {
                frame = Mp3Frame.FromStream(stream);
                if (frame == null && stream.IsEndOfStream())
                    throw new FormatException("Stream is no mp3-stream. No MP3-Frame was found.");

            } while (frame == null && !stream.IsEndOfStream());

            frameLength = frame.FrameLength;
            sampleRate = frame.SampleRate;
            XingHeader = XingHeader.FromFrame(frame); //Im ersten Frame kann XingHeader enthalten sein
            if (XingHeader != null)
            {
                //Todo: dataInitPosition = stream.Position
                dataStartIndex = stream.Position;
            }

            dataLength = stream.Length - dataStartIndex - lengthOffset;

            if (scanStream)
            {
                stream.Position = dataStartIndex;
                PreScanFile(stream);
                CanSeek = true;
            }
            else
            {
                CanSeek = false;
            }

            stream.Position = dataStartIndex;

            /*
             * bytes * 8 (8bits perbyte) / ms = totalbits / totalms = bits per ms
             */
            if (scanStream)
            {
                bitRate = ((dataLength * 8.0) / ((double)frameInfoCollection.TotalSamples / (double)sampleRate));
            }
            else
            {
                bitRate = ( (frame.BitRate) / 1);
            }
            MP3Format = new Mp3Format(sampleRate, frame.ChannelMode.ToShort(), frameLength, (int)Math.Round(bitRate));
            _converter = new AcmConverter(MP3Format);
            WaveFormat = _converter.DestinationFormat;

            bytesPerSample = (WaveFormat.BitsPerSample / 8 * WaveFormat.Channels);
            pcmDstBuffer = new byte[SamplesPerFrame * bytesPerSample * 2];

            stream.Position = dataStartIndex;
            _stream = stream;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (IsEOF) return 0;

            try
            {
                int read = 0;
                lock (lockObject)
                {
                    read += CheckForOverflows(buffer, ref offset, count);
                    if (read == count)
                        return read; //if we've already read enough -> exit

                    while (read < count)
                    {
                        try
                        {
                            frame = ReadNextMP3Frame();
                            if (frame == null) { IsEOF = true; break; }

                            int converted = _converter.Convert(_frameBuffer, frame.FrameLength, pcmDstBuffer, 0);
                            int BTCC = Math.Min(count - read, converted);

                            Array.Copy(pcmDstBuffer, 0, buffer, offset, BTCC);
                            offset += BTCC; read += BTCC;

                            /*
                             * If there are any overflows -> store them in a 
                             * buffer to use it next time.
                             */
                            overflows = ((converted > BTCC) ? (converted - BTCC) : 0);
                            bufferoffset = ((converted > BTCC) ? (BTCC) : 0);
                        }
                        catch (Exception ex)
                        {
                            Context.Current.Logger.Error(ex, "Mp3Stream.Read(byte[], int, int)", false);
                        }
                    }

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
            if (overflows != 0)
            {
                int result = 0;
                int BTCC = Math.Min(count, overflows);
                Array.Copy(pcmDstBuffer, bufferoffset, buffer, offset, BTCC);

                overflows -= BTCC;
                bufferoffset = overflows == 0 ? 0 : (bufferoffset + BTCC);
                result += BTCC;
                offset += BTCC;

                return result;
            }
            else
            {
                return 0;
            }
        }

        private Mp3Frame ReadNextMP3Frame()
        {
            Mp3Frame frame = Mp3Frame.FromStream(_stream, ref _frameBuffer);
            if (frame != null && frameInfoCollection != null)
                frameInfoCollection.PlaybackIndex++;

            return frame;
        }

        public long Position
        {
            get 
            {
                if (CanSeek)
                {
                    if (frameInfoCollection.PlaybackIndex < frameInfoCollection.Count)
                        return (frameInfoCollection[frameInfoCollection.PlaybackIndex].SampleIndex * bytesPerSample);
                    else
                        return Length;
                }
                return -1;
            }
            set
            {
                if (!CanSeek)
                    return;//throw new InvalidOperationException("This Mp3Stream does not support seeking. You have to use Pre_Scan strategy to use this feature");

                lock (lockObject)
                {
                    value = Math.Min(value, Length);
                    value = (value > 0)? value: 0;

                    for (int i = 0; i < frameInfoCollection.Count; i++)
                    {
                        if ((value / bytesPerSample) <= frameInfoCollection[i].SampleIndex)
                        {
                            _stream.Position = frameInfoCollection[i].StreamPosition;
                            frameInfoCollection.PlaybackIndex = i;
                            if (_stream.Position < _stream.Length)
                                IsEOF = false;

                            break;
                        }
                    }

                    bufferoffset = 0; overflows = 0;
                }

            }
        }

        private void PreScanFile(Stream stream)
        {
            do
            {
                //nothing.. :)
            } while (frameInfoCollection.AddFromMP3Stream(stream));
        }

        public XingHeader XingHeader { get; private set; }

        public Boolean CanSeek { get; private set; }

        public Mp3Format MP3Format { get; private set; }

        public WaveFormat WaveFormat { get; private set; }

        public Boolean IsEOF { get; private set; }

        public long Length
        {
            get
            {
                return CanSeek ? (frameInfoCollection.TotalSamples * bytesPerSample) : -1;
            }
        }

        internal double BitRate { get { return bitRate; } }
        internal long DataStartIndex { get { return dataStartIndex; } }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        protected void Dispose(bool disposing)
        {
            lock (lockObject)
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
                    if (frameInfoCollection != null)
                    {
                        frameInfoCollection.Dispose();
                        frameInfoCollection = null;
                    }
                    pcmDstBuffer = null;
                    frame = null;
                    Context.Current.Logger.Info(String.Format("Disposed Mp3Stream Disposing: {0}", disposing), "Mp3Stream.Dispose(bool)");
                }
            }
        }

        ~Mp3Stream()
        {
            Dispose(false);
        }
    }
}
