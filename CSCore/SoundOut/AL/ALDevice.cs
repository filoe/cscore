using System;
using System.Collections.Generic;
using System.Linq;

namespace CSCore.SoundOut.AL
{
    public class ALDevice : IDisposable
    {
        private static ALDevice[] _devices;

        /// <summary>
        /// Gets the name
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// Gets the openal context
        /// </summary>
        internal ALContext Context { get; private set; }

        private IntPtr _deviceHandle;
        private readonly List<ALSource> _sources; 

        /// <summary>
        /// Initializes a new ALDevice class
        /// </summary>
        internal ALDevice(string deviceName)
        {
            Name = deviceName;
            _sources = new List<ALSource>();
        }

        /// <summary>
        /// Initializes the openal device
        /// </summary>
        internal void Initialize()
        {
            _deviceHandle = ALInterops.alcOpenDevice(Name);
            Context = ALContext.CreateContext(_deviceHandle);
        }

        /// <summary>
        /// Generates a new openal source
        /// </summary>
        /// <returns></returns>
        internal ALSource GenerateALSource()
        {
            Context.MakeCurrent();

            var sources = new uint[1];
            ALInterops.alGenSources(1, sources);

            return new ALSource(this, sources[0]);
        }

        /// <summary>
        /// Deletes the specified openal source
        /// </summary>
        /// <param name="source">The source</param>
        internal void DeleteALSource(ALSource source)
        {
            Context.MakeCurrent();

            var sources = new uint[1];
            sources[0] = source.Id;

            ALInterops.alDeleteSources(1, sources);
        }

        /// <summary>
        /// Enumerates the openal devices
        /// </summary>
        /// <returns></returns>
        public static ALDevice[] EnumerateALDevices()
        {
            if (_devices == null)
            {
                var deviceNames = ALInterops.GetALDeviceNames();
                var devices = new ALDevice[deviceNames.Length];

                for (int i = 0; i < devices.Length; i++)
                {
                    devices[i] = new ALDevice(deviceNames[i]);
                    devices[i].Initialize();
                }

                _devices = devices;
            }

            return _devices;
        }

        /// <summary>
        /// Gets the default playback device
        /// </summary>
        public static ALDevice DefaultDevice
        {
            get { return EnumerateALDevices().FirstOrDefault(); }
        }

        /// <summary>
        /// Disposes the openal device
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the openal device
        /// </summary>
        /// <param name="disposing">The disposing state</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }

            if (_deviceHandle != IntPtr.Zero)
            {
                ALInterops.alcCloseDevice(_deviceHandle);
                _deviceHandle = IntPtr.Zero;
            }
        }
    }
}
