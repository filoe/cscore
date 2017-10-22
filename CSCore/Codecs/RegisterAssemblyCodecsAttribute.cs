using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCore.Codecs
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public abstract class RegisterAssemblyCodecsAttribute : Attribute
    {
        public abstract void RegisterAssemblyCodecs();
    }
}
