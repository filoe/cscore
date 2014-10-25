using System;
using System.Collections.Generic;
using CSCore.DMO;

namespace CSCore.DSP
{
    /// <summary>
    ///     Defines a channel conversion matrix. For more details see
    ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ff819070(v=vs.85).aspx" />.
    /// </summary>
    public class ChannelMatrix
    {
        /// <summary>
        ///     Defines a stereo to 5.1 surround (with rear) channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix StereoToFiveDotOneSurroundWithRear;

        /// <summary>
        ///     Defines a 5.1 surround (with rear) to stereo channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix FiveDotOneSurroundWithRearToStereo;

        /// <summary>
        ///     Defines a stereo to 5.1 surround (with side) channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix StereoToFiveDotOneSurroundWithSide;

        /// <summary>
        ///     Defines a 5.1 surround (with side) to stereo channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix FiveDotOneSurroundWithSideToStereo;

        /// <summary>
        ///     Defines a stereo to 7.1 surround channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix StereoToSevenDotOneSurround;

        /// <summary>
        ///     Defines a 7.1 surround to stereo channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix SevenDotOneSurroundToStereo;

        //--

        /// <summary>
        ///     Defines a mono to 5.1 surround (with rear) channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix MonoToFiveDotOneSurroundWithRear;

        /// <summary>
        ///     Defines a 5.1 surround (with rear) to mono channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix FiveDotOneSurroundWithRearToMono;


        /// <summary>
        ///     Defines a mono to 5.1 surround (with side) channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix MonoToFiveDotOneSurroundWithSide;

        /// <summary>
        ///     Defines a 5.1 surround (with side) to mono channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix FiveDotOneSurroundWithSideToMono;

        /// <summary>
        ///     Defines a mono to 7.1 surround channel conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix MonoToSevenDotOneSurround;

        /// <summary>
        ///     Defines a 7.1 surround channel to mono conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix SevenDotOneSurroundToMono;

        /// <summary>
        ///     Defines a stereo to mono conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix StereoToMonoMatrix;

        /// <summary>
        ///     Defines a mono to stereo conversion matrix.
        /// </summary>
        public static readonly ChannelMatrix MonoToStereoMatrix;

        internal static ChannelMatrix GetToStereoChannelMatrix(ChannelMask inputChannelMask)
        {
            if (inputChannelMask == FiveDotOneSurroundWithRearToStereo.InputMask)
                return FiveDotOneSurroundWithRearToStereo;
            if (inputChannelMask == FiveDotOneSurroundWithSideToStereo.InputMask)
                return FiveDotOneSurroundWithSideToStereo;
            if (inputChannelMask == SevenDotOneSurroundToStereo.InputMask)
                return SevenDotOneSurroundToStereo;
            if (inputChannelMask == MonoToStereoMatrix.InputMask)
                return MonoToStereoMatrix;
            throw new ArgumentException("No suitable channelmatrix could be found.");
        }

        internal static ChannelMatrix GetToMonoChannelMatrix(ChannelMask inputChannelMask)
        {
            if (inputChannelMask == FiveDotOneSurroundWithRearToMono.InputMask)
                return FiveDotOneSurroundWithRearToMono;
            if (inputChannelMask == FiveDotOneSurroundWithSideToMono.InputMask)
                return FiveDotOneSurroundWithSideToMono;
            if (inputChannelMask == SevenDotOneSurroundToMono.InputMask)
                return SevenDotOneSurroundToMono;
            if (inputChannelMask == StereoToMonoMatrix.InputMask)
                return StereoToMonoMatrix;
            throw new ArgumentException("No suitable channelmatrix could be found.");
        }

        private readonly ChannelMask _inputMask;
        private readonly ChannelMatrixElement[,] _matrix;
        private readonly ChannelMask _outputMask;

        static ChannelMatrix()
        {
            StereoToFiveDotOneSurroundWithRear = new ChannelMatrix(
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight,
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight |
                ChannelMask.SpeakerFrontCenter | ChannelMask.SpeakerLowFrequency |
                ChannelMask.SpeakerBackLeft | ChannelMask.SpeakerBackRight);
            StereoToFiveDotOneSurroundWithRear.SetMatrix(
                new[,]
                {
                    {0.314f, 0f, 0.222f, 0.031f, 0.268f, 0.164f}, //left      - input
                    {0f, 0.314f, 0.222f, 0.031f, 0.164f, 0.268f} //right     - output
                });
            FiveDotOneSurroundWithRearToStereo = StereoToFiveDotOneSurroundWithRear.Flip();

            StereoToFiveDotOneSurroundWithSide = new ChannelMatrix(
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight,
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight |
                ChannelMask.SpeakerFrontCenter | ChannelMask.SpeakerLowFrequency |
                ChannelMask.SpeakerSideLeft | ChannelMask.SpeakerSideRight);
            StereoToFiveDotOneSurroundWithSide.SetMatrix(
                new[,]
                {
                    {0.320f, 0f, 0.226f, 0.032f, 0.292f, 0.130f},
                    {0f, 0.320f, 0.226f, 0.032f, 0.130f, 0.292f}
                });
            FiveDotOneSurroundWithSideToStereo = StereoToFiveDotOneSurroundWithSide.Flip();

            StereoToSevenDotOneSurround = new ChannelMatrix(
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight,
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight |
                ChannelMask.SpeakerFrontCenter | ChannelMask.SpeakerLowFrequency |
                ChannelMask.SpeakerSideLeft | ChannelMask.SpeakerSideRight |
                ChannelMask.SpeakerBackLeft | ChannelMask.SpeakerBackRight);
            StereoToSevenDotOneSurround.SetMatrix(
                new[,]
                {
                    {0.222f, 0f, 0.157f, 0.022f, 0.189f, 0.116f, 0.203f, 0.090f},
                    {0f, 0.222f, 0.157f, 0.022f, 0.116f, 0.189f, 0.090f, 0.203f}
                });
            SevenDotOneSurroundToStereo = StereoToSevenDotOneSurround.Flip();

            //--

            MonoToFiveDotOneSurroundWithRear = new ChannelMatrix(
                ChannelMask.SpeakerFrontCenter,
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight |
                ChannelMask.SpeakerFrontCenter | ChannelMask.SpeakerLowFrequency |
                ChannelMask.SpeakerBackLeft | ChannelMask.SpeakerBackRight);
            MonoToFiveDotOneSurroundWithRear.SetMatrix(
                new[,]
                {
                    {0.192f, 0.192f, 0.192f, 0.038f, 0.192f, 0.192f}
                });
            MonoToFiveDotOneSurroundWithSide = MonoToFiveDotOneSurroundWithRear; //its the same
            FiveDotOneSurroundWithRearToMono = MonoToFiveDotOneSurroundWithRear.Flip();
            FiveDotOneSurroundWithSideToMono = MonoToFiveDotOneSurroundWithSide.Flip();

            MonoToSevenDotOneSurround = new ChannelMatrix(
                ChannelMask.SpeakerFrontCenter, 
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight |
                ChannelMask.SpeakerFrontCenter | ChannelMask.SpeakerLowFrequency |
                ChannelMask.SpeakerSideLeft | ChannelMask.SpeakerSideRight |
                ChannelMask.SpeakerBackLeft | ChannelMask.SpeakerBackRight);
            MonoToSevenDotOneSurround.SetMatrix(
                new[,]
                {
                    {0.139f, 0.139f, 0.139f, 0.028f, 0.139f, 0.139f, 0.139f, 0.139f}
                });
            SevenDotOneSurroundToMono = MonoToSevenDotOneSurround.Flip();

            StereoToMonoMatrix = new ChannelMatrix(ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight,
                ChannelMask.SpeakerFrontCenter);
            StereoToMonoMatrix.SetMatrix(
                new[,]
                {
                    {0.5f}, //left
                    {0.5f} //right
                });

            MonoToStereoMatrix = new ChannelMatrix(ChannelMask.SpeakerFrontCenter,
                ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight);
            MonoToStereoMatrix.SetMatrix(
                new[,]
                {
                    //left|right
                    {1f, 1f} //mono
                });
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChannelMatrix" /> class.
        /// </summary>
        /// <param name="inputMask">The <see cref="ChannelMask" /> of the input signal.</param>
        /// <param name="outputMask">The <see cref="ChannelMask" /> of the output signal.</param>
        /// <exception cref="ArgumentException">Invalid <paramref name="inputMask" />/<paramref name="outputMask" />.</exception>
        public ChannelMatrix(ChannelMask inputMask, ChannelMask outputMask)
        {
            _inputMask = inputMask;
            _outputMask = outputMask;

            if ((int) inputMask <= 0)
                throw new ArgumentException("Invalid inputMask");
            if ((int) outputMask <= 0)
                throw new ArgumentException("Invalid outputMask");

            _matrix =
                new ChannelMatrixElement[GetValuesOfChannelMask(inputMask).Length,
                    GetValuesOfChannelMask(outputMask).Length];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _matrix[y, x] = new ChannelMatrixElement(GetValuesOfChannelMask(inputMask)[y],
                        GetValuesOfChannelMask(outputMask)[x]);
                }
            }
        }

