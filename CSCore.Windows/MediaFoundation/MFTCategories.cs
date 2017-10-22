using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Defines categories for Media Foundation transforms (MFTs). These categories are used to register and enumerate MFTs.
    /// </summary>
    /// <remarks>For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd388660(v=vs.85).aspx"/>.</remarks>
    public static class MFTCategories
    {
        //audio        
        /// <summary>
        /// Audio decoders.
        /// </summary>
        public static readonly Guid AudioDecoder = new Guid("9ea73fb4-ef7a-4559-8d5d-719d8f0426c7");

        /// <summary>
        /// Audio encoders.
        /// </summary>
        public static readonly Guid AudioEncoder = new Guid("91c64bd0-f91e-4d8c-9276-db248279d975");
        /// <summary>
        /// Audio effects.
        /// </summary>
        public static readonly Guid AudioEffect = new Guid("11064c48-3648-4ed0-932e-05ce8ac811b7");

        //video        
        /// <summary>
        /// Video encoders.
        /// </summary>
        public static readonly Guid VideoEncoder = new Guid("f79eac7d-e545-4387-bdee-d647d7bde42a");

        /// <summary>
        /// Video decoders.
        /// </summary>
        public static readonly Guid VideoDecoder = new Guid("d6c02d4b-6833-45b4-971a-05a4b04bab91");
        /// <summary>
        /// Video effects.
        /// </summary>
        public static readonly Guid VideoEffect = new Guid("12e17c21-532c-4a6e-8a1c-40825a736397");
        /// <summary>
        /// Video processors.
        /// </summary>
        public static readonly Guid VideoProcessor = new Guid("302ea3fc-aa5f-47f9-9f7a-c2188bb16302");

        //...        
        /// <summary>
        /// Demultiplexers.
        /// </summary>
        public static readonly Guid Demultiplexer = new Guid("a8700a7a-939b-44c5-99d7-76226b23b3f1");

        /// <summary>
        /// Multiplexers.
        /// </summary>
        public static readonly Guid Multiplexer = new Guid("059c561e-05ae-4b61-b69d-55b61ee54a7b");
        /// <summary>
        /// Miscellaneous MFTs.
        /// </summary>
        public static readonly Guid Other = new Guid("90175d57-b7ea-4901-aeb3-933a8747756f");
    }
}