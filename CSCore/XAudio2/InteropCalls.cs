#pragma warning disable 1591

using System;
using CSCore.Utils;

namespace CSCore.XAudio2
{
    [CLSCompliant(false)]
    [RemoveObj]
    public class InteropCalls
    {
        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, IXAudio2EngineCallback callback, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* ptr, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, int effectIndex, int operationSet, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, int effectIndex, void* p1, void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, int effectIndex, IntPtr pParamters, int parametersByteSize,
            int operationSet, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, int effectIndex, void* p1, int parameterByteSize, void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, FilterParameters* filterParameters, int operationSet, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* p1, FilterParameters* filterParameters, int operationSet,
            void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* p1, void* p2, void* p3)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, float volume, int operationSet, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* p1, int sourceChannels, int destinationChannels, void* p2,
            int operationSet, void* p3)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* p1, int sourceChannels, int destinationChannels, void* p2,
            void* p3)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe void CallI1(void* _basePtr, int effectIndex, void* p1, void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe void CallI2(void* _basePtr, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* ptr, VoiceFlags flags, float maxFrequencyRatio, void* p1,
            void* p2, void* p3, void* p4)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* ptr, IntPtr sourceFormat, VoiceFlags flags,
            float maxFrequencyRatio, void* p1, void* p2, void* p3, void* p4)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* ptr, IntPtr sourceFormat, VoiceFlags flags,
            float maxFrequencyRatio, IXAudio2VoiceCallback voiceCallback, void* p1, void* p2, void* p3)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        public static unsafe int CallI(void* basePtr, void* filterParameters, int sourceChannels, int parametersByteSize,
            VoiceFlags maxFrequencyRatio, int processingStage, void* p3, void* p4, void* p5)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* ptr, int inputChannels, int inputSampleRate, int flags,
            void* p1, void* p2, AudioStreamCategory streamCategory, void* p3)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, int operationSet, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe void CallI4(void* _basePtr, DebugConfiguration* debugConfiguration, void* p1, void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe void CallI5(void* _basePtr, void* p1, void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe void CallI6(void* _basePtr, IXAudio2EngineCallback callback, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe void CallI7(void* _basePtr, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, SourceVoiceStopFlags flags, int operationSet, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe void CallI1(void* _basePtr, void* voiceState, GetVoiceStateFlags flags, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe float CallI1(void* _basePtr, float* p1, void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, int flags, XAudio2Processor processor, void* p)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* ptr, int inputChannels, int inputSampleRate, int flags,
            void* p1, void* p2, void* p3)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe int CallI(void* _basePtr, void* ptr, int inputChannels, int inputSampleRate, int flags,
            int device, void* p1, void* p2)
        {
            throw new NotImplementedException();
        }

        [CSCalli]
        internal static unsafe void CallI1(void* UnsafeBasePtr, VoiceState* voiceState, void* p)
        {
            throw new NotImplementedException();
        }
    }
}

#pragma warning restore 1591