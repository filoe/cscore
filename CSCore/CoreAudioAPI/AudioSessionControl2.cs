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
    /// IAudioSessionControl2
    /// </summary>
    [Guid("BFB7FF88-7239-4FC9-8FA2-07C950BE9C6D")]
    public class AudioSessionControl2 : AudioSessionControl
    {
        const string c = "IAudioSessionControl2";
        const int AUDCLNT_S_NO_SINGLE_PROCESS = 0x889000d;

        public AudioSessionControl2(IntPtr ptr)
            : base(ptr)
		{
		}

        /// <summary>
        /// Gets the session identifier. See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368252(v=vs.85).aspx
        /// </summary>
        public string SessionIdentifier
        {
            get
            {
                string str;
                CoreAudioAPIException.Try(GetSessionIdentifierNative(out str), c, "GetSessionIdentifier");
                return str;
            }
        }

        /// <summary>
        /// Gets the identifier of the audio session instance. See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368255(v=vs.85).aspx.
        /// </summary>
        public string SessionIstanceIdentifier
        {
            get
            {
                string str;
                CoreAudioAPIException.Try(GetSessionInstanceIdentifierNative(out str), c, "GetSessionInstanceIdentifier");
                return str;
            }
        }

        /// <summary>
        /// Gets the process identifier of the audio session.
        /// In the case of that the session is no SingleProcessSession (see <see cref="IsSingleProcessSession"/>), the ProcessID is the initial identifier of the process that created the session.
        /// </summary>
        public int ProcessID
        {
            get
            {
                int processID;
                int result = GetProcessIdNative(out processID);
                if (result != AUDCLNT_S_NO_SINGLE_PROCESS)
                    CoreAudioAPIException.Try(result, c, "GetProcessId");

                return processID;
            }
        }

        /// <summary>
        /// Indicates whether the session spans more than one process. 
        /// </summary>
        public bool IsSingleProcessSession
        {
            get
            {
                int processID;
                int result = GetProcessIdNative(out processID);
                return result != AUDCLNT_S_NO_SINGLE_PROCESS;
            }
        }

        /// <summary>
        /// Gets the process of the audio session.
        /// In the case of that the session is no SingleProcessSession (see <see cref="IsSingleProcessSession"/>), the Process is the process that created the session.
        /// If the process that created the session is not available anymore, the returnvalue is null.
        /// </summary>
        public Process Process
        {
            get
            {
                try
                {
                    return System.Diagnostics.Process.GetProcessById(ProcessID);
                }
                catch (ArgumentException)
                {
                    return null; 
                }
            }
        }

        /// <summary>
        /// Indicates whether the session is a system sounds session.
        /// </summary>
        public bool IsSystemSoundSession
        {
            get 
            {
                int result = IsSystemSoundSessionNative();
                if (result == (int)HResult.S_OK)
                    return true;
                else if (result == (int)HResult.S_FALSE)
                    return false;
                else
                    CoreAudioAPIException.Try(result, c, "IsSystemSoundSession");

                return false;
            }
        }

        /// <summary>
        /// Gets the session identifier. See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368252(v=vs.85).aspx
        /// </summary>
        /// <param name="sessionID">Audio session identifier.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSessionIdentifierNative(out string sessionID)
        {
            sessionID = null;
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(_basePtr, &ptr, ((void**)(*(void**)_basePtr))[12]);
            if (result == 0 && ptr != IntPtr.Zero)
            {
                sessionID = Marshal.PtrToStringUni(ptr);
                Marshal.FreeCoTaskMem(ptr);
            }

            return result;
        }

        /// <summary>
        /// Gets the identifier of the audio session instance. See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368255(v=vs.85).aspx.
        /// </summary>
        /// <param name="sessionID">Identifier of a particular instance of the audio session.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSessionInstanceIdentifierNative(out string sessionID)
        {
            sessionID = null;
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(_basePtr, &ptr, ((void**)(*(void**)_basePtr))[13]);
            if (result == 0 && ptr != IntPtr.Zero)
            {
                sessionID = Marshal.PtrToStringUni(ptr);
                Marshal.FreeCoTaskMem(ptr);
            }

            return result;
        } 

        /// <summary>
        /// Gets the process identifier of the audio session.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetProcessIdNative(out int processID)
        {
            fixed (void* p = &processID)
            {
                return InteropCalls.CallI(_basePtr, p, ((void**)(*(void**)_basePtr))[14]);
            }
        }

        /// <summary>
        /// The IsSystemSoundsSession method indicates whether the session is a system sounds session.
        /// </summary>
        /// <returns>HRESULT; S_OK = true, S_FALSE = false</returns>
        public unsafe int IsSystemSoundSessionNative()
        {
            return InteropCalls.CallI(_basePtr, ((void**)(*(void**)_basePtr))[15]);
        }

        /// <summary>
        /// The SetDuckingPreference method enables or disables the default stream attenuation experience (auto-ducking) provided by the system.
        /// </summary>
        /// <param name="enableSystemAutoDucking">A BOOL variable that enables or disables system auto-ducking.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetDuckingPreferenceNative(NativeBool enableSystemAutoDucking)
        {
            return InteropCalls.CallI(_basePtr, enableSystemAutoDucking, ((void**)(*(void**)_basePtr))[16]);
        }

        /// <summary>
        /// The SetDuckingPreference method enables or disables the default stream attenuation experience (auto-ducking) provided by the system.
        /// </summary>
        /// <param name="enableSystemAutoDucking">A BOOL variable that enables or disables system auto-ducking.</param>
        public void SetDuckingPreference(bool enableSystemAutoDucking)
        {
            CoreAudioAPIException.Try(SetDuckingPreferenceNative(enableSystemAutoDucking), c, "SetDuckingPreferenceNative");
        }
    }
}
