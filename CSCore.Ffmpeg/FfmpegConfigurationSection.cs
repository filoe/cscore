using System;
using System.Configuration;

namespace CSCore.Ffmpeg
{
    /// <summary>
    /// Encapsulates configuration properties for CSCore.Ffmpeg.
    /// </summary>
    public class FfmpegConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the HTTP proxy.
        /// </summary>
        /// <value>
        /// A proxy with the following format: http://[User@]MyProxy.MyDomain:Port/.
        /// </value>
        [ConfigurationProperty("httpProxy", DefaultValue = "", IsRequired = false)]
        public string HttpProxy
        {
            get { return (string)this["httpProxy"]; }
            set { this["httpProxy"] = value; }
        }

        /// <summary>
        /// Gets or sets a whitelist where no proxy should be used.
        /// </summary>
        /// <value>
        /// The proxy whitelist. For examples see https://ffmpeg.org/doxygen/3.2/noproxy_8c_source.html.
        /// </value>
        [ConfigurationProperty("proxyWhitelist", DefaultValue = "*", IsRequired = false)]
        public string ProxyWhitelist
        {
            get { return (string) this["proxyWhitelist"]; }
            set { this["proxyWhitelist"] = value; }
        }

        /// <summary>
        /// Gets or sets the log level. For more details see <see cref="FfmpegUtils.LogLevel"/>.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        [ConfigurationProperty("loglevel", DefaultValue = null, IsRequired = false)]
        public LogLevel? LogLevel
        {
            get
            {
                var obj = this["loglevel"];
                if (obj is LogLevel && Enum.IsDefined(typeof (LogLevel), obj))
                {
                    return (LogLevel?) obj;
                }
                return null;
            }
            set { this["loglevel"] = value; }
        }
    }
}
