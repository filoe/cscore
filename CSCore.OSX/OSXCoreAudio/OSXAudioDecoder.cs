using System;
using System.Runtime.InteropServices;
using MonoMac.AudioToolbox;
using MonoMac.AudioUnit;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;


namespace CSCore.OSXCoreAudio
{
    /// <summary>
    ///     The <see cref="OSXAudioDecoder"/> is a generic decoder for all formats 
    ///     supported by the CoreAudio API on OSX
    /// </summary>
    public class OSXAudioDecoder : IWaveSource
    {
        private WaveFormat _waveFormat;

        private ExtAudioFile _audioFileReader;
        private AudioStreamSource _audioStreamSource;
        private AudioBuffers _fillBuffers;
        private IntPtr _audioBufferMemory;
        private AudioStreamBasicDescription _destinationFormat;
        private AudioStreamBasicDescription _clientFormat;
        private AudioStreamBasicDescription _inputFormat;

        private int _headerFrames = 0;
        private int _bufferSize;
        private long _position;
        private long _totalFrameCount;

        private bool _disposed = false;
        private object _lockObj = new object();

        /// <summary>
        ///     Initializes a new instance of the <see cref="OSXAudioDecoder"/> class.
        /// </summary>
        /// <param name="url">url which points to an audio source which can be decoded.</param>
        public OSXAudioDecoder(string url)
        {
            if (String.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            _audioFileReader = Initialize(url);

            //Seek to position 0, which will skip over all header frames if they exist,
            //to the first sample of audio
            SetPosition(0);        
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:CSCore.OSXCoreAudio.OSXAudioDecoder"/> class.
        ///     Note that this constructor downloads the entire stream from the URI into a memory stream
        ///     This is needed for the Core Audio decoder
        /// </summary>
        /// <param name="uri">URI which points to an audio source which can be decoded.</param>
        /// <param name="fileType">The filetype of the audio source</param>
        public OSXAudioDecoder(Uri uri, AudioFileType fileType = AudioFileType.MP3)
        {
            if (String.IsNullOrEmpty(uri.AbsolutePath))
                throw new ArgumentNullException(nameof(uri));

            //get the stream     

            MemoryStream memoryStream = new MemoryStream();
            WebRequest req = WebRequest.Create(uri);

            using (Stream rStream = req.GetResponse().GetResponseStream())
            {
                rStream.CopyTo(memoryStream);
            }

            _audioFileReader = Initialize(memoryStream, fileType);

            //Seek to position 0, which will skip over all header frames if they exist,
            //to the first sample of audio
            SetPosition(0);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OSXAudioDecoder" /> class.
        /// </summary>
        /// <param name="stream">Stream which provides the audio data to decode.</param>
        /// <param name="fileType">The filetype of the stream. Defaults to MP3</param>
        public OSXAudioDecoder(Stream stream, AudioFileType fileType = AudioFileType.MP3)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", nameof(stream));

            _audioFileReader = Initialize(stream, fileType);

            //Seek to position 0, which will skip over all header frames if they exist,
            //to the first sample of audio
            SetPosition(0);
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="OSXAudioDecoder"/>  and advances the position
        ///     within the stream by the number of bytes read
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes. When this method resturns, the <paramref name="buffer"/> contains the specified
        ///     byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1)
        ///     replaced by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based byte offset in the <paramref name="buffer"/> at which to begin storing
        ///     the data read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the current source.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            CheckForDisposed();

            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length < count)
                throw new ArgumentException("Length is too small.", "buffer");

            lock (_lockObj)
            {
                int read = 0;
                if (_audioFileReader == null || _disposed)
                    return read;

                //align count to block boundary
                count -= count % _waveFormat.BlockAlign;

                while (read < count)
                {
                    //get how much we are reading - clamp to maximum buffer size - possibly not needed?
                    int bytesToRead = Math.Min(count - read, _bufferSize);
                    int framesToRead = bytesToRead / _clientFormat.BytesPerFrame; // we shouldn't have to do any error checking on this division

                    ExtAudioFileError status;

                    // we cast to/from uint here because the coreaudio api requires it
                    // this shouldn't be an issue, because framesToRead is an int, so
                    // actualFramesRead should be less than max(int)
                    int actualFramesRead = (int)_audioFileReader.Read((uint)framesToRead, _fillBuffers, out status); 

                    //Write method status to debug
                    Debug.WriteLine(status.ToString());

                    //break if we read no frames (reached end of file or something)
                    if (actualFramesRead == 0)
                        break;

                    //Convert frames read to bytes read
                    int actualBytesRead = actualFramesRead * _clientFormat.BytesPerFrame;

                    //copy the bytes read from audiobuffer to destination buffer
                    Marshal.Copy(_audioBufferMemory, buffer, offset + read, actualBytesRead);

                    //update read count
                    read += actualBytesRead;
                }

                //don't forget to update position
                //position is in frames(!!), not bytes
                _position += read / _clientFormat.BytesPerFrame;

                return read;
            }
        }

        private ExtAudioFile Initialize(Stream stream, AudioFileType fileType)
        {
            //Load audio stream source (child of AudioFile)
            _audioStreamSource = new AudioStreamSource(stream, fileType);

            //Wrap AudioStreamSource in extAudioFile object
            ExtAudioFile extAudioFile;
            ExtAudioFile.WrapAudioFileID(_audioStreamSource.Handle, false, out extAudioFile);

            //Initialize
            return Initialize(extAudioFile);
        }

        private ExtAudioFile Initialize(string url)
        {
            //Load the audio file
            ExtAudioFile extAudioFile = ExtAudioFile.OpenUrl(MonoMac.CoreFoundation.CFUrl.FromFile(url));

            //Initialize
            return Initialize(extAudioFile);

        }

        ExtAudioFile Initialize(ExtAudioFile extAudioFile)
        {
            try
            {
                //This is the file input format
                _inputFormat = extAudioFile.FileDataFormat;

                //Create the destination format - use WAV LPCM, 2 channels, input sample rate
                _destinationFormat = AudioStreamBasicDescription.CreateLinearPCM(sampleRate: _inputFormat.SampleRate);

                //Unset the integer flag and set the float flag
                //Integer flag is set from the constructor above
                _destinationFormat.FormatFlags |= AudioFormatFlags.IsFloat;
                if (_destinationFormat.FormatFlags.HasFlag(AudioFormatFlags.IsSignedInteger))
                    _destinationFormat.FormatFlags |= AudioFormatFlags.IsSignedInteger;

                //Create the client format (what audio is converted too)
                //Want 32bit float
                _clientFormat = _destinationFormat;
                _clientFormat.ChannelsPerFrame = 2;
                _clientFormat.BytesPerFrame = sizeof(float) * _clientFormat.ChannelsPerFrame;
                _clientFormat.BitsPerChannel = sizeof(float) * 8;
                _clientFormat.FramesPerPacket = 1;
                _clientFormat.BytesPerPacket = _clientFormat.BytesPerFrame * _clientFormat.FramesPerPacket;
                _clientFormat.Reserved = 0;

                //Set the client data format
                extAudioFile.ClientDataFormat = _clientFormat;

                //Set the wave format
                _waveFormat = new WaveFormat((int)_clientFormat.SampleRate, _clientFormat.BitsPerChannel, _clientFormat.ChannelsPerFrame, AudioEncoding.IeeeFloat);

                //Get the total length in frames of the audio file
                _totalFrameCount = extAudioFile.FileLengthFrames;

                //
                // WORKAROUND for a bug in ExtFileAudio
                // Taken from https://github.com/asantoni/libaudiodecoder/blob/master/src/audiodecodercoreaudio.cpp
                //
                // Not sure if this is still needed?
                //

                AudioConverter ac = extAudioFile.AudioConverter;

                //throws an argument exception if this does not exist
                //will only exist if we are decompressing
                try
                {
                    AudioConverterPrimeInfo primeInfo = ac.PrimeInfo;
                    _headerFrames = ac.PrimeInfo.LeadingFrames;
                }
                catch (ArgumentException)
                {
                }

                Debug.WriteLine("Initial audio file position is: " + extAudioFile.FileTell());
                Debug.WriteLine("Header frames are: " + _headerFrames);


                //create audio buffers
                _fillBuffers = new AudioBuffers(1);

                const int averageFramesRead = 2048;
                _bufferSize = averageFramesRead * _clientFormat.BytesPerFrame;
                _audioBufferMemory = Marshal.AllocHGlobal(_bufferSize);

                _fillBuffers.SetData(0, _audioBufferMemory, _bufferSize);

                //We can't just set this directly, because of .net property quirks
                AudioBuffer buf = _fillBuffers[0];
                buf.NumberChannels = _clientFormat.ChannelsPerFrame;
                _fillBuffers[0] = buf;

                return extAudioFile;
            }
            catch (Exception)
            {
                DisposeInternal();

                throw;
            }
        }

        /// <summary>
        ///     Gets a value which indicates whether the seeking is supported. True means that seeking is supported.
        ///     False means that seeking is not supported.
        ///     Currently always returns true, because CoreAudio can always seek (?). Need to double check this is correct.
        /// </summary>
        public bool CanSeek
        {
            get
            {
                //CoreAudio can always seek?
                return true;
            }
        }

        /// <summary>
        ///     Gets the format of the decoded audio data provided by the <see cref="Read"/> method. 
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        ///     Gets or sets the position of the output stream, in bytes.
        /// </summary>
        public long Position
        {
            get { return !_disposed ? _position*_clientFormat.BytesPerFrame : 0; }
            set
            {
                CheckForDisposed();
                SetPosition(value);
            }
        }

        /// <summary>
        ///     Disposes the <see cref="OSXAudioDecoder" />.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        private void SetPosition(long position)
        {
            if (CanSeek)
            {
                lock (_lockObj)
                {
                    position -= position % _waveFormat.BlockAlign; 

                    //need seek position in frames
                    long seek = position / _clientFormat.BytesPerFrame; // shouldn't need to do any error checking on this division

                    _audioFileReader.Seek(seek+_headerFrames);
                    var new_pos = _audioFileReader.FileTell();
                    if (new_pos + _headerFrames != seek)
                        throw new Exception("Setting OSXAudioDecoder position failed!");

                    _position = seek;
                }
            }
        }

        /// <summary>
        ///     Gets the total length of the decoded audio, in bytes.
        /// </summary>
        public long Length
        {
            get
            {
                if (_disposed)
                    return 0;
                return _totalFrameCount * _clientFormat.BytesPerFrame;
            }
        }

        private void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }


        /// <summary>
        ///     Disposes the <see cref="OSXAudioDecoder"/> and its internal resources. 
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources;
        ///     false to release only managed resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            lock (_lockObj)
            {
                DisposeInternal();
            }
        }

        private void DisposeInternal()
        {
            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }
            if (_audioStreamSource != null)
            {
                if (_audioStreamSource.AudioStream != null)
                    _audioStreamSource.AudioStream.Dispose();
                _audioStreamSource = null;
            }
            if (_fillBuffers != null)
            {
                for (int i = 0; i < _fillBuffers.Count; i++)
                {
                    AudioBuffer buf = _fillBuffers[i];
                    Marshal.FreeHGlobal(buf.Data);
                }
                _fillBuffers.Dispose();
                _fillBuffers = null;
            }
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="OSXAudioDecoder" /> class.
        /// </summary>
        ~OSXAudioDecoder()
        {
            Dispose(false);
        }
    }
}

