#pragma warning disable 1591

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using System.Threading;
using CSCore.ACM;
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
        private byte[] _frameBuffer;
        private WebResponse _response;
        private Stream _stream;
        private WaveFormat _waveFormat;

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
            do
            {
                read += _buffer.Read(buffer, offset + read, count - read);
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

                    success = resetEvent.WaitOne(1000);
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

                Mp3Frame frame = GetNextFrame(_stream);

                int channels = frame.ChannelMode == Mp3ChannelMode.Stereo ? 2 : 1;
                var converter =
                    new AcmConverter(new Mp3Format(frame.SampleRate, frame.ChannelCount, frame.FrameLength,
                        frame.BitRate));

                _waveFormat = converter.DestinationFormat;

                var buffer = new byte[16384 * 4];
                _buffer = new FixedSizeBuffer<byte>(converter.DestinationFormat.BytesPerSecond * 10);

                if (resetEvent != null) 
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
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            while (_bufferThread.ThreadState != ThreadState.Stopped)
                            {
                            }
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

                converter.Dispose();
            }
            finally
            {
                if (resetEvent != null)
                    resetEvent.Set();
            }
        }

        private Mp3Frame GetNextFrame(Stream stream)
        {
            Mp3Frame frame;
            do
            {
                frame = Mp3Frame.FromStream(stream, ref _frameBuffer);
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
