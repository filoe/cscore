using System;
using System.Collections.Generic;
using System.IO;

namespace CSCore.Codecs
{
    public class CodecFactory
    {
        public const string SupportedFilesFilterDE = "Unterstütze Dateien|*.wav;*.mp3;*.flac|Wavesound-Dateien (*.wav)|*.wav|MP3-Dateien (*.mp3)|*.mp3|FLAC-Dateien (*.flac, *.fla)|*.flac;*.fla|Alle Dateien (*.*)|*.*";
        public const string SupportedFilesFilterEN = "Supported Files|*.wav;*.mp3;*.flac|Wavesound-Files (*.wav)|*.wav|MP3-Files (*.mp3)|*.mp3|FLAC-Files (*.flac, *.fla)|*.flac;*.fla|All Files (*.*)|*.*";

        static CodecFactory _instance;
        public static CodecFactory Instance
        {
            get { return _instance ?? (_instance = new CodecFactory()); }
        }

        Dictionary<object, Func<Stream, IWaveSource>> _codecs = new Dictionary<object, Func<Stream, IWaveSource>>();

        private CodecFactory()
        {
            Register("mp3", (e) =>
                {
                    return new MP3.Mp3FileReader(e).DataStream;
                });
            Register("wave", (e) =>
                {
                    return new WAV.WaveFileReader(e);
                });
            Register("wav", (e) =>
                {
                    return new WAV.WaveFileReader(e);
                    //return new WAV.WaveFile(e);
                });
            Register("flac", (e) =>
                {
                    return new FLAC.FlacFile(e);
                });
            Register("fla", (e) =>
                {
                    return new FLAC.FlacFile(e);
                });
        }

        public void Register(object key, Func<Stream, IWaveSource> produceProc)
        {
            if (key is string)
                key = (key as string).ToLower();

            if(_codecs.ContainsKey(key) != true)
                _codecs.Add(key, produceProc);
        }

        public IWaveSource GetCodec(Stream stream, object key)
        {
            if (_codecs.ContainsKey(key))
            {
                if (key is string)
                    key = (key as string).ToLower();

                return _codecs[key](stream);
            }
            else
            {
                Context.Current.Logger.Error("Not supported codec: " + key + ".", "CodecFactory.GetCodec(Stream, object)");
                throw new NotSupportedException("Not supported codec. Use Register(object, Func<Stream, IWaveSource> to register a new codec.");
            }
        }

        public IWaveSource GetCodec(string fileName)
        {
            Stream stream = File.OpenRead(fileName);
            object key = Path.GetExtension(fileName).Remove(0, 1).ToLower();

            return GetCodec(stream, key);
        }
    }
}
