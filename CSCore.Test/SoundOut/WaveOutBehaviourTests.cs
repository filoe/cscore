using CSCore.SoundOut;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.SoundOut
{
    [TestClass]
    public class WaveOutBehaviourTests : SoundOutBehaviourTests
    {
        protected override ISoundOut CreateSoundOut()
        {
            return new WaveOut(150);
        }
    }
}