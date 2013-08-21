using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.DSP
{
    public class ChannelMatrixElement
    {
        public ChannelMask InputChannel { get; private set; }

        public ChannelMask OutputChannel { get; private set; }

        /// <summary>
        /// Value from 0 to 1
        /// </summary>
        public float Value { get; set; }

        public ChannelMatrixElement(ChannelMask inputChannel, ChannelMask outputChannel)
        {
            InputChannel = inputChannel;
            OutputChannel = outputChannel;
        }
    }
}
