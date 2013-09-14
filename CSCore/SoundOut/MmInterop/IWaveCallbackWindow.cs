using System;

namespace CSCore.SoundOut.MMInterop
{
    public interface IWaveCallbackWindow : IDisposable
    {
        IntPtr Handle { get; }

        MMInterops.WaveCallback CallBack { get; }
    }
}