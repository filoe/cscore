using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    public class MFAttribute<TValue>
    {
        public Guid Key { get; private set; }

        public TValue Value { get; private set; }

        public MFAttribute(Guid key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}