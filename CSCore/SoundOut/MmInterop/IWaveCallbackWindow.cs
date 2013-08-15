using System;

namespace CSCore.SoundOut.MmInterop
{
    public interface IWaveCallbackWindow : IDisposable
    {
        IntPtr Handle { get; }

        MMInterops.WaveCallback CallBack { get; }
    }
}