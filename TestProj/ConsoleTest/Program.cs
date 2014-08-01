using System;
using System.Threading;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.Effects;
using CSCore.XAudio2.X3DAudio;

namespace ConsoleTest
{
    /// <summary>
    /// For testing only
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            ISoundIn soundIn = null;
            var source = new SoundInSource(soundIn).AppendSource(x => new DmoDistortionEffect(x));
            //effekt über source.* einstellen

            ISoundOut soundOut = new WasapiOut();
            soundOut.Initialize(source);
            soundOut.Play();
        }
    }
}