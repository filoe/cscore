using CSCore.SoundOut.MMInterop;
using System;

namespace CSCore.SoundIn
{
    public class WaveInWindow : WaveIn
    {
        private IWaveCallbackWindow _window;

        public IntPtr WindowHandle { get { return _window.Handle; } }

        public WaveInWindow()
            : this(DefaultFormat)
        {
        }

        public WaveInWindow(WaveFormat waveFormat)
            : base(waveFormat)
        {
            _window = new WaveWindowForm(new MMInterops.WaveCallback(Callback));
            ((System.Windows.Forms.Form)_window).CreateControl();
        }

        public WaveInWindow(IntPtr windowHandle, WaveFormat waveFormat)
            : this(waveFormat)
        {
            if (windowHandle == IntPtr.Zero)
                throw new ArgumentException("windowHandle is zero", "windowHandle");
            _window = new WaveWindow(new MMInterops.WaveCallback(Callback));
            ((WaveWindow)_window).AssignHandle(windowHandle);
        }

        protected override void OpenWaveDevice(int device)
        {
            var result = MMInterops.waveInOpenWithWindow(out handle, (IntPtr)Device, WaveFormat, WindowHandle, IntPtr.Zero, MMInterops.WaveInOutOpenFlags.CALLBACK_WINDOW);
            MmException.Try(result, "waveInOpen");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_window != null)
            {
                _window.Dispose();
                _window = null;
            }
        }
    }
}