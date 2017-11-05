using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.DSP
{
    /// <summary>
    /// Encapsulates a window function.
    /// </summary>
    /// <param name="index">The current index of the input signal.</param>
    /// <param name="width">The width of window.</param>
    /// <returns>The result of the window function.</returns>
    public delegate float WindowFunction(int index, int width);
}
