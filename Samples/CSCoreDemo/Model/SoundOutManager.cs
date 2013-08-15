using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.SoundOut.DirectSound;
using CSCore.Win32;
using System;
using System.Collections.Generic;

namespace CSCoreDemo.Model
{
    public class SoundOutManager
    {
        private ISoundOut _soundOut;
        private SoundOutType _soundOutType;

        public bool IsPlaying
        {
            get { return IsCreated && _soundOut.PlaybackState == PlaybackState.Playing; }
        }

        public bool IsPaused
        {
            get { return IsCreated && _soundOut.PlaybackState == PlaybackState.Paused; }
        }

        public bool IsStopped
        {
            get { return IsCreated && _soundOut.PlaybackState == PlaybackState.Stopped; }
        }

        public bool IsCreated
        {
            get { return _soundOutType != SoundOutType.None && _soundOut != null; }
        }

        private float _volume = 1.0f;

        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                if (IsCreated && IsInitialized)
                    _soundOut.Volume = Volume;
            }
        }

        private bool _isinitialized = false;

        public bool IsInitialized
        {
            get { return IsCreated && _isinitialized; }
            private set { _isinitialized = value; }
        }

        public void CreateSoundOut(SoundOutType soundOutType)
        {
            if (IsCreated)
                Destroy();

            IsInitialized = false;
            switch (soundOutType)
            {
                case SoundOutType.WaveOut:
                    _soundOut = new WaveOutWindow() { Latency = 70 };
                    break;

                case SoundOutType.DirectSound:
                    _soundOut = new DirectSoundOut() { Latency = 50 };
                    break;

                case SoundOutType.Wasapi:
                    _soundOut = new WasapiOut();
                    break;

                default:
                    _soundOutType = SoundOutType.None;
                    throw new ArgumentOutOfRangeException("soundOutType");
            }

            _soundOutType = soundOutType;
            _soundOut.Stopped += (s, e) => Stop();
        }

        public IEnumerable<SoundOutDevice> GetDevices()
        {
            CheckForCreated();
            switch (_soundOutType)
            {
                case SoundOutType.WaveOut:
                    for (int i = 0; i < WaveOut.GetDeviceCount(); i++)
                    {
                        var dev = WaveOut.GetDevice(i);
                        yield return new SoundOutDevice(dev.szPname, i);
                    }
                    break;

                case SoundOutType.DirectSound:
                    foreach (var dev in DirectSoundDevice.EnumerateDevices())
                    {
                        yield return new SoundOutDevice(dev.Description, dev);
                    }
                    break;

                case SoundOutType.Wasapi:
                    foreach (var dev in new MMDeviceEnumerator().EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                    {
                        yield return new SoundOutDevice(dev.PropertyStore[PropertyStore.FriendlyName].ToString(), dev);
                    }
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        public void SetDevice(SoundOutDevice device)
        {
            CheckForCreated();
            switch (_soundOutType)
            {
                case SoundOutType.WaveOut:
                    var waveOut = (WaveOut)_soundOut;
                    waveOut.Device = (int)device.NativeDevice;
                    break;

                case SoundOutType.DirectSound:
                    var dsound = (DirectSoundOut)_soundOut;
                    dsound.Device = ((DirectSoundDevice)device.NativeDevice).Guid;
                    break;

                case SoundOutType.Wasapi:
                    var wasapi = (WasapiOut)_soundOut;
                    wasapi.Device = (MMDevice)device.NativeDevice;
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        public void Initialize(IWaveSource source)
        {
            CheckForCreated();
            if (source == null)
                throw new ArgumentNullException("source");
            _soundOut.Initialize(source);
            IsInitialized = true;
            _soundOut.Volume = Volume;
        }

        public void Play()
        {
            CheckForCreated();
            if (IsPaused || IsStopped)
                _soundOut.Play();
        }

        public void Pause()
        {
            CheckForCreated();
            if (IsPlaying)
                _soundOut.Pause();
        }

        public void Stop()
        {
            CheckForCreated();
            if (IsPlaying || IsPaused)
                _soundOut.Stop();

            if (_soundOut.WaveSource != null)
                _soundOut.WaveSource.Dispose();

            IsInitialized = false;
        }

        public void Destroy()
        {
            CheckForCreated();
            Stop();
            _soundOut.Dispose();
            IsInitialized = false;
        }

        private void CheckForCreated()
        {
            if (!IsCreated)
                throw new InvalidOperationException("SoundOut not created");
        }
    }

    public class SoundOutDevice
    {
        public string FriendlyName { get; private set; }

        public object NativeDevice { get; private set; }

        public SoundOutDevice(string friendlyName, object nativeDevice)
        {
            FriendlyName = friendlyName;
            NativeDevice = nativeDevice;
        }

        public override string ToString()
        {
            return FriendlyName;
        }
    }

    public enum SoundOutType
    {
        None,
        WaveOut,
        DirectSound,
        Wasapi
    }
}