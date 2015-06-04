using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Represents a primary directsound buffer.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [Guid("279AFA85-4981-11CE-A521-0020AF0BE560")]
    public class DirectSoundPrimaryBuffer : DirectSoundBuffer
    {
        private static readonly DSBufferDescription DefaultPrimaryBufferDescription =
            new DSBufferDescription()
            {
                BufferBytes = 0,
                Flags = DSBufferCapsFlags.PrimaryBuffer | DSBufferCapsFlags.ControlVolume,
                Reserved = 0,
                PtrFormat = IntPtr.Zero,
                Guid3DAlgorithm = Guid.Empty
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSoundPrimaryBuffer"/> class.
        /// </summary>
        /// <param name="directSound">A <see cref="DirectSoundBase"/> instance which provides the <see cref="DirectSoundBase.CreateSoundBuffer"/> method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="directSound"/></exception>
        public DirectSoundPrimaryBuffer(DirectSoundBase directSound)
            : this(directSound, DefaultPrimaryBufferDescription)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSoundPrimaryBuffer"/> class.
        /// </summary>
        /// <param name="directSound">A <see cref="DirectSoundBase"/> instance which provides the <see cref="DirectSoundBase.CreateSoundBuffer"/> method.</param>
        /// <param name="bufferDescription">The buffer description which describes the buffer to create.</param>
        /// <exception cref="ArgumentNullException"><paramref name="directSound"/></exception>
        /// <exception cref="ArgumentException">The <paramref name="bufferDescription"/> is invalid.</exception>
        public DirectSoundPrimaryBuffer(DirectSoundBase directSound, DSBufferDescription bufferDescription)
        {
            if (directSound == null)
                throw new ArgumentNullException("directSound");

            if((bufferDescription.Flags & DSBufferCapsFlags.PrimaryBuffer) != DSBufferCapsFlags.PrimaryBuffer)
                throw new ArgumentException("The PrimaryBuffer flag is not set.", "bufferDescription");
            if(bufferDescription.BufferBytes != 0)
                throw new ArgumentException("BufferBytes must be zero.", "bufferDescription");
            bufferDescription.Size = Marshal.SizeOf(bufferDescription);
            if (bufferDescription.PtrFormat != IntPtr.Zero)
                throw new ArgumentException("PtrFormat must be NULL.", "bufferDescription");

            BasePtr = directSound.CreateSoundBuffer(bufferDescription, IntPtr.Zero);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSoundPrimaryBuffer"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public DirectSoundPrimaryBuffer(IntPtr ptr)
            : base(ptr)
        {
        }
    }
}