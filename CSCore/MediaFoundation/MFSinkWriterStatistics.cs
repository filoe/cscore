using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    //mostly copied from sharpdx
    /// <summary>
    /// Contains statistics about the performance of the sink writer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct MFSinkWriterStatistics
    {
        /// <summary>	
        /// The size of the structure, in bytes.
        /// </summary>	
        public int Cb;
        /// <summary>	
        /// The time stamp of the most recent sample given to the sink writer. The sink writer updates this value each time the application calls <see cref="MFSinkWriter.WriteSample"/>.
        /// </summary>	
        public long LastTimestampReceived;
        /// <summary>	
        /// The time stamp of the most recent sample to be encoded. The sink writer updates this value whenever it calls IMFTransform::ProcessOutput on the encoder.
        /// </summary>	
        public long LastTimestampEncoded;
        /// <summary>	
        /// The time stamp of the most recent sample given to the media sink. The sink writer updates this value whenever it calls IMFStreamSink::ProcessSample on the media sink.
        /// </summary>	
        public long LastTimestampProcessed;
        /// <summary>	
        /// The time stamp of the most recent stream tick. The sink writer updates this value whenever the application calls <see cref="MFSinkWriter.SendStreamTick"/>.
        /// </summary>		
        public long LastStreamTickReceived;
        /// <summary>	
        /// The system time of the most recent sample request from the media sink. The sink writer updates this value whenever it receives an MEStreamSinkRequestSample event from the media sink. The value is the current system time.
        /// </summary>	
        public long LastSinkSampleRequest;
        /// <summary>	
        /// The number of samples received.
        /// </summary>	
        public long NumSamplesReceived;
        /// <summary>	
        /// The number of samples encoded.
        /// </summary>	
        public long NumSamplesEncoded;
        /// <summary>	
        /// The number of samples given to the media sink.
        /// </summary>	
        public long NumSamplesProcessed;
        /// <summary>	
        /// The number of stream ticks received.
        /// </summary>	
        public long NumStreamTicksReceived;
        /// <summary>	
        /// The amount of data, in bytes, currently waiting to be processed. 
        /// </summary>	
        public int ByteCountQueued;
        /// <summary>	
        /// The total amount of data, in bytes, that has been sent to the media sink.
        /// </summary>	
        public long ByteCountProcessed;
        /// <summary>	
        /// The number of pending sample requests.
        /// </summary>	
        public int NumOutstandingSinkSampleRequests;
        /// <summary>	
        /// The average rate, in media samples per 100-nanoseconds, at which the application sent samples to the sink writer.
        /// </summary>	
        public int AverageSampleRateReceived;
        /// <summary>	
        /// The average rate, in media samples per 100-nanoseconds, at which the sink writer sent samples to the encoder.
        /// </summary>		
        public int AverageSampleRateEncoded;
        /// <summary>	
        /// The average rate, in media samples per 100-nanoseconds, at which the sink writer sent samples to the media sink.
        /// </summary>	
        public int AverageSampleRateProcessed;
    }
}
