using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
// ReSharper disable once InconsistentNaming
    internal sealed partial class FlacSubFrameLPC : FlacSubFrameBase
    {
#if FLAC_DEBUG
        public int QLPCoeffPrecision { get; private set; }

        public int LPCShiftNeeded { get; private set; }

        public int[] QLPCoeffs { get; private set; }

        public int[] Warmup { get; private set; }

        public FlacResidual Residual { get; private set; }
#endif
        public unsafe FlacSubFrameLPC(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int bitsPerSample, int order)
            : base(header)
        {
            var warmup = new int[order];
            for (int i = 0; i < order; i++)
            {
                warmup[i] = data.ResidualBuffer[i] = reader.ReadBitsSigned(bitsPerSample);
            }

            int coefPrecision = (int)reader.ReadBits(4);
            if (coefPrecision == 0x0F)
                throw new FlacException("Invalid \"quantized linear predictor coefficients' precision in bits\" was invalid. Must not be 0x0F.",
                    FlacLayer.SubFrame);
            coefPrecision += 1;

            int shiftNeeded = reader.ReadBitsSigned(5);
            if (shiftNeeded < 0)
                throw new FlacException("'\"Quantized linear predictor coefficient shift needed in bits\" was negative.", FlacLayer.SubFrame);

            var q = new int[order];
            for (int i = 0; i < order; i++)
            {
                q[i] = reader.ReadBitsSigned(coefPrecision);
            }

            //decode the residual
            var residual = new FlacResidual(reader, header, data, order);
            for (int i = 0; i < order; i++)
            {
                data.DestinationBuffer[i] = data.ResidualBuffer[i];
            }

            int* residualBuffer0 = data.ResidualBuffer + order;
            int* destinationBuffer0 = data.DestinationBuffer + order;
            int blockSizeToProcess = header.BlockSize - order;

            if (bitsPerSample + coefPrecision + Log2(order) <= 32)
            {
                RestoreLPCSignal32(residualBuffer0, destinationBuffer0, blockSizeToProcess, order, q, shiftNeeded);
            }
            else
            {
                RestoreLPCSignal64(residualBuffer0, destinationBuffer0, blockSizeToProcess, order, q, shiftNeeded);
            }

#if FLAC_DEBUG
            QLPCoeffPrecision = coefPrecision;
            LPCShiftNeeded = shiftNeeded;
            Warmup = warmup;
            Residual = residual;
            QLPCoeffs = q;
#endif
        }

        /// <summary>
        /// Copied from http://stackoverflow.com/questions/8970101/whats-the-quickest-way-to-compute-log2-of-an-integer-in-c 14.01.2015
        /// </summary>
        private int Log2(int x)
        {
            int bits = 0;
            while (x > 0)
            {
                bits++;
                x >>= 1;
            }
            return bits;
        }
    }
}