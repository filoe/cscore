using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.SoundOut.DirectSound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProj.HowTo
{
    public class HowTo04
    {
        public IEnumerable<DirectSoundDevice> EnumerateDirectSoundDevices()
        {
            return new DirectSoundDeviceEnumerator().Devices;
        }

        public void ChooseADeviceUsingDirectSound(IWaveSource source)
        {
            DirectSoundDevice anyDevice = EnumerateDirectSoundDevices().First();

            using(DirectSoundOut soundOut = new DirectSoundOut())
            {
                soundOut.Device = anyDevice.Guid;

                soundOut.Initialize(source);
            }
        }

        public IEnumerable<MMDevice> EnumerateWasapiDevices()
        {
            using(MMDeviceEnumerator enumerator = new MMDeviceEnumerator())
            {
                return enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
            }
        }

        public void ChooseADeviceUsingWasapi(IWaveSource source)
        {
            MMDevice anyDevice = EnumerateWasapiDevices().First();

            using(WasapiOut soundOut = new WasapiOut())
            {
                soundOut.Device = anyDevice;
                
                soundOut.Initialize(source);
            }
        }
    }
}
