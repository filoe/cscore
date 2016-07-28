using System;
using System.Collections.Generic;
using System.Threading;

namespace CSCore.SoundOut.AL
{
    // ReSharper disable once InconsistentNaming    
    /// <summary>
    /// Represents an OpenAL Context.
    /// </summary>
    public class ALContext : IDisposable
    {
        private static readonly Dictionary<ALDevice, ContextRef> ContextDictionary = new Dictionary<ALDevice, ContextRef>();
        private static readonly object ContextDictionaryLockObj = new object();

        private readonly ALDevice _device;

        /// <summary>
        /// Gets the current context handle.
        /// </summary>
        public static IntPtr CurrentContextHandle
        {
            get { return ALInterops.alcGetCurrentContext(); }
        }

        /// <summary>
        /// Gets the handle of the context.
        /// </summary>
        public IntPtr Handle { private set; get; }

        /// <summary>
        /// Gets a value indicating whether 32 bit float audio is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if 32 bit float audio is supported; otherwise, <c>false</c>.
        /// </value>
        public bool Supports32Float
        {
            get
            {
                using (LockContext())
                {
                    return ALInterops.IsExtensionPresent("AL_EXT_float32");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ALContext"/> class.
        /// </summary>
        /// <param name="device">The device to create a context for.</param>
        /// <exception cref="System.ArgumentNullException">device</exception>
        /// <exception cref="ALException">Could not create ALContext.</exception>
        public ALContext(ALDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");
            _device = device;

            lock (ContextDictionaryLockObj)
            {
                if (ContextDictionary.ContainsKey(device))
                {
                    Handle = ContextDictionary[device].Context;
                    ContextDictionary[device].RefCount++;
                }
                else
                {
                    Handle = ALInterops.alcCreateContext(device.DeviceHandle, IntPtr.Zero);
                    if (Handle == IntPtr.Zero)
                        throw new ALException("Could not create ALContext.", ALInterops.alcGetError(device.DeviceHandle));

                    ContextDictionary.Add(device, new ContextRef()
                    {
                        Context = Handle,
                        RefCount = 1
                    });
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ALContext" /> class.
        /// </summary>
        /// <param name="contextHandle">The handle of the context.</param>
        /// <exception cref="System.ArgumentNullException">contextHandle</exception>
        public ALContext(IntPtr contextHandle)
        {
            if (contextHandle == IntPtr.Zero)
                throw new ArgumentNullException("contextHandle");

            Handle = contextHandle;
        }

        /// <summary>
        /// Makes the context the current context.
        /// </summary>
        public void MakeCurrent()
        {
            ALInterops.alcGetError(_device.DeviceHandle);
            if (!ALInterops.alcMakeContextCurrent(Handle))
            {
                var error = ALInterops.alcGetError(_device.DeviceHandle);
                throw new ALException("Could not set context. alcMakeContextCurrent returned " + error, error);
            }
        }

        /// <summary>
        /// Makes the context the current context and locks it.
        /// To unlock the current context, call the <see cref="ContextLock.Dispose"/> method
        /// of the returned <see cref="ContextLock"/> object.
        /// </summary>
        /// <returns>
        /// A <see cref="ContextLock"/> object which represents the locked context.
        /// Call its <see cref="ContextLock.Dispose"/> method to release/unlock the context.
        /// </returns>
        public ContextLock LockContext()
        {
            return new ContextLock(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ALContext"/> class.
        /// </summary>
        ~ALContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the <see cref="ALContext"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the <see cref="ALContext"/>.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (Handle != IntPtr.Zero)
            {
                lock (ContextDictionaryLockObj)
                {
                    ContextDictionary[_device].RefCount--;
                    if (ContextDictionary[_device].RefCount <= 0)
                    {
                        ALException.Try(
                            () =>
                                ALInterops.alcDestroyContext(Handle),
                            "alcDestroyContext", _device.DeviceHandle);
                        ContextDictionary.Remove(_device);
                    }

                    Handle = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Represents a locked Context.
        /// </summary>
        public sealed class ContextLock : IDisposable
        {
            private static readonly object ContextLockObj = new object();
            private static int _lockLevel;
            private static readonly bool ResetContext;

            static ContextLock()
            {
                ResetContext =
                    Environment.OSVersion.Platform != PlatformID.Win32NT &&
                    Environment.OSVersion.Platform != PlatformID.Win32S &&
                    Environment.OSVersion.Platform != PlatformID.Win32Windows &&
                    Environment.OSVersion.Platform != PlatformID.WinCE;
            }

            /// <summary>
            /// Gets the locked context.
            /// </summary>
            public ALContext Context { get; private set; }

            internal ContextLock(ALContext context)
            {
                Monitor.Enter(ContextLockObj);
                _lockLevel++;

                if(CurrentContextHandle != context.Handle)
                    context.MakeCurrent();
                Context = context;
            }

            /// <summary>
            /// Unlocks the locked Context.
            /// </summary>
            public void Dispose()
            {
                _lockLevel--;
                if (_lockLevel == 0 && ResetContext)
                    ALInterops.alcMakeContextCurrent(IntPtr.Zero);
                Monitor.Exit(ContextLockObj);
            }
        }

        private class ContextRef
        {
            public IntPtr Context;
            public int RefCount;
        }
    }
}
