using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Visualization
{
    public interface IFFTVisualization
    {
        FFTDataProvider DataProvider { get; set; }

        void Update(object sender, CSCore.DSP.FFTCalculatedEventArgs e);
    }
}