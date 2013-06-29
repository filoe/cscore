using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore.Tags.ID3.Frames;

namespace CSCore.Tags.ID3
{
    public class ID3v2QuickInfo
    {
        ID3v2 _id3;

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

        public string Year
        {
            get
            {
                Frame f;
                if ((f = _id3[FrameID.Year]) != null)
                    return (f as NumericTextFrame).Text;
                return String.Empty;
            }
        }

        public string OriginalReleaseYear
        {
            get
            {
                Frame f;
                if ((f = _id3[FrameID.OriginalReleaseYear]) != null)
                    return (f as NumericTextFrame).Text;
                return String.Empty;
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
