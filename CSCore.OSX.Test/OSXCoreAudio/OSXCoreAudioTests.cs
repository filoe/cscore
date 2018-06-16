using System;
using NUnit.Framework;
using System.IO;

namespace CSCore.OSX.Test.OSXCoreAudio
{
    [TestFixture]
    public class OSXCoreAudioTests
    {
        [Test]
        public void CanCreateWavSourceFromURL()
        {
            
            var filename = Path.ChangeExtension(Path.GetTempFileName(), "mp3");
            try
            {
                using (var stream = GlobalTestConfig.TestMp3AsStream())
                {
                    File.WriteAllBytes(filename, stream.ToArray());
                    using (var waveSource = new CSCore.OSXCoreAudio.OSXAudioDecoder(filename))
                    {
                        Assert.IsNotNull(waveSource);
                        Assert.IsTrue(waveSource.Length > 0);
                    }
                }
            }
            finally
            {
                File.Delete(filename);
            }
        }
    }
}

