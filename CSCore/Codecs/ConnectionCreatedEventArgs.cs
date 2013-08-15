using System;

namespace CSCore.Codecs
{
    public class ConnectionCreatedEventArgs : EventArgs
    {
        public Uri Uri { get; private set; }

        public bool Success { get; private set; }

        public ConnectionCreatedEventArgs(Uri uri, bool success)
        {
            Uri = uri;
            Success = success;
        }
    }
}