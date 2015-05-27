using System;
using CSCore.Codecs.MP3;

namespace CSCore.Codecs
{
    /// <summary>
    /// Provides data for all events which notify the client that a connection got established. For example the <see cref="Mp3WebStream.ConnectionEstablished"/> event.
    /// </summary>
    public class ConnectionEstablishedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the uri of the connection.
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the connection got established successfully or not. <c>true</c> if the connection got established successfully, otherwise <c>false</c>.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionEstablishedEventArgs"/> class.
        /// </summary>
        /// <param name="uri">The uri of the connection.</param>
        /// <param name="success">A value indicating whether the connection got established successfully or not. <c>true</c> if the connection got established successfully, otherwise <c>false</c>.</param>
        public ConnectionEstablishedEventArgs(Uri uri, bool success)
        {
            Uri = uri;
            Success = success;
        }
    }
}