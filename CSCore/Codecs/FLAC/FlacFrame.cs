#define GET_BUFFER_INTERNAL

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;

namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Represents a frame inside of an Flac-Stream.
    /// </summary>
    public sealed partial class FlacFrame : IDisposable
    {
        private List<FlacSubFrameData> _subFrameData;
        private ReadOnlyCollection<FlacSubFrameBase> _subFrames;  
        private Stream _stream;
        private FlacMetadataStreamInfo _streamInfo;

        private GCHandle _handle1, _handle2;
        private int[] _destBuffer;
        private int[] _residualBuffer;

        /// <summary>
        /// Gets the header of the flac frame.
        /// </summary>
        public FlacFrameHeader Header { get; private set; }

        /// <summary>
        /// Gets the CRC16-checksum.
        /// </summary>
        public short Crc16 { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the decoder has encountered an error with this frame.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this frame contains an error; otherwise, <c>false</c>.
        /// </value>
        public bool HasError { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="FlacFrame"/> class based on the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the flac frame.</param>
        /// <returns>A new instance of the <see cref="FlacFrame"/> class.</returns>
        public static FlacFrame FromStream(Stream stream)
        {
            FlacFrame frame = new FlacFrame(stream);
            return frame;
            //return frame.HasError ? null : frame;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FlacFrame"/> class based on the specified <paramref name="stream"/> and some basic stream information.
        /// </summary>
        /// <param name="stream">The stream which contains the flac frame.</param>
        /// <param name="streamInfo">Some basic information about the flac stream.</param>
        /// <returns>A new instance of the <see cref="FlacFrame"/> class.</returns>
        public static FlacFrame FromStream(Stream stream, FlacMetadataStreamInfo streamInfo)
        {
            FlacFrame frame = new FlacFrame(stream, streamInfo);
            return frame;
            //return frame.HasError ? null : frame;
        }

        private FlacFrame(Stream stream, FlacMetadataStreamInfo streamInfo = null)
        {
            if (stream == null) 
                throw new ArgumentNullException("stream");
            if (stream.CanRead == false) 
                throw new ArgumentException("Stream is not readable");

            _stream = stream;
            _streamInfo = streamInfo;
        }

        /// <summary>
        /// Tries to read the next flac frame inside of the specified stream and returns a value which indicates whether the next flac frame could be successfully read.
        /// </summary>
        /// <returns>True if the next flac frame could be successfully read; false if not.</returns>
        public bool NextFrame()
        {
            Decode(_stream, _streamInfo);
            return !HasError;
        }

        private void Decode(Stream stream, FlacMetadataStreamInfo streamInfo)
        {
            Header = new FlacFrameHeader(stream, streamInfo);
            _stream = stream;
            _streamInfo = streamInfo;
            HasError = Header.HasError;
            if (!HasError)
            {
                ReadSubFrames();
                FreeBuffers();
            }
        }

        private unsafe void ReadSubFrames()
        {
            List<FlacSubFrameBase> subFrames = new List<FlacSubFrameBase>();

            //alocateOutput
            var data = AllocOuputMemory();
            _subFrameData = data;

            byte[] buffer = new byte[0x20000];
            if ((_streamInfo.MaxFrameSize * Header.Channels * Header.BitsPerSample * 2 >> 3) > buffer.Length)
            {
                buffer = new byte[(_streamInfo.MaxFrameSize * Header.Channels * Header.BitsPerSample * 2 >> 3) - FlacConstant.FrameHeaderSize];
            }

            int read = _stream.Read(buffer, 0, (int)Math.Min(buffer.Length, _stream.Length - _stream.Position));

            fixed (byte* ptrBuffer = buffer)
            {
                FlacBitReader reader = new FlacBitReader(ptrBuffer, 0);
                for (int c = 0; c < Header.Channels; c++)
                {
                    int bitsPerSample = Header.BitsPerSample;
                    if (Header.ChannelAssignment == ChannelAssignment.MidSide || Header.ChannelAssignment == ChannelAssignment.LeftSide)
                        bitsPerSample += c;
                    else if (Header.ChannelAssignment == ChannelAssignment.RightSide)
                        bitsPerSample += 1 - c;

                    var subframe = FlacSubFrameBase.GetSubFrame(reader, data[c], Header, bitsPerSample);
                    subFrames.Add(subframe);
                }

                reader.Flush(); //Zero-padding to byte alignment.

                //footer
                Crc16 = (short) reader.ReadBits(16);

                _stream.Position -= read - reader.Position;
                MapToChannels(_subFrameData);
            }

#if FLAC_DEBUG
            _subFrames = subFrames.AsReadOnly();
#endif
        }

        private unsafe void MapToChannels(List<FlacSubFrameData> subFrames)
        {
            if (Header.ChannelAssignment == ChannelAssignment.LeftSide)
            {
                for (int i = 0; i < Header.BlockSize; i++)
                {
                    subFrames[1].DestinationBuffer[i] = subFrames[0].DestinationBuffer[i] - subFrames[1].DestinationBuffer[i];
                }
            }
            else if (Header.ChannelAssignment == ChannelAssignment.RightSide)
            {
                for (int i = 0; i < Header.BlockSize; i++)
                {
                    subFrames[0].DestinationBuffer[i] += subFrames[1].DestinationBuffer[i];
                }
            }
            else if (Header.ChannelAssignment == ChannelAssignment.MidSide)
            {
                for (int i = 0; i < Header.BlockSize; i++)
                {
                    int mid = subFrames[0].DestinationBuffer[i] << 1;
                    int side = subFrames[1].DestinationBuffer[i];

                    mid |= (side & 1);

                    subFrames[0].DestinationBuffer[i] = (mid + side) >> 1;
                    subFrames[1].DestinationBuffer[i] = (mid - side) >> 1;
                }
            }
        }

        /// <summary>
        /// Gets the raw pcm data of the flac frame.
        /// </summary>
        /// <param name="buffer">The buffer which should be used to store the data in. This value can be null.</param>
        /// <returns>The number of read bytes.</returns>
        public
#if FLAC_DEBUG && !GET_BUFFER_INTERNAL
            unsafe 
#endif
 int GetBuffer(ref byte[] buffer)
        {
#if !FLAC_DEBUG || GET_BUFFER_INTERNAL
            return GetBufferInternal(ref buffer);
#else 
            int desiredsize = Header.BlockSize * Header.Channels * ((Header.BitsPerSample + 7) / 2);
            if (buffer == null || buffer.Length < desiredsize)
                buffer = new byte[desiredsize];

            fixed (byte* ptrBuffer = buffer)
            {
                byte* ptr = ptrBuffer;
                if (Header.BitsPerSample == 8)
                {
                    for (int i = 0; i < Header.BlockSize; i++)
                    {
                        for (int c = 0; c < Header.Channels; c++)
                        {
                            *(ptr++) = (byte)(_subFrames[c].DestinationBuffer[i] + 0x80);
                        }
                    }
                }
                else if (Header.BitsPerSample == 16)
                {
                    for (int i = 0; i < Header.BlockSize; i++)
                    {
                        for (int c = 0; c < Header.Channels; c++)
                        {
                            short val = (short)(_subFrames[c].DestinationBuffer[i]);
                            *(ptr++) = (byte)(val & 0xFF);
                            *(ptr++) = (byte)((val >> 8) & 0xFF);
                        }
                    }
                }
                else if (Header.BitsPerSample == 24)
                {
                    for (int i = 0; i < Header.BlockSize; i++)
                    {
                        for (int c = 0; c < Header.Channels; c++)
                        {
                            int val = (_subFrames[c].DestinationBuffer[i]);
                            *(ptr++) = (byte)(val & 0xFF);
                            *(ptr++) = (byte)((val >> 8) & 0xFF);
                            *(ptr++) = (byte)((val >> 16) & 0xFF);
                        }
                    }
                }
                else
                {
                    throw new FlacException(String.Format("FlacFrame::GetBuffer: Invalid BitsPerSample value: {0}", Header.BitsPerSample), FlacLayer.Frame);
                }

                return (int)(ptr - ptrBuffer);
            }
#endif
        }

        private unsafe List<FlacSubFrameData> AllocOuputMemory()
        {
            if (_destBuffer == null || _destBuffer.Length < (Header.Channels * Header.BlockSize))
                _destBuffer = new int[Header.Channels * Header.BlockSize];
            if (_residualBuffer == null || _residualBuffer.Length < (Header.Channels * Header.BlockSize))
                _residualBuffer = new int[Header.Channels * Header.BlockSize];

            List<FlacSubFrameData> output = new List<FlacSubFrameData>();

            for (int c = 0; c < Header.Channels; c++)
            {
                fixed (int* ptrDestBuffer = _destBuffer, ptrResidualBuffer = _residualBuffer)
                {
                    _handle1 = GCHandle.Alloc(_destBuffer, GCHandleType.Pinned);
                    _handle2 = GCHandle.Alloc(_residualBuffer, GCHandleType.Pinned);

                    FlacSubFrameData data = new FlacSubFrameData
                    {
                        DestinationBuffer = (ptrDestBuffer + c * Header.BlockSize),
                        ResidualBuffer = (ptrResidualBuffer + c * Header.BlockSize)
                    };
                    output.Add(data);
                }
            }

            return output;
        }

        private void FreeBuffers()
        {
            if (_handle1.IsAllocated)
                _handle1.Free();
            if (_handle2.IsAllocated)
                _handle2.Free();
        }

        private bool _disposed;

        /// <summary>
        /// Disposes the <see cref="FlacFrame"/> and releases all associated resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                GC.SuppressFinalize(this);
                FreeBuffers();
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FlacFrame"/> class.
        /// </summary>
        ~FlacFrame()
        {
            Dispose();
        }
    }
}