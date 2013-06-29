using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visualization3D.Core.Graphics
{
    public interface IVisualisationItem : IRenderComponent
    {
        float Value { get; set; }
        Context Context { get; set; }
        void BeginItemRendering();
        void EndItemRendering();
    }
}
