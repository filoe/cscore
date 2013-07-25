using CSCore.SoundOut.MmInterop;
using System;
using System.Windows.Forms;

namespace CSCore.SoundOut
{
    public class WaveOutWindow : WaveOut
    {
        IWaveCallbackWindow _window;

        public IntPtr WindowHandle { get { return _window.Handle; } }

        public WaveOutWindow(IntPtr windowHandle)
        {
            _window = new WaveWindow(new MMInterops.WaveCallback(Callback));
            if (windowHandle == IntPtr.Zero)
                throw new ArgumentException("windowHandle is zero", "windowHandle");
            ((WaveWindow)_window).AssignHandle(windowHandle);
        }

        public WaveOutWindow()
        {
            _window = new WaveWindowForm(new MMInterops.WaveCallback(Callback));
            ((Form)_window).CreateControl();
        }

        protected override IntPtr CreateWaveOut()
        {
            IntPtr ptr;
            lock (_lockObj)
            {
                Context.Current.Logger.MMResult(MMInterops.waveOutOpenWithWindow(out ptr, (IntPtr)Device, WaveSource.WaveFormat, WindowHandle, 
                    IntPtr.Zero, MMInterops.WaveInOutOpenFlags.CALLBACK_WINDOW),
                    "waveOutOpenWithWindow <-- waveOutOpen", "WaveOutWindow.OpenWaveOut");
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
