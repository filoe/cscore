using System;
using System.IO;
using System.Linq;

namespace CSCore.Tags.ID3
{
    //http://id3.org/ID3v1
    public class ID3v1
    {
        public static ID3v1 FromFile(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                return FromStream(stream);
            }
        }

        public static ID3v1 FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("stream is not readable");

            long? pos = null;
            if (stream.CanSeek)
            {
                pos = stream.Position;
                stream.Position = stream.Length - 128;
            }

            ID3v1 tag = null;
            BinaryReader reader = new BinaryReader(stream);
            if (reader.ReadByte() == 0x54 && reader.ReadByte() == 0x41 && reader.ReadByte() == 0x47)
            {
                tag = new ID3v1(stream);
            }

            if (pos != null)
                stream.Position = pos.Value;

            return tag;
        }

        public static ID3v1 CreateEmpty()
        {
            return new ID3v1();
        }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public int? Year { get; set; }

        public string Comment { get; set; }

        public ID3Genre Genre { get; set; }

        private ID3v1() { }

        private ID3v1(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            Title = new string(reader.ReadChars(30)).Replace("\0", String.Empty).TrimEnd();
            Artist = new string(reader.ReadChars(30)).Replace("\0", String.Empty).TrimEnd();
            Album = new string(reader.ReadChars(30)).Replace("\0", String.Empty).TrimEnd();
            int year;
            bool parseResult = Int32.TryParse(new string(reader.ReadChars(4)), out year);
            if (parseResult)
                Year = year;
            else
                Year = null;
            Comment = new string(reader.ReadChars(30)).Replace("\0", String.Empty).TrimEnd();
            Genre = (ID3Genre)reader.ReadByte();
        }

        public void SaveToStream(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            var title = Title.Length > 30 ? Title.Substring(0, 30) : Title;
            var artist = Artist.Length > 30 ? Title.Substring(0, 30) : Artist;
            var album = Album.Length > 30 ? Album.Substring(0, 30) : Album;
            int year = Year.HasValue ? Year.Value : 0;
            var comment = Comment.Length > 30 ? Comment.Substring(0, 30) : Comment;
            var genre = (byte)Genre;

            writer.Write(title);
            writer.Write(artist);
            writer.Write(album);
            writer.Write(year);
            writer.Write(comment);
            writer.Write(genre);
            writer.Flush();
        }
    }
}