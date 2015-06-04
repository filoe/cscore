using CSCore.SoundIn;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.SoundIn
{
    [TestClass]
    public class WasapiLoopbackCaptureBehaviourTests : SoundInBehaviourTests
    {
        protected override ISoundIn CreateSoundIn()
        {
            return new WasapiLoopbackCapture();
        }
    }
}