using CSCore.SoundOut.MMInterop;
using System;

namespace CSCore.SoundIn
{
#pragma warning disable 612
    public class WaveInWindow : WaveIn
#pragma warning restore 612
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
            _window = new WaveWindowForm(new WaveCallback(Callback));
            ((System.Windows.Forms.Form)_window).CreateControl();
        }

        public WaveInWindow(IntPtr windowHandle, WaveFormat waveFormat)
            : this(waveFormat)
        {
            if (windowHandle == IntPtr.Zero)
                throw new ArgumentException("windowHandle is zero", "windowHandle");
            _window = new WaveWindow(new WaveCallback(Callback));
            ((WaveWindow)_window).AssignHandle(windowHandle);
        }

        protected override void OpenWaveDevice(int device)
        {
            var result = MMInterops.waveInOpenWithWindow(out InternalHandle, (IntPtr)Device, WaveFormat, WindowHandle, IntPtr.Zero, MMInterops.WaveInOutOpenFlags.CALLBACK_WINDOW);
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