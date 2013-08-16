using CSCore.Tags.ID3.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Tags.ID3
{
    public class ID3v2QuickInfo
    {
        private ID3v2 _id3;

        public string Title
        {
            get
            {
                Frame f;
                if ((f = _id3[FrameID.Title]) != null)
                    return (f as TextFrame).Text;
                return String.Empty;
            }
        }

        public string Album
        {
            get
            {
                Frame f;
                if ((f = _id3[FrameID.Album]) != null)
                    return (f as TextFrame).Text;
                return String.Empty;
            }
        }

        public string Artist
        {
            get
            {
                Frame f;
                if ((f = _id3[FrameID.OriginalArtist]) != null)
                    return (f as TextFrame).Text;
                return String.Empty;
            }
        }

        public string LeadPerformers
        {
            get
            {
                Frame f;
                if ((f = _id3[FrameID.LeadPerformers]) != null)
                    return (f as TextFrame).Text;
                return String.Empty;
            }
        }

        public string Comments
        {
            get
            {
                Frame f;
                if ((f = _id3[FrameID.Comments]) != null)
                    return (f as CommentAndLyricsFrame).Text;
                return String.Empty;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                Frame f;
                if ((f = _id3[FrameID.AttachedPicutre]) != null)
                    return (f as PictureFrame).Image;
                return null;
            }
        }

        public int? Year
        {
            get
            {
                Frame f;
                int result;
                if ((f = _id3[FrameID.Year]) != null &&
                     Int32.TryParse((f as NumericTextFrame).Text, out result))
                    return result;
                return null;
            }
        }

        //Thanks to AliveDevil
        public int? TrackNumber
        {
            get
            {
                Frame f;
                int result;
                if ((f = _id3[FrameID.Year]) != null &&
                     Int32.TryParse((f as MultiStringTextFrame).Text, out result))
                    return result;
                return null;
            }
        }

        public int? OriginalReleaseYear
        {
            get
            {
                Frame f;
                if ((f = _id3[FrameID.OriginalReleaseYear]) != null)
                    return Int32.Parse((f as NumericTextFrame).Text);
                return null;
            }
        }

        public ID3Genre? Genre
        {
            get
            {
                MultiStringTextFrame f = _id3[FrameID.ContentType] as MultiStringTextFrame;
                if (f == null)
                    return null;

                var str = f.Text;
                if (String.IsNullOrEmpty(str) || !str.StartsWith("(") || str.Length < 3)
                {
                    try
                    {
                        return (ID3Genre)Enum.Parse(typeof(ID3Genre), str);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }

                char c;
                int i = 1;
                string sr = String.Empty;
                do
                {
                    c = str[i++];
                    if (Char.IsNumber(c))
                        sr += c;
                } while (i < str.Length && Char.IsNumber(c));

                int res = 0;
                if (Int32.TryParse(sr, out res))
                {
                    return (ID3Genre)res;
                }
                return null;
            }
        }

        public ID3v2QuickInfo(ID3v2 id3)
        {
            if (id3 == null)
                throw new ArgumentNullException("id3");
            _id3 = id3;
        }
    }
}