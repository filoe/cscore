using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCore.DSP.Resampler
{
	/// <summary>
	///
	/// </summary>
	public abstract class ResamplerBase : SampleAggregatorBase
	{
		/// <summary>
		/// Sample rate of input source.
		/// </summary>
		public int SampleRateFrom => BaseSource.WaveFormat.SampleRate;

		/// <summary>
		/// Conversion ratio(out/in).
		/// </summary>
		protected double ConversionRatio => (double)WaveFormat.SampleRate / SampleRateFrom;

		/// <summary>
		/// Conversion ratio(in/out).
		/// </summary>
		protected double InverseConversionRatio => (double)SampleRateFrom / WaveFormat.SampleRate;

		/// <summary>
		///     Gets the <see cref="IAudioSource.WaveFormat" /> of the waveform-audio data.
		/// </summary>
		public override WaveFormat WaveFormat { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="ResamplerBase" /> class.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="SampleRate"></param>
		public ResamplerBase(ISampleSource source, int SampleRate) : base(source)
		{
			WaveFormat = new WaveFormat(SampleRate, 32, source.WaveFormat.Channels, AudioEncoding.IeeeFloat);
		}

		/// <summary>
		///     Reads a sequence of samples from the <see cref="SampleAggregatorBase" /> and advances the position within the stream by
		///     the number of samples read.
		/// </summary>
		/// <param name="buffer">
		///     An array of floats. When this method returns, the <paramref name="buffer" /> contains the specified
		///     float array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
		///     <paramref name="count" /> - 1) replaced by the floats read from the current source.
		/// </param>
		/// <param name="offset">
		///     The zero-based offset in the <paramref name="buffer" /> at which to begin storing the data
		///     read from the current stream.
		/// </param>
		/// <param name="count">The maximum number of samples to read from the current source.</param>
		/// <returns>The total number of samples read into the buffer.</returns>
		public override int Read(float[] buffer, int offset, int count)
		{
			if (offset % WaveFormat.Channels != 0)
				offset -= offset % WaveFormat.Channels;
			if (count % WaveFormat.Channels != 0)
				count -= count % WaveFormat.Channels;
			return Process(buffer, offset, count);
		}

		/// <summary>
		/// Actual Resampling...
		/// </summary>
		/// <param name="output"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		protected abstract int Process(float[] output, int offset, int count);
	}
}
