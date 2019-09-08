using System;
using System.Runtime.InteropServices;

namespace SoundTouchPitchAndTempo
{
    public interface ISoundTouch : IDisposable
    {
        void Flush();
        void Clear();
        int NumberOfSamples();
        void PutSamples(float[] samples, int numberOfSamples);
        int ReceiveSamples(float[] outBuffer, int maxSamples);
        void SetChannels(int numberOfChannels);
        void SetPitchSemiTones(float newPitch);
        void SetSampleRate(int sampleRate);
        void SetTempoChange(float newTempo);
    }

    public class SoundTouch : ISoundTouch
    {
        private const string SoundTouchDLL = "SoundTouch.dll";

        private bool _isDisposed;
        private IntPtr _soundTouchHandle;

        public SoundTouch()
        {
            _soundTouchHandle = CreateInstance();
        }

        public int NumberOfSamples()
        {
            return (int)NumberOfSamples(_soundTouchHandle);
        }

        public void PutSamples(float[] samples, int numberOfSamples)
        {
            PutSamples(_soundTouchHandle, samples, (uint)numberOfSamples);
        }

        public void SetChannels(int numberOfChannels)
        {
            SetChannels(_soundTouchHandle, (uint)numberOfChannels);
        }

        public void SetSampleRate(int sampleRate)
        {
            SetSampleRate(_soundTouchHandle, (uint)sampleRate);
        }

        public int ReceiveSamples(float[] outBuffer, int maxSamples)
        {
            return (int)ReceiveSamples(_soundTouchHandle, outBuffer, (uint)maxSamples);
        }

        public void Flush()
        {
            Flush(_soundTouchHandle);
        }

        public void Clear()
        {
            Clear(_soundTouchHandle);
        }

        public void SetTempoChange(float newTempo)
        {
            SetTempoChange(_soundTouchHandle, newTempo);
        }

        public void SetPitchSemiTones(float newPitch)
        {
            SetPitchSemiTones(_soundTouchHandle, newPitch);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if(_isDisposed)
            {
                return;
            }

            if(isDisposing)
            {
                DestroyInstance(_soundTouchHandle);
                _soundTouchHandle = IntPtr.Zero;
            }

            _isDisposed = true;
        }

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_createInstance")]
        private static extern IntPtr CreateInstance();

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_destroyInstance")]
        private static extern void DestroyInstance(IntPtr handle);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setTempoChange")]
        private static extern void SetTempoChange(IntPtr handle, float newTempo);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setPitch")]
        private static extern void SetPitch(IntPtr handle, float newPitch);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setPitchSemiTones")]
        private static extern void SetPitchSemiTones(IntPtr handle, float newPitch);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setChannels")]
        private static extern void SetChannels(IntPtr handle, uint numChannels);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setSampleRate")]
        private static extern void SetSampleRate(IntPtr handle, uint srate);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_flush")]
        private static extern void Flush(IntPtr handle);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_putSamples")]
        private static extern void PutSamples(IntPtr handle, float[] samples, uint numSamples);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_receiveSamples")]
        private static extern uint ReceiveSamples(IntPtr handle, float[] outBuffer, uint maxSamples);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_numSamples")]
        private static extern uint NumberOfSamples(IntPtr handle);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_clear")]
        private static extern uint Clear(IntPtr handle);
    }
}