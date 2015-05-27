using CSCore.SoundOut;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.SoundOut
{
    [TestClass]
    public class DirectSoundOutBehaviourTests : SoundOutBehaviourTests
    {
        protected override ISoundOut CreateSoundOut()
        {
            return new DirectSoundOut();
        }
    }
}