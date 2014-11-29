using System;

namespace CSCore.SoundOut.MMInterop
{
    public interface IWaveCallbackWindow : IDisposable
    {
        IntPtr Handle { get; }

        WaveCallback CallBack { get; }
    }
}