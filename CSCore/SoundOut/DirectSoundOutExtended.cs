using CSCore.SoundOut.DirectSound;

namespace CSCore.SoundOut
{
    public class DirectSoundOutExtended : DirectSoundOut
    {
        public float Pan
        {
            get { return GetPan(); }
            set { SetPan(value); }
        }

        public int Frequency
        {
            get { return GetFrequency(); }
            set { SetFrequency(value); }
        }

        private float GetPan()
        {
            CheckForInitialize();
            float pan;
            DirectSoundException.Try(_secondaryBuffer.GetPan(out pan), "IDirectSoundBuffer", "GetPan");
            return pan;
        }

        private void SetPan(float pan)
        {
            CheckForInitialize();
            DirectSoundException.Try(_secondaryBuffer.SetPan(pan), "IDirectSoundBuffer", "GetPan");
        }

        private int GetFrequency()
        {
            CheckForInitialize();
            int frequency;
            DirectSoundException.Try(_secondaryBuffer.GetFrequency(out frequency), "IDirectSoundBuffer", "GetFrequency");
            return frequency;
        }

        private void SetFrequency(int frequency)
        {
            CheckForInitialize();
            DirectSoundException.Try(_secondaryBuffer.SetFrequency(frequency), "IDirectSoundBuffer", "SetFrequency");
        }
    }
}