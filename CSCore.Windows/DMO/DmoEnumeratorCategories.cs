using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     Defines DMO-Categories for enumerating DMOs.
    /// </summary>
    public static class DmoEnumeratorCategories
    {
        /// <summary>
        ///     All DMOs.
        /// </summary>
        public static readonly Guid All = Guid.Empty;

        /// <summary>
        ///     AudioEffects
        /// </summary>
        public static readonly Guid AudioEffect = new Guid("f3602b3f-0592-48df-a4cd-674721e7ebeb");

        /// <summary>
        ///     AudioCaptureEffects
        /// </summary>
        public static readonly Guid AudioCaptureEffects = new Guid("f665aaba-3e09-4920-aa5f-219811148f09");

        /// <summary>
        ///     Category which includes audio decoder.
        /// </summary>
        public static readonly Guid AudioDecoder = new Guid("57f2db8b-e6bb-4513-9d43-dcd2a6593125");

        /// <summary>
        ///     Category which includes audio encoder.
        /// </summary>
        public static readonly Guid AudioEncoder = new Guid("33D9A761-90C8-11d0-BD43-00A0C911CE86");

        //note: There would be some more categories. But since this library is an audio library, We do not include the video,...
    }
}