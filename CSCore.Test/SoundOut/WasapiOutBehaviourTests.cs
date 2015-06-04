using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.SoundOut
{
    [TestClass]
    public class WasapiOutBehaviourTests : SoundOutBehaviourTests
    {
        protected override ISoundOut CreateSoundOut()
        {
            return new WasapiOut();
        }
    }

    [TestClass]
    public class WasapiOutEventSyncBehaviourTests : SoundOutBehaviourTests
    {
        protected override ISoundOut CreateSoundOut()
        {
            return new WasapiOut(true, AudioClientShareMode.Shared, 100);
        }
    }

    [TestClass]
    public class WasapiOutExclusiveEventSyncBehaviourTests : SoundOutBehaviourTests
    {
        protected override ISoundOut CreateSoundOut()
        {
            return new WasapiOut(true, AudioClientShareMode.Exclusive, 100);
        }
    }

    [TestClass]
    public class WasapiOutExclusiveBehaviourTests : SoundOutBehaviourTests
    {
        protected override ISoundOut CreateSoundOut()
        {
            return new WasapiOut(false, AudioClientShareMode.Exclusive, 100);
        }
    }
}