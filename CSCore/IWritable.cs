using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore
{
    public interface IWritable
    {
        void Write(byte[] buffer, int offset, int count);
    }
}
