using CSCore.ACM;
using CSCore.Utils.Buffer;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace CSCore.Codecs.MP3
{
    public class MP3WebStream : IWaveSource
    {
        private Stream _stream;
        private WebResponse _response;

        private FixedSizeBuffer<byte> _buffer;
        private byte[] _frameBuffer;

        private bool _disposing = false;

        private Thread _bufferThread;

        public event EventHandler<ConnectionEstablishedEventArgs> ConnectionCreated;

        private Uri _uri;

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

        public MP3WebStream(string uri, bool async)
            : this(new Uri(uri), async)
        {
        }

        public MP3WebStream(Uri uri, bool async)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");
            _uri = uri;

            if (!SetAllowUnsafeHeaderParsing20())
                throw new Exception("Setting allowed Unsafe-Header-Parsing failed.");

            CreateStream(async);
        }

        private void CreateStream(bool async)
        {
            _disposing = false;

            Func<bool> action = () =>
                {
                    ManualResetEvent resetEvent = new ManualResetEvent(false);

                    bool success = InitializeConnection();
                    if (success)
                    {
                        _bufferThread = new Thread(new ParameterizedThreadStart(BufferProc));
                        _bufferThread.Start(resetEvent);

                        success = resetEvent.WaitOne(1000);
                    }
                    if (ConnectionCreated != null && async)
                        ConnectionCreated(this, new ConnectionEstablishedEventArgs(_uri, success));

                    return success;
                };
            if (async)
                ThreadPool.QueueUserWorkItem(new WaitCallback((o) => action()));
            else
                action.Invoke();
        }

        private bool InitializeConnection()
        {
            try
            {
                WebRequest.DefaultWebProxy = null;
                HttpWebRequest request = HttpWebRequest.Create(StreamUri) as HttpWebRequest;
                if (request == null) throw new WebException("Could not create HttpWebRequest to {" + StreamUri.AbsolutePath + "}");

                _response = request.GetResponse();
                if (_response == null) throw new WebException("Could not create WebResponse to {" + StreamUri.AbsolutePath + "}");

                var responseStream = _response.GetResponseStream();
                responseStream.ReadTimeout = 1500;
                _stream = new Utils.ReadBlockStream(responseStream);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Mp3WebStream::InitializeConnection: " + ex.ToString());
                return false;
            }
        }

        private void BufferProc(object o)
        {
            if (_stream == null || _stream.CanRead == false) 
                throw new Exception("Mp3WebStream not initialized");

            MP3Frame frame = GetNextFrame(_stream);

            int channels = frame.ChannelMode == MP3ChannelMode.Stereo ? 2 : 1;
            AcmConverter converter = new AcmConverter(new MP3Format(frame.SampleRate, frame.ChannelCount, frame.FrameLength, frame.BitRate));

            _waveFormat = converter.DestinationFormat;

            byte[] buffer = new byte[16384 * 4];
            _buffer = new FixedSizeBuffer<byte>(converter.DestinationFormat.BytesPerSecond * 10);

            ManualResetEvent resetEvent = o as ManualResetEvent;
            resetEvent.Set();

            do
            {
                if (_buffer.Buffered >= _buffer.Length * 0.85 && !_disposing)
                {
                    Thread.Sleep(250);
                    continue;
                }
                try
                {
                    frame = GetNextFrame(_stream);
                    //_frameBuffer is set in GetNextFrame
                    int count = converter.Convert(_frameBuffer, frame.FrameLength, buffer, 0);
                    if (count > 0)
                    {
                        int written = _buffer.Write(buffer, 0, count);
                    }
                }
                catch (MmException)
                {
                    _disposing = true;
                    ThreadPool.QueueUserWorkItem((c) =>
                    {
                        while (_bufferThread.ThreadState != System.Threading.ThreadState.Stopped) ;
                        CreateStream(false);
                    });
                }
                catch (WebException)
                {
                    InitializeConnection();
                }
                catch (IOException)
                {
                    InitializeConnection();
                }
            } while (!_disposing);

            if (converter != null)
                converter.Dispose();
        }

        private MP3Frame GetNextFrame(Stream stream)
        {
            MP3Frame frame;
            do
            {
                frame = MP3Frame.FromStream(stream, ref _frameBuffer);
            } while (frame == null);

            return frame;
        }

        private WaveFormat _waveFormat;

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;
            Array.Clear(buffer, offset, count - offset);

            if (_buffer.Buffered < _buffer.Length / 2)
                return count;
            else
            {
                do
                {
                    read += _buffer.Read(buffer, offset + read, count - read);
                } while (read < count);
                return read;
            }
        }

        public long Position
        {
            get
            {
                return 0;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public long Length
        {
            get { return 0; }
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
                _disposing = true;
                if (_bufferThread != null &&
                    _bufferThread.ThreadState != System.Threading.ThreadState.Stopped &&
                    !_bufferThread.Join(500))
                    _bufferThread.Abort();
                CloseResponse();
            }
            _disposed = true;
        }

        ~MP3WebStream()
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
        /// Copied from
        /// http:
        ///       //social.msdn.microsoft.com/forums/en-US/netfxnetcom/thread/ff098248-551c-4da9-8ba5-358a9f8ccc57/
        /// </summary>
        private static bool SetAllowUnsafeHeaderParsing20()
        {
            //Get the assembly that contains the internal class
            Assembly aNetAssembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                      BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
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