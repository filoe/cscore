using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace CSCore.Codecs.FLAC
{
    public sealed class FlacFrame
    {
        private List<FlacSubFrameData> _data;
        private Stream _stream;
        private FlacMetadataStreamInfo _streamInfo;

        private GCHandle handle1, handle2;
        private int[] destBuffer;
        private int[] residualBuffer;

        private FlacFrameHeader _header;

        public FlacFrameHeader Header
        {
            get { return _header; }
            private set { _header = value; }
        }

        public ushort CRC16 { get; private set; }

        public bool HasError { get; private set; }

        public static FlacFrame FromStream(Stream stream)
        {
            FlacFrame frame = new FlacFrame(stream);
            return frame;
            //return frame.HasError ? null : frame;
        }

        public static FlacFrame FromStream(Stream stream, FlacMetadataStreamInfo streamInfo)
        {
            FlacFrame frame = new FlacFrame(stream, streamInfo);
            return frame;
            //return frame.HasError ? null : frame;
        }

        private FlacFrame(Stream stream)
            : this(stream, null)
        {
        }

        private FlacFrame(Stream stream, FlacMetadataStreamInfo streamInfo)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (stream.CanRead == false) throw new ArgumentException("Stream is not readable");

            _stream = stream;
            _streamInfo = streamInfo;
        }

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

        private unsafe int ReadSubFrames()
        {
            List<FlacSubFrameBase> subFrames = new List<FlacSubFrameBase>();

            //alocateOutput
            var data = AllocOuputMemory();
            _data = data;

            byte[] buffer = new byte[0x20000];
            if ((_streamInfo.MaxFrameSize * Header.Channels * Header.BitsPerSample * 2 >> 3) > buffer.Length)
            {
                buffer = new byte[(_streamInfo.MaxFrameSize * Header.Channels * Header.BitsPerSample * 2 >> 3) - FlacConstant.FrameHeaderSize];
            }

            int read = _stream.Read(buffer, 0, (int)Math.Min(buffer.Length, _stream.Length - _stream.Position));

            fixed (byte* ptrBuffer = buffer)
            {
                FlacBitReader reader = new FlacBitReader(ptrBuffer, 0);
                ChannelAssignment channelAssignment = Header.ChannelAssignment;
                for (int c = 0; c < Header.Channels; c++)
                {
                    int bps = Header.BitsPerSample;
                    if (Header.ChannelAssignment == ChannelAssignment.MidSide || Header.ChannelAssignment == ChannelAssignment.LeftSide)
                        bps += c;
                    else if (Header.ChannelAssignment == ChannelAssignment.RightSide)
                        bps += 1 - c;

                    var subframe = FlacSubFrameBase.GetSubFrame(reader, data[c], Header, bps);

                    subFrames.Add(subframe);
                }

                reader.Flush();
                CRC16 = (ushort)reader.ReadBits(16);

                _stream.Position -= read - reader.Position;

                SamplesToBytes(_data);

                return reader.Position;
            }
        }

        private unsafe void SamplesToBytes(List<FlacSubFrameData> data)
        {
            if (Header.ChannelAssignment == ChannelAssignment.LeftSide)
            {
                for (int i = 0; i < Header.BlockSize; i++)
                {
                    data[1].destBuffer[i] = data[0].destBuffer[i] - data[1].destBuffer[i];
                }
            }
            else if (Header.ChannelAssignment == ChannelAssignment.RightSide)
            {
                for (int i = 0; i < Header.BlockSize; i++)
                {
                    data[0].destBuffer[i] += data[1].destBuffer[i];
                }
            }
            else if (Header.ChannelAssignment == ChannelAssignment.MidSide)
            {
                int mid, side;
                for (int i = 0; i < Header.BlockSize; i++)
                {
                    mid = data[0].destBuffer[i] << 1;
                    side = data[1].destBuffer[i];

                    mid |= (side & 1);

                    data[0].destBuffer[i] = (mid + side) >> 1;
                    data[1].destBuffer[i] = (mid - side) >> 1;
                }
            }
        }

        public unsafe int GetBuffer(ref byte[] buffer, int offset)
        {
            //int desiredsize = Header.BlockSize * Header.Channels * Header.BitsPerSample;
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
                            *(ptr++) = (byte)(_data[c].destBuffer[i] + 0x80);
                        }
                    }
                }
                else if (Header.BitsPerSample == 16)
                {
                    for (int i = 0; i < Header.BlockSize; i++)
                    {
                        for (int c = 0; c < Header.Channels; c++)
                        {
                            short val = (short)(_data[c].destBuffer[i]); //remove
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
                            int val = (_data[c].destBuffer[i]);
                            *(ptr++) = (byte)(val & 0xFF);
                            *(ptr++) = (byte)((val >> 8) & 0xFF);
                            *(ptr++) = (byte)((val >> 16) & 0xFF);
                        }
                    }
                }
                else
                {
                    string error = "FlacFrame::GetBuffer: Invalid Flac-BitsPerSample: " + Header.BitsPerSample.ToString() + ".";
                    Debug.WriteLine(error);
                    throw new FlacException(error, FlacLayer.Frame);
                }

                return (int)(ptr - ptrBuffer);
            }
        }

        private unsafe List<FlacSubFrameData> AllocOuputMemory()
        {
            if (destBuffer == null || destBuffer.Length < (Header.Channels * Header.BlockSize))
                destBuffer = new int[Header.Channels * Header.BlockSize];
            if (residualBuffer == null || residualBuffer.Length < (Header.Channels * Header.BlockSize))
                residualBuffer = new int[Header.Channels * Header.BlockSize];

            List<FlacSubFrameData> output = new List<FlacSubFrameData>();

            for (int c = 0; c < Header.Channels; c++)
            {
                fixed (int* ptrDestBuffer = destBuffer, ptrResidualBuffer = residualBuffer)
                {
                    handle1 = GCHandle.Alloc(destBuffer, GCHandleType.Pinned);
                    handle2 = GCHandle.Alloc(residualBuffer, GCHandleType.Pinned);

                    FlacSubFrameData data = new FlacSubFrameData();
                    data.destBuffer = (ptrDestBuffer + c * Header.BlockSize);
                    data.residualBuffer = (ptrResidualBuffer + c * Header.BlockSize);
                    output.Add(data);
                }
            }

            return output;
        }

        public void FreeBuffers()
        {
            if (handle1 != null && handle1.IsAllocated)
                handle1.Free();
            if (handle2 != null && handle2.IsAllocated)
                handle2.Free();
        }
    }

    public unsafe class FlacSubFrameData
    {
        public int* destBuffer;
        public int* residualBuffer;

        private FlacPartitionedRiceContent _content;

        public FlacPartitionedRiceContent Content
        {
            get { return _content ?? (_content = new FlacPartitionedRiceContent()); }
            set { _content = value; }
        }

        public FlacSubFrameData()
        {
        }
    }
}