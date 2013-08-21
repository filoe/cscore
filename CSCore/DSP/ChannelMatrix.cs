using System;
using System.Collections.Generic;

namespace CSCore.DSP
{
    public class ChannelMatrix
    {
        public static readonly ChannelMatrix TwoToSixChannels;
        public static readonly ChannelMatrix StereoToMonoMatrix;
        public static readonly ChannelMatrix MonoToStereoMatrix;

        static ChannelMatrix()
        {
            ChannelMatrix c = new ChannelMatrix((ChannelMask)0x3, (ChannelMask)0x3F);
            float[,] m = new float[,]
            {
                { 0.314f, 0f    , 0.222f, 0.031f, 0.268f, 0.164f }, //left      - input
                { 0f    , 0.314f, 0.222f, 0.031f, 0.164f, 0.268f }  //right     - output
            };
            c.SetMatrix(m);
            TwoToSixChannels = c;

            StereoToMonoMatrix = new ChannelMatrix(ChannelMask.SpeakerFrontCenter, ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight);
            StereoToMonoMatrix[0, 0].Value = 0.5f;
            StereoToMonoMatrix[0, 1].Value = 0.5f;

            MonoToStereoMatrix = new ChannelMatrix(ChannelMask.SpeakerFrontLeft | ChannelMask.SpeakerFrontRight, ChannelMask.SpeakerFrontCenter);
            MonoToStereoMatrix[0, 0].Value = 1f;
            MonoToStereoMatrix[1, 0].Value = 1f;
        }

        private ChannelMask _inputMask;
        private ChannelMask _outputMask;

        public ChannelMask InputMask { get { return _inputMask; } }

        public ChannelMask OutputMask { get { return _outputMask; } }

        private ChannelMatrixElement[,] _matrix;

        public int Height
        {
            get { return _matrix.GetLength(1); }
        }

        public int Width
        {
            get { return _matrix.GetLength(0); }
        }

        /// <summary>
        /// Equals Width <see cref="Width"/>
        /// </summary>
        public int InputChannelCount
        {
            get { return Width; }
        }

        /// <summary>
        /// Equals Height <see cref="Height"/>
        /// </summary>
        public int OutputChannelCount
        {
            get { return Height; }
        }

        /// <summary>
        /// </summary>
        /// <param name="input">X Axis</param>
        /// <param name="output">Y Axis</param>
        /// <returns></returns>
        public ChannelMatrixElement this[int input, int output]
        {
            get { return _matrix[input, output]; }
        }

        public ChannelMatrix(ChannelMask inputMask, ChannelMask outputMask)
        {
            _inputMask = inputMask;
            _outputMask = outputMask;

            if ((int)inputMask <= 0)
                throw new ArgumentException("Invalid inputMask");
            if ((int)outputMask <= 0)
                throw new ArgumentException("Invalid outputMask");

            _matrix = new ChannelMatrixElement[GetValuesOfChannelMask(inputMask).Length, GetValuesOfChannelMask(outputMask).Length];
            for (int x = 0; x < _matrix.GetLength(0); x++)
            {
                for (int y = 0; y < _matrix.GetLength(1); y++)
                {
                    _matrix[x, y] = new ChannelMatrixElement(GetValuesOfChannelMask(inputMask)[x], GetValuesOfChannelMask(outputMask)[y]);
                }
            }
        }

        public void SetMatrix(float[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentException("matrix");
            if (matrix.GetLength(0) != Width)
                throw new ArgumentException("Matrix has to have a width of " + Width);
            if (matrix.GetLength(1) != Height)
                throw new ArgumentException("Matrix has to have a height of " + Height);

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    this[x, y].Value = matrix[x, y];
                }
            }
        }

        public float[] GetMatrix()
        {
            List<float> result = new List<float>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    result.Add(this[x, y].Value);
                }
            }

            return result.ToArray();
        }

        private static ChannelMask[] GetValuesOfChannelMask(ChannelMask channelMask)
        {
            var totalChannelMaskValues = Enum.GetValues(typeof(ChannelMask));
            List<ChannelMask> values = new List<ChannelMask>();
            for (int i = 0; i < totalChannelMaskValues.Length; i++)
            {
                if ((channelMask & ((CSCore.ChannelMask)totalChannelMaskValues.GetValue(i))) == (CSCore.ChannelMask)totalChannelMaskValues.GetValue(i))
                    values.Add((ChannelMask)totalChannelMaskValues.GetValue(i));
            }

            return values.ToArray();
        }
    }
}