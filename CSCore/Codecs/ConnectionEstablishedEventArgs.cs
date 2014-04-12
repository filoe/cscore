using System;

namespace CSCore.Codecs
{
    public class ConnectionEstablishedEventArgs : EventArgs
    {
        public Uri Uri { get; private set; }

        public bool Success { get; private set; }

        public ConnectionEstablishedEventArgs(Uri uri, bool success)
        {
            Uri = uri;
            Success = success;
        }
    }
}