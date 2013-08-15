using CSCore.Tags.ID3;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CSCoreDemo.ViewModel
{
    public class TagsViewModel : ViewModelBase
    {
        public void LoadTags(string filename)
        {
            ID3v2 t2 = ID3v2.FromFile(filename);
            ID3v1 t1 = ID3v1.FromFile(filename);

            if (t2 != null)
            {
                Title = t2.QuickInfo.Title;
                Artist = t2.QuickInfo.Artist;
                Album = t2.QuickInfo.Album;
                Year = t2.QuickInfo.Year.ToString();
                LeadPerformers = t2.QuickInfo.LeadPerformers;
                Genre = t2.QuickInfo.Genre.ToString();
                TrackNumber = t2.QuickInfo.TrackNumber.ToString();
                Comments = t2.QuickInfo.Comments;

                if (t2.QuickInfo.Image != null)
                {
                    if (Image != null)
                    {
                        (Image as BitmapImage).StreamSource.Dispose();
                        Image = null;
                    }

                    MemoryStream imgstream = new MemoryStream();
                    t2.QuickInfo.Image.Save(imgstream, System.Drawing.Imaging.ImageFormat.Png);
                    var img = new BitmapImage();
                    img.BeginInit();
                    imgstream.Seek(0, SeekOrigin.Begin);
                    img.StreamSource = imgstream;
                    img.EndInit();
                    Image = img;
                }
            }
            if (t1 != null)
            {
                if (String.IsNullOrWhiteSpace(Title))
                    Title = t1.Title;
                if (String.IsNullOrWhiteSpace(Artist))
                    Artist = t1.Artist;
                if (String.IsNullOrWhiteSpace(Album))
                    Album = t1.Album;
                if (String.IsNullOrWhiteSpace(Year))
                    Year = t1.Year.ToString();
                if (string.IsNullOrWhiteSpace(Comments))
                    Comments = t1.Comment;
                Genre = t1.Genre.ToString();
            }
        }

        public void ResetTags()
        {
            var properties = GetType().GetProperties();
            foreach (var p in properties)
            {
                if (p.GetCustomAttribute(typeof(UIDisplayPropertyAttribute)) != null)
                    p.SetValue(this, p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null);
            }
        }

        private string _title;

        [UIDisplayProperty]
        public string Title
        {
            get { return _title; }
            set { SetProperty(value, ref _title, () => Title); }
        }

        private string _artist;

        [UIDisplayProperty]
        public string Artist
        {
            get { return _artist; }
            set { SetProperty(value, ref _artist, () => Artist); }
        }

        private string _album;

        [UIDisplayProperty]
        public string Album
        {
            get { return _album; }
            set { SetProperty(value, ref _album, () => Album); }
        }

        private string _leadperformers;

        [UIDisplayProperty]
        public string LeadPerformers
        {
            get { return _leadperformers; }
            set { SetProperty(value, ref _leadperformers, () => LeadPerformers); }
        }

        private string _year;

        [UIDisplayProperty]
        public string Year
        {
            get { return _year; }
            set { SetProperty(value, ref _year, () => Year); }
        }

        private string _trackNumber;

        [UIDisplayProperty]
        public string TrackNumber
        {
            get { return _trackNumber; }
            set { SetProperty(value, ref _trackNumber, () => TrackNumber); }
        }

        private string _genre;

        [UIDisplayProperty]
        public string Genre
        {
            get { return _genre; }
            set { SetProperty(value, ref _genre, () => Genre); }
        }

        private string _comment;

        [UIDisplayProperty]
        public string Comments
        {
            get { return _comment; }
            set { SetProperty(value, ref _comment, () => Comments); }
        }

        private ImageSource _imageSource;

        [UIDisplayProperty]
        public ImageSource Image
        {
            get { return _imageSource; }
            set { SetProperty(value, ref _imageSource, () => Image); }
        }
    }
}