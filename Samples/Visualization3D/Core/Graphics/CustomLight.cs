using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visualization3D.Core.Graphics
{
    public class CustomLight : IComponent
    {
        private DeviceManager _deviceMgr;

        public Light Light;

        public CustomLight(DeviceManager devicemgr)
        {
            _deviceMgr = devicemgr;
        }

        public void Load()
        {
            var device = _deviceMgr.Device;
            device.SetRenderState(RenderState.ZEnable, true);
            device.SetRenderState(RenderState.CullMode, Cull.None);
            device.SetRenderState(RenderState.Lighting, true);

            device.SetRenderState(RenderState.SpecularEnable, true);

            Light light = new Light();
            light.Type = LightType.Spot;
            light.Specular = new Color4(1f, 0f, 0f, 1f);
            light.Diffuse = new Color4(1f, 0f, 0f, 1f);
            light.Range = 2000;
            light.Position = new Vector3(0, 10, 15);

            light.Attenuation0 = 0.0f;
            light.Attenuation1 = 0.0f;
            light.Attenuation2 = 0.0f;

            device.SetLight(0, ref light);
            device.EnableLight(0, true);

            this.Light = light;
        }

        public void Update(float time)
        {
        }

        public void Dispose()
        {
        }
    }
}