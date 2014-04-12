#define DIAGNOSTICS

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CSCore.Codecs.FLAC
{
    public class FlacFile : IWaveSource
    {
        private Stream _stream;
        private FlacMetadataStreamInfo _streamInfo;
        private FlacPreScan _scan;

        private object _bufferLock = new object();

        //overflow:
        private byte[] _overflowBuffer;

        private int _overflowCount;
        private int _overflowOffset;

        private int _frameIndex = 0;

        public List<FlacMetadata> Metadata
        {
            get;
            protected set;
        }

        private WaveFormat _waveFormat;

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public bool CanSeek
        {
            get { return _scan != null; }
        }

        private FlacFrame _frame;

        protected FlacFrame Frame
        {
            get { return _frame ?? (_frame = FlacFrame.FromStream(_stream, _streamInfo)); }
        }

        public FlacFile(string fileName)
            : this(File.OpenRead(fileName))
        {
        }

        public FlacFile(Stream stream)
            : this(stream, FlacPreScanMethodMode.Default)
        {
        }

        public FlacFile(Stream stream, FlacPreScanMethodMode? scanFlag)
            : this(stream, scanFlag, null)
        {
        }

        public FlacFile(Stream stream, FlacPreScanMethodMode? scanFlag, Action<FlacPreScanFinishedEventArgs> onscanFinished)
        {

            if (stream == null)
                throw new ArgumentNullException();
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");

            _stream = stream;

            //skip ID3v2
            Tags.ID3.ID3v2.SkipTag(stream);
            int read = 0;

            //read fLaC sync
            byte[] beginSync = new byte[4];
            read = stream.Read(beginSync, 0, beginSync.Length);
            if (read < beginSync.Length)
                throw new EndOfStreamException("Can not read \"fLaC\" sync.");
            if (beginSync[0] == 0x66 && beginSync[1] == 0x4C &&
               beginSync[2] == 0x61 && beginSync[3] == 0x43)
            {
                //read metadata
                var metadata = FlacMetadata.AllDataFromStream(stream);

                Metadata = metadata;
                if (metadata == null || metadata.Count <= 0)
                {
                    throw new FlacException("No Metadata found.", FlacLayer.Metadata);
                }

                FlacMetadataStreamInfo streamInfo = metadata.Where(x => x.MetaDataType == FlacMetaDataType.StreamInfo).First() as FlacMetadataStreamInfo;
                if (streamInfo == null)
                    new FlacException("No StreamInfo-Metadata found.", FlacLayer.Metadata);

                _streamInfo = streamInfo;
                _waveFormat = new WaveFormat(streamInfo.SampleRate, (short)streamInfo.BitsPerSample, (short)streamInfo.Channels, AudioEncoding.Pcm);
                Debug.WriteLine("Flac StreamInfo found -> WaveFormat: " + _waveFormat.ToString());
                Debug.WriteLine("Flac-File-Metadata read.");
            }
            else
            {
                throw new FlacException("Invalid Flac-File. \"fLaC\" Sync not found.", FlacLayer.Top);
            }

            //prescan stream
            if (scanFlag != null)
            {
                FlacPreScan scan = new FlacPreScan(stream);
                scan.ScanFinished += (s, e) =>
                {
                    if (onscanFinished != null)
                        onscanFinished(e);
                };
                scan.ScanStream(_streamInfo, (FlacPreScanMethodMode)scanFlag);
                _scan = scan;
            }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;
            count -= (count % WaveFormat.BlockAlign);

            lock (_bufferLock)
            {
                read += GetOverflows(buffer, ref offset, count);

                while (read < count)
                {
                    FlacFrame frame = Frame;
                    if (frame == null) return 0;

                    while (!frame.NextFrame())
                    {
                        if (CanSeek) //go to next frame
                        {
                            if (++_frameIndex >= _scan.Frames.Count) return 0;
                            _stream.Position = _scan.Frames[_frameIndex].StreamOffset;
                        }
                    }
                    _frameIndex++;

                    int bufferlength = frame.GetBuffer(ref _overflowBuffer, 0);
                    int bytesToCopy = Math.Min(count - read, bufferlength);
                    Array.Copy(_overflowBuffer, 0, buffer, offset, bytesToCopy);
                    read += bytesToCopy;
                    offset += bytesToCopy;

                    _overflowCount = ((bufferlength > bytesToCopy) ? (bufferlength - bytesToCopy) : 0);
                    _overflowOffset = ((bufferlength > bytesToCopy) ? (bytesToCopy) : 0);
                }
            }
#if DIAGNOSTICS
            _position += read;
#endif

            return read;
        }

        protected int GetOverflows(byte[] buffer, ref int offset, int count)
        {
            if (_overflowCount != 0 && _overflowBuffer != null && count > 0)
            {
                int bytesToCopy = Math.Min(count, _overflowCount);
                Array.Copy(_overflowBuffer, _overflowOffset, buffer, offset, bytesToCopy);

                _overflowCount -= bytesToCopy;
                _overflowOffset += bytesToCopy;
                offset += bytesToCopy;
                return bytesToCopy;
            }
            else
            {
                return 0;
            }
        }

#if DIAGNOSTICS
        private long _position = 0;
#endif

        public long Position
        {
            get
            {
                if (!CanSeek)
                    return 0;

                lock (_bufferLock)
                {
#if !DIAGNOSTICS
                    if (_frameIndex == _scan.Frames.Count)
                        return Length;
                    return _scan.Frames[_frameIndex].SampleOffset * WaveFormat.BlockAlign + _overflowOffset;
#else
                    return _position;
#endif
                }
            }
            set
            {
                if (!CanSeek)
                    return;//Context.Current.Logger.Fatal(new InvalidOperationException("Not seekable"), "FlacFile.Position.set(value)", true);
                lock (_bufferLock)
                {
                    value = Math.Min(value, Length);
                    value = value > 0 ? value : 0;

                    for (int i = 0; i < _scan.Frames.Count; i++)
                    {
                        if ((value / WaveFormat.BlockAlign) <= _scan.Frames[i].SampleOffset)
                        {
                            _stream.Position = _scan.Frames[i].StreamOffset;
                            _frameIndex = i;
                            if (_stream.Position >= _stream.Length)
                                throw new EndOfStreamException("Stream got EOF.");

#if DIAGNOSTICS
                            _position = _scan.Frames[i].SampleOffset * WaveFormat.BlockAlign;
#endif
                            _overflowCount = 0;
                            _overflowOffset = 0;
                            break;
                        }
                    }
                }
            }
        }

        public long Length
        {
            get
            {
                if (CanSeek)
                    return _scan.TotalSamples * WaveFormat.BlockAlign;
                return -1;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_bufferLock)
            {
                if (_frame != null)
                {
                    _frame.FreeBuffers();
                    _frame = null;
                }

                if (_stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }
            }
        }

        ~FlacFile()
        {
            Dispose(false);
        }
    }
}