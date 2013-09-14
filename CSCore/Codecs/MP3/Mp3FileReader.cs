using CSCore.Tags.ID3;
using System;
using System.IO;

namespace CSCore.Codecs.MP3
{
    public class MP3FileReader : IDisposable
    {
        private long dataStartIndex;
        private long dataLength;
        private MP3Stream dataStream;

        public MP3FileReader(string fileName)
            : this(File.OpenRead(fileName))
        {
        }

        public MP3FileReader(Stream stream)
        {
            CSCore.Tags.ID3.ID3v2.SkipTag(stream);
            dataStartIndex = stream.Position;

            var id3v1Tag = ID3v1.FromStream(stream);
            if (id3v1Tag != null)
            {
                dataLength = stream.Length - dataStartIndex - 128; //128 = id3v1 length
            }
            else
            {
                dataLength = stream.Length - dataStartIndex;
            }

            stream.Position = dataStartIndex;

            dataStream = new MP3Stream(stream, true, id3v1Tag != null ? 128 : 0);
        }

        public MP3Stream DataStream
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

        ~MP3FileReader()
        {
            Dispose(false);
        }
    }
}