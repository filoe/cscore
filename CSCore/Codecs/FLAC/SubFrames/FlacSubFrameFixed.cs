using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    internal sealed class FlacSubFrameFixed : FlacSubFrameBase
    {
#if FLAC_DEBUG
        public FlacResidual Residual { get; private set; }
#endif
        public unsafe FlacSubFrameFixed(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int bitsPerSample, int order)
            : base(header)
        {
            for (int i = 0; i < order; i++) //order = predictor order
            {
                data.ResidualBuffer[i] = data.DestinationBuffer[i] = reader.ReadBitsSigned(bitsPerSample);
            }

            var residual = new FlacResidual(reader, header, data, order); //necessary for decoding
            RestoreSignal(data, header.BlockSize - order, order);

#if FLAC_DEBUG
            Residual = residual;
#endif
        }

        private unsafe void RestoreSignal(FlacSubFrameData subframeData, int length, int order)
        {
            //see ftp://svr-ftp.eng.cam.ac.uk/pub/reports/auto-pdf/robinson_tr156.pdf chapter 3.2
            int* residual = subframeData.ResidualBuffer + order;
            int* destBuffer = subframeData.DestinationBuffer + order;

            switch (order)
            {
                case 0:
                    for (int i = 0; i < length; i++)
                    {
                        destBuffer[i] = residual[i];
                    }
                    //ILUtils.MemoryCopy(data, residual, length);
                    break;

                case 1:
                    for (int i = 0; i < length; i++)
                    {
                        //s(t-1)
                        destBuffer[i] = residual[i] + destBuffer[i - 1];
                    }
                    break;

                case 2:
                    for (int i = 0; i < length; i++)
                    {
                        //2s(t-1) - s(t-2)
                        destBuffer[i] = residual[i] + 2 * destBuffer[i - 1] - destBuffer[i - 2];
                    }

                    break;

                case 3:
                    for (int t = 0; t < length; t++)
                    {
                        //3s(t-1) - 3s(t-2) + s(t-3)
                        destBuffer[t] = residual[t] + 
                            3 * (destBuffer[t - 1]) - 3 * (destBuffer[t - 2]) + destBuffer[t - 3]; 
                    }
                    break;

                case 4:
                    //"FLAC adds a fourth-order predictor to the zero-to-third-order predictors used by Shorten." (see https://xiph.org/flac/format.html#prediction)
                    for (int t = 0; t < length; t++)
                    {
                        destBuffer[t] = residual[t] +
                            4 * destBuffer[t - 1] - 6 * destBuffer[t - 2] + 4 * destBuffer[t - 3] - destBuffer[t - 4];
                    }
                    break;

                default:
                    Debug.WriteLine("Invalid FlacFixedSubFrame predictororder.");
                    return;
            }
        }
    }
}