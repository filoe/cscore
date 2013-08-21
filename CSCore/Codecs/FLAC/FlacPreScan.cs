using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CSCore.Codecs.FLAC
{
    public class FlacPreScan
    {
        private const int bufferSize = 50000;
        private Stream _stream;
        private bool _isRunning = false;

        public event EventHandler<FlacPreScanFinishedEventArgs> ScanFinished;

        public List<FlacFrameInformation> Frames { get; private set; }

        public long TotalLength { get; private set; }

        public long TotalSamples { get; private set; }

        public FlacPreScan(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (!stream.CanRead) throw new ArgumentException("stream is not readable");

            _stream = stream;
        }

        public void ScanStream(FlacMetadataStreamInfo streamInfo, FlacPreScanMethodMode method)
        {
            long saveOffset = _stream.Position;
            StartScan(streamInfo, method);
            _stream.Position = saveOffset;

            long totalLength = 0, totalsamples = 0;
            foreach (var frame in Frames)
            {
                totalLength += frame.Header.BlockSize * frame.Header.BitsPerSample * frame.Header.Channels;
                totalsamples += frame.Header.BlockSize;
            }
            TotalLength = totalLength;
            TotalSamples = totalsamples;
        }

        protected void StartScan(FlacMetadataStreamInfo streamInfo, FlacPreScanMethodMode method)
        {
            if (_isRunning)
                throw new Exception("Scan is already running.");

            _isRunning = true;

            if (method == FlacPreScanMethodMode.Async)
            {
                new Thread((o) =>
                {
                    Frames = RunScan(streamInfo);
                    _isRunning = false;
                }).Start();
            }
            else
            {
                Frames = ScanThisShit(streamInfo);
                _isRunning = false;
            }
        }

        protected virtual List<FlacFrameInformation> RunScan(FlacMetadataStreamInfo streamInfo)
        {
#if DEBUG
            Stopwatch watch = new Stopwatch();
            watch.Start();
#endif
            var result = ScanThisShit(streamInfo);

#if DEBUG
            watch.Stop();
            Debug.WriteLine(String.Format("FlacPreScan finished: {0} Bytes processed in {1} ms.",
                _stream.Length.ToString(), watch.ElapsedMilliseconds.ToString()));
#endif
            RaiseScanFinished(result);
            return result;
        }

        protected virtual void RaiseScanFinished(List<FlacFrameInformation> frames)
        {
            if (ScanFinished != null)
                ScanFinished(this, new FlacPreScanFinishedEventArgs(frames));
        }

        private unsafe List<FlacFrameInformation> ScanThisShit(FlacMetadataStreamInfo streamInfo)
        {
            Stream stream = _stream;

            //if (!(stream is BufferedStream))
            //    stream = new BufferedStream(stream);

            byte[] buffer = new byte[bufferSize];
            int read = 0;
            stream.Position = 4; //fLaC

            FlacMetadata.AllDataFromStream(stream);

            List<FlacFrameInformation> frames = new List<FlacFrameInformation>();
            FlacFrameInformation frameInfo = new FlacFrameInformation();
            frameInfo.IsFirstFrame = true;

            FlacFrameHeader header;
            FlacFrameHeader tmp = null;
            FlacFrameHeader baseHeader = null;

            while (true)
            {
                read = stream.Read(buffer, 0, buffer.Length);
                if (read <= FlacConstant.FrameHeaderSize)
                    break;

                fixed (byte* bufferPtr = buffer)
                {
                    byte* ptr = bufferPtr;
                    byte* ptrSafe;
                    //for (int i = 0; i < read - FlacConstant.FrameHeaderSize; i++)
                    while ((bufferPtr + read - FlacConstant.FrameHeaderSize) > ptr)
                    {
                        if ((*ptr++ & 0xFF) == 0xFF && (*ptr & 0xF8) == 0xF8) //check sync
                        {
                            ptrSafe = ptr;
                            ptr--;
                            if (IsFrame(ref ptr, streamInfo, baseHeader, out tmp))
                            {
                                header = tmp;
                                if (frameInfo.IsFirstFrame)
                                {
                                    baseHeader = header;
                                    frameInfo.IsFirstFrame = false;
                                }

                                if (baseHeader.CompareTo(header))
                                {
                                    frameInfo.StreamOffset = stream.Position - read + ((ptrSafe - 1) - bufferPtr);
                                    frameInfo.Header = header;
                                    frames.Add(frameInfo);

                                    frameInfo.SampleOffset += header.BlockSize;
                                }
                                else
                                {
                                    ptr = ptrSafe;
                                }
                                //todo:
                            }
                            else
                            {
                                ptr = ptrSafe;
                            }
                        }
                    }
                }

                stream.Position -= FlacConstant.FrameHeaderSize;
            }

            return frames;
        }

        private unsafe bool IsFrame(ref byte* buffer, FlacMetadataStreamInfo streamInfo, FlacFrameHeader baseHeader, out FlacFrameHeader header)
        {
            header = new FlacFrameHeader(ref buffer, streamInfo, true, false);
            return !header.HasError;
        }
    }

    [System.Diagnostics.DebuggerDisplay("StreamOffset: {StreamOffset}")]
    public struct FlacFrameInformation
    {
        public FlacFrameHeader Header { get; set; }

        public Boolean IsFirstFrame { get; set; }

        public long StreamOffset { get; set; }

        public long SampleOffset { get; set; }
    }

    public enum FlacPreScanMethodMode
    {
        Default,

        /// <summary>
        /// Scan async BUT don't use the stream while scan is running because the stream position
        /// will change while scanning. If you playback the stream, it will cause an error!
        /// </summary>
        Async
    }

    public class FlacPreScanFinishedEventArgs : EventArgs
    {
        public List<FlacFrameInformation> Frames { get; private set; }

        public FlacPreScanFinishedEventArgs(List<FlacFrameInformation> frames)
        {
            Frames = frames;
        }
    }
}