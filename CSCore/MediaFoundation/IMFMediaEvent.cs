using CSCore.CoreAudioAPI;
using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    [ComImport]
    [Guid("DF598932-F10C-4E39-BBA2-C308F101DAA3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Security.SuppressUnmanagedCodeSecurity]
    public interface IMFMediaEvent : IMFAttributes
    {
        /// <summary>
        /// Retrieves the event type.
        /// </summary>
        void GetType([Out] out MediaEventType mediaType);

        /// <summary>
        /// Retrieves the extended type of the event.
        /// </summary>
        void GetExtendedType([Out] out Guid extendedType);

        /// <summary>
        /// Retrieves an HRESULT that specifies the event status.
        /// </summary>
        void GetStatus([MarshalAs(UnmanagedType.Error)] out int status);

        /// <summary>
        /// Retrieves the value associated with the event, if any.
        /// </summary>
        void GetValue(out PropertyVariant value);
    }
}