using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore.Codecs;
using CSCore.MediaFoundation;
using CSCore.Codecs.AAC;
using CSCore.Codecs.DDP;
using CSCore.Codecs.MP1;
using CSCore.Codecs.MP2;
using CSCore.Codecs.MP3;
using CSCore.Codecs.WMA;

namespace CSCore.Windows
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class RegisterAssemblyCodecsAttribute : Attribute
    {
        public RegisterAssemblyCodecsAttribute()
        {
            RegisterCodecs();
        }

        private void RegisterCodecs()
        {
            CodecFactory.Instance.Register("mp3", new CodecFactoryEntry(s =>
                {
                    try
                    {
                        return new DmoMp3Decoder(s);
                    }
                    catch (Exception)
                    {
                        if (Mp3MediafoundationDecoder.IsSupported)
                            return new Mp3MediafoundationDecoder(s);
                        throw;
                    }
                },
                "mp3", "mpeg3"));
            if (AacDecoder.IsSupported)
            {
                CodecFactory.Instance.Register("aac", new CodecFactoryEntry(s => new AacDecoder(s),
                    "aac", "adt", "adts", "m2ts", "mp2", "3g2", "3gp2", "3gp", "3gpp", "m4a", "m4v", "mp4v", "mp4",
                    "mov"));
            }

            if (WmaDecoder.IsSupported)
            {
                CodecFactory.Instance.Register("wma", new CodecFactoryEntry(s => new WmaDecoder(s),
                    "asf", "wm", "wmv", "wma"));
            }

            if (Mp1Decoder.IsSupported)
            {
                CodecFactory.Instance.Register("mp1", new CodecFactoryEntry(s => new Mp1Decoder(s),
                    "mp1", "m2ts"));
            }

            if (Mp2Decoder.IsSupported)
            {
                CodecFactory.Instance.Register("mp2", new CodecFactoryEntry(s => new Mp2Decoder(s),
                    "mp2", "m2ts"));
            }

            if (DDPDecoder.IsSupported)
            {
                CodecFactory.Instance.Register("ddp", new CodecFactoryEntry(s => new DDPDecoder(s),
                    "mp2", "m2ts", "m4a", "m4v", "mp4v", "mp4", "mov", "asf", "wm", "wmv", "wma", "avi", "ac3", "ec3"));
            }

            if (MediaFoundationCore.IsSupported)
            {
                CodecFactory.Instance.Register("mf-generic", new CodecFactoryEntry()
                {
                   GetCodecActionUrl = url => new MediaFoundationDecoder(url),
                   CanHandleUrl = true,
                   IsGenericDecoder = true
                });
            }

            CodecFactory.Instance.Register("mp3-web", new CodecFactoryEntry()
            {
                GetCodecActionUrl = url => new Mp3WebStream(url),
                CanHandleUrl = true
            });
        }
    }
}
