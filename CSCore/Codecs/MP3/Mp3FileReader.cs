using CSCore.Tags.ID3;
using System;
using System.IO;

namespace CSCore.Codecs.MP3
{
    public class Mp3FileReader : IDisposable
    {
        long dataStartIndex;
        long dataLength;
        Mp3Stream dataStream;

        public Mp3FileReader(string fileName) : this(File.OpenRead(fileName))
        {
        }
        public Mp3FileReader(Stream stream)
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

            dataStream = new Mp3Stream(stream, true, id3v1Tag != null?128:0);
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
