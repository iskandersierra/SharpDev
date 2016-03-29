using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.EventSourcing
{
    /// <summary>
    /// This interface represents a event stream, probably persistent in a durable media
    /// </summary>
    public interface IEventStream
    {
        /// <summary>
        /// Reads events from the stream, from a given timestamp on. By default all events are read
        /// </summary>
        /// <param name="fromTimestamp">Is the least timestamp to be read on this operation</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns></returns>
        IObservable<EventCommitEnvelope> ReadAsync(long fromTimestamp = 0, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Writes a commit to the event stream store
        /// </summary>
        /// <param name="commit">Is the commit to be written to the store</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns></returns>
        Task<EventCommitEnvelope> WriteAsync(EventCommitEnvelope commit, CancellationToken cancellationToken = default(CancellationToken));
    }
}
