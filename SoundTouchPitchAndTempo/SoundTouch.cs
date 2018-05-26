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
        private bool disposed;
        private readonly IntPtr _soundTouchHandle;

        public SoundTouch()
        {
            _soundTouchHandle = soundtouch_createInstance();
        }

        public uint NumberOfSamples()
        {
            return soundtouch_numSamples(_soundTouchHandle);
        }

        public void PutSamples(float[] samples, uint numSamples)
        {
            soundtouch_putSamples(_soundTouchHandle, samples, numSamples);
        }

        public void SetChannels(uint numChannels)
        {
            soundtouch_setChannels(_soundTouchHandle, numChannels);
        }

        public void SetSampleRate(uint srate)
        {
            soundtouch_setSampleRate(_soundTouchHandle, srate);
        }

        public uint ReceiveSamples(float[] outBuffer, uint maxSamples)
        {
            return soundtouch_receiveSamples(_soundTouchHandle, outBuffer, maxSamples);
        }

        public void Flush()
        {
            soundtouch_flush(_soundTouchHandle);
        }

        public void SetTempoChange(float newTempo)
        {
            soundtouch_setTempoChange(_soundTouchHandle, newTempo);
        }

        public void SetPitchSemiTones(float newPitch)
        {
            soundtouch_setPitchSemiTones(_soundTouchHandle, newPitch);
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
                soundtouch_destroyInstance(_soundTouchHandle);
            }

            disposed = true;
        }

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr soundtouch_createInstance();

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_destroyInstance(IntPtr h);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setTempoChange(IntPtr h, float newTempo);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setPitch(IntPtr h, float newPitch);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setPitchSemiTones(IntPtr h, float newPitch);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setChannels(IntPtr h, uint numChannels);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setSampleRate(IntPtr h, uint srate);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_flush(IntPtr h);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_putSamples(IntPtr h, float[] samples, uint numSamples);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint soundtouch_receiveSamples(IntPtr h, float[] outBuffer, uint maxSamples);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint soundtouch_numSamples(IntPtr h);
    }
}
