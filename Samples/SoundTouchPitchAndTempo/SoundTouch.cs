using System;
using System.Runtime.InteropServices;

namespace SoundTouchPitchAndTempo
{
    public interface ISoundTouch : IDisposable
    {
        void Flush();
        uint NumberOfSamples();
        void PutSamples(float[] samples, uint numSamples);
        uint ReceiveSamples(float[] outBuffer, uint maxSamples);
        void SetChannels(uint numChannels);
        void SetPitchSemiTones(float newPitch);
        void SetSampleRate(uint srate);
        void SetTempoChange(float newTempo);
    }

    public class SoundTouch : ISoundTouch
    {
        private const string SoundTouchDLL = "SoundTouch.dll";

        private bool disposed;
        private readonly IntPtr _soundTouchHandle;

        public SoundTouch()
        {
            _soundTouchHandle = CreateInstance();
        }

        public uint NumberOfSamples()
        {
            return NumberOfSamples(_soundTouchHandle);
        }

        public void PutSamples(float[] samples, uint numSamples)
        {
            PutSamples(_soundTouchHandle, samples, numSamples);
        }

        public void SetChannels(uint numChannels)
        {
            SetChannels(_soundTouchHandle, numChannels);
        }

        public void SetSampleRate(uint srate)
        {
            SetSampleRate(_soundTouchHandle, srate);
        }

        public uint ReceiveSamples(float[] outBuffer, uint maxSamples)
        {
            return ReceiveSamples(_soundTouchHandle, outBuffer, maxSamples);
        }

        public void Flush()
        {
            Flush(_soundTouchHandle);
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

        protected virtual void Dispose(bool disposing)
        {
            if(disposed)
            {
                return;
            }

            if(disposing)
            {
                DestroyInstance(_soundTouchHandle);
            }

            disposed = true;
        }

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_createInstance")]
        private static extern IntPtr CreateInstance();

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_destroyInstance")]
        private static extern void DestroyInstance(IntPtr h);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setTempoChange")]
        private static extern void SetTempoChange(IntPtr h, float newTempo);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setPitch")]
        private static extern void SetPitch(IntPtr h, float newPitch);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setPitchSemiTones")]
        private static extern void SetPitchSemiTones(IntPtr h, float newPitch);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setChannels")]
        private static extern void SetChannels(IntPtr h, uint numChannels);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_setSampleRate")]
        private static extern void SetSampleRate(IntPtr h, uint srate);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_flush")]
        private static extern void Flush(IntPtr h);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_putSamples")]
        private static extern void PutSamples(IntPtr h, float[] samples, uint numSamples);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_receiveSamples")]
        private static extern uint ReceiveSamples(IntPtr h, float[] outBuffer, uint maxSamples);

        [DllImport(SoundTouchDLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_numSamples")]
        private static extern uint NumberOfSamples(IntPtr h);
    }
}
