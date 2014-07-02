using System;
using System.Collections.ObjectModel;
using System.IO;

namespace CSCore.Codecs.MP3
{
    internal class FrameInfoCollection : Collection<Mp3FrameInfo>, IDisposable
    {
        private bool _disposed;
        private Mp3Frame _frame;

        public FrameInfoCollection()
        {
            PlaybackIndex = 0;
        }

        public int TotalSamples { get; private set; }

        public int PlaybackIndex { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool AddFromMp3Stream(Stream stream)
        {
            try
            {
                var info = new Mp3FrameInfo
                {
                    StreamPosition = stream.Position,
                    SampleIndex = TotalSamples
                };

                _frame = Mp3Frame.FromStream(stream);
                if (_frame != null)
                {
                    info.SampleAmount = _frame.SampleCount;
                    info.Size = Convert.ToInt32(stream.Position - info.StreamPosition);
                    TotalSamples += _frame.SampleCount;

                    Add(info);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        this[i] = null;
                    }
                    _frame = null;
                }
            }
            _disposed = true;
        }

        ~FrameInfoCollection()
        {
            Dispose(false);
        }
    }
}