using CSCore.DMO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Codecs.MP3
{
    public class DmoMP3Decoder : DmoStream
    {
        private Stream _stream;

        private DmoMP3DecoderObject _comObj;
        private FrameInfoCollection _frameInfoCollection;
        private MP3Format _inputFormat;
        private long _position;

        private bool _parsedStream = false;

        public DmoMP3Decoder(Stream stream)
            : base()
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");
            if (!stream.CanSeek)
                throw new ArgumentException("Stream is not seekable.", "stream");

            _stream = stream;

            ParseForMP3Frame(stream);
            Initialize();
        }

        private void ParseForMP3Frame(Stream stream)
        {
            if (_parsedStream)
                return;

            MP3Frame frame = null;
            long offsetOfFirstFrame = 0;
            
            while(frame == null && !stream.IsEndOfStream())
            {
                offsetOfFirstFrame = stream.Position;
                frame = MP3Frame.FromStream(stream);
            }

            if (frame == null)
                throw new MP3Exception("Could not find any MP3-Frames in the stream.");

            XingHeader xingHeader = XingHeader.FromFrame(frame);
            if(xingHeader != null)
            {
                offsetOfFirstFrame = stream.Position;
            }

            _inputFormat = new MP3Format(frame.SampleRate, frame.ChannelCount, frame.FrameLength, frame.BitRate); //implement VBR?

            //Prescan stream
            _frameInfoCollection = new FrameInfoCollection();
            while (_frameInfoCollection.AddFromMP3Stream(stream)) ;

            stream.Position = offsetOfFirstFrame;

            _parsedStream = true;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            _position += read;
            return read;
        }

        protected override MediaObject CreateMediaObject(WaveFormat inputFormat, WaveFormat outputFormat)
        {
            _comObj = new DmoMP3DecoderObject();
            var mediaObject = new MediaObject(Marshal.GetComInterfaceForObject(_comObj, typeof(IMediaObject)));

            return mediaObject;
        }

        protected override WaveFormat GetInputFormat()
        {
            return _inputFormat;   
        }

        protected override WaveFormat GetOutputFormat()
        {
            return new WaveFormat(_inputFormat.SampleRate, 16, _inputFormat.Channels);
        }

        protected override int GetInputData(ref byte[] inputDataBuffer, int requested)
        {
            MP3Frame frame = ReadNextMP3Frame(ref inputDataBuffer);
            if (frame == null)
            {
                inputDataBuffer = new byte[0];
                return 0;
            }

            if (inputDataBuffer.Length > frame.FrameLength)
                Array.Clear(inputDataBuffer, frame.FrameLength, inputDataBuffer.Length - frame.FrameLength);

            return frame.FrameLength;
        }

        public override long Position
        {
            get
            {
                return _position;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                SetPosition(value);
            }
        }

        public override long Length
        {
            get { return _frameInfoCollection.TotalSamples * WaveFormat.BytesPerBlock; }
        }

        private void SetPosition(long value)
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

                    break;
                }
            }

            ResetOverflowBuffer();
        }

        private MP3Frame ReadNextMP3Frame(ref byte[] frameBuffer)
        {
            MP3Frame frame = MP3Frame.FromStream(_stream, ref frameBuffer);
            if (frame != null && _frameInfoCollection != null)
                _frameInfoCollection.PlaybackIndex++;

            return frame;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(_comObj != null)
            {
                Marshal.ReleaseComObject(_comObj);
                _comObj = null;
            }
        }

        [ComImport]
        [Guid("bbeea841-0a63-4f52-a7ab-a9b3a84ed38a")]
        private sealed class DmoMP3DecoderObject
        {
        }
    }
}
