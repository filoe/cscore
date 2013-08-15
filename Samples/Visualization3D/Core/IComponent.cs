using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visualization3D.Core
{
    public interface IComponent : IDisposable
    {
        void Load();

        void Update(float time);
    }

    public interface IRenderComponent : IComponent
    {
        void Render(SharpDX.Matrix worldViewProj, float time);
    }
}