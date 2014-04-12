using CSCore.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs
{
    public class CodecFactory
    {
        private static CodecFactory _instance;

        public static CodecFactory Instance
        {
            get { return _instance ?? (_instance = new CodecFactory()); }
        }

        private Dictionary<object, CodecFactoryEntry> _codecs;

        private CodecFactory()
        {
            _codecs = new Dictionary<object, CodecFactoryEntry>();
            Register("mp3", new CodecFactoryEntry((s) =>
            {
                //if (MP3.MP3MediafoundationDecoder.IsSupported)
                //    return new MP3.MP3MediafoundationDecoder(s);
                //else
                //    return new MP3.MP3FileReader(s).DataStream;
                return new MP3.DmoMP3Decoder(s);
            },
                "mp3", "mpeg3"));
            Register("wave", new CodecFactoryEntry((s) =>
            {
                IWaveSource res = new WAV.WaveFileReader(s);
                if (res.WaveFormat.WaveFormatTag != AudioEncoding.Pcm &&
                    res.WaveFormat.WaveFormatTag != AudioEncoding.IeeeFloat &&
                    res.WaveFormat.WaveFormatTag != AudioEncoding.Extensible)
                {
                    res.Dispose();
                    res = new MediaFoundation.MediaFoundationDecoder(s);
                }
                return res;
            },
                "wav", "wave"));
            Register("flac", new CodecFactoryEntry((s) =>
            {
                return new FLAC.FlacFile(s);
            },
                "flac", "fla"));

            if (AAC.AACDecoder.IsSupported)
            {
                Register("aac", new CodecFactoryEntry((s) =>
                {
                    return new AAC.AACDecoder(s);
                },
                    "aac", "adt", "adts", "m2ts", "mp2", "3g2", "3gp2", "3gp", "3gpp", "m4a", "m4v", "mp4v", "mp4", "mov"));
            }

            if (WMA.WMADecoder.IsSupported)
            {
                Register("wma", new CodecFactoryEntry((s) =>
                {
                    return new WMA.WMADecoder(s);
                },
                    "asf", "wm", "wmv", "wma"));
            }

            if (MP1.MP1Decoder.IsSupported)
            {
                Register("mp1", new CodecFactoryEntry((s) =>
                {
                    return new MP1.MP1Decoder(s);
                },
                    "mp1", "m2ts"));
            }

            if (MP2.MP2Decoder.IsSupported)
            {
                Register("mp2", new CodecFactoryEntry((s) =>
                {
                    return new MP1.MP1Decoder(s);
                },
                    "mp2", "m2ts"));
            }

            if (DDP.DDPDecoder.IsSupported)
            {
                Register("ddp", new CodecFactoryEntry((s) =>
                {
                    return new DDP.DDPDecoder(s);
                },
                    "mp2", "m2ts", "m4a", "m4v", "mp4v", "mp4", "mov", "asf", "wm", "wmv", "wma", "avi", "ac3", "ec3"));
            }
        }

        public void Register(object key, CodecFactoryEntry codec)
        {
            if (key is string)
                key = (key as string).ToLower();

            if (_codecs.ContainsKey(key) != true)
                _codecs.Add(key, codec);
        }

        public IWaveSource GetCodec(string filename)
        {
            return GetCodec(new Uri(filename));
        }

        public IWaveSource GetCodec(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            try
            {
                if (uri.IsFile)
                {
                    if (!File.Exists(uri.OriginalString))
                        throw new FileNotFoundException("File not found.", uri.OriginalString);

                    var extension = Path.GetExtension(uri.OriginalString).Remove(0, 1);
                    foreach (var codecEntry in _codecs)
                    {
                        try
                        {
                            if (codecEntry.Value.FileExtensions.Contains(extension))
                                return codecEntry.Value.GetCodecAction(File.OpenRead(uri.OriginalString));
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                    return Default(uri.OriginalString);
                }
                else
                {
                    return Default(uri.OriginalString);
                }
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new NotSupportedException("Codec not supported.", e);
            }
        }

        public IWaveSource GetCodec(object key, Stream stream)
        {
            try
            {
                CodecFactoryEntry entry;
                if (_codecs.TryGetValue(key, out entry))
                    return entry.GetCodecAction(stream);
                else
                    return Default(stream);
            }
            catch (Exception e)
            {
                throw new NotSupportedException("Codec not supported.", e);
            }
            throw new NotSupportedException("Codec not supported");
        }

        private IWaveSource Default(string url)
        {
            return new MediaFoundation.MediaFoundationDecoder(url);
        }

        private IWaveSource Default(Stream stream)
        {
            return new MediaFoundation.MediaFoundationDecoder(stream);
        }

        public string[] GetSupportedFileExtensions()
        {
            List<string> extensions = new List<string>();
            foreach (var item in _codecs.Select(x => x.Value))
            {
                foreach (var e in item.FileExtensions)
                {
                    if (!extensions.Contains(e))
                        extensions.Add(e);
                }
            }
            return extensions.ToArray();
        }

        public string GenerateFilter()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Supported Files|");
            result.Append(String.Concat(GetSupportedFileExtensions().Select(x => "*." + x + ";").ToArray()));
            result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        public static string SupportedFilesFilterEN
        {
            get { return Instance.GenerateFilter(); }
        }
    }
}