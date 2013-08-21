using System.Diagnostics;
namespace CSCore.Codecs.FLAC
{
    public sealed class FlacSubFrameFixed : FlacSubFrameBase
    {
        public FlacResidual Residual { get; private set; }

        public unsafe FlacSubFrameFixed(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int bps, int order)
            : base(header)
        {
            for (int i = 0; i < order; i++)
            {
                data.residualBuffer[i] = data.destBuffer[i] = reader.ReadBitsSigned(bps);
            }

            Residual = new FlacResidual(reader, header, data, order);
            RestoreSignal(data, header.BlockSize - order, order);
        }

        //http://www.hpl.hp.com/techreports/1999/HPL-1999-144.pdf
        private unsafe bool RestoreSignal(FlacSubFrameData subframeData, int length, int predictorOrder)
        {
            int* residual = subframeData.residualBuffer + predictorOrder;
            int* data = subframeData.destBuffer + predictorOrder;

            int t0, t1, t2; //temp

            switch (predictorOrder)
            {
                case 0:
                    for (int i = 0; i < length; i++)
                    {
                        *(data++) = *(residual++);
                    }
                    break;

                case 1:
                    t1 = data[-1];
                    for (int i = 0; i < length; i++)
                    {
                        t1 += *(residual++);
                        *(data++) = t1;
                    }
                    break;

                case 2:
                    t2 = data[-2];
                    t1 = data[-1];
                    for (int i = 0; i < length; i++)
                    {
                        *(data++) = t0 = ((t1 << 1) + *(residual++)) - t2;
                        t2 = t1;
                        t1 = t0;
                    }
                    break;

                case 3:
                    for (int i = 0; i < length; i++)
                    {
                        *(data) = *(residual) +
                                    (((data[-1] - data[-2]) << 1) + (data[-1] - data[-2])) +
                                    data[-3];

                        data++;
                        residual++;
                    }
                    break;

                case 4:
                    for (int i = 0; i < length; i++)
                    {
                        *(data) = *(residual) +
                                    ((data[-1] + data[-3]) << 2) -
                                    ((data[-2] << 2) + (data[-2] << 1)) -
                                    data[-4];

                        data++;
                        residual++;
                    }
                    break;

                default:
                    Debug.WriteLine("Invalid FlacFixedSubFrame predictororder.");
                    return false;
            }

            return true;
        }
    }
}