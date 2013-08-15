using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs
{
    public class CodecFactoryEntry
    {
        public GetCodecAction GetCodecAction { get; private set; }

        public string[] FileExtensions { get; private set; }

        public CodecFactoryEntry(GetCodecAction getCodecAction, params string[] fileextensions)
        {
            if (getCodecAction == null)
                throw new ArgumentNullException("GetCodecAction");
            if (fileextensions == null || fileextensions.Length <= 0)
                throw new ArgumentException("No fileextensions", "fileextensions");

            GetCodecAction = getCodecAction;
            FileExtensions = fileextensions;
        }
    }
}