using System;

namespace CSCore.Utils
{
    [RemoveObj]
    [AttributeUsage(AttributeTargets.Method)]
    internal class CSCliAttribute : Attribute
    {
    }

    [RemoveObj]
    [AttributeUsage(AttributeTargets.Method)]
    internal class MemoryCopyAttribute : Attribute
    {
    }

    [RemoveObj]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    internal class RemoveObjAttribute : Attribute
    {
    }
}
