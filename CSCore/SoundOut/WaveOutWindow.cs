using CSCore.SoundOut.MMInterop;
using System;
using System.Windows.Forms;

namespace CSCore.SoundOut
{
#pragma warning disable 618
    public class WaveOutWindow : WaveOut
#pragma warning restore 618
    {
        private IWaveCallbackWindow _window;

        public IntPtr WindowHandle { get { return _window.Handle; } }

        public WaveOutWindow(IntPtr windowHandle)
        {
            _window = new WaveWindow(new WaveCallback((handle, msg, user, header, reserved) => Callback(handle, msg, user, header, reserved)));
            if (windowHandle == IntPtr.Zero)
                throw new ArgumentException("windowHandle is zero", "windowHandle");
            ((WaveWindow)_window).AssignHandle(windowHandle);
        }

        public WaveOutWindow()
        {
            _window = new WaveWindowForm((handle, msg, user, header, reserved) => Callback(handle, msg, user, header, reserved));
            ((Form)_window).CreateControl();
        }

        protected override IntPtr CreateWaveOut()
        {
            IntPtr ptr;
            lock (LockObj)
            {
                MmException.Try(MMInterops.waveOutOpenWithWindow(out ptr, (IntPtr)Device, WaveSource.WaveFormat, WindowHandle,
                    IntPtr.Zero, MMInterops.WaveInOutOpenFlags.CALLBACK_WINDOW),
                    "waveOutOpenWithWindow <-- waveOutOpen");
            }
            return ptr;
        }

        public override void Dispose(bool disposing)
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