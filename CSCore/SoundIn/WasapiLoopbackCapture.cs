using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.SoundIn
{
    public class WasapiLoopbackCapture : WasapiCapture
    {
        public WasapiLoopbackCapture()
            : base(false, AudioClientShareMode.Shared)
        {
        }

        protected override MMDevice GetDefaultDevice()
        {
            return MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Console);
        }

        protected override AudioClientStreamFlags GetStreamFlags()
        {
            return AudioClientStreamFlags.StreamFlags_Loopback;
        }
    }
}
