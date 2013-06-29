using CSCore.Utils;
using System;

namespace CSCore.Codecs.FLAC
{
    public sealed class FlacSubFrameLPC : FlacSubFrameBase
    {
        public int QLPCoeffPrecision { get; private set; }
        public int LPCShiftNeeded { get; private set; }
        public int[] QLPCoeffs { get; private set; }
        public int[] Warmup { get; private set; }

        public FlacResidual Residual { get; private set; }

        public unsafe FlacSubFrameLPC(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int bps, int order)
            : base(header)
        {
            const string loggerLocation = "FlacSubFrameLPC.ctor(...)";

            //warmup
            int[] warmup = new int[FlacConstant.MAX_LPC_ORDER];
            for (int i = 0; i < order; i++)
            {
                warmup[i] = data.residualBuffer[i] = reader.ReadBitsSigned(bps);
            }

            //header
            int u32 = (int)reader.ReadBits(FlacConstant.SUBFRAME_LPC_QLP_COEFF_PRECISION_LEN);
            if (u32 == (1 << FlacConstant.SUBFRAME_LPC_QLP_COEFF_PRECISION_LEN) - 1)
            {
                Context.Current.Logger.Fatal("Invalid FlacLPC qlp coeff precision.", loggerLocation);
                return; //return false;
            }
            QLPCoeffPrecision = u32 + 1;

            int level = reader.ReadBitsSigned(FlacConstant.SUBFRAME_LPC_QLP_SHIFT_LEN);
            if (level < 0)
                throw new Exception("negative shift");
            LPCShiftNeeded = level;

            int[] coeffs = new int[FlacConstant.MAX_LPC_ORDER];

            //qlp coeffs
            for (int i = 0; i < order; i++)
            {
                coeffs[i] = reader.ReadBitsSigned(QLPCoeffPrecision);
            }

            QLPCoeffs = coeffs;


            Residual = new FlacResidual(reader, header, data, order);

            for (int i = 0; i < order; i++)
            {
                data.destBuffer[i] = data.residualBuffer[i];
            }

            if (bps + QLPCoeffPrecision + CSMath.ILog(order) <= 32)
            {
                if (bps <= 16 && QLPCoeffPrecision <= 16)
                    RestoreLPCSignal(data.residualBuffer + order, data.destBuffer + order, header.BlockSize - order, order); //Restore(data.residualBuffer + order, data.destBuffer, Header.BlockSize - order, order, order);
                else
                    RestoreLPCSignal(data.residualBuffer + order, data.destBuffer + order, header.BlockSize - order, order);
            }
            else
            {
                RestoreLPCSignalWide(data.residualBuffer + order, data.destBuffer + order, header.BlockSize - order, order);//RestoreWide(data.residualBuffer + order, data.destBuffer, Header.BlockSize - order, order, order);
            }

            Warmup = warmup;
        }

        private unsafe void Restore(int* residual, int* dest, int length, int predictorOrder, int destOffset)
        {
            for (int i = 0; i < length; i++)
            {
                int sum = 0;
                for (int j = 0; j < predictorOrder; j++)
                {
                    sum += (int)QLPCoeffs[j] * (int)dest[destOffset + i - j - 1];
                }
                //System.Diagnostics.Debug.WriteLine(i + " " + (residual[i] + (int)(sum >> LPCShiftNeeded)));
                dest[destOffset + i] = residual[i] + (int)(sum >> LPCShiftNeeded);
            }
        }

        private unsafe void RestoreLPCSignal(int* residual, int* destination, int length, int order)
        {
            int sum = 0;

            int* r = residual;
            int* history;
            int* dest = destination;

            for (int i = 0; i < length; i++)
            {
                sum = 0;
                history = dest;
                for (int j = 0; j < order; j++)
                {
                    sum += QLPCoeffs[j] * *(--history);
                }

                *(dest++) = *(r++) + (sum >> LPCShiftNeeded);
            }
        }

        private unsafe void RestoreLPCSignalWide(int* residual, int* destination, int length, int order)
        {
            long sum = 0;

            int* r = residual;
            int* history;
            int* dest = destination;

            for (int i = 0; i < length; i++)
            {
                sum = 0;
                history = dest;
                for (int j = 0; j < order; j++)
                {
                    sum += (long)QLPCoeffs[j] * ((long)*(--history));
                }

                *(dest++) = *(r++) + (int)(sum >> LPCShiftNeeded);
            }
        }
    }
}
