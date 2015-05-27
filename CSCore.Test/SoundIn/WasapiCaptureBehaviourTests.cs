using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.SoundIn
{
    [TestClass]
    public class WasapiCaptureBehaviourTests : SoundInBehaviourTests
    {
        protected override ISoundIn CreateSoundIn()
        {
            return new WasapiCapture(false, AudioClientShareMode.Shared);
        }
    }

    [TestClass]
    public class WasapiCaptureEventSyncBehaviourTests : SoundInBehaviourTests
    {
        protected override ISoundIn CreateSoundIn()
        {
            return new WasapiCapture(true, AudioClientShareMode.Shared);
        }
    }

    [TestClass]
    public class WasapiCaptureExclusiveBehaviourTests : SoundInBehaviourTests
    {
        protected override ISoundIn CreateSoundIn()
        {
            return new WasapiCapture(false, AudioClientShareMode.Exclusive);
        }
    }
}