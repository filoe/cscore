using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSCore.Codecs.FLAC.Metadata;

// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Represents a flac metadata block.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Type:{MetaDataType}   LastBlock:{IsLastMetaBlock}   Length:{Length} bytes")]
    public abstract class FlacMetadata
    {
        /// <summary>
        /// Reads and returns a single <see cref="FlacMetadata"/> from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the <see cref="FlacMetadata"/>.</param>
        /// <returns>Returns the read <see cref="FlacMetadata"/>.</returns>
        [Obsolete("Use FlacMetaDataFactory.ParseMetaData")]
        public static FlacMetadata FromStream(Stream stream)
        {
            return FlacMetadataFactory.Instance.ParseMetadata(stream);
        }

        /// <summary>
        /// Reads all <see cref="FlacMetadata"/> from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the <see cref="FlacMetadata"/>.</param>
        /// <returns>All <see cref="FlacMetadata"/>.</returns>
        public static IEnumerable<FlacMetadata> ReadAllMetadataFromStream(Stream stream)
        {
            while (true)
            {
                FlacMetadata data = FlacMetadataFactory.Instance.ParseMetadata(stream);
                yield return data;

                if (data == null || data.IsLastMetaBlock)
                    break;
            }
        }

        /// <summary>
        /// Skips all <see cref="FlacMetadata"/> of the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the <see cref="FlacMetadata"/>.</param>
        public static void SkipMetadata(Stream stream)
        {
            ReadAllMetadataFromStream(stream).ToArray();
        }

        /// <summary>
        /// Initializes the properties of the <see cref="FlacMetadata"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the metadata.</param>
        /// <param name="length">The length of <see cref="FlacMetadata"/> block inside of the stream in bytes. Does not include the metadata header.</param>
        /// <param name="isLastBlock">A value which indicates whether this is the last <see cref="FlacMetadata"/> block inside of the stream. <c>true</c> means that this is the last <see cref="FlacMetadata"/> block inside of the stream.</param>
        public virtual void Initialize(Stream stream, int length, bool isLastBlock)
        {
            IsLastMetaBlock = isLastBlock;
            Length = length;

            InitializeByStream(stream);
        }

        /// <summary>
        /// Initializes the properties of the <see cref="FlacMetadata"/> by reading them from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the metadata.</param>
        protected abstract void InitializeByStream(Stream stream);

        /// <summary>
        /// Gets the type of the <see cref="FlacMetadata"/>.
        /// </summary>
        public abstract FlacMetaDataType MetaDataType { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is the last <see cref="FlacMetadata"/> block.
        /// </summary>
        public Boolean IsLastMetaBlock { get; private set; }

        /// <summary>
        /// Gets the length of the <see cref="FlacMetadata"/> block inside of the stream in bytes.
        /// </summary>
        /// <remarks>The length does not include the metadata header.</remarks>
        public int Length { get; private set; }
    }
}