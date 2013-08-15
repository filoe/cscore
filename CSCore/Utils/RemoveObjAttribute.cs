using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Utils
{
    [RemoveObj]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    internal class RemoveObjAttribute : Attribute
    {
    }
}