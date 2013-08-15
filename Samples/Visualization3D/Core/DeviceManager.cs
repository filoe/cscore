using CSCore.Codecs;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D9;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Visualization3D.Core
{
    public class DeviceManager : IDisposable
    {
        private Direct3D _direct3D;
        private Device _device;

        public Direct3D D3D { get { return _direct3D; } }

        public Device Device { get { return _device; } }

        public DeviceManager(Direct3D direct3D)
        {
            _direct3D = direct3D;
        }

        public void InitializeDX(int adapter, IntPtr handle, int width, int heigth)
        {
            var adapterinfo = _direct3D.Adapters[adapter];
            Format deviceFormat;
            if (_direct3D.CheckDeviceFormat(adapter, SharpDX.Direct3D9.DeviceType.Hardware, adapterinfo.CurrentDisplayMode.Format, Usage.RenderTarget, ResourceType.Surface, Format.X8R8G8B8))
                deviceFormat = Format.X8R8G8B8;
            else if (_direct3D.CheckDeviceFormat(adapter, SharpDX.Direct3D9.DeviceType.Hardware, adapterinfo.CurrentDisplayMode.Format, Usage.RenderTarget, ResourceType.Surface, Format.A8R8G8B8))
                deviceFormat = Format.A8R8G8B8;
            else
                throw new NotSupportedException("Deviceformat not supported.");

            Format depthformat;
            if (_direct3D.CheckDepthStencilMatch(adapter, DeviceType.Hardware, adapterinfo.CurrentDisplayMode.Format, deviceFormat, Format.D24X8))
                depthformat = Format.D24X8;
            else if (_direct3D.CheckDepthStencilMatch(adapter, DeviceType.Hardware, adapterinfo.CurrentDisplayMode.Format, deviceFormat, Format.D16))
                depthformat = Format.D16;
            else
                throw new NotSupportedException("Dephformat not supported");

            int quality;
            MultisampleType multisampleType;
            /*if (CheckDeviceMultisampleType(MultisampleType.FourSamples, adapter, deviceFormat, depthformat, out quality))
                multisampleType = MultisampleType.FourSamples;
            else if (CheckDeviceMultisampleType(MultisampleType.TwoSamples, adapter, deviceFormat, depthformat, out quality))
                multisampleType = MultisampleType.TwoSamples;
            else
                multisampleType = MultisampleType.None;
            */
            if (CheckDeviceMultisampleType(MultisampleType.NonMaskable, adapter, deviceFormat, depthformat, out quality))
                multisampleType = MultisampleType.NonMaskable;
            else
                multisampleType = MultisampleType.None;

            quality = Math.Min(4, quality);

            PresentParameters pp = new PresentParameters(width, heigth)
            {
                MultiSampleType = multisampleType,
                MultiSampleQuality = quality,
                AutoDepthStencilFormat = depthformat,
                BackBufferFormat = deviceFormat,
                DeviceWindowHandle = handle,
                SwapEffect = SwapEffect.Discard,
                Windowed = true,
            };

            _device = new Device(_direct3D, adapter, DeviceType.Hardware, handle, CreateFlags.HardwareVertexProcessing, pp);

            //_device.SetRenderState(RenderState.ShadeMode, ShadeMode.Phong);
            _device.SetRenderState(RenderState.MultisampleAntialias, true);
        }

        private bool CheckDeviceMultisampleType(MultisampleType type, int adapter, Format deviceformat, Format depthformat, out int quality)
        {
            return _direct3D.CheckDeviceMultisampleType(adapter, DeviceType.Hardware, deviceformat, true, type, out quality) &&
                   _direct3D.CheckDeviceMultisampleType(adapter, DeviceType.Hardware, depthformat, true, type);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose managed
            }

            if (_direct3D != null && !_direct3D.IsDisposed)
                _direct3D.Dispose();
            if (_device != null && !_device.IsDisposed)
                _device.Dispose();
        }

        ~DeviceManager()
        {
            Dispose(false);
        }
    }
}