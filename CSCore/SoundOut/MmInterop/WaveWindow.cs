using System;
using System.Windows.Forms;

namespace CSCore.SoundOut.MMInterop
{
    public class WaveWindow : NativeWindow, IWaveCallbackWindow
    {
        private WaveCallback _waveCallback;

        public WaveWindow(WaveCallback callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");
            _waveCallback = callback;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WaveMsg.WOM_DONE:
                case (int)WaveMsg.WIM_DATA:
                    {
                        WaveHeader header = new WaveHeader();
                        IntPtr waveOutHandle = m.WParam;
                        System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, header); //header von wparam
                        _waveCallback(waveOutHandle, (WaveMsg)m.Msg, IntPtr.Zero, header, IntPtr.Zero);
                        break;
                    }
                case (int)WaveMsg.WOM_OPEN:
                case (int)WaveMsg.WOM_CLOSE:
                case (int)WaveMsg.WIM_CLOSE:
                case (int)WaveMsg.WIM_OPEN:
                    {
                        _waveCallback(m.WParam, (WaveMsg)m.Msg, IntPtr.Zero, null, IntPtr.Zero);
                        break;
                    }
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #region ICallbackWindow Member

        public WaveCallback CallBack
        {
            get { return _waveCallback; }
        }

        public void Dispose()
        {
            ReleaseHandle();
        }

        #endregion ICallbackWindow Member
    }
}