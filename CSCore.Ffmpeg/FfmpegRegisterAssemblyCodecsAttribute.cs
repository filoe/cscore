using CSCore.Codecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCore.Ffmpeg
{
    public class FfmpegRegisterAssemblyCodecsAttribute : RegisterAssemblyCodecsAttribute
    {
        public override void RegisterAssemblyCodecs()
        {
            var formats = FfmpegUtils.GetInputFormats().ToList();
            foreach (var format in formats)
            {
                var extensions = format.FileExtensions.ToArray();
                if (!(extensions?.Any() ?? false))
                    extensions = new[] { format.Name };

                CodecFactory.Instance.Register(format.Name, new CodecFactoryEntry((uri) => new FfmpegDecoder(uri), extensions)
                {
                    GetCodecActionUrl = (url) => new FfmpegDecoder(url),
                    CanHandleUrl = true
                });
            }

            CodecFactory.Instance.Register("ffmpeg-generic", new CodecFactoryEntry()
            {
                GetCodecActionUrl = (url) => new FfmpegDecoder(url),
                CanHandleUrl = true,
                IsGenericDecoder = true
            });
        }
    }
}
