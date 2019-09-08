using CSCore.Linux;
using System;

[assembly:Startup]

namespace CSCore.Linux
{
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class StartupAttribute : Attribute
    {
        public StartupAttribute()
        {
            Locator.Instance.Register<IResamplerFactory, LinuxResamplerFactory>(() => new LinuxResamplerFactory());
            Locator.Instance.Register<IChannelMapperFactory, LinuxChannelMapperFactory>(() => new LinuxChannelMapperFactory());
        }
    }
}