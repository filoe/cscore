using CSCore.SoundIn;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.SoundIn
{
    [TestClass]
    public class WaveInCaptureBehaviourTests : SoundInBehaviourTests
    {
        protected override ISoundIn CreateSoundIn()
        {
            return new WaveIn(new WaveFormat(44100, 16, 2));
        }
    }
}