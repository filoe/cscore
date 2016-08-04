/****************************************************************************
* This code was copied from an external source to CSCore (see below).
* The author of the code gave the explicit permission to use this code
* inside of CSCore and republish it under the MS-PL.
*
* Thanks!
****************************************************************************/

/****************************************************************************
*
* VERSION: 1.0
* HOME URL: http://www.dspdimension.com
* KNOWN BUGS: none
*
* SYNOPSIS: Routine for doing pitch shifting while maintaining
* duration using the Short Time Fourier Transform.
*
* DESCRIPTION: The routine takes a pitchShift factor value which is between 0.5
* (one octave down) and 2. (one octave up). A value of exactly 1 does not change
* the pitch. numSampsToProcess tells the routine how many samples in indata[0...
* numSampsToProcess-1] should be pitch shifted and moved to outdata[0 ...
* numSampsToProcess-1]. The two buffers can be identical (ie. it can process the
* data in-place). fftFrameSize defines the FFT frame size used for the
* processing. Typical values are 1024, 2048 and 4096. It may be any value <=
* MAX_FRAME_LENGTH but it MUST be a power of 2. osamp is the STFT
* oversampling factor which also determines the overlap between adjacent STFT
* frames. It should at least be 4 for moderate scaling ratios. A value of 32 is
* recommended for best quality. sampleRate takes the sample rate for the signal 
* in unit Hz, ie. 44100 for 44.1 kHz audio. The data passed to the routine in 
* indata[] should be in the range [-1.0, 1.0), which is also the output range 
* for the data, make sure you scale the data accordingly (for 16bit signed integers
* you would have to divide (and multiply) by 32768). 
*
* COPYRIGHT 1999-2006 Stephan M. Bernsee <smb [AT] dspdimension [DOT] com>
*****************************************************************************/

/****************************************************************************
*
* This code was converted to C# by Michael Knight
* madmik3 at gmail dot com. 
* http://sites.google.com/site/mikescoderama/
*
*****************************************************************************/

using System;

namespace CSCore.DSP
{
    internal class PitchShifterInternal
    {
        private static readonly int MaxFrameLength = 16000;
        private static readonly float[] InFifo = new float[MaxFrameLength];
        private static readonly float[] OutFifo = new float[MaxFrameLength];
        private static readonly float[] FfTworksp = new float[2 * MaxFrameLength];
        private static readonly float[] LastPhase = new float[MaxFrameLength / 2 + 1];
        private static readonly float[] SumPhase = new float[MaxFrameLength / 2 + 1];
        private static readonly float[] OutputAccum = new float[2 * MaxFrameLength];
        private static readonly float[] AnaFreq = new float[MaxFrameLength];
        private static readonly float[] AnaMagn = new float[MaxFrameLength];
        private static readonly float[] SynFreq = new float[MaxFrameLength];
        private static readonly float[] SynMagn = new float[MaxFrameLength];
        private static long _gRover;

        public static void PitchShift(float pitchShift, long numSampsToProcess,
           float sampleRate, float[] indata)
        {
            PitchShift(pitchShift, numSampsToProcess, 2048, 10, sampleRate, indata);
        }
        public static void PitchShift(float pitchShift, long numSampsToProcess, long fftFrameSize,
            long osamp, float sampleRate, float[] indata)
        {
            long i;


            float[] outdata = indata;
            /* set up some handy variables */
            var fftFrameSize2 = fftFrameSize / 2;
            var stepSize = fftFrameSize / osamp;
            var freqPerBin = sampleRate / (double)fftFrameSize;
            var expct = 2.0 * Math.PI * stepSize / fftFrameSize;
            var inFifoLatency = fftFrameSize - stepSize;
            if (_gRover == 0) _gRover = inFifoLatency;


            /* main processing loop */
            for (i = 0; i < numSampsToProcess; i++)
            {

                /* As long as we have not yet collected enough data just read in */
                InFifo[_gRover] = indata[i];
                outdata[i] = OutFifo[_gRover - inFifoLatency];
                _gRover++;

                /* now we have enough data for processing */
                if (_gRover >= fftFrameSize)
                {
                    _gRover = inFifoLatency;

                    /* do windowing and re,im interleave */
                    double window;
                    long k;
                    for (k = 0; k < fftFrameSize; k++)
                    {
                        window = -.5 * Math.Cos(2.0 * Math.PI * k / fftFrameSize) + .5;
                        FfTworksp[2 * k] = (float)(InFifo[k] * window);
                        FfTworksp[2 * k + 1] = 0.0F;
                    }


                    /* ***************** ANALYSIS ******************* */
                    /* do transform */
                    ShortTimeFourierTransform(FfTworksp, fftFrameSize, -1);

                    /* this is the analysis step */
                    double magn;
                    double phase;
                    double tmp;
                    for (k = 0; k <= fftFrameSize2; k++)
                    {

                        /* de-interlace FFT buffer */
                        double real = FfTworksp[2 * k];
                        double imag = FfTworksp[2 * k + 1];

                        /* compute magnitude and phase */
                        magn = 2.0 * Math.Sqrt(real * real + imag * imag);
                        phase = Math.Atan2(imag, real);

                        /* compute phase difference */
                        tmp = phase - LastPhase[k];
                        LastPhase[k] = (float)phase;

                        /* subtract expected phase difference */
                        tmp -= k * expct;

                        /* map delta phase into +/- Pi interval */
                        var qpd = (long)(tmp / Math.PI);
                        if (qpd >= 0) qpd += qpd & 1;
                        else qpd -= qpd & 1;
                        tmp -= Math.PI * qpd;

                        /* get deviation from bin frequency from the +/- Pi interval */
                        tmp = osamp * tmp / (2.0 * Math.PI);

                        /* compute the k-th partials' true frequency */
                        tmp = k * freqPerBin + tmp * freqPerBin;

                        /* store magnitude and true frequency in analysis arrays */
                        AnaMagn[k] = (float)magn;
                        AnaFreq[k] = (float)tmp;

                    }

                    /* ***************** PROCESSING ******************* */
                    /* this does the actual pitch shifting */
                    for (int zero = 0; zero < fftFrameSize; zero++)
                    {
                        SynMagn[zero] = 0;
                        SynFreq[zero] = 0;
                    }

                    for (k = 0; k <= fftFrameSize2; k++)
                    {
                        var index = (long)(k * pitchShift);
                        if (index <= fftFrameSize2)
                        {
                            SynMagn[index] += AnaMagn[k];
                            SynFreq[index] = AnaFreq[k] * pitchShift;
                        }
                    }

                    /* ***************** SYNTHESIS ******************* */
                    /* this is the synthesis step */
                    for (k = 0; k <= fftFrameSize2; k++)
                    {

                        /* get magnitude and true frequency from synthesis arrays */
                        magn = SynMagn[k];
                        tmp = SynFreq[k];

                        /* subtract bin mid frequency */
                        tmp -= k * freqPerBin;

                        /* get bin deviation from freq deviation */
                        tmp /= freqPerBin;

                        /* take osamp into account */
                        tmp = 2.0 * Math.PI * tmp / osamp;

                        /* add the overlap phase advance back in */
                        tmp += k * expct;

                        /* accumulate delta phase to get bin phase */
                        SumPhase[k] += (float)tmp;
                        phase = SumPhase[k];

                        /* get real and imag part and re-interleave */
                        FfTworksp[2 * k] = (float)(magn * Math.Cos(phase));
                        FfTworksp[2 * k + 1] = (float)(magn * Math.Sin(phase));
                    }

                    /* zero negative frequencies */
                    for (k = fftFrameSize + 2; k < 2 * fftFrameSize; k++) FfTworksp[k] = 0.0F;

                    /* do inverse transform */
                    ShortTimeFourierTransform(FfTworksp, fftFrameSize, 1);

                    /* do windowing and add to output accumulator */
                    for (k = 0; k < fftFrameSize; k++)
                    {
                        window = -.5 * Math.Cos(2.0 * Math.PI * k / fftFrameSize) + .5;
                        OutputAccum[k] += (float)(2.0 * window * FfTworksp[2 * k] / (fftFrameSize2 * osamp));
                    }
                    for (k = 0; k < stepSize; k++) OutFifo[k] = OutputAccum[k];

                    /* shift accumulator */
                    //memmove(gOutputAccum, gOutputAccum + stepSize, fftFrameSize * sizeof(float));
                    for (k = 0; k < fftFrameSize; k++)
                    {
                        OutputAccum[k] = OutputAccum[k + stepSize];
                    }

                    /* move input FIFO */
                    for (k = 0; k < inFifoLatency; k++) InFifo[k] = InFifo[k + stepSize];
                }
            }
        }

        public static void ShortTimeFourierTransform(float[] fftBuffer, long fftFrameSize, long sign)
        {
            long i;
            long j, le;
            long k;

            for (i = 2; i < 2 * fftFrameSize - 2; i += 2)
            {
                long bitm;
                for (bitm = 2, j = 0; bitm < 2 * fftFrameSize; bitm <<= 1)
                {
                    if ((i & bitm) != 0) j++;
                    j <<= 1;
                }
                if (i < j)
                {
                    var temp = fftBuffer[i];
                    fftBuffer[i] = fftBuffer[j];
                    fftBuffer[j] = temp;
                    temp = fftBuffer[i + 1];
                    fftBuffer[i + 1] = fftBuffer[j + 1];
                    fftBuffer[j + 1] = temp;
                }
            }
            long max = (long)(Math.Log(fftFrameSize) / Math.Log(2.0) + .5);
            for (k = 0, le = 2; k < max; k++)
            {
                le <<= 1;
                var le2 = le >> 1;
                var ur = 1.0F;
                var ui = 0.0F;
                var arg = (float)Math.PI / (le2 >> 1);
                var wr = (float)Math.Cos(arg);
                var wi = (float)(sign * Math.Sin(arg));
                for (j = 0; j < le2; j += 2)
                {
                    float tr;
                    for (i = j; i < 2 * fftFrameSize; i += le)
                    {
                        tr = fftBuffer[i + le2] * ur - fftBuffer[i + le2 + 1] * ui;
                        var ti = fftBuffer[i + le2] * ui + fftBuffer[i + le2 + 1] * ur;
                        fftBuffer[i + le2] = fftBuffer[i] - tr;
                        fftBuffer[i + le2 + 1] = fftBuffer[i + 1] - ti;
                        fftBuffer[i] += tr;
                        fftBuffer[i + 1] += ti;

                    }
                    tr = ur * wr - ui * wi;
                    ui = ur * wi + ui * wr;
                    ur = tr;
                }
            }
        }
    }
}
