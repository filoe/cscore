using System;
using System.Threading;

namespace CSCore.SoundOut.DirectSound
{
    public class DirectSoundNotifyEventArgs : EventArgs
    {
        public int HandleIndex { get; private set; }
        public int SampleOffset { get; private set; }
        public int BufferSize { get; private set; }
        public bool IsTimeOut { get; private set; }
        public bool DSoundBufferStopped { get; private set; }

        /// <summary>
        /// Set this to stop the notification thread
        /// </summary>
        public bool StopPlayback { get; set; }

        public DirectSoundNotifyEventArgs(int handleIndex, int bufferSize)
        {
            HandleIndex = handleIndex;
            IsTimeOut = handleIndex == WaitHandle.WaitTimeout;
            DSoundBufferStopped = handleIndex == 2;

            BufferSize = bufferSize;
            handleIndex = (handleIndex == 0) ? 1 : 0;
            SampleOffset = handleIndex * bufferSize;
            StopPlayback = false;
        }
    }
}
