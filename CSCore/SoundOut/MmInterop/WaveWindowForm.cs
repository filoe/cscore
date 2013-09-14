using System;
using System.Windows.Forms;

namespace CSCore.SoundOut.MMInterop
{
    public class WaveWindowForm : Form, IWaveCallbackWindow
    {
        private MMInterops.WaveCallback _waveCallback;

        public WaveWindowForm(MMInterops.WaveCallback callBack)
        {
            if (callBack == null)
                throw new ArgumentNullException("callback equals null");
            _waveCallback = callBack;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WaveMsg.WOM_DONE:
                case (int)WaveMsg.WIM_DATA:
                    WaveHeader header = new WaveHeader();
                    IntPtr hWaveOut = m.WParam;
                    System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, header); //header von wparam
                    _waveCallback(hWaveOut, (WaveMsg)m.Msg, UIntPtr.Zero, header, UIntPtr.Zero);
                    break;

                case (int)WaveMsg.WOM_OPEN:
                case (int)WaveMsg.WOM_CLOSE:
                case (int)WaveMsg.WIM_CLOSE: //WaveIn Messages für spätere WaveIn implementierung
                case (int)WaveMsg.WIM_OPEN:
                    _waveCallback(m.WParam, (WaveMsg)m.Msg, UIntPtr.Zero, null, UIntPtr.Zero);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #region ICallbackWindow Member

        public MMInterops.WaveCallback CallBack
        {
            get { return _waveCallback; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion ICallbackWindow Member
    }
}