using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Contains performance information. Used by <see cref="XAudio2.PerformanceData" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PerformanceData
    {
        /// <summary>
        ///     CPU cycles spent on audio processing since the last call to the <see cref="XAudio2.StartEngine" /> or
        ///     <see cref="XAudio2.PerformanceData" /> function.
        /// </summary>
        public Int64 AudioCyclesSinceLastQuery;

        /// <summary>
        ///     Total CPU cycles elapsed since the last call. Note: This only counts cycles on the CPU on which XAudio2 is running.
        /// </summary>
        public Int64 TotalCyclesSinceLastQuery;

        /// <summary>
        ///     Fewest CPU cycles spent on processing any single audio quantum since the last call.
        /// </summary>
        public Int32 MinimumCyclesPerQuantum;

        /// <summary>
        ///     Most CPU cycles spent on processing any single audio quantum since the last call.
        /// </summary>
        public Int32 MaximumCyclesPerQuantum;

        /// <summary>
        ///     Total memory currently in use.
        /// </summary>
        public Int32 MemoryUsageInBytes;

        /// <summary>
        ///     Minimum delay that occurs between the time a sample is read from a source buffer and the time it reaches the
        ///     speakers.
        /// </summary>
        public Int32 CurrentLatencyInSamples;

        /// <summary>
        ///     Total audio dropouts since the engine started.
        /// </summary>
        public Int32 GlitchesSinceEngineStarted;

        /// <summary>
        ///     Number of source voices currently playing.
        /// </summary>
        public Int32 ActiveSourceVoiceCount;

        /// <summary>
        ///     Total number of source voices currently in existence.
        /// </summary>
        public Int32 TotalSourceVoiceCount;

        /// <summary>
        ///     Number of submix voices currently playing.
        /// </summary>
        public Int32 ActiveSubmixVoiceCount;

        /// <summary>
        ///     Number of resampler xAPOs currently active.
        /// </summary>
        public Int32 ActiveResamplerCount;

        /// <summary>
        ///     Number of matrix mix xAPOs currently active.
        /// </summary>
        public Int32 ActiveMatrixMixCount;

        /// <summary>
        ///     Not supported on Windows. Xbox 360. Number of source voices decoding XMA data.
        /// </summary>
        public Int32 ActiveXmaSourceVoices;

        /// <summary>
        ///     Not supported on Windows. A voice can use more than one XMA stream.
        /// </summary>
        public Int32 ActiveXmaStreams;
    }
}