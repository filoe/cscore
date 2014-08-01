using CSCore;
using CSCore.Codecs;
using CSCore.Streams;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProj.HowTo
{
    public class HowTo03
    {
        public IWaveSource BuildASourceChain()
        {
            IWaveSource fileSource = CodecFactory.Instance.GetCodec(@"C:\Temp\test.mp3");

            VolumeSource volumeSource = new VolumeSource(fileSource);
            PanSource panSource = new PanSource(volumeSource);

            IWaveSource panWaveSource = panSource.ToWaveSource();

            DmoEchoEffect echoEffect = new DmoEchoEffect(panWaveSource);

            volumeSource.Volume = 0.5f; //50% volume
            echoEffect.LeftDelay = 500; //500 ms
            echoEffect.RightDelay = 250; //250 ms

            return echoEffect;
        }
    }
}
