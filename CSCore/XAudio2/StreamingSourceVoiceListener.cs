using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Provides a mechanism for playing <see cref="StreamingSourceVoice" /> instances.
    /// </summary>
    public class StreamingSourceVoiceListener : IDisposable
    {
        /// <summary>
        ///     Maximum amount of <see cref="StreamingSourceVoice" /> instances a <see cref="StreamingSourceVoiceListener" /> can
        ///     contain.
        /// </summary>
        /// <remarks>Limited by the <see cref="WaitHandle.WaitAny(System.Threading.WaitHandle[],int,bool)" /> method.</remarks>
        public const int MaxItems = 64;

        private static volatile StreamingSourceVoiceListener _default;
// ReSharper disable once InconsistentNaming
        private static readonly object _singleTonSync = new object();

        private readonly List<StreamingSourceVoice> _items = new List<StreamingSourceVoice>();
        private readonly object _lockObject = new object();
        private bool _disposed;
        private bool _isRunning;
        private volatile bool _itemsChanged;
        private volatile bool _shutDown;

        /// <summary>
        ///     Gets the default <see cref="StreamingSourceVoiceListener" /> singleton instance.
        /// </summary>
        public static StreamingSourceVoiceListener Default
        {
            get
            {
                if (_default == null)
                {
                    lock (_singleTonSync)
                    {
                        return _default ?? (_default = new StreamingSourceVoiceListener());
                    }
                }
                return _default;
            }
        }

        /// <summary>
        ///     Gets the number of items which got added to the <see cref="StreamingSourceVoiceListener" />.
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        ///     Disposes the <see cref="StreamingSourceVoiceListener" />.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Adds a <see cref="StreamingSourceVoice" /> to the <see cref="StreamingSourceVoiceListener" />.
        /// </summary>
        /// <param name="streamingSourceVoice">
        ///     The <see cref="StreamingSourceVoice" /> instance to add to the
        ///     <see cref="StreamingSourceVoiceListener" />.
        /// </param>
        public void Add(StreamingSourceVoice streamingSourceVoice)
        {
            if (!_items.Contains(streamingSourceVoice))
            {
                if (_items.Count > MaxItems) //64 = Max waithandles
                    throw new NotSupportedException("The maximum number of items is limited to 64.");

                lock (_lockObject)
                {
                    _items.Add(streamingSourceVoice);
                    _itemsChanged = true;

                    TryStart();
                }
            }
        }

        /// <summary>
        ///     Removes a <see cref="StreamingSourceVoice" /> from the <see cref="StreamingSourceVoiceListener" />.
        /// </summary>
        /// <param name="streamingSourceVoice">
        ///     The <see cref="StreamingSourceVoice" /> instance to remove from the
        ///     <see cref="StreamingSourceVoiceListener" />.
        /// </param>
        public void Remove(StreamingSourceVoice streamingSourceVoice)
        {
            if (_items.Contains(streamingSourceVoice))
            {
                lock (_lockObject)
                {
                    _items.Remove(streamingSourceVoice);
                    _itemsChanged = true;
                }
            }
        }

        private void TryStart()
        {
            if (!_isRunning)
                ThreadPool.QueueUserWorkItem(WorkerProc);
            _isRunning = true;
        }

        private void WorkerProc(object o)
        {
            const int waitTimeout = 10;

            WaitHandle[] waitHandles = {};
            StreamingSourceVoice[] itemsCopy = {};

            while (!_shutDown)
            {
                if (_itemsChanged)
                {
                    itemsCopy = _items.ToArray();
                    waitHandles = itemsCopy.Select(x => x.BufferEndWaitHandle).Cast<WaitHandle>().ToArray();
                    _itemsChanged = false;
                }

                if (waitHandles.Length == 0)
                {
                    Thread.Sleep(waitTimeout);
                    continue;
                }

                int index = WaitHandle.WaitAny(waitHandles, waitTimeout);
                if (index == WaitHandle.WaitTimeout)
                    continue;

                StreamingSourceVoice item = itemsCopy[index]; //todo: make sure that we've got the right item
                item.Refill();
            }
        }

        /// <summary>
        ///     Disposes the <see cref="StreamingSourceVoiceListener" /> and stops the internal playback thread.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                _shutDown = true;
            _disposed = true;
        }

        /// <summary>
        /// Destructor which calls the <see cref="Dispose(bool)"/> method.
        /// </summary>
        ~StreamingSourceVoiceListener()
        {
            Dispose(false);
        }
    }
}