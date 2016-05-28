using CSCore.SoundOut;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.SoundOut
{
    [TestClass]
    public class ALSoundOutBehaviourTests : SoundOutBehaviourTests
    {
        protected override ISoundOut CreateSoundOut()
        {
            return new ALSoundOut();
        }
    }
}
