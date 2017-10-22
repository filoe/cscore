using System;
using System.Runtime.InteropServices;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Describes the characteristics of a new buffer object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
// ReSharper disable once InconsistentNaming
    public struct DSBufferDescription
    {
        /// <summary>
        /// Size of the structure, in bytes. This member must be initialized before the structure is used.
        /// </summary>
        /// <remarks>Use the <see cref="Marshal.SizeOf(object)"/> or the <see cref="Marshal.SizeOf(Type)"/> method to </remarks>
        public int Size;

        /// <summary>
        /// Flags specifying the capabilities of the buffer.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public DSBufferCapsFlags Flags;

        /// <summary>
        /// Size of the new buffer, in bytes. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.reference.dsbufferdesc%28v=vs.85%29.aspx"/>.
        /// </summary>
        /// <remarks>Must be a value between 4 and 0x0FFFFFFF.</remarks>
        public int BufferBytes;
        internal int Reserved;
        /// <summary>
        /// Address of a <see cref="WaveFormat"/> or <see cref="WaveFormatExtensible"/> class specifying the waveform format for the buffer. This value must be <see cref="IntPtr.Zero"/> for primary buffers.
        /// </summary>
        public IntPtr PtrFormat;
        /// <summary>
        /// Unique identifier of the two-speaker virtualization algorithm to be used by DirectSound3D hardware emulation. If <see cref="DSBufferCapsFlags.Control3D"/> is not set in <see cref="Flags"/>, this member must be <see cref="Guid.Empty"/>.
        /// For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.reference.dsbufferdesc%28v=vs.85%29.aspx"/>.
        /// </summary>
        public Guid Guid3DAlgorithm;
    }
}