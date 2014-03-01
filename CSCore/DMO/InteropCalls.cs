using CSCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.DMO
{
    [RemoveObj]
    internal class InteropCalls
    {
        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, int streamIndex, void* pflags, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, int streamIndex, IMediaBuffer mediaBuffer, InputdataBufferFlags flags, long timestamp, long timeduration, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, ProcessOutputFlags flags, DmoOutputDataBuffer[] buffers, int bufferCount)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, ProcessOutputFlags flags, int bufferCount, DmoOutputDataBuffer[] buffers, int status)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, ProcessOutputFlags flags, int bufferCount, DmoOutputDataBuffer[] buffers, void* pstatus, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, int streamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, int streamIndex, MediaType mediaType, SetTypeFlags flags, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, int streamIndex, void* pvalue, SetTypeFlags flags, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, int quality, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, float[] channelConverstionMatrix, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, void* pccm, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, int streamIndex, IntPtr intPtr, SetTypeFlags flags, void* ptr)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, void* i0, void* i1, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CalliMethodPtr(void* _basePtr, int streamIndex, int typeIndex, void* p1, void* p2)
        {
            throw new NotImplementedException();
        }
    }
}