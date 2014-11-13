#pragma warning disable 1591

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using System.Threading;
using CSCore.Utils;
using CSCore.Utils.Buffer;
using ThreadState = System.Threading.ThreadState;

namespace CSCore.Codecs.MP3
{
    [Obsolete("Use the CSCore.MediaFoundation.MediaFoundationDecoder instead.")]
    public class Mp3WebStream : IWaveSource
    {
        private readonly Uri _uri;
        private FixedSizeBuffer<byte> _buffer;

        private Thread _bufferThread;
        private bool _disposed;
        private bool _disposing;
        private WebResponse _response;
        private Stream _stream;

        private Stream _bufferingStream;
        private DmoMp3Decoder _decoder;

        public Mp3WebStream(string uri, bool async)
            : this(new Uri(uri), async)
        {
        }

        public Mp3WebStream(Uri uri, bool async)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");
            _uri = uri;

            if (!SetAllowUnsafeHeaderParsing20())
                throw new Exception("Setting allowed Unsafe-Header-Parsing failed.");

            CreateStream(async);
        }

        public Uri StreamUri
        {
            get { return _uri; }
        }

        public int BufferedBytes
        {
            get { return _buffer.Buffered; }
        }

        public int BufferSize
        {
            get { return _buffer.Length; }
        }

        public bool CanSeek { get { return false; }}

        public WaveFormat WaveFormat
        {
            get { return _decoder == null ? null : _decoder.WaveFormat; }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;
            Array.Clear(buffer, offset, count - offset);

            if (_buffer.Buffered < _buffer.Length / 2)
                return count;
            do
            {
                read += _decoder.Read(buffer, offset + read, count - read);
            } while (read < count);
            return read;
        }

        public long Position
        {
            get { return -1; }
            set { throw new InvalidOperationException(); }
        }

        public long Length
        {
            get { return 0; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event EventHandler<ConnectionEstablishedEventArgs> ConnectionCreated;

        private void CreateStream(bool async)
        {
            _disposing = false;

            Func<bool> func = () =>
            {
                var resetEvent = new ManualResetEvent(false);

                bool success = InitializeConnection();
                if (success)
                {
                    _bufferThread = new Thread(BufferProc);
                    _bufferThread.Start(resetEvent);

                    success = resetEvent.WaitOne();
                }
                if (ConnectionCreated != null && async)
                    ConnectionCreated(this, new ConnectionEstablishedEventArgs(_uri, success));

                return success;
            };
            if (async)
                ThreadPool.QueueUserWorkItem(o => func());
            else
                func();
        }

        private bool InitializeConnection()
        {
            try
            {
                WebRequest.DefaultWebProxy = null;
                var request = WebRequest.Create(StreamUri) as HttpWebRequest;
                if (request == null)
                    throw new WebException("Could not create HttpWebRequest to {" + StreamUri.AbsolutePath + "}");

                _response = request.GetResponse();
                if (_response == null)
                    throw new WebException("Could not create WebResponse to {" + StreamUri.AbsolutePath + "}");

                Stream responseStream = _response.GetResponseStream();
// ReSharper disable once PossibleNullReferenceException
                responseStream.ReadTimeout = 1500;
                _stream = new ReadBlockStream(responseStream);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Mp3WebStream::InitializeConnection: " + ex);
                return false;
            }
        }

        private void BufferProc(object o)
        {
            var resetEvent = o as EventWaitHandle;

            try
            {
                if (_stream == null || _stream.CanRead == false)
                    throw new Exception("Mp3WebStream not initialized");

                byte[] buffer = null;
                int read = 0;

                Mp3Format format, prevFormat;
                Mp3Frame frame;

                read = ReadRawDataFromFrame(ref buffer, out frame);
                format = new Mp3Format(frame.SampleRate, frame.ChannelCount, frame.FrameLength,
                    frame.BitRate);

                _buffer = new FixedSizeBuffer<byte>(format.BytesPerSecond * 10);
                _bufferingStream = new ReadBlockStream(_buffer.ToStream());

                do
                {
                    _buffer.Write(buffer, 0, read);
                    read = ReadRawDataFromFrame(ref buffer, out frame);
                    prevFormat = format;
                    format = new Mp3Format(frame.SampleRate, frame.ChannelCount, frame.FrameLength,
                        frame.BitRate);
                }
                while (_buffer.Buffered < _buffer.Length / 10 || !format.Equals(prevFormat));

                _decoder = new DmoMp3Decoder(_bufferingStream, false);

                if (resetEvent != null) 
                    resetEvent.Set();

                do
                {
                    if (_buffer.Buffered >= _buffer.Length * 0.85 && !_disposing)
                    {
                        Thread.Sleep(250);
                    }
                    else
                    {
                        _buffer.Write(buffer, 0, read);
                        read = ReadRawDataFromFrame(ref buffer, out frame);
                    }
                    
                } while (!_disposing);
            }
            finally
            {
                if (resetEvent != null)
                    resetEvent.Set();
            }
        }

        private int ReadRawDataFromFrame(ref byte[] buffer, out Mp3Frame frame)
        {
            frame = GetNextFrame(_stream, ref buffer);
            if (frame == null)
            {
                buffer = new byte[0];
                return 0;
            }

            if (buffer.Length > frame.FrameLength)
                Array.Clear(buffer, frame.FrameLength, buffer.Length - frame.FrameLength);

            return frame.FrameLength;
        }

        private Mp3Frame GetNextFrame(Stream stream, ref byte[] frameBuffer)
        {
            Mp3Frame frame;
            do
            {
                frame = Mp3Frame.FromStream(stream, ref frameBuffer);
            } while (frame == null);

            return frame;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposing = true;
                if (_bufferThread != null &&
                    _bufferThread.ThreadState != ThreadState.Stopped &&
                    !_bufferThread.Join(500))
                    _bufferThread.Abort();
                CloseResponse();
            }
            _disposed = true;
        }

        ~Mp3WebStream()
        {
            Dispose(false);
        }

        private void CloseResponse()
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }

            if (_response != null)
            {
                _response.Close();
                _response = null;
            }
        }

        /// <summary>
        ///     Copied from
        ///     http:
        ///     //social.msdn.microsoft.com/forums/en-US/netfxnetcom/thread/ff098248-551c-4da9-8ba5-358a9f8ccc57/
        /// </summary>
        private static bool SetAllowUnsafeHeaderParsing20()
        {
            //Get the assembly that contains the internal class
            Assembly aNetAssembly = Assembly.GetAssembly(typeof (SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                        BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null,
                        new object[] {});

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing",
                            BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}

#pragma warning restore 1591
