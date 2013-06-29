using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.SoundOut;
using CSCore.Codecs;

namespace SimplePlayback
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.WriteLine("Invalid arguments: {inputfile}");
                Environment.Exit(-1);
            }

            ISoundOut soundOut;
            using (soundOut = new DirectSoundOut())
            {
                soundOut.Initialize(CodecFactory.Instance.GetCodec(args[0]));
                soundOut.Play();

                Console.ReadKey();
            }
        }
    }
}
