using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Visualization
{
    public interface ISampleVisualization
    {
        SampleDataProvider DataProvider { get; set; }

        void Update(object sender, BlockReadEventArgs e);
    }
}