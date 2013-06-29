using System;

namespace CSCore.SoundOut.Interop
{
    public interface IWaveCallbackWindow : IDisposable
    {
        IntPtr Handle { get; }
        MMInterops.WaveCallback CallBack { get; }
    }
}
