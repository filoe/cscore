using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Represents a block of memory that contains media data. Use this interface to access the data in the buffer.
    /// </summary>
    [Guid("045FA593-8799-42b8-BC8D-8968C6453507")]
// ReSharper disable once InconsistentNaming
    public class MFMediaBuffer : ComObject
    {
        private const string InterfaceName = "IMFMediaBuffer";

        /// <summary>
        /// Gets or sets the length of the valid data, in bytes. If the buffer does not contain any valid data, the value is zero.
        /// </summary>
        public int CurrentLength
        {
            get { return GetCurrentLength(); }
            set { SetCurrentLength(value); }
        }

        /// <summary>
        /// Gets the allocated size of the buffer, in bytes.
        /// </summary>
        public int MaxLength
        {
            get { return GetMaxLength(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MFMediaBuffer"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public MFMediaBuffer(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MFMediaBuffer"/> class with the specified maximum <paramref name="size"/>.
        /// </summary>
        /// <param name="size">The size of the <see cref="MFMediaBuffer"/> in bytes. The specified <paramref name="size"/> will be the <see cref="MaxLength"/> of the constructed <see cref="MFMediaBuffer"/>.</param>
        /// <remarks>The caller needs to release the allocated memory by disposing the <see cref="MFMediaBuffer"/>.</remarks>
        public MFMediaBuffer(int size)
            : this(MediaFoundationCore.CreateMemoryBuffer(size))
        {
        }

        /// <summary>
        /// Gives the caller access to the memory in the buffer, for reading or writing.
        /// </summary>
        /// <param name="buffer">Receives a pointer to the start of the buffer.</param>
        /// <param name="maxLength">Receives the maximum amount of data that can be written to the buffer. The same value is returned by the <see cref="GetMaxLength"/> method.</param>
        /// <param name="currentLength">Receives the length of the valid data in the buffer, in bytes. The same value is returned by the <see cref="GetCurrentLength"/> method.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>When you are done accessing the buffer, call <see cref="Unlock"/> to unlock the buffer. You must call <see cref="Unlock"/> once for each call to <see cref="Lock()"/>.</remarks>
        public unsafe int LockNative(out IntPtr buffer, out int maxLength, out int currentLength)
        {
            fixed (void* pbuffer = &buffer, pmaxlength = &maxLength, pcurrentlength = &currentLength)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pbuffer, pmaxlength, pcurrentlength, ((void**)(*(void**)UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        /// Gives the caller access to the memory in the buffer, for reading or writing.
        /// </summary>
        /// <param name="maxLength">Receives the maximum amount of data that can be written to the buffer. The same value is returned by the <see cref="GetMaxLength"/> method.</param>
        /// <param name="currentLength">Receives the length of the valid data in the buffer, in bytes. The same value is returned by the <see cref="GetCurrentLength"/> method.</param>
        /// <returns>A pointer to the start of the buffer.</returns>
        /// <remarks>When you are done accessing the buffer, call <see cref="Unlock"/> to unlock the buffer. You must call <see cref="Unlock"/> once for each call to <see cref="Lock()"/>.</remarks>        
        public IntPtr Lock(out int maxLength, out int currentLength)
        {
            IntPtr p;
            MediaFoundationException.Try(LockNative(out p, out maxLength, out currentLength), InterfaceName, "Lock");
            return p;
        }

        /// <summary>
        /// Gives the caller access to the memory in the buffer, for reading or writing.
        /// </summary>
        /// <returns>A disposable object which provides the information returned by the <see cref="Lock(out int,out int)"/> method. Call its <see cref="LockDisposable.Dispose"/> method to unlock the <see cref="MFMediaBuffer"/>.</returns>
        /// <example>
        /// This example shows how to use the <see cref="Lock()"/> method:
        /// <code>
        /// partial class TestClass
        /// {
        /// 	public void DoStuff(MFMediaBuffer mediaBuffer)
        /// 	{
        /// 		using(var lock = mediaBuffer.Lock())
        /// 		{
        /// 			//do some stuff
        /// 		}
        /// 		//the mediaBuffer gets automatically unlocked by the using statement after "doing your stuff"
        /// 	}
        /// }
        /// </code>
        /// </example>
        public LockDisposable Lock()
        {
            int currentLength;
            int maxLength;
            return new LockDisposable(this, Lock(out maxLength, out currentLength), maxLength, currentLength);
        }

        /// <summary>
        /// Unlocks a buffer that was previously locked. Call this method once for every call to <see cref="Lock(out int,out int)"/>.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int UnlockNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[4]);
        }

        /// <summary>
        /// Unlocks a buffer that was previously locked. Call this method once for every call to <see cref="Lock(out int,out int)"/>.
        /// </summary>
        public void Unlock()
        {
            MediaFoundationException.Try(UnlockNative(), InterfaceName, "Unlock");
        }

        /// <summary>
        /// Retrieves the length of the valid data in the buffer.
        /// <seealso cref="CurrentLength"/>
        /// </summary>
        /// <param name="currentLength">Receives the length of the valid data, in bytes. If the buffer does not contain any valid data, the value is zero.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetCurrentLengthNative(out int currentLength)
        {
            fixed (void* ptr = &currentLength)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        /// Retrieves the length of the valid data in the buffer.
        /// <seealso cref="CurrentLength"/>        
        /// </summary>
        /// <returns>The length of the valid data, in bytes. If the buffer does not contain any valid data, the value is zero.</returns>
        public int GetCurrentLength()
        {
            int res;
            MediaFoundationException.Try(GetCurrentLengthNative(out res), InterfaceName, "GetCurrentLength");
            return res;
        }

        /// <summary>
        /// Sets the length of the valid data in the buffer.
        /// </summary>
        /// <seealso cref="CurrentLength"/>        
        /// <param name="currentLength">Length of the valid data, in bytes. This value cannot be greater than the allocated size of the buffer, which is returned by the <see cref="GetMaxLength"/> method.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetCurrentLengthNative(int currentLength)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, currentLength, ((void**)(*(void**)UnsafeBasePtr))[6]);
        }

        /// <summary>
        /// Sets the length of the valid data in the buffer.
        /// </summary>
        /// <seealso cref="CurrentLength"/>        
        /// <param name="currentLength">Length of the valid data, in bytes. This value cannot be greater than the allocated size of the buffer, which is returned by the <see cref="GetMaxLength"/> method.</param>
        public void SetCurrentLength(int currentLength)
        {
            MediaFoundationException.Try(SetCurrentLengthNative(currentLength), InterfaceName, "SetCurrentLength");
        }

        /// <summary>
        /// Retrieves the allocated size of the buffer.
        /// <seealso cref="MaxLength"/>
        /// </summary>
        /// <param name="maxlength">Receives the allocated size of the buffer, in bytes.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMaxLengthNative(out int maxlength)
        {
            fixed (void* ptr = &maxlength)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[7]);
            }
        }

        /// <summary>
        /// Retrieves the allocated size of the buffer.
        /// <seealso cref="MaxLength"/>        
        /// </summary>
        /// <returns>The allocated size of the buffer, in bytes.</returns>
        public int GetMaxLength()
        {
            int maxlength;
            MediaFoundationException.Try(GetMaxLengthNative(out maxlength), InterfaceName, "GetMaxLength");
            return maxlength;
        }

        /// <summary>
        /// Used to unlock a <see cref="MFMediaBuffer"/> after locking it by calling the <see cref="MFMediaBuffer.Lock()"/> method.
        /// </summary>
        public class LockDisposable : IDisposable
        {
            private readonly MFMediaBuffer _mediaBuffer;
            private bool _disposed;

            /// <summary>
            /// Gets a pointer to the start of the buffer.
            /// </summary>
            public IntPtr Buffer { get; private set; }

            /// <summary>
            /// Gets the maximum amount of data that can be written to the buffer.
            /// </summary>
            public int MaxLength { get; private set; }

            /// <summary>
            /// Gets the length of the valid data in the buffer, in bytes.
            /// </summary>
            public int CurrentLength { get; private set; }

            internal LockDisposable(MFMediaBuffer mediaBuffer, IntPtr buffer, int maxLength, int currentLength)
            {
                _mediaBuffer = mediaBuffer;
                Buffer = buffer;
                MaxLength = maxLength;
                CurrentLength = currentLength;
            }

            /// <summary>
            /// Unlocks the locked <see cref="MFMediaBuffer"/>.
            /// </summary>
            public void Dispose()
            {
                if (!_disposed)
                {
                    _mediaBuffer.Dispose();
                    _disposed = true;
                }
            }
        }
    }
}