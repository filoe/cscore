using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs
{
    public delegate IWaveSource GetCodecAction(Stream stream);
}