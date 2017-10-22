using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs
{
    /// <summary>
    /// Delegate which initializes a new decoder for a specific codec based on a <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream"><see cref="Stream"/> which contains the data that should be decoded by the codec decoder.</param>
    /// <returns>Decoder for a specific coded based on a <paramref name="stream"/>.</returns>
    public delegate IWaveSource GetCodecAction(Stream stream);

    /// <summary>
    /// Delegate which initializes a new decoder for a specific codec based on a <paramref name="url"/>.
    /// </summary>
    /// <param name="url">Url of the resource to be decoded.</param>
    /// <returns>Decoder for a specific coded based on the provided <paramref name="url"/>.</returns>
    public delegate IWaveSource GetCodecActionUrl(string url);
}