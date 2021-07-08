using System;

namespace CSCore.SoundOut.AL
{
    internal class ALSource : IDisposable
    {
        /// <summary>
        /// Gets the openal source id
        /// </summary>
        public uint Id { private set; get; }

        private readonly ALDevice _device;

        /// <summary>
        /// Initializes a new ALSource class
        /// </summary>
        /// <param name="device">The device</param>
        /// <param name="sourceId">The source id</param>
        public ALSource(ALDevice device, uint sourceId)
        {
            Id = sourceId;
            _device = device;
        }

        ~ALSource()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the openal source
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the openal source
        /// </summary>
        /// <param name="disposing">The disposing state</param>
        protected void Dispose(bool disposing)
        {
            _device.DeleteALSource(this);
        }
    }
}
