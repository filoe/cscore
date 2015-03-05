using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="AudioSessionControl2"/> class can be used by a client to get information about the audio session.
    /// For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd368248%28v=vs.85%29.aspx"/>.
    /// </summary>
    [Guid("BFB7FF88-7239-4FC9-8FA2-07C950BE9C6D")]
    public class AudioSessionControl2 : AudioSessionControl
    {
        private const string InterfaceName = "IAudioSessionControl2";
        private const int AUDCLNT_S_NO_SINGLE_PROCESS = 0x889000d;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionControl2"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer to the IAudioSessionControl2 object.</param>
        public AudioSessionControl2(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <remarks>For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd368252(v=vs.85).aspx"/>.</remarks>
        public string SessionIdentifier
        {
            get
            {
                string str;
                CoreAudioAPIException.Try(GetSessionIdentifierNative(out str), InterfaceName, "GetSessionIdentifier");
                return str;
            }
        }

        /// <summary>
        /// Gets the identifier of the audio session instance.
        /// </summary>
        /// <remarks>For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd368255(v=vs.85).aspx"/>.</remarks>        
        public string SessionInstanceIdentifier
        {
            get
            {
                string str;
                CoreAudioAPIException.Try(GetSessionInstanceIdentifierNative(out str), InterfaceName, "GetSessionInstanceIdentifier");
                return str;
            }
        }

        /// <summary>
        /// Gets the process identifier of the audio session.
        /// In the case of that the session is no single-process-session (see <see cref="IsSingleProcessSession"/>), the <see cref="ProcessID"/> is the initial identifier of the process that created the session.
        /// </summary>
        public int ProcessID
        {
            get
            {
                int processId;
                int result = GetProcessIdNative(out processId);
                if (result != AUDCLNT_S_NO_SINGLE_PROCESS)
                    CoreAudioAPIException.Try(result, InterfaceName, "GetProcessId");

                return processId;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the session spans more than one process. If <c>True</c>, the session spans more than one process; If <c>False</c> otherwise. 
        /// </summary>
        public bool IsSingleProcessSession
        {
            get
            {
                int processId;
                int result = GetProcessIdNative(out processId);
                return result != AUDCLNT_S_NO_SINGLE_PROCESS;
            }
        }

        /// <summary>
        /// Gets the process of the audio session.
        /// In the case of that the session is no SingleProcessSession (see <see cref="IsSingleProcessSession"/>), the Process is the process that created the session.
        /// If the process that created the session is not available anymore, the value is null.
        /// </summary>
        public Process Process
        {
            get
            {
                try
                {
                    return Process.GetProcessById(ProcessID);
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the session is a system sounds session. If <c>True</c>, the session is a system sound session; If <C>False</C> otherwise.
        /// </summary>
        public bool IsSystemSoundSession
        {
            get
            {
                int result = IsSystemSoundSessionNative();
                if (result == (int) HResult.S_OK)
                    return true;
                if (result == (int) HResult.S_FALSE)
                    return false;
                CoreAudioAPIException.Try(result, InterfaceName, "IsSystemSoundSession");

                return false;
            }
        }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <seealso cref="SessionIdentifier"/>
        /// <param name="sessionId">A variable which retrieves the session identifier.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSessionIdentifierNative(out string sessionId)
        {
            sessionId = null;
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(UnsafeBasePtr, &ptr, ((void**) (*(void**) UnsafeBasePtr))[12]);
            try
            {
                if (result == 0 && ptr != IntPtr.Zero)
                    sessionId = Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);                
            }

            return result;
        }

        /// <summary>
        /// Gets the identifier of the audio session instance.
        /// <seealso cref="SessionInstanceIdentifier"/>
        /// </summary>
        /// <param name="sessionInstanceId">A variable which retrieves the identifier of a particular instance of the audio session.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSessionInstanceIdentifierNative(out string sessionInstanceId)
        {
            sessionInstanceId = null;
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(UnsafeBasePtr, &ptr, ((void**) (*(void**) UnsafeBasePtr))[13]);
            if (result == 0 && ptr != IntPtr.Zero)
            {
                sessionInstanceId = Marshal.PtrToStringUni(ptr);
                Marshal.FreeCoTaskMem(ptr);
            }

            return result;
        }

        /// <summary>
        /// Gets the process identifier of the audio session.
        /// <seealso cref="ProcessID"/>
        /// </summary>
        /// <param name="processId">A variable which receives the process id of the audio session.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetProcessIdNative(out int processId)
        {
            fixed (void* p = &processId)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[14]);
            }
        }

        /// <summary>
        /// Indicates whether the session is a system sounds session.
        /// <seealso cref="IsSystemSoundSession"/>
        /// </summary>
        /// <returns>HRESULT; S_OK = true, S_FALSE = false</returns>
        public unsafe int IsSystemSoundSessionNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[15]);
        }

        /// <summary>
        /// Enables or disables the default stream attenuation experience (auto-ducking) provided by the system.
        /// </summary>
        /// <param name="enableSystemAutoDucking">A <see cref="NativeBool"/> variable that enables or disables system auto-ducking.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetDuckingPreferenceNative(NativeBool enableSystemAutoDucking)
        {
            return InteropCalls.CallI(UnsafeBasePtr, enableSystemAutoDucking, ((void**) (*(void**) UnsafeBasePtr))[16]);
        }

        /// <summary>
        /// Enables or disables the default stream attenuation experience (auto-ducking) provided by the system.
        /// </summary>
        /// <param name="enableSystemAutoDucking">A <see cref="NativeBool"/> variable that enables or disables system auto-ducking.</param>
        public void SetDuckingPreference(bool enableSystemAutoDucking)
        {
            CoreAudioAPIException.Try(SetDuckingPreferenceNative(enableSystemAutoDucking), InterfaceName,
                "SetDuckingPreferenceNative");
        }
    }
}