using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    [Guid("3137F1CD-FE5E-4805-A5D8-FB477448CB3D")]
	public class MFSinkWriter : ComObject
	{
		private const string c = "IMFSinkWriter";

		public MFSinkWriter(IntPtr ptr)
			: base(ptr)
		{
		}

		/// <summary>
		/// Adds a stream to the sink writer.
		/// </summary>
		/// <returns>HRESULT</returns>
		public unsafe int AddStreamNative(MFMediaType targetMediaType, out int streamIndex)
		{
			fixed (void* psi = &streamIndex)
			{
				void* ptmt = targetMediaType == null ? IntPtr.Zero.ToPointer() : targetMediaType.BasePtr.ToPointer();
				return InteropCalls.CalliMethodPtr(_basePtr, ptmt, psi, ((void**)(*(void**)_basePtr))[3]);
			}
		}

		/// <summary>
		/// Adds a stream to the sink writer.
		/// </summary>
		/// <returns>The zero-based index of the new stream.</returns>
		public int AddStream(MFMediaType targetMediaType)
		{
			int t;
			MediaFoundationException.Try(AddStreamNative(targetMediaType, out t), c, "AddStream");
			return t;
		}

		/// <summary>
		/// Sets the input format for a stream on the sink writer.
		/// </summary>
		/// <param name="encodingParameters">Optional</param>
		/// <returns>HRESULT</returns>
		public unsafe int SetInputMediaTypeNative(int streamIndex, MFMediaType inputMediaType, MFAttributes encodingParameters)
		{
			void* imt = (void*)(inputMediaType == null ? IntPtr.Zero : inputMediaType.BasePtr);
			void* ep = (void*)(encodingParameters == null ? IntPtr.Zero : encodingParameters.BasePtr);
			return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, imt, ep, ((void**)(*(void**)_basePtr))[4]);
		}

		/// <summary>
		/// Sets the input format for a stream on the sink writer.
		/// </summary>
		/// <param name="encodingParameters">Optional</param>
		public void SetInputMediaType(int streamIndex, MFMediaType inputMediaType, MFAttributes encodingParameters)
		{
			MediaFoundationException.Try(SetInputMediaTypeNative(streamIndex, inputMediaType, encodingParameters), c, "SetInputMediaType");
		}

		/// <summary>
		/// Initializes the sink writer for writing.
		/// </summary>
		/// <returns>HRESULT</returns>
		public unsafe int BeginWritingNative()
		{
			return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[5]);
		}

		/// <summary>
		/// Initializes the sink writer for writing.
		/// </summary>
		public void BeginWriting()
		{
			MediaFoundationException.Try(BeginWritingNative(), c, "BeginWriting");
		}

		/// <summary>
		/// Delivers a sample to the sink writer.
		/// </summary>
		/// <returns>HRESULT</returns>
		public unsafe int WriteSampleNative(int streamIndex, MFSample sample)
		{
			void* ps = (void*)(sample == null ? IntPtr.Zero : sample.BasePtr);
			return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, ps, ((void**)(*(void**)_basePtr))[6]);
		}

		/// <summary>
		/// Delivers a sample to the sink writer.
		/// </summary>
		public void WriteSample(int streamIndex, MFSample sample)
		{
			MediaFoundationException.Try(WriteSampleNative(streamIndex, sample), c, "WriteSample");
		}

		/// <summary>
		/// Indicates a gap in an input stream.
		/// </summary>
		/// <param name="timeStamp">The position in the stream where the gap in the data occurs. The value is given in 100-nanosecond units, relative to the start of the stream.</param>
		/// <returns>HRESULT</returns>
		public unsafe int SendStreamTickNative(int streamIndex, long timeStamp)
		{
			return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, timeStamp, ((void**)(*(void**)_basePtr))[7]);
		}

		/// <summary>
		/// Indicates a gap in an input stream.
		/// </summary>
		/// <param name="timeStamp">The position in the stream where the gap in the data occurs. The value is given in 100-nanosecond units, relative to the start of the stream.</param>
		public void SendStreamTick(int streamIndex, long timeStamp)
		{
			MediaFoundationException.Try(SendStreamTickNative(streamIndex, timeStamp), c, "SendStreamTick");
		}

		/// <summary>
		/// Places a marker in the specified stream.
		/// </summary>
		/// <param name="context">Pointer to an application-defined value. The value of this parameter is returned to the caller in the pvContext parameter of the caller's IMFSinkWriterCallback::OnMarker callback method. The application is responsible for any memory allocation associated with this data. This parameter can be NULL.</param>
		/// <returns>HRESULT</returns>
		public unsafe int PlaceMarkerNative(int streamIndex, IntPtr context)
		{
			return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, (void*)context, ((void**)(*(void**)_basePtr))[8]);
		}

		/// <summary>
		/// Places a marker in the specified stream.
		/// </summary>
		/// <param name="context">Pointer to an application-defined value. The value of this parameter is returned to the caller in the pvContext parameter of the caller's IMFSinkWriterCallback::OnMarker callback method. The application is responsible for any memory allocation associated with this data. This parameter can be NULL.</param>
		public void PlaceMarker(int streamIndex, IntPtr context)
		{
			MediaFoundationException.Try(PlaceMarkerNative(streamIndex, context), c, "PlaceMarker");
		}

		/// <summary>
		/// Notifies the media sink that a stream has reached the end of a segment.
		/// </summary>
		/// <returns>HRESULT</returns>
		public unsafe int NotifyEndOfSegmentNative(int streamIndex)
		{
			return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, ((void**)(*(void**)_basePtr))[9]);
		}

		/// <summary>
		/// Notifies the media sink that a stream has reached the end of a segment.
		/// </summary>
		public void NotifyEndOfSegment(int streamIndex)
		{
			MediaFoundationException.Try(NotifyEndOfSegmentNative(streamIndex), c, "NotifyEndOfSegment");
		}

		/// <summary>
		/// Flushes one or more streams.
		/// </summary>
		/// <returns>HRESULT</returns>
		public unsafe int FlushNative(int streamIndex)
		{
			return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, ((void**)(*(void**)_basePtr))[10]);
		}

		/// <summary>
		/// Flushes one or more streams.
		/// </summary>
		public void Flush(int streamIndex)
		{
			MediaFoundationException.Try(FlushNative(streamIndex), c, "Flush");
		}

		/// <summary>
		/// Completes all writing operations on the sink writer.
		/// </summary>
		/// <returns>HRESULT</returns>
		public unsafe int FinalizeWritingNative()
		{
			return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[11]);
		}

		/// <summary>
		/// Completes all writing operations on the sink writer.
		/// </summary>
        /// <remarks>Renamed from 'Finalize' to 'FinalizeWriting' to suppress "CS0465 warning".</remarks>
        public void FinalizeWriting()
		{
            MediaFoundationException.Try(FinalizeWritingNative(), c, "Finalize");
		}

		/// <summary>
		/// Queries the underlying media sink or encoder for an interface.
		/// </summary>
		/// <param name="guidService">A service identifier GUID, or GUID_NULL. If the value is GUID_NULL, the method calls QueryInterface to get the requested interface. Otherwise, the method calls IMFGetService::GetService. For a list of service identifiers, see Service Interfaces( http://msdn.microsoft.com/en-us/library/windows/desktop/ms695350(v=vs.85).aspx ).</param>
		/// <param name="riid">The interface identifier (IID) of the interface being requested.</param>
		/// <param name="pObject">Receives a pointer to the requested interface. The caller must release the interface.</param>
		/// <returns>HRESULT</returns>
		public unsafe int GetServiceForStreamNative(int streamIndex, Guid guidService, Guid riid, out IntPtr pObject)
		{
			fixed (void* ptr = &pObject)
			{
				return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, &guidService, &riid, ptr, ((void**)(*(void**)_basePtr))[12]);
			}
		}

		/// <summary>
		/// Queries the underlying media sink or encoder for an interface.
		/// </summary>
		/// <param name="guidService">A service identifier GUID, or GUID_NULL. If the value is GUID_NULL, the method calls QueryInterface to get the requested interface. Otherwise, the method calls IMFGetService::GetService. For a list of service identifiers, see Service Interfaces( http://msdn.microsoft.com/en-us/library/windows/desktop/ms695350(v=vs.85).aspx ).</param>
		/// <param name="riid">The interface identifier (IID) of the interface being requested.</param>
		/// <returns>A pointer to the requested interface. The caller must release the interface.</returns>
		public IntPtr GetServiceForStream(int streamIndex, Guid guidService, Guid riid)
		{
			IntPtr t;
			MediaFoundationException.Try(GetServiceForStreamNative(streamIndex, guidService, riid, out t), c, "GetServiceForStream");
			return t;
		}

        /// <summary>
        /// Gets statistics about the performance of the sink writer.
        /// </summary>
        /// <returns>HRESULT</returns>
		public unsafe int GetStatisticsNative(int streamIndex, out MFSinkWriterStatistics statistics)
        {
            fixed (void* ptr = &statistics)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, ptr, ((void**)(*(void**)_basePtr))[13]);
            }
        }

        /// <summary>
        /// Gets statistics about the performance of the sink writer.
        /// </summary>
        public MFSinkWriterStatistics GetStatistics(int streamIndex)
        {
            MFSinkWriterStatistics s;
            s.Cb = Marshal.SizeOf(typeof(MFSinkWriterStatistics));
            MediaFoundationException.Try(GetStatisticsNative(streamIndex, out s), c, "GetStatistics");
            return s;
        }
	}
}
