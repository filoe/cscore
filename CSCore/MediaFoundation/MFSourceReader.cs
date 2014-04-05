using CSCore.CoreAudioAPI;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    [Guid("70ae66f2-c809-4e4f-8915-bdcb406b7993")]
    public class MFSourceReader : ComObject
    {
        private const string c = "IMFSourceReader";

        public MFSourceReader(IntPtr ptr)
            : base(ptr)
        {
        }

        public bool CanSeek
        {
            get { return (MediaSourceCharacteristics & MFMediaSourceCharacteristics.CanSeek) == MFMediaSourceCharacteristics.CanSeek; }
        }

        public MFMediaSourceCharacteristics MediaSourceCharacteristics
        {
            get { return (MFMediaSourceCharacteristics)GetSourceFlags(); }
        }

        public int GetSourceFlags()
        {
            int flags = 0;
            var value = GetPresentationAttribute(MFInterops.MF_SOURCE_READER_MEDIASOURCE, MediaFoundationAttributes.MF_SOURCE_READER_MEDIASOURCE_CHARACTERISTICS);

            flags = (int)value.UIntValue;
            value.Dispose();
            return flags;
        }

        /// <summary>
        /// Queries whether a stream is selected.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetStreamSelectionNative(int streamIndex, out NativeBool selectedRef)
        {
            fixed (NativeBool* p = (&selectedRef))
            {
                return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, (IntPtr*)p, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        /// <summary>
        /// Queries whether a stream is selected. 
        /// </summary>
        public NativeBool GetStreamSelection(int streamIndex)
        {
            NativeBool result;
            MediaFoundationException.Try(GetStreamSelectionNative(streamIndex, out result), c, "GetStreamSelection");
            return result;
        }

        /// <summary>
        /// Selects or deselects one or more streams.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetStreamSelectionNative(int streamIndex, NativeBool selected)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, selected, ((void**)(*(void**)_basePtr))[4]);
        }

        /// <summary>
        /// Selects or deselects one or more streams.
        /// </summary>
        public void SetStreamSelection(int streamIndex, NativeBool selected)
        {
            MediaFoundationException.Try(SetStreamSelectionNative(streamIndex, selected), c, "SetStreamSelection");
        }

        /// <summary>
        /// Gets a format that is supported natively by the media source.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetNativeMediaTypeNative(int streamIndex, int mediatypeIndex, out MFMediaType mediaType)
        {
            IntPtr ptr = IntPtr.Zero;
            var result = InteropCalls.CalliMethodPtr(_basePtr, streamIndex, mediatypeIndex, &ptr, ((void**)(*(void**)_basePtr))[5]);
            mediaType = new MFMediaType(ptr);
            return result;
        }

        /// <summary>
        /// Gets a format that is supported natively by the media source.
        /// </summary>
        /// <returns>HRESULT</returns>
        public MFMediaType GetNativeMediaType(int streamIndex, int mediatypeIndex)
        {
            MFMediaType res;
            MediaFoundationException.Try(GetNativeMediaTypeNative(streamIndex, mediatypeIndex, out res), c, "GetNativeMediaType");
            return res;
        }

        /// <summary>
        /// Gets the current media type for a stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetCurrentMediaType(int streamIndex, out MFMediaType mediaType)
        {
            IntPtr ptr = IntPtr.Zero;
            var result = InteropCalls.CalliMethodPtr(_basePtr, streamIndex, &ptr, ((void**)(*(void**)_basePtr))[6]);
            mediaType = new MFMediaType(ptr);
            return result;
        }

        /// <summary>
        /// Gets the current media type for a stream.
        /// </summary>
        public MFMediaType GetCurrentMediaType(int streamIndex)
        {
            MFMediaType res;
            MediaFoundationException.Try(GetCurrentMediaType(streamIndex, out res), c, "GetCurrentMediaType");
            return res;
        }

        /// <summary>
        /// This media type defines that format that the Source Reader produces as output. It can
        /// differ from the native format provided by the media source. See Remarks for more
        /// information.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetCurrentMediaTypeNative(int streamIndex, IntPtr reserved, MFMediaType mediaType)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, reserved, (void*)((mediaType == null) ? IntPtr.Zero : mediaType.BasePtr), ((void**)(*(void**)_basePtr))[7]);
        }

        /// <summary>
        /// This media type defines that format that the Source Reader produces as output. It can
        /// differ from the native format provided by the media source. See Remarks for more
        /// information.
        /// </summary>
        public void SetCurrentMediaType(int streamIndex, MFMediaType mediaType)
        {
            MediaFoundationException.Try(SetCurrentMediaTypeNative(streamIndex, IntPtr.Zero, mediaType), c, "SetCurrentMediaType");
        }

        /// <summary>
        /// Seeks to a new position in the media source.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetCurrentPositionNative(Guid guidTimeFormat, PropertyVariant position)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &guidTimeFormat, &position, ((void**)(*(void**)_basePtr))[8]);
        }

        /// <summary>
        /// Seeks to a new position in the media source.
        /// </summary>
        public void SetCurrentPosition(Guid guidTimeFormat, PropertyVariant position)
        {
            MediaFoundationException.Try(SetCurrentPositionNative(guidTimeFormat, position), c, "SetCurrentPosition");
        }

        /// <summary>
        /// Reads the next sample from the media source.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int ReadSampleNative(int streamIndex, int controlFlags, out int actualStreamIndex, out MFSourceReaderFlag streamFlags, out long timestamp, out MFSample sample)
        {
            IntPtr psample = IntPtr.Zero;
            fixed (void* ptr0 = &actualStreamIndex, ptr1 = &streamFlags, ptr2 = &timestamp)
            {
                int result = InteropCalls.CalliMethodPtr(_basePtr, streamIndex, controlFlags, ptr0, ptr1, ptr2, &psample, ((void**)(*(void**)_basePtr))[9]);
                sample = psample == IntPtr.Zero ? null : new MFSample(psample);
                return result;
            }
        }

        /// <summary>
        /// Reads the next sample from the media source.
        /// </summary>
        public MFSample ReadSample(int streamIndex, int controlFlags, out int actualStreamIndex, out MFSourceReaderFlag streamFlags, out long timestamp)
        {
            MFSample sample;
            MediaFoundationException.Try(ReadSampleNative(streamIndex, controlFlags, out actualStreamIndex, out streamFlags, out timestamp, out sample), c, "ReadSample");
            return sample;
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
        /// Queries the underlying media source or decoder for an interface.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetServiceForStreamNative(int streamIndex, Guid guidService, Guid riid, out IntPtr service)
        {
            fixed (void* ptr = &service)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, &guidService, &riid, ptr, ((void**)(*(void**)_basePtr))[11]);
            }
        }

        /// <summary>
        /// Queries the underlying media source or decoder for an interface.
        /// </summary>
        public IntPtr GetServiceForStream(int streamIndex, Guid guidService, Guid riid)
        {
            IntPtr ptr = IntPtr.Zero;
            MediaFoundationException.Try(GetServiceForStreamNative(streamIndex, guidService, riid, out ptr), c, "GetServiceForStream");
            return ptr;
        }

        /// <summary>
        /// Gets an attribute from the underlying media source.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetPresentationAttributeNative(int streamIndex, Guid guidAttribute, out PropertyVariant variant)
        {
            variant = default(PropertyVariant);
            fixed (void* ptr = &variant)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, &guidAttribute, ptr, ((void**)(*(void**)_basePtr))[12]);
            }
        }

        /// <summary>
        /// Gets an attribute from the underlying media source.
        /// </summary>
        public PropertyVariant GetPresentationAttribute(int streamIndex, Guid guidAttribute)
        {
            PropertyVariant res;
            MediaFoundationException.Try(GetPresentationAttributeNative(streamIndex, guidAttribute, out res), c, "GetPresentationAttribute");
            return res;
        }
    }
}