using System;
using CSCore.DSP;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// A pitch shifting effect. 
    /// </summary>
    /// <remarks>
    /// The internal pitch shifting code is based on the implementation of
    /// Stephan M. Bernsee smb@dspdimension.com (see http://www.dspdimension.com) and 
    /// Michael Knight madmik3@gmail.com (http://sites.google.com/site/mikescoderama/) who 
    /// translated Stephan's code to C#.
    /// 
    /// Both gave the explicit permission to republish the code as part of CSCore under the MS-PL.
    /// Big thanks!
    /// </remarks>
    public class PitchShifter : SampleAggregatorBase
    {
        /// <summary>
        /// Gets or sets the pitch shift factor.
        /// </summary>
        /// <value>
        /// A pitch shift factor value which is between 0.5
        /// (one octave down) and 2. (one octave up). A value of exactly 1 does not change
        /// the pitch.
        /// </value>
        public float PitchShiftFactor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PitchShifter"/> class.
        /// </summary>
        /// <param name="source">Underlying base source which provides audio data.</param>
        public PitchShifter(ISampleSource source) : base(source)
        {
            PitchShiftFactor = 1.0f;
        }

        /// <summary>
        /// Reads a sequence of samples from the <see cref="SampleAggregatorBase" />, applies the pitch shifting to them 
        /// and advances the position within the stream by the number of samples read.
        /// </summary>
        /// <param name="buffer">An array of floats. When this method returns, the <paramref name="buffer" /> contains the specified
        /// float array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        /// <paramref name="count" /> - 1) replaced by the floats read from the current source including the applied pitch shift.</param>
        /// <param name="offset">The zero-based offset in the <paramref name="buffer" /> at which to begin storing the data
        /// read from the current stream.</param>
        /// <param name="count">The maximum number of samples to read from the current source.</param>
        /// <returns>
        /// The total number of samples read into the buffer.
        /// </returns>
        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);

            if (read > 0 && Math.Abs(PitchShiftFactor - 1.0) > 0.001)
            {
                float[] pitchBuffer = buffer;
                if (offset != 0)
                {
                    pitchBuffer = new float[read];
                    Buffer.BlockCopy(buffer, offset, pitchBuffer, 0, read);
                }

                PitchShifterInternal.PitchShift(PitchShiftFactor, read, WaveFormat.SampleRate, buffer);

                if (offset != 0)
                {
                    Buffer.BlockCopy(pitchBuffer, 0, buffer, offset, read);
                }

                for (int i = offset; i < offset + read; i++)
                {
                    if (buffer[i] < -1.0 || buffer[i] > 1.0)
                        buffer[i] = Math.Max(-1.0f, Math.Min(1.0f, buffer[i]));
                }
            }

            return read;
        }
    }
}
