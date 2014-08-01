using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestProj.HowTo
{
    public class HowTo01
    {
        public void PlayASound()
        {
            //Contains the sound to play
            using (IWaveSource soundSource = GetSoundSource())
            {
                //SoundOut implementation which plays the sound
                using (ISoundOut soundOut = GetSoundOut())
                {
                    //Tell the SoundOut which sound it has to play
                    soundOut.Initialize(soundSource);
                    //Play the sound
                    soundOut.Play();

                    Thread.Sleep(2000);

                    //Stop the playback
                    soundOut.Stop();
                }
            }
        }

        private ISoundOut GetSoundOut()
        {
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                return new WasapiOut();
            else
                return new DirectSoundOut();
        }

        private IWaveSource GetSoundSource()
        {
            //return any source ... in this example, we'll just play a mp3 file
            return CodecFactory.Instance.GetCodec(@"C:\Temp\sound.mp3");
        }
    }
}
