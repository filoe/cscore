using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs
{
    /// <summary>
    /// Represents an entry of the <see cref="CodecFactory"/> class which provides information about a codec.
    /// </summary>
    public class CodecFactoryEntry
    {
        /// <summary>
        /// Gets the <see cref="Codecs.GetCodecAction"/> which initializes a codec decoder based on a <see cref="Stream"/>.
        /// </summary>
        public GetCodecAction GetCodecAction { get; private set; }

        public GetCodecActionUrl GetCodecActionUrl { get; set; }

        /// <summary>
        /// Gets all with the codec associated file extensions.
        /// </summary>
        public ReadOnlyCollection<string> FileExtensions { get; private set; }

        /// <summary>
        /// Gets a value indicating whether an url as can be handled as input.
        /// </summary>
        public bool CanHandleUrl { get; set; }

        /// <summary>
        /// Gets a value indicating whether this is a generic decoder like the Microsoft MediaFoundationDecoder or FFmpeg.
        /// </summary>
        public bool IsGenericDecoder { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="CodecFactoryEntry"/> class.
        /// </summary>
        public CodecFactoryEntry()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodecFactoryEntry"/> class.
        /// </summary>
        /// <param name="getCodecAction">Delegate which initializes a codec decoder based on a <see cref="Stream"/>.</param>
        /// <param name="fileExtensions">All which the codec associated file extensions.</param>
        public CodecFactoryEntry(GetCodecAction getCodecAction, params string[] fileExtensions)
        {
            if (getCodecAction == null)
                throw new ArgumentNullException("getCodecAction");
            if (fileExtensions == null || fileExtensions.Length <= 0)
                throw new ArgumentException("No specified file extensions.", "fileExtensions");

            fileExtensions = fileExtensions.Where(x => x != null).Select(x => x.Replace(".", String.Empty)).ToArray();

            GetCodecAction = getCodecAction;
            FileExtensions = new ReadOnlyCollection<string>(fileExtensions);
        }
    }
}