using System;

namespace CSCore.SoundOut.AL
{
    internal class ALContext : IDisposable
    {
        /// <summary>
        /// Gets the handle
        /// </summary>
        public IntPtr Handle { private set; get; }

        /// <summary>
        /// Initializes a new ALContext class
        /// </summary>
        /// <param name="contextHandle">The handle</param>
        private ALContext(IntPtr contextHandle)
        {
            Handle = contextHandle;
        }

        /// <summary>
        /// Makes the context the current context
        /// </summary>
        public void MakeCurrent()
        {
            ALInterops.alcMakeContextCurrent(Handle);
        }

        /// <summary>
        /// Deconstructs the ALContext class
        /// </summary>
        ~ALContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the openal context
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the openal context
        /// </summary>
        /// <param name="disposing">The disposing state</param>
        protected void Dispose(bool disposing)
        {
            if (Handle != IntPtr.Zero)
            {
                ALInterops.alcDestroyContext(Handle);
                Handle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Creates a new openal context
        /// </summary>
        /// <param name="deviceHandle">The device handle</param>
        /// <returns>OpenALContext</returns>
        public static ALContext CreateContext(IntPtr deviceHandle)
        {
            return new ALContext(ALInterops.alcCreateContext(deviceHandle, IntPtr.Zero));
        }
    }
}
