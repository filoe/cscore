using System;
namespace CSCore.Codecs
{
    /// <summary>
    /// Delegate which initializes the default decoder
    /// </summary>
    /// <param name="url"><see cref="string"/> of the audio file to be opened by the decoder</param>
    /// <param name="isWebURL"><see cref="bool"/> Whether the url is a web url or not</param>
    /// <returns>Decoder for a specific coded based on a <paramref name="stream"/>.</returns>
    public delegate IWaveSource DefaultCodecAction(string url, bool isWebURL = false);
}

