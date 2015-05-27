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
    /// <summary>
    /// An <see cref="IWaveSource"/> implementation for streaming mp3 streams like mp3 radio stations, etc. 
    /// </summary>
    public class Mp3WebStream : IWaveSource
    {
        private readonly Uri _address;
        private FixedSizeBuffer<byte> _buffer;

        private Thread _bufferThread;
        private DmoMp3Decoder _decoder;
        private bool _disposed;
        private bool _disposing;
        private WebResponse _response;
        private Stream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp3WebStream"/> class.
        /// </summary>
        /// <param name="address">The address of the mp3 stream.</param>
        public Mp3WebStream(string address)
            : this(address, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp3WebStream"/> class.
        /// </summary>
        /// <param name="address">The address of the mp3 stream.</param>
        /// <param name="async">If set to <c>true</c>, the connection will be established asynchronously and the constructor will return immediately.
        /// Doing that, requires the usage of the <see cref="ConnectionEstablished"/> event which will notify the caller when the <see cref="Mp3WebStream"/>
        /// is ready for use. If set to <c>false</c> the constructor will block the current thread as long as it takes to establish the connection. 
        /// </param>
        public Mp3WebStream(string address, bool async)
            : this(new Uri(address), async)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp3WebStream"/> class.
        /// </summary>
        /// <param name="address">The address of the mp3 stream.</param>
        public Mp3WebStream(Uri address) : this(address, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp3WebStream"/> class.
        /// </summary>
        /// <param name="address">The address of the mp3 stream.</param>
        /// <param name="async">If set to <c>true</c>, the connection will be established asynchronously and the constructor will return immediately.
        /// Doing that, requires the usage of the <see cref="ConnectionEstablished"/> event which will notify the caller when the <see cref="Mp3WebStream"/>
        /// is ready for use. If set to <c>false</c> the constructor will block the current thread as long as it takes to establish the connection. 
        /// </param>
        public Mp3WebStream(Uri address, bool async)
        {
            if (address == null)
                throw new ArgumentNullException("address");
            _address = address;

            if (!SetAllowUnsafeHeaderParsing20())
                throw new Exception("Setting allowed Unsafe-Header-Parsing failed.");

            CreateStream(async);
        }

        /// <summary>
        /// Gets the stream address.
        /// </summary>
        public Uri StreamAddress
        {
            get { return _address; }
        }

        /// <summary>
        /// Gets the number buffered bytes.
        /// </summary>
        public int BufferedBytes
        {
            get { return _buffer.Buffered; }
        }

        /// <summary>
        /// Gets the size of the internal buffer in bytes.
        /// </summary>
        public int BufferSize
        {
            get { return _buffer.Length; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Mp3WebStream"/> supports seeking.
        /// </summary>
        /// <remarks>This property will always be set to <c>false</c>.</remarks>
        public bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the <see cref="WaveFormat" /> of the decoded mp3 stream.
        /// If the internal decoder got not initialized yet, the value of the property is set to null.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _decoder == null ? null : _decoder.WaveFormat; }
        }

        /// <summary>
        /// Reads a sequence of elements from the <see cref="IReadableAudioSource{T}" /> and advances the position within the stream by the number of elements read.
        /// </summary>
        /// <param name="buffer">An array of elements. When this method returns, the <paramref name="buffer" /> contains the specified array of elements with the values between <paramref name="offset" /> and (<paramref name="offset" /> +     <paramref name="count" /> - 1) replaced by the elements read from the current source.</param>
        /// <param name="offset">The zero-based offset in the <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of elements to read from the current source.</param>
        /// <returns>
        /// The total number of elements read into the buffer.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">Mp3WebStream</exception>
        public int Read(byte[] buffer, int offset, int count)
        {
            if (_disposing)
                throw new ObjectDisposedException("Mp3WebStream");

            int read = 0;
            Array.Clear(buffer, offset, count - offset);

            if (_buffer.Buffered < _buffer.Length / 2)
                return count;
            do
            {
                read += _decoder.Read(buffer, offset + read, count - read);
            } while (read < count || _disposing);
            return read;
        }

        /// <summary>
        /// Gets or sets the current position. This property is not supported by the <see cref="Mp3WebStream" /> class.
        /// </summary>
        /// <exception cref="System.NotSupportedException">The Mp3WebStream class does not support seeking.</exception>
        public long Position
        {
            get { return 0; }
            set { throw new NotSupportedException("The Mp3WebStream class does not support seeking."); }
        }

        /// <summary>
        /// Gets the length of the waveform-audio data. The value of this property will always be set to zero.
        /// </summary>
        public long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Occurs when connection got established and the async argument of the constructor was set to <c>true</c>.
        /// </summary>
        public event EventHandler<ConnectionEstablishedEventArgs> ConnectionEstablished;

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
                if (ConnectionEstablished != null && async)
                    ConnectionEstablished(this, new ConnectionEstablishedEventArgs(_address, success));

                return success;
            };
            if (async)
                ThreadPool.QueueUserWorkItem(o => func());
            else
                func();
        }

        /// <summary>
        /// Initializes the connection.
        /// </summary>
        /// <returns><c>true</c> if the connection was initialized successfully; otherwise <c>false</c>.</returns>
        /// <exception cref="System.Net.WebException">
        /// Could not create HttpWebRequest
        /// or
        /// Could not create WebResponse
        /// </exception>
        protected virtual bool InitializeConnection()
        {
            try
            {
                var request = WebRequest.Create(StreamAddress) as HttpWebRequest;
                if (request == null)
                    throw new WebException("Could not create HttpWebRequest to {" + StreamAddress.AbsolutePath + "}");

                _response = request.GetResponse();
                if (_response == null)
                    throw new WebException("Could not create WebResponse to {" + StreamAddress.AbsolutePath + "}");

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
                int read;

                Mp3Format format, prevFormat;
                Mp3Frame frame;

                read = ReadRawDataFromFrame(ref buffer, out frame);
                format = new Mp3Format(frame.SampleRate, frame.ChannelCount, frame.FrameLength,
                    frame.BitRate);

                _buffer = new FixedSizeBuffer<byte>(format.BytesPerSecond * 2);

                do
                {
                    _buffer.Write(buffer, 0, read);
                    read = ReadRawDataFromFrame(ref buffer, out frame);
                    prevFormat = format;
                    format = new Mp3Format(frame.SampleRate, frame.ChannelCount, frame.FrameLength,
                        frame.BitRate);
                } while (_buffer.Buffered < _buffer.Length / 10 || !format.Equals(prevFormat));

                _decoder = new DmoMp3Decoder(new ReadBlockStream(_buffer.ToStream()), false);

                if (resetEvent != null)
                    resetEvent.Set();

                do
                {
                    if (_buffer.Buffered >= _buffer.Length * 0.85 && !_disposing)
                        Thread.Sleep(250);
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
            bool success = false;
            frame = null;
            do
            {
                try
                {
                    frame = GetNextFrame(_stream, ref buffer);
                    success = true;
                }
                catch (IOException)
                {
                }
                catch (WebException)
                {
                }

                if (!success)
                {
                    do
                    {
                        CloseResponse();
                        InitializeConnection();
                        if (_stream == null)
                            Thread.Sleep(100);
                    } while (_stream == null);
                }
            } while (!success);

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

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposing = true;
                if (_bufferThread != null &&
                    _bufferThread.ThreadState != ThreadState.Stopped &&
                    !_bufferThread.Join(500))
                    _bufferThread.Abort();
                if (_buffer != null)
                {
                    _buffer.Dispose();
                    _buffer = null;
                }
                if (_decoder != null)
                {
                    _decoder.Dispose();
                    _decoder = null;
                }
                CloseResponse();
            }
            _disposed = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Mp3WebStream"/> class.
        /// </summary>
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

        //Copied from http://social.msdn.microsoft.com/forums/en-US/netfxnetcom/thread/ff098248-551c-4da9-8ba5-358a9f8ccc57/
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