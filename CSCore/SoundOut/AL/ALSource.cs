using System;
// ReSharper disable InconsistentNaming

namespace CSCore.SoundOut.AL
{
    /// <summary>
    /// Represents an OpenAL Source.
    /// </summary>
    public class ALSource : IDisposable
    {
        /// <summary>
        /// Gets the openal source id
        /// </summary>
        public uint Id { private set; get; }

        private readonly ALContext _context;

        /// <summary>
        /// Initializes a new ALSource class
        /// </summary>
        /// <param name="context">The context used to create the source.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public ALSource(ALContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;

            var sources = new uint[1];
            using (context.LockContext())
            {
                ALInterops.alGenSources(1, sources);
            }

            Id = sources[0];
        }

        /// <summary>
        /// Plays the <see cref="ALSource"/>.
        /// </summary>
        public void Play()
        {
            using (_context.LockContext())
            {
                ALInterops.alSourcePlay(Id);
            }
        }

        /// <summary>
        /// Pauses the <see cref="ALSource"/>.
        /// </summary>
        public void Pause()
        {
            using (_context.LockContext())
            {
                ALInterops.alSourcePause(Id);
            }
        }


        /// <summary>
        /// Stops the <see cref="ALSource"/>.
        /// </summary>
        public void Stop()
        {
            using (_context.LockContext())
            {
                ALInterops.alSourceStop(Id);
            }
        }

        /// <summary>
        /// Queues a buffer specified by the <paramref name="bufferHandle"/>.
        /// </summary>
        /// <param name="bufferHandle">The buffer handle.</param>
        public void QueueBuffer(uint bufferHandle)
        {
            using (_context.LockContext())
            {
                ALInterops.alSourceQueueBuffers(Id, 1, new[] {bufferHandle});
            }
        }

        /// <summary>
        /// Unqueues a specified number of buffers and returns their handles.
        /// </summary>
        /// <param name="count">The number of buffers to unqueue.</param>
        /// <returns>The handles of the unqueued buffers.</returns>
        public uint[] UnqueueBuffers(int count)
        {
            uint[] result = new uint[count];
            using (_context.LockContext())
            {
                ALInterops.alSourceUnqueueBuffers(Id, count, result);
            }
            return result;
        }

        /// <summary>
        /// Gets the number of processed buffers.
        /// </summary>
        public int BuffersProcessed
        {
            get
            {
                using (_context.LockContext())
                {
                    int numberOfProcessedBuffers;
                    ALInterops.alGetSourcei(Id, ALSourceParameters.BuffersProcessed, out numberOfProcessedBuffers);
                    return numberOfProcessedBuffers;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="ALSourceState"/> of the <see cref="ALSource"/>.
        /// </summary>
        public ALSourceState SourceState
        {
            get
            {
                using (_context.LockContext())
                {
                    int alSourceState;
                    ALInterops.alGetSourcei(Id, ALSourceParameters.SourceState, out alSourceState);
                    return (ALSourceState) alSourceState;
                }
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ALSource"/> class.
        /// </summary>
        ~ALSource()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the <see cref="ALSource"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the OpenAL source.
        /// </summary>
        /// <param name="disposing">The disposing state</param>
        protected void Dispose(bool disposing)
        {
            using (_context.LockContext())
            {
                var sources = new uint[1];
                sources[0] = Id;
                ALInterops.alDeleteSources(1, sources);
            }
        }
    }
}
