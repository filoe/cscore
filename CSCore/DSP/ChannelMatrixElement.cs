using System.Diagnostics;

namespace CSCore.DSP
{
    /// <summary>
    /// Represents an element inside of a <see cref="ChannelMatrix"/>.
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public class ChannelMatrixElement
    {
        /// <summary>
        /// Gets the assigned input channel of the <see cref="ChannelMatrixElement"/>.
        /// </summary>
        public ChannelMask InputChannel { get; private set; }

        /// <summary>
        /// Gets the assigned output channel of the <see cref="ChannelMatrixElement"/>.        
        /// </summary>
        public ChannelMask OutputChannel { get; private set; }

        /// <summary>
        /// Gets or sets the coefficient in the range from 0.0f to 1.0f.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelMatrixElement"/> class.
        /// </summary>
        /// <param name="inputChannel">The input channel.</param>
        /// <param name="outputChannel">The output channel.</param>
        public ChannelMatrixElement(ChannelMask inputChannel, ChannelMask outputChannel)
        {
            InputChannel = inputChannel;
            OutputChannel = outputChannel;
        }
    }
}
