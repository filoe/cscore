using System.IO;
using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Implemented by the Microsoft Media Foundation source reader object.
    /// </summary>
    [Guid("70ae66f2-c809-4e4f-8915-bdcb406b7993")]
    public class MFSourceReader : ComObject
    {
        private const string InterfaceName = "IMFSourceReader";

        /// <summary>
        /// Initializes a new instance of the <see cref="MFSourceReader"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public MFSourceReader(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MFSourceReader"/> class based on a given <paramref name="url"/>.
        /// </summary>
        /// <param name="url">The URL.</param>
        public MFSourceReader(string url)
            : this(MediaFoundationCore.CreateSourceReaderFromUrlNative(url))
        {
        }

        /// <summary>
        /// Gets a value indicating whether this instance can seek.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can seek; otherwise, <c>false</c>.
        /// </value>
        public bool CanSeek
        {
            get { return (MediaSourceCharacteristics & MFMediaSourceCharacteristics.CanSeek) == MFMediaSourceCharacteristics.CanSeek; }
        }

        /// <summary>
        /// Gets the media source characteristics.
        /// </summary>
        /// <value>
        /// The media source characteristics.
        /// </value>
        public MFMediaSourceCharacteristics MediaSourceCharacteristics
        {
            get { return (MFMediaSourceCharacteristics)GetSourceFlags(); }
        }

        internal int GetSourceFlags()
        {
            using (
                var value = GetPresentationAttribute(NativeMethods.MF_SOURCE_READER_MEDIASOURCE,
                    MediaFoundationAttributes.MF_SOURCE_READER_MEDIASOURCE_CHARACTERISTICS))
            {

                return (int) value.UIntValue;
            }
        }

        /// <summary>
        /// Queries whether a stream is selected.
        /// </summary>
        /// <param name="streamIndex">The stream to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374664(v=vs.85).aspx"/>.</param>
        /// <param name="selectedRef">Receives <see cref="NativeBool.True"/> if the stream is selected and will generate data. Receives <see cref="NativeBool.False"/> if the stream is not selected and will not generate data.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetStreamSelectionNative(int streamIndex, out NativeBool selectedRef)
        {
            fixed (NativeBool* p = (&selectedRef))
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, (IntPtr*)p, ((void**)(*(void**)UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        /// Queries whether a stream is selected. 
        /// </summary>
        /// <param name="streamIndex">The stream to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374664(v=vs.85).aspx"/>.</param>
        /// <returns><see cref="NativeBool.True"/> if the stream is selected and will generate data; <see cref="NativeBool.False"/> if the stream is not selected and will not generate data.</returns>        
        public NativeBool GetStreamSelection(int streamIndex)
        {
            NativeBool result;
            MediaFoundationException.Try(GetStreamSelectionNative(streamIndex, out result), InterfaceName, "GetStreamSelection");
            return result;
        }

        /// <summary>
        /// Selects or deselects one or more streams.
        /// </summary>
        /// <param name="streamIndex">The stream to set. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374669(v=vs.85).aspx"/>.</param>
        /// <param name="selected">Specify <see cref="NativeBool.True"/> to select streams or <see cref="NativeBool.False"/> to deselect streams. If a stream is deselected, it will not generate data.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetStreamSelectionNative(int streamIndex, NativeBool selected)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, selected, ((void**)(*(void**)UnsafeBasePtr))[4]);
        }

        /// <summary>
        /// Selects or deselects one or more streams.
        /// </summary>
        /// <param name="streamIndex">The stream to set. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374669(v=vs.85).aspx"/>.</param>
        /// <param name="selected">Specify <see cref="NativeBool.True"/> to select streams or <see cref="NativeBool.False"/> to deselect streams. If a stream is deselected, it will not generate data.</param>                
        public void SetStreamSelection(int streamIndex, NativeBool selected)
        {
            MediaFoundationException.Try(SetStreamSelectionNative(streamIndex, selected), InterfaceName, "SetStreamSelection");
        }

        /// <summary>
        /// Gets a format that is supported natively by the media source.
        /// </summary>
        /// <param name="streamIndex">Specifies which stream to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374661(v=vs.85).aspx"/>.</param>
        /// <param name="mediatypeIndex">The zero-based index of the media type to retrieve.</param>
        /// <param name="mediaType">Receives the <see cref="MFMediaType"/>. The caller must dispose the object.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetNativeMediaTypeNative(int streamIndex, int mediatypeIndex, out MFMediaType mediaType)
        {
            IntPtr ptr = IntPtr.Zero;
            var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, mediatypeIndex, &ptr, ((void**)(*(void**)UnsafeBasePtr))[5]);
            mediaType = new MFMediaType(ptr);
            return result;
        }

        /// <summary>
        /// Gets a format that is supported natively by the media source.
        /// </summary>
        /// <param name="streamIndex">Specifies which stream to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374661(v=vs.85).aspx"/>.</param>
        /// <param name="mediatypeIndex">The zero-based index of the media type to retrieve.</param>
        /// <returns>The <see cref="MFMediaType"/>. The caller must dispose the <see cref="MFMediaType"/>.</returns>
        public MFMediaType GetNativeMediaType(int streamIndex, int mediatypeIndex)
        {
            MFMediaType res;
            MediaFoundationException.Try(GetNativeMediaTypeNative(streamIndex, mediatypeIndex, out res), InterfaceName, "GetNativeMediaType");
            return res;
        }

        /// <summary>
        /// Gets the current media type for a stream.
        /// </summary>
        /// <param name="streamIndex">Specifies which stream to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374660(v=vs.85).aspx"/>.</param>
        /// <param name="mediaType">Receives the <see cref="MFMediaType"/>. The caller must dispose the <see cref="MFMediaType"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetCurrentMediaTypeNative(int streamIndex, out MFMediaType mediaType)
        {
            IntPtr ptr = IntPtr.Zero;
            var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, &ptr, ((void**)(*(void**)UnsafeBasePtr))[6]);
            mediaType = new MFMediaType(ptr);
            return result;
        }

        /// <summary>
        /// Gets the current media type for a stream.
        /// </summary>
        /// <param name="streamIndex">Specifies which stream to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374660(v=vs.85).aspx" />.</param>
        /// <returns>The <see cref="MFMediaType"/>. The caller must dispose the <see cref="MFMediaType"/>.</returns>
         public MFMediaType GetCurrentMediaType(int streamIndex)
        {
            MFMediaType res;
            MediaFoundationException.Try(GetCurrentMediaTypeNative(streamIndex, out res), InterfaceName, "GetCurrentMediaType");
            return res;
        }

        /// <summary>
        /// Sets the media type for a stream.
        /// This media type defines the format that the <see cref="MFSourceReader"/> produces as output. It can differ from the native format provided by the media source. See Remarks for more information.
        /// </summary>
         /// <param name="streamIndex">The stream to configure. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374667(v=vs.85).aspx"/>.</param>
         /// <param name="reserved">Reserved. Set to <see cref="IntPtr.Zero"/>.</param>
         /// <param name="mediaType">The media type to set.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetCurrentMediaTypeNative(int streamIndex, IntPtr reserved, MFMediaType mediaType)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, reserved, (void*)((mediaType == null) ? IntPtr.Zero : mediaType.BasePtr), ((void**)(*(void**)UnsafeBasePtr))[7]);
        }

        /// <summary>
        /// Sets the media type for a stream.
        /// This media type defines the format that the <see cref="MFSourceReader"/> produces as output. It can differ from the native format provided by the media source. See Remarks for more information.
        /// </summary>
        /// <param name="streamIndex">The stream to configure. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374667(v=vs.85).aspx"/>.</param>
        /// <param name="mediaType">The media type to set.</param>
        public void SetCurrentMediaType(int streamIndex, MFMediaType mediaType)
        {
            MediaFoundationException.Try(SetCurrentMediaTypeNative(streamIndex, IntPtr.Zero, mediaType), InterfaceName, "SetCurrentMediaType");
        }

        /// <summary>
        /// Seeks to a new position in the media source.
        /// </summary>
        /// <param name="guidTimeFormat">A GUID that specifies the time format. The time format defines the units for the varPosition parameter. Pass <see cref="Guid.Empty"/> for "100-nanosecond units". Some media sources might support additional values.</param>
        /// <param name="position">The position from which playback will be started. The units are specified by the <paramref name="guidTimeFormat"/> parameter. If the <paramref name="guidTimeFormat"/> parameter is <see cref="Guid.Empty"/>, set the variant type to <see cref="VarEnum.VT_I8"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetCurrentPositionNative(Guid guidTimeFormat, PropertyVariant position)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &guidTimeFormat, &position, ((void**)(*(void**)UnsafeBasePtr))[8]);
        }

        /// <summary>
        /// Seeks to a new position in the media source.
        /// </summary>
        /// <param name="guidTimeFormat">A GUID that specifies the time format. The time format defines the units for the varPosition parameter. Pass <see cref="Guid.Empty"/> for "100-nanosecond units". Some media sources might support additional values.</param>
        /// <param name="position">The position from which playback will be started. The units are specified by the <paramref name="guidTimeFormat"/> parameter. If the <paramref name="guidTimeFormat"/> parameter is <see cref="Guid.Empty"/>, set the variant type to <see cref="VarEnum.VT_I8"/>.</param>
        public void SetCurrentPosition(Guid guidTimeFormat, PropertyVariant position)
        {
            MediaFoundationException.Try(SetCurrentPositionNative(guidTimeFormat, position), InterfaceName, "SetCurrentPosition");
        }

        /// <summary>
        /// Reads the next sample from the media source.
        /// </summary>
        /// <param name="streamIndex">Index of the stream.The stream to pull data from. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374665(v=vs.85).aspx"/>.</param>
        /// <param name="controlFlags">A bitwise OR of zero or more flags from the <see cref="SourceReaderControlFlags"/> enumeration.</param>
        /// <param name="actualStreamIndex">Receives the zero-based index of the stream.</param>
        /// <param name="streamFlags">Receives a bitwise OR of zero or more flags from the <see cref="MFSourceReaderFlags"/> enumeration.</param>
        /// <param name="timestamp">Receives the time stamp of the sample, or the time of the stream event indicated in <paramref name="streamFlags"/>. The time is given in 100-nanosecond units.</param>
        /// <param name="sample">Receives the <see cref="MFSample"/> instance or null. If this parameter receives a non-null value, the caller must release the received <see cref="MFSample"/>.</param>
        /// <returns>
        /// HRESULT
        /// </returns>
        public unsafe int ReadSampleNative(int streamIndex, int controlFlags, out int actualStreamIndex, out MFSourceReaderFlags streamFlags, out long timestamp, out MFSample sample)
        {
            IntPtr psample = IntPtr.Zero;
            fixed (void* ptr0 = &actualStreamIndex, ptr1 = &streamFlags, ptr2 = &timestamp)
            {
                int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, controlFlags, ptr0, ptr1, ptr2, &psample, ((void**)(*(void**)UnsafeBasePtr))[9]);
                sample = psample == IntPtr.Zero ? null : new MFSample(psample);
                return result;
            }
        }

        /// <summary>
        /// Reads the next sample from the media source.
        /// </summary>
        /// <param name="streamIndex">Index of the stream.The stream to pull data from. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374665(v=vs.85).aspx"/>.</param>
        /// <param name="controlFlags">A bitwise OR of zero or more flags from the <see cref="SourceReaderControlFlags"/> enumeration.</param>
        /// <param name="actualStreamIndex">Receives the zero-based index of the stream.</param>
        /// <param name="streamFlags">Receives a bitwise OR of zero or more flags from the <see cref="MFSourceReaderFlags"/> enumeration.</param>
        /// <param name="timestamp">Receives the time stamp of the sample, or the time of the stream event indicated in <paramref name="streamFlags"/>. The time is given in 100-nanosecond units.</param>
        /// <returns>The <see cref="MFSample"/> instance or null. If this parameter receives a non-null value, the caller must release the received <see cref="MFSample"/>.</returns>
        public MFSample ReadSample(int streamIndex, int controlFlags, out int actualStreamIndex, out MFSourceReaderFlags streamFlags, out long timestamp)
        {
            MFSample sample;
            MediaFoundationException.Try(ReadSampleNative(streamIndex, controlFlags, out actualStreamIndex, out streamFlags, out timestamp, out sample), InterfaceName, "ReadSample");
            return sample;
        }

        /// <summary>
        /// Flushes one or more streams.
        /// </summary>
        /// <param name="streamIndex">The stream to flush. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374659(v=vs.85).aspx"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int FlushNative(int streamIndex)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, ((void**)(*(void**)UnsafeBasePtr))[10]);
        }

        /// <summary>
        /// Flushes one or more streams.
        /// </summary>
        /// <param name="streamIndex">The stream to flush. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374659(v=vs.85).aspx"/>.</param>        
        public void Flush(int streamIndex)
        {
            MediaFoundationException.Try(FlushNative(streamIndex), InterfaceName, "Flush");
        }

        /// <summary>
        /// Queries the underlying media source or decoder for an interface.
        /// </summary>
        /// <param name="streamIndex">The stream or object to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374663(v=vs.85).aspx"/>.</param>
        /// <param name="guidService">A service identifier <see cref="Guid"/>. If the value is <see cref="Guid.Empty"/>, the method calls <c>QueryInterface</c> to get the requested interface. Otherwise, the method calls the IMFGetService::GetService method. For a list of service identifiers, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms695350(v=vs.85).aspx"/>.</param>
        /// <param name="riid">The interface identifier (IID) of the interface being requested.</param>
        /// <param name="service">Receives a pointer to the requested interface. The caller must release the interface.</param>
        /// <returns>
        /// HRESULT
        /// </returns>
        public unsafe int GetServiceForStreamNative(int streamIndex, Guid guidService, Guid riid, out IntPtr service)
        {
            fixed (void* ptr = &service)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, &guidService, &riid, ptr, ((void**)(*(void**)UnsafeBasePtr))[11]);
            }
        }

        /// <summary>
        /// Queries the underlying media source or decoder for an interface.
        /// </summary>
        /// <param name="streamIndex">The stream or object to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374663(v=vs.85).aspx"/>.</param>
        /// <param name="guidService">A service identifier <see cref="Guid"/>. If the value is <see cref="Guid.Empty"/>, the method calls <c>QueryInterface</c> to get the requested interface. Otherwise, the method calls the IMFGetService::GetService method. For a list of service identifiers, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms695350(v=vs.85).aspx"/>.</param>
        /// <param name="riid">The interface identifier (IID) of the interface being requested.</param>
        /// <returns>A pointer to the requested interface. The caller must release the interface.</returns>
        public IntPtr GetServiceForStream(int streamIndex, Guid guidService, Guid riid)
        {
            IntPtr ptr;
            MediaFoundationException.Try(GetServiceForStreamNative(streamIndex, guidService, riid, out ptr), InterfaceName, "GetServiceForStream");
            return ptr;
        }

        /// <summary>
        /// Gets an attribute from the underlying media source.
        /// </summary>
        /// <param name="streamIndex">The stream or object to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374662(v=vs.85).aspx"/>.</param>
        /// <param name="guidAttribute">A <see cref="Guid"/> that identifies the attribute to retrieve. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374662(v=vs.85).aspx"/>.</param>
        /// <param name="variant">Receives a <see cref="PropertyVariant"/> that receives the value of the attribute. Call the <see cref="PropertyVariant.Dispose"/> method to free the <see cref="PropertyVariant"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetPresentationAttributeNative(int streamIndex, Guid guidAttribute, out PropertyVariant variant)
        {
            variant = default(PropertyVariant);
            fixed (void* ptr = &variant)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, &guidAttribute, ptr, ((void**)(*(void**)UnsafeBasePtr))[12]);
            }
        }

        /// <summary>
        /// Gets an attribute from the underlying media source.
        /// </summary>
        /// <param name="streamIndex">The stream or object to query. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374662(v=vs.85).aspx"/>.</param>
        /// <param name="guidAttribute">A <see cref="Guid"/> that identifies the attribute to retrieve. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd374662(v=vs.85).aspx"/>.</param>
        /// <returns>A <see cref="PropertyVariant"/> that receives the value of the attribute. Call the <see cref="PropertyVariant.Dispose"/> method to free the <see cref="PropertyVariant"/>.</returns>
        public PropertyVariant GetPresentationAttribute(int streamIndex, Guid guidAttribute)
        {
            PropertyVariant res;
            MediaFoundationException.Try(GetPresentationAttributeNative(streamIndex, guidAttribute, out res), InterfaceName, "GetPresentationAttribute");
            return res;
        }
    }
}