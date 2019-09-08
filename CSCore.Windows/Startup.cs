using CSCore.Windows;
using System;

[assembly:WindowsCodecs]
[assembly:Startup]

namespace CSCore.Windows
{
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class StartupAttribute : Attribute
    {
        public StartupAttribute()
        {
            Locator.Instance.Register<IResamplerFactory, WindowsResamplerFactory>(() => new WindowsResamplerFactory());
            Locator.Instance.Register<IChannelMapperFactory, WindowsChannelMapperFactory>(() => new WindowsChannelMapperFactory());
        }
    }
}