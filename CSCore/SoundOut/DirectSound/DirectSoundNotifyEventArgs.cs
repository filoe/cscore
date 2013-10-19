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
        public bool RequestStopPlayback { get; set; }

        public DirectSoundNotifyEventArgs(int handleIndex, int sampleOffset, int bufferSize, bool isTimeOut, bool bufferStopped)
        {
            HandleIndex = handleIndex;
            SampleOffset = sampleOffset;
            BufferSize = bufferSize;
            IsTimeOut = isTimeOut;
            DSoundBufferStopped = bufferStopped;
        }
    }
}