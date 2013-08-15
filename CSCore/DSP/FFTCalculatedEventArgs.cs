using CSCore.Utils;
using System;

namespace CSCore.DSP
{
    public class FFTCalculatedEventArgs : EventArgs
    {
        public FFTCalculatedEventArgs(Complex[] data)
        {
            Data = data;
        }

        public Utils.Complex[] Data { get; private set; }
    }
}