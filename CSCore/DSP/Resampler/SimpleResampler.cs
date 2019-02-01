using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCore.DSP.Resampler
{
	/// <summary>
	/// Linear interpolation method.
	/// </summary>
	public sealed class SimpleResampler : ResamplerBase
	{
		/// <summary>
		/// Creates a new instance of the <see cref="SimpleResampler" /> class.
		/// </summary>
		/// <param name="source">The input sample source.</param>
		/// <param name="SampleRate">Output sampling rate</param>
		public SimpleResampler(ISampleSource source, int SampleRate) : base(source, SampleRate)
		{
			channelFilterBuffer = new float[source.WaveFormat.Channels];
		}

		float[] processBuffer = new float[8];
		readonly float[] channelFilterBuffer;
		/// <summary>
		/// Linear-Interpolated Resampling
		/// </summary>
		/// <param name="output">output buffer</param>
		/// <param name="offset">required offset</param>
		/// <param name="count">required count</param>
		/// <returns></returns>
		protected override int Process(float[] output, int offset, int count)
		{
			int samplesToRead = (int)Math.Floor(count / WaveFormat.Channels * InverseConversionRatio) * WaveFormat.Channels;
			processBuffer = processBuffer.CheckBuffer(samplesToRead);
			int read = BaseSource.Read(processBuffer, offset, count);
			int samples = count / WaveFormat.Channels;
			for (int i = 0; i < WaveFormat.Channels; i++)
			{
				for (int j = 0; j < samples; j++)
				{
					var pos = j * InverseConversionRatio;
					int indexF = (int)Math.Floor(pos);
					int indexC = Math.Min(indexF + 1, samples - 1);
					var ratio = pos - indexF;
					output[i + j * WaveFormat.Channels] = (float)(ratio * processBuffer[i + WaveFormat.Channels * indexF] + (1 - ratio) * processBuffer[i + WaveFormat.Channels * indexC]);
				}
			}
			return count;
		}
	}
}
