using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct MFSinkWriterStatistics
    {
        /// <summary>	
        /// The size of the structure, in bytes.
        /// </summary>	
        public int Cb;
        /// <summary>	
        /// The time stamp of the most recent sample given to the sink writer. The sink writer updates this value each time the application calls <strong><see cref="M:SharpDX.MediaFoundation.SinkWriter.WriteSample(System.Int32,SharpDX.MediaFoundation.Sample)" /></strong>.
        /// </summary>	
        public long LlLastTimestampReceived;
        /// <summary>	
        /// The time stamp of the most recent sample to be encoded. The sink writer updates this value whenever it calls <strong><see cref="M:SharpDX.MediaFoundation.Transform.ProcessOutput(SharpDX.MediaFoundation.TransformProcessOutputFlags,System.Int32,SharpDX.MediaFoundation.TOutputDataBuffer,SharpDX.MediaFoundation.TransformProcessOutputStatus@)" /></strong> on the encoder.
        /// </summary>	
        public long LlLastTimestampEncoded;
        /// <summary>	
        /// The time stamp of the most recent sample given to the media sink. The sink writer updates this value whenever it calls <strong><see cref="M:SharpDX.MediaFoundation.StreamSink.ProcessSample(SharpDX.MediaFoundation.Sample)" /></strong> on the media sink.
        /// </summary>	
        public long LlLastTimestampProcessed;
        /// <summary>	
        /// The time stamp of the most recent stream tick. The sink writer updates this value whenever the application calls <strong><see cref="M:SharpDX.MediaFoundation.SinkWriter.SendStreamTick(System.Int32,System.Int64)" /></strong>.
        /// </summary>		
        public long LlLastStreamTickReceived;
        /// <summary>	
        /// The system time of the most recent sample request from the media sink. The sink writer updates this value whenever it receives an <see cref="F:SharpDX.MediaFoundation.MediaEventTypes.StreamSinkRequestSample" /> event from the media sink. The value is the current system time.
        /// </summary>	
        public long LlLastSinkSampleRequest;
        /// <summary>	
        /// The number of samples received.
        /// </summary>	
        public long QwNumSamplesReceived;
        /// <summary>	
        /// The number of samples encoded.
        /// </summary>	
        public long QwNumSamplesEncoded;
        /// <summary>	
        /// The number of samples given to the media sink.
        /// </summary>	
        public long QwNumSamplesProcessed;
        /// <summary>	
        /// The number of stream ticks received.
        /// </summary>	
        public long QwNumStreamTicksReceived;
        /// <summary>	
        /// The amount of data, in bytes, currently waiting to be processed. 
        /// </summary>	
        public int DwByteCountQueued;
        /// <summary>	
        /// The total amount of data, in bytes, that has been sent to the media sink.
        /// </summary>	
        public long QwByteCountProcessed;
        /// <summary>	
        /// The number of pending sample requests.
        /// </summary>	
        public int DwNumOutstandingSinkSampleRequests;
        /// <summary>	
        /// The average rate, in media samples per 100-nanoseconds, at which the application sent samples to the sink writer.
        /// </summary>	
        public int DwAverageSampleRateReceived;
        /// <summary>	
        /// The average rate, in media samples per 100-nanoseconds, at which the sink writer sent samples to the encoder.
        /// </summary>		
        public int DwAverageSampleRateEncoded;
        /// <summary>	
        /// The average rate, in media samples per 100-nanoseconds, at which the sink writer sent samples to the media sink.
        /// </summary>	
        public int DwAverageSampleRateProcessed;
    }
}
