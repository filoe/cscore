using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
	/// <summary>
	/// Implemented by the Microsoft Media Foundation sink writer object.
	/// </summary>
	[Guid("3137F1CD-FE5E-4805-A5D8-FB477448CB3D")]
	public class MFSinkWriter : ComObject
	{
		/// <summary>
		/// Stream index to selected all streams.
		/// </summary>
		public const int MF_SINK_WRITER_ALL_STREAMS = unchecked((int)0xFFFFFFFE);

	    /// <summary>
        /// MF_SINK_WRITER_MEDIASINK constant.
	    /// </summary>
	    public const int MF_SINK_WRITER_MEDIASINK = unchecked ((int) 0xFFFFFFFF);

		private const string InterfaceName = "IMFSinkWriter";

		/// <summary>
		/// Initializes a new instance of the <see cref="MFSinkWriter"/> class.
		/// </summary>
		/// <param name="ptr">The native pointer of the COM object.</param>
		public MFSinkWriter(IntPtr ptr)
			: base(ptr)
		{
		}

	    /// <summary>
        /// Initializes a new instance of the <see cref="MFSinkWriter"/> class with a underlying <paramref name="byteStream"/>.
	    /// </summary>
	    /// <param name="byteStream">The underlying <see cref="MFByteStream"/> to use.</param>
        /// <param name="attributes">Attributes to configure the <see cref="MFSinkWriter"/>. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd389284(v=vs.85).aspx"/>. Use null/nothing as the default value.</param>
	    public MFSinkWriter(MFByteStream byteStream, MFAttributes attributes = null)
            : this(MediaFoundationCore.CreateSinkWriterFromMFByteStreamNative(byteStream, attributes))
	    {
	    }

		/// <summary>
		/// Adds a stream to the sink writer.
		/// <seealso cref="AddStream"/>
		/// </summary>
		/// <param name="targetMediaType">The target mediatype which specifies the format of the samples that will be written to the file. It does not need to match the input format. To set the input format, call <see cref="SetInputMediaType"/>.</param>
		/// <param name="streamIndex">Receives the zero-based index of the new stream.</param>
		/// <returns>HRESULT</returns>
		public unsafe int AddStreamNative(MFMediaType targetMediaType, out int streamIndex)
		{
			fixed (void* psi = &streamIndex)
			{
				void* ptmt = targetMediaType == null ? IntPtr.Zero.ToPointer() : targetMediaType.BasePtr.ToPointer();
				return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptmt, psi, ((void**)(*(void**)UnsafeBasePtr))[3]);
			}
		}

		/// <summary>
		/// Adds a stream to the sink writer.
		/// </summary>
		/// <param name="targetMediaType">The target mediatype which specifies the format of the samples that will be written to the file. It does not need to match the input format. To set the input format, call <see cref="SetInputMediaType"/>.</param>		
		/// <returns>The zero-based index of the new stream.</returns>
		public int AddStream(MFMediaType targetMediaType)
		{
			int t;
			MediaFoundationException.Try(AddStreamNative(targetMediaType, out t), InterfaceName, "AddStream");
			return t;
		}

		/// <summary>
		/// Sets the input format for a stream on the sink writer.
		/// <seealso cref="SetInputMediaType"/>
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream. The index is returned by the <see cref="AddStream"/> method.</param>
		/// <param name="inputMediaType">The input media type that specifies the input format.</param>
		/// <param name="encodingParameters">An attribute store. Use the attribute store to configure the encoder. This parameter can be NULL.</param>
		/// <returns>HRESULT</returns>
		public unsafe int SetInputMediaTypeNative(int streamIndex, MFMediaType inputMediaType, MFAttributes encodingParameters)
		{
			void* imt = (void*)(inputMediaType == null ? IntPtr.Zero : inputMediaType.BasePtr);
			void* ep = (void*)(encodingParameters == null ? IntPtr.Zero : encodingParameters.BasePtr);
			return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, imt, ep, ((void**)(*(void**)UnsafeBasePtr))[4]);
		}

		/// <summary>
		/// Sets the input format for a stream on the sink writer.
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream. The index is returned by the <see cref="AddStream"/> method.</param>
		/// <param name="inputMediaType">The input media type that specifies the input format.</param>
		/// <param name="encodingParameters">An attribute store. Use the attribute store to configure the encoder. This parameter can be NULL.</param>
		public void SetInputMediaType(int streamIndex, MFMediaType inputMediaType, MFAttributes encodingParameters)
		{
			MediaFoundationException.Try(SetInputMediaTypeNative(streamIndex, inputMediaType, encodingParameters), InterfaceName, "SetInputMediaType");
		}

		/// <summary>
		/// Initializes the sink writer for writing.
        /// <seealso cref="BeginWritingNative"/>
		/// </summary>
		/// <returns>HRESULT</returns>
		public unsafe int BeginWritingNative()
		{
			return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[5]);
		}

		/// <summary>
		/// Initializes the sink writer for writing.
		/// </summary>
		public void BeginWriting()
		{
			MediaFoundationException.Try(BeginWritingNative(), InterfaceName, "BeginWriting");
		}

		/// <summary>
		/// Delivers a sample to the sink writer.
        /// <seealso cref="WriteSample"/>
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream for this sample.</param>
		/// <param name="sample">The sample to write.</param>
		/// <returns>HRESULT</returns>
		/// <remarks>You must call <see cref="BeginWriting"/> before calling this method.</remarks>
		public unsafe int WriteSampleNative(int streamIndex, MFSample sample)
		{
			void* ps = (void*)(sample == null ? IntPtr.Zero : sample.BasePtr);
			return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, ps, ((void**)(*(void**)UnsafeBasePtr))[6]);
		}

		/// <summary>
		/// Delivers a sample to the sink writer.
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream for this sample.</param>
		/// <param name="sample">The sample to write.</param>
		/// <remarks>You must call <see cref="BeginWriting"/> before calling this method.</remarks>        
		public void WriteSample(int streamIndex, MFSample sample)
		{
			MediaFoundationException.Try(WriteSampleNative(streamIndex, sample), InterfaceName, "WriteSample");
		}

		/// <summary>
		/// Indicates a gap in an input stream.
        /// <seealso cref="SendStreamTick"/>
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream.</param>
		/// <param name="timeStamp">The position in the stream where the gap in the data occurs. The value is given in 100-nanosecond units, relative to the start of the stream.</param>
		/// <returns>HRESULT</returns>
		public unsafe int SendStreamTickNative(int streamIndex, long timeStamp)
		{
			return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, timeStamp, ((void**)(*(void**)UnsafeBasePtr))[7]);
		}

		/// <summary>
		/// Indicates a gap in an input stream.
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream.</param>		
		/// <param name="timeStamp">The position in the stream where the gap in the data occurs. The value is given in 100-nanosecond units, relative to the start of the stream.</param>
		public void SendStreamTick(int streamIndex, long timeStamp)
		{
			MediaFoundationException.Try(SendStreamTickNative(streamIndex, timeStamp), InterfaceName, "SendStreamTick");
		}

		/// <summary>
		/// Places a marker in the specified stream.
        /// <seealso cref="PlaceMarker"/>
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream.</param>        
		/// <param name="context">Pointer to an application-defined value. The value of this parameter is returned to the caller in the pvContext parameter of the caller's IMFSinkWriterCallback::OnMarker callback method. The application is responsible for any memory allocation associated with this data. This parameter can be NULL.</param>
		/// <returns>HRESULT</returns>
		public unsafe int PlaceMarkerNative(int streamIndex, IntPtr context)
		{
			return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, (void*)context, ((void**)(*(void**)UnsafeBasePtr))[8]);
		}

		/// <summary>
		/// Places a marker in the specified stream.
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream.</param>
		/// <param name="context">Pointer to an application-defined value. The value of this parameter is returned to the caller in the pvContext parameter of the caller's IMFSinkWriterCallback::OnMarker callback method. The application is responsible for any memory allocation associated with this data. This parameter can be NULL.</param>
		public void PlaceMarker(int streamIndex, IntPtr context)
		{
			MediaFoundationException.Try(PlaceMarkerNative(streamIndex, context), InterfaceName, "PlaceMarker");
		}

		/// <summary>
		/// Notifies the media sink that a stream has reached the end of a segment.
        /// <seealso cref="NotifyEndOfSegment"/>
		/// </summary>
		/// <param name="streamIndex">The zero-based index of a stream, or <see cref="MF_SINK_WRITER_ALL_STREAMS"/> to signal that all streams have reached the end of a segment.</param>
		/// <returns>HRESULT</returns>
		public unsafe int NotifyEndOfSegmentNative(int streamIndex)
		{
			return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, ((void**)(*(void**)UnsafeBasePtr))[9]);
		}

		/// <summary>
		/// Notifies the media sink that a stream has reached the end of a segment.
		/// </summary>
		/// <param name="streamIndex">The zero-based index of a stream, or <see cref="MF_SINK_WRITER_ALL_STREAMS"/> to signal that all streams have reached the end of a segment.</param>		
		public void NotifyEndOfSegment(int streamIndex)
		{
			MediaFoundationException.Try(NotifyEndOfSegmentNative(streamIndex), InterfaceName, "NotifyEndOfSegment");
		}

		/// <summary>
		/// Flushes one or more streams.
        /// <seealso cref="Flush"/>
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream to flush, or <see cref="MF_SINK_WRITER_ALL_STREAMS"/> to flush all of the streams.</param>		
		/// <returns>HRESULT</returns>
		public unsafe int FlushNative(int streamIndex)
		{
			return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, ((void**)(*(void**)UnsafeBasePtr))[10]);
		}

		/// <summary>
		/// Flushes one or more streams.
		/// </summary>
		/// <param name="streamIndex">The zero-based index of the stream to flush, or <see cref="MF_SINK_WRITER_ALL_STREAMS"/> to flush all of the streams.</param>				
		public void Flush(int streamIndex)
		{
			MediaFoundationException.Try(FlushNative(streamIndex), InterfaceName, "Flush");
		}

		/// <summary>
		/// Completes all writing operations on the sink writer.
        /// <seealso cref="FinalizeWriting"/>
		/// </summary>
		/// <returns>HRESULT</returns>
		public unsafe int FinalizeWritingNative()
		{
			return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[11]);
		}

		/// <summary>
		/// Completes all writing operations on the sink writer.
		/// </summary>
		/// <remarks>Renamed from 'Finalize' to 'FinalizeWriting' to suppress "CS0465 warning".</remarks>
		public void FinalizeWriting()
		{
			MediaFoundationException.Try(FinalizeWritingNative(), InterfaceName, "Finalize");
		}

		/// <summary>
		/// Queries the underlying media sink or encoder for an interface.
        /// <seealso cref="GetServiceForStream"/>
		/// </summary>
        /// <param name="streamIndex">The zero-based index of a stream to query, or <see cref="MF_SINK_WRITER_MEDIASINK"/> to query the media sink itself.</param>		
        /// <param name="guidService">A service identifier GUID, or <see cref="Guid.Empty"/>. If the value is <see cref="Guid.Empty"/>, the method calls QueryInterface to get the requested interface. Otherwise, the method calls IMFGetService::GetService. 
        /// For a list of service identifiers, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms695350(v=vs.85).aspx"/>.</param>        
		/// <param name="riid">The interface identifier (IID) of the interface being requested.</param>
		/// <param name="pObject">Receives a pointer to the requested interface. The caller must release the interface.</param>
		/// <returns>HRESULT</returns>
		public unsafe int GetServiceForStreamNative(int streamIndex, Guid guidService, Guid riid, out IntPtr pObject)
		{
			fixed (void* ptr = &pObject)
			{
				return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, &guidService, &riid, ptr, ((void**)(*(void**)UnsafeBasePtr))[12]);
			}
		}

		/// <summary>
		/// Queries the underlying media sink or encoder for an interface.
		/// </summary>
        /// <param name="streamIndex">The zero-based index of a stream to query, or <see cref="MF_SINK_WRITER_MEDIASINK"/> to query the media sink itself.</param>		
        /// <param name="guidService">A service identifier GUID, or <see cref="Guid.Empty"/>. If the value is <see cref="Guid.Empty"/>, the method calls QueryInterface to get the requested interface. Otherwise, the method calls IMFGetService::GetService. 
        /// For a list of service identifiers, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms695350(v=vs.85).aspx"/>.</param>        
        /// <param name="riid">The interface identifier (IID) of the interface being requested.</param>
		/// <returns>A pointer to the requested interface. The caller must release the interface.</returns>
		public IntPtr GetServiceForStream(int streamIndex, Guid guidService, Guid riid)
		{
			IntPtr t;
			MediaFoundationException.Try(GetServiceForStreamNative(streamIndex, guidService, riid, out t), InterfaceName, "GetServiceForStream");
			return t;
		}

		/// <summary>
		/// Gets statistics about the performance of the sink writer.
		/// </summary>
		/// <param name="streamIndex">The zero-based index of a stream to query, or <see cref="MF_SINK_WRITER_ALL_STREAMS"/> to query the media sink itself.</param>
		/// <param name="statistics">Receives statistics about the performance of the sink writer.</param>
		/// <returns>HRESULT</returns>
		public unsafe int GetStatisticsNative(int streamIndex, out MFSinkWriterStatistics statistics)
		{
			statistics = default(MFSinkWriterStatistics);
			statistics.Cb = Marshal.SizeOf(statistics);
			fixed (void* ptr = &statistics)
			{
				return InteropCalls.CalliMethodPtr(UnsafeBasePtr, streamIndex, ptr, ((void**)(*(void**)UnsafeBasePtr))[13]);
			}
		}

		/// <summary>
		/// Gets statistics about the performance of the sink writer.
		/// </summary>
		/// <param name="streamIndex">The zero-based index of a stream to query, or <see cref="MF_SINK_WRITER_ALL_STREAMS"/> to query the media sink itself.</param>
		/// <returns>Statistics about the performance of the sink writer.</returns>		
		public MFSinkWriterStatistics GetStatistics(int streamIndex)
		{
			MFSinkWriterStatistics s;
			s.Cb = Marshal.SizeOf(typeof(MFSinkWriterStatistics));
			MediaFoundationException.Try(GetStatisticsNative(streamIndex, out s), InterfaceName, "GetStatistics");
			return s;
		}
	}
}
