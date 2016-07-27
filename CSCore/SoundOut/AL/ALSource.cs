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
        /// Gets the openal source id.
        /// </summary>
        public uint Id { private set; get; }

        private readonly ALContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ALSource"/> class.
        /// </summary>
        /// <param name="context">The context used to create the <see cref="ALSource"/>.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public ALSource(ALContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;

            var sources = new uint[1];
            using (context.LockContext())
            {
                ALException.Try(
                    () =>
                        ALInterops.alGenSources(1, sources),
                    "alGenSources");
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
                ALException.Try(
                    () =>
                        ALInterops.alSourcePlay(Id),
                    "alSourcePlay");
            }
        }

        /// <summary>
        /// Pauses the <see cref="ALSource"/>.
        /// </summary>
        public void Pause()
        {
            using (_context.LockContext())
            {
                ALException.Try(
                    () =>
                        ALInterops.alSourcePause(Id),
                    "alSourcePause");
            }
        }


        /// <summary>
        /// Stops the <see cref="ALSource"/>.
        /// </summary>
        public void Stop()
        {
            using (_context.LockContext())
            {
                ALException.Try(
                    () =>
                        ALInterops.alSourceStop(Id),
                    "alSourceStop");
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
                ALException.Try(
                    () =>
                        ALInterops.alSourceQueueBuffers(Id, 1, new[] {bufferHandle}),
                    "alSourceQueueBuffers");
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
                ALException.Try(
                    () => 
                        ALInterops.alSourceUnqueueBuffers(Id, count, result),
                    "alSourceUnqueueBuffers");
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
                    int numberOfProcessedBuffers = 0;
                    ALException.Try(
                        () =>
                            ALInterops.alGetSourcei(Id, ALSourceParameters.BuffersProcessed,
                                out numberOfProcessedBuffers),
                        "alGetSourcei");
                    return numberOfProcessedBuffers;
                }
            }
        }

        /// <summary>
        /// Gets the number of queued buffers.
        /// </summary>
        public int BuffersQueued
        {
            get
            {
                using (_context.LockContext())
                {
                    int numberOfQueuedBuffers = 0;
                    ALException.Try(
                        () =>
                            ALInterops.alGetSourcei(Id, ALSourceParameters.BuffersQueued,
                                out numberOfQueuedBuffers),
                        "alGetSourcei");
                    return numberOfQueuedBuffers;
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
                    int alSourceState = 0;
                    ALException.Try(
                        () =>
                            ALInterops.alGetSourcei(Id, ALSourceParameters.SourceState,
                                out alSourceState),
                        "alGetSourcei");
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
                ALException.Try(
                    () => 
                        ALInterops.alDeleteSources(1, sources),
                    "alDeleteSources");
            }
        }
    }
}
