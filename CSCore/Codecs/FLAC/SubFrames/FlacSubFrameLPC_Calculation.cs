namespace CSCore.Codecs.FLAC
{
    internal sealed partial class FlacSubFrameLPC
    {
        private unsafe void RestoreLPCSignal32(int* residual, int* destination, int length, int order, int[] qlpCoeff,
            int lpcShiftNeeded)
        {
            int* d = destination;
            int[] q = qlpCoeff;
            if (order <= 12)
            {
                int z;
                switch (order)
                {
                    case 12:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 11:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 10:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 9:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 8:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 7:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 6:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 5:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 4:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 3:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 2:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 1:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                }
            }
            else if (order > 12)
            {
                int z;
                switch (order)
                {
                    case 32:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[31] * d[i - 32]) +
                                (q[30] * d[i - 31]) +
                                (q[29] * d[i - 30]) +
                                (q[28] * d[i - 29]) +
                                (q[27] * d[i - 28]) +
                                (q[26] * d[i - 27]) +
                                (q[25] * d[i - 26]) +
                                (q[24] * d[i - 25]) +
                                (q[23] * d[i - 24]) +
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 31:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[30] * d[i - 31]) +
                                (q[29] * d[i - 30]) +
                                (q[28] * d[i - 29]) +
                                (q[27] * d[i - 28]) +
                                (q[26] * d[i - 27]) +
                                (q[25] * d[i - 26]) +
                                (q[24] * d[i - 25]) +
                                (q[23] * d[i - 24]) +
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 30:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[29] * d[i - 30]) +
                                (q[28] * d[i - 29]) +
                                (q[27] * d[i - 28]) +
                                (q[26] * d[i - 27]) +
                                (q[25] * d[i - 26]) +
                                (q[24] * d[i - 25]) +
                                (q[23] * d[i - 24]) +
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 29:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[28] * d[i - 29]) +
                                (q[27] * d[i - 28]) +
                                (q[26] * d[i - 27]) +
                                (q[25] * d[i - 26]) +
                                (q[24] * d[i - 25]) +
                                (q[23] * d[i - 24]) +
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 28:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[27] * d[i - 28]) +
                                (q[26] * d[i - 27]) +
                                (q[25] * d[i - 26]) +
                                (q[24] * d[i - 25]) +
                                (q[23] * d[i - 24]) +
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 27:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[26] * d[i - 27]) +
                                (q[25] * d[i - 26]) +
                                (q[24] * d[i - 25]) +
                                (q[23] * d[i - 24]) +
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 26:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[25] * d[i - 26]) +
                                (q[24] * d[i - 25]) +
                                (q[23] * d[i - 24]) +
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 25:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[24] * d[i - 25]) +
                                (q[23] * d[i - 24]) +
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 24:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[23] * d[i - 24]) +
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 23:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[22] * d[i - 23]) +
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 22:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[21] * d[i - 22]) +
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 21:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[20] * d[i - 21]) +
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 20:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[19] * d[i - 20]) +
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 19:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[18] * d[i - 19]) +
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 18:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[17] * d[i - 18]) +
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 17:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[16] * d[i - 17]) +
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 16:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[15] * d[i - 16]) +
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 15:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[14] * d[i - 15]) +
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 14:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[13] * d[i - 14]) +
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                    case 13:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[12] * d[i - 13]) +
                                (q[11] * d[i - 12]) +
                                (q[10] * d[i - 11]) +
                                (q[9] * d[i - 10]) +
                                (q[8] * d[i - 9]) +
                                (q[7] * d[i - 8]) +
                                (q[6] * d[i - 7]) +
                                (q[5] * d[i - 6]) +
                                (q[4] * d[i - 5]) +
                                (q[3] * d[i - 4]) +
                                (q[2] * d[i - 3]) +
                                (q[1] * d[i - 2]) +
                                (q[0] * d[i - 1])
                                ;
                            d[i] = residual[i] + (z >> lpcShiftNeeded);
                        }
                        break;
                }
            }
        }

        private unsafe void RestoreLPCSignal64(int* residual, int* destination, int length, int order, int[] qlpCoeff,
            int lpcShiftNeeded)
        {
            int* d = destination;
            int[] q = qlpCoeff;
            if (order <= 12)
            {
                long z;
                switch (order)
                {
                    case 12:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 11:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 10:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 9:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 8:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 7:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 6:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 5:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 4:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 3:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 2:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 1:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                }
            }
            else if (order > 12)
            {
                long z;
                switch (order)
                {
                    case 32:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[31] * (long) d[i - 32]) +
                                (q[30] * (long) d[i - 31]) +
                                (q[29] * (long) d[i - 30]) +
                                (q[28] * (long) d[i - 29]) +
                                (q[27] * (long) d[i - 28]) +
                                (q[26] * (long) d[i - 27]) +
                                (q[25] * (long) d[i - 26]) +
                                (q[24] * (long) d[i - 25]) +
                                (q[23] * (long) d[i - 24]) +
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 31:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[30] * (long) d[i - 31]) +
                                (q[29] * (long) d[i - 30]) +
                                (q[28] * (long) d[i - 29]) +
                                (q[27] * (long) d[i - 28]) +
                                (q[26] * (long) d[i - 27]) +
                                (q[25] * (long) d[i - 26]) +
                                (q[24] * (long) d[i - 25]) +
                                (q[23] * (long) d[i - 24]) +
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 30:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[29] * (long) d[i - 30]) +
                                (q[28] * (long) d[i - 29]) +
                                (q[27] * (long) d[i - 28]) +
                                (q[26] * (long) d[i - 27]) +
                                (q[25] * (long) d[i - 26]) +
                                (q[24] * (long) d[i - 25]) +
                                (q[23] * (long) d[i - 24]) +
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 29:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[28] * (long) d[i - 29]) +
                                (q[27] * (long) d[i - 28]) +
                                (q[26] * (long) d[i - 27]) +
                                (q[25] * (long) d[i - 26]) +
                                (q[24] * (long) d[i - 25]) +
                                (q[23] * (long) d[i - 24]) +
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 28:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[27] * (long) d[i - 28]) +
                                (q[26] * (long) d[i - 27]) +
                                (q[25] * (long) d[i - 26]) +
                                (q[24] * (long) d[i - 25]) +
                                (q[23] * (long) d[i - 24]) +
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 27:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[26] * (long) d[i - 27]) +
                                (q[25] * (long) d[i - 26]) +
                                (q[24] * (long) d[i - 25]) +
                                (q[23] * (long) d[i - 24]) +
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 26:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[25] * (long) d[i - 26]) +
                                (q[24] * (long) d[i - 25]) +
                                (q[23] * (long) d[i - 24]) +
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 25:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[24] * (long) d[i - 25]) +
                                (q[23] * (long) d[i - 24]) +
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 24:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[23] * (long) d[i - 24]) +
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 23:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[22] * (long) d[i - 23]) +
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 22:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[21] * (long) d[i - 22]) +
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 21:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[20] * (long) d[i - 21]) +
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 20:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[19] * (long) d[i - 20]) +
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 19:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[18] * (long) d[i - 19]) +
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 18:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[17] * (long) d[i - 18]) +
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 17:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[16] * (long) d[i - 17]) +
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 16:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[15] * (long) d[i - 16]) +
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 15:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[14] * (long) d[i - 15]) +
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 14:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[13] * (long) d[i - 14]) +
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                    case 13:
                        for (int i = 0; i < length; i++)
                        {
                            z =
                                (q[12] * (long) d[i - 13]) +
                                (q[11] * (long) d[i - 12]) +
                                (q[10] * (long) d[i - 11]) +
                                (q[9] * (long) d[i - 10]) +
                                (q[8] * (long) d[i - 9]) +
                                (q[7] * (long) d[i - 8]) +
                                (q[6] * (long) d[i - 7]) +
                                (q[5] * (long) d[i - 6]) +
                                (q[4] * (long) d[i - 5]) +
                                (q[3] * (long) d[i - 4]) +
                                (q[2] * (long) d[i - 3]) +
                                (q[1] * (long) d[i - 2]) +
                                (q[0] * (long) d[i - 1])
                                ;
                            d[i] = residual[i] + (int) (z >> lpcShiftNeeded);
                        }
                        break;
                }
            }
        }
    }
}

