using CSCore.Utils;
using System;

namespace CSCore.CoreAudioAPI.Interop
{
    [RemoveObj]
    internal static unsafe class InteropCalls
    {
        [CSCli]
        internal static unsafe int CallI(void* _basePtr, AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, long hnsBufferDuration, long hnsPeriodicity, void* p1, void* psession, void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCli]
        internal static unsafe int CallI(void* _basePtr, void* pbfc, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCli]
        internal static unsafe int CallI(void* _basePtr, AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, long hnsBufferDuration, long hnsPeriodicity, void* p1, Guid audioSessionGuid, void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCli]
        internal static unsafe int CallI(void* _basePtr, void* p1, void* p2, void* p3)
        {
            throw new NotImplementedException();
        }

        [CSCli]
        internal static unsafe int CallI(void* _basePtr, void* p)
        {
            throw new NotImplementedException();
        }
    }
}
