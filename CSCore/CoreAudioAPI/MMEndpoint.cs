using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Represents an audio endpoint device
    /// (see also <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370793(v=vs.85).aspx"/>).
    /// </summary>
    [Guid("1BE09788-6894-4089-8586-9A2A6C265AC5")]
    // ReSharper disable once InconsistentNaming
    public class MMEndpoint : ComObject
    {
        private const string InterfaceName = "IMMEndpoint";

        /// <summary>
        /// Initializes a new instance of the <see cref="MMEndpoint"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        /// <remarks>Obtain an instance of the <see cref="MMEndpoint"/> by using the <see cref="MMEndpoint(MMDevice)"/> constructor.
        /// </remarks>
        public MMEndpoint(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MMEndpoint"/> class based on an 
        /// <see cref="MMDevice"/> by calling its <see cref="ComObject.QueryInterface{T}"/> method.
        /// </summary>
        /// <param name="device">The <see cref="MMDevice"/> used to obtain an <see cref="MMEndpoint"/> instance.</param>
        /// <exception cref="System.ArgumentNullException">device</exception>
        public MMEndpoint(MMDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            BasePtr = device.QueryInterfacePtr(typeof (MMEndpoint).GUID);
        }

        /// <summary>
        /// Gets the data flow of the associated device.
        /// </summary>
        /// <value>
        /// The data flow of the associated device.
        /// </value>
        public DataFlow DataFlow
        {
            get
            {
                DataFlow result;
                CoreAudioAPIException.Try(GetDataFlowNative(out result), InterfaceName, "GetDataFlow");
                return result;
            }
        }

        /// <summary>	
        /// Indicates whether the endpoint is associated with a rendering device or a capture device.
        /// </summary>
        /// <param name="dataFlow">A variable into which the method writes the data-flow direction of the endpoint device.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>Use the <see cref="DataFlow"/> property instead.</remarks>
        public unsafe int GetDataFlowNative(out DataFlow dataFlow)
        {
            fixed (void* p = &dataFlow)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p, ((void**) *(void**) UnsafeBasePtr)[3]);
            }
        }
    }
}
