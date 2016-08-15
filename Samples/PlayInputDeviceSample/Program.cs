using System;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.Effects;

namespace PlayInputDeviceSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("The following sample will use the default input device");
            Console.WriteLine("and will play its input data on the default output device.");
            Console.WriteLine("In addition, this sample will apply an echo effect.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            Foo();
        }

        static void Foo()
        {
            //create a new soundIn instance for using input devices
            using (var soundIn = new WasapiCapture(true, AudioClientShareMode.Shared, 30))
            {
                //important: always initialize the soundIn instance before creating the
                //SoundInSource. The SoundInSource needs the WaveFormat of the soundIn,
                //which gets determined by the soundIn.Initialize method.
                soundIn.Initialize();

                //wrap a sound source around the soundIn instance
                //in order to prevent playback interruptions, set FillWithZeros to true
                //otherwise, if the SoundIn does not provide enough data, the playback stops
                IWaveSource source = new SoundInSource(soundIn) { FillWithZeros = true };

                //wrap a EchoEffect around the previously created "source"
                //note: disposing the echoSource will also dispose 
                //the previously created "source"
                using (var echoSource = new DmoEchoEffect(source))
                {
                    //start capturing data
                    soundIn.Start();

                    //create a soundOut instance to play the data
                    using (var soundOut = new WasapiOut())
                    {
                        //initialize the soundOut with the echoSource
                        //the echoSource provides data from the "source" and applies the echo effect
                        //the "source" provides data from the "soundIn" instance
                        soundOut.Initialize(echoSource);

                        //play
                        soundOut.Play();

                        Console.WriteLine("Press any key to exit the program.");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
