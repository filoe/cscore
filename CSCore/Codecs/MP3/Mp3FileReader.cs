#pragma warning disable 1591 //since this class is obsolete

using CSCore.Tags.ID3;
using System;
using System.IO;

namespace CSCore.Codecs.MP3
{
    [Obsolete("Use the DmoMp3Decoder or the MP3MediafoundationDecoder instead.")]
    public class Mp3FileReader : IDisposable
    {
        private long _dataLength;
        private readonly Mp3Stream dataStream;

        public Mp3FileReader(string fileName)
            : this(File.OpenRead(fileName))
        {
        }

        public Mp3FileReader(Stream stream)
        {
            ID3v2.SkipTag(stream);
            long dataStartIndex = stream.Position;

            var id3v1Tag = ID3v1.FromStream(stream);
            if (id3v1Tag != null)
            {
                _dataLength = stream.Length - dataStartIndex - 128; //128 = id3v1 length
            }
            else
            {
                _dataLength = stream.Length - dataStartIndex;
            }

            stream.Position = dataStartIndex;

            dataStream = new Mp3Stream(stream, true, id3v1Tag != null ? 128 : 0);
        }

        public Mp3Stream DataStream
        {
            get { return dataStream; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing)
                {
                    //dispose managed
                    dataStream.Dispose();
                }
            }
        }

        ~Mp3FileReader()
        {
            Dispose(false);
        }
    }
}

#pragma warning restore 1591