        /// <summary>
        ///     Gets the <see cref="ChannelMask" /> of the input signal.
        /// </summary>
        public ChannelMask InputMask
        {
            get { return _inputMask; }
        }

        /// <summary>
        ///     Gets the <see cref="ChannelMask" /> of the output signal.
        /// </summary>
        public ChannelMask OutputMask
        {
            get { return _outputMask; }
        }

        /// <summary>
        ///     Gets the number of rows of the channel conversion matrix.
        /// </summary>
        public int Height
        {
            get { return _matrix.GetLength(0); }
        }

        /// <summary>
        ///     Gets the number of columns of the channel conversion matrix.
        /// </summary>
        public int Width
        {
            get { return _matrix.GetLength(1); }
        }

        /// <summary>
        ///     Gets the input signals number of channels.
        /// </summary>
        /// <remarks>
        ///     The <see cref="InputChannelCount" /> property always returns the same value as the <see cref="Height" />
        ///     property.
        /// </remarks>
        public int InputChannelCount
        {
            get { return Height; }
        }

        /// <summary>
        ///     Gets the output signals number of channels.
        /// </summary>
        /// <remarks>
        ///     The <see cref="OutputChannelCount" /> property always returns the same value as the <see cref="Width" />
        ///     property.
        /// </remarks>
        public int OutputChannelCount
        {
            get { return Width; }
        }

        /// <summary>
        ///     Gets or sets a <see cref="ChannelMatrixElement" /> of the <see cref="ChannelMatrix" />.
        /// </summary>
        /// <param name="input">The zero-based index of the input channel.</param>
        /// <param name="output">The zero-based index of the output channel.</param>
        /// <returns>The <see cref="ChannelMatrixElement" /> of the <see cref="ChannelMatrix" /> at the specified position.</returns>
        public ChannelMatrixElement this[int input, int output]
        {
            get { return _matrix[input, output]; }
            set { _matrix[input, output] = value; }
        }

        /// <summary>
        ///     Sets the channel conversion matrix.
        ///     The x-axis of the <paramref name="matrix" /> specifies the output channels. The y-axis
        ///     of the <paramref name="matrix" /> specifies the input channels.
        /// </summary>
        /// <param name="matrix">Channel conversion matrix to use.</param>
        public void SetMatrix(float[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentException("matrix");
            if (matrix.GetLength(1) != Width)
                throw new ArgumentException("Matrix has to have a width of " + Width);
            if (matrix.GetLength(0) != Height)
                throw new ArgumentException("Matrix has to have a height of " + Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    this[y, x].Value = matrix[y, x];
                }
            }
        }

        /// <summary>
        ///     Returns a one dimensional array which contains the channel conversion matrix coefficients.
        /// </summary>
        /// <returns>A one dimensional array which contains the channel conversion matrix coefficients</returns>
        /// <remarks>
        ///     This method is primarily used in combination with the <see cref="WMResamplerProps.SetUserChannelMtx" />
        ///     method.
        /// </remarks>
        public float[] GetOneDimensionalMatrix()
        {
            var result = new List<float>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    result.Add(this[y, x].Value);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        ///     Flips the axis of the matrix and returns the new matrix with the flipped axis.
        /// </summary>
        /// <returns>A matrix with flipped axis.</returns>
        /// <remarks>
        ///     This could be typically used in the following scenario: There is a
        ///     5.1 to stereo matrix. By using the <see cref="Flip" /> method the 5.1 to stereo matrix can be
        ///     converted into a stereo to 5.1 matrix.
        /// </remarks>
        public ChannelMatrix Flip()
        {
            var result = new ChannelMatrix(OutputMask, InputMask);
            for (int x = 0; x < OutputChannelCount; x++)
            {
                for (int y = 0; y < InputChannelCount; y++)
                {
                    ChannelMatrixElement input = this[y, x];
                    result[x, y] = new ChannelMatrixElement(input.OutputChannel, input.InputChannel)
                    {
                        Value = input.Value
                    };
                }
            }
            return result;
        }

        private static ChannelMask[] GetValuesOfChannelMask(ChannelMask channelMask)
        {
            Array totalChannelMaskValues = Enum.GetValues(typeof (ChannelMask));
            var values = new List<ChannelMask>();
            for (int i = 0; i < totalChannelMaskValues.Length; i++)
            {
                if ((channelMask & ((ChannelMask) totalChannelMaskValues.GetValue(i))) ==
                    (ChannelMask) totalChannelMaskValues.GetValue(i))
                    values.Add((ChannelMask) totalChannelMaskValues.GetValue(i));
            }

            return values.ToArray();
        }
    }
}