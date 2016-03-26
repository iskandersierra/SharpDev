using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.EventSourcing
{
    /// <summary>
    /// Represents a stream of events, possibly with a snapshot as the first argument.
    /// It allows to append new events and snapshots
    /// This interface is low-level and deals with byte[] messages
    /// Possible implementations of this interface include:
    ///     CloudBlobEventStream : A AppendBlob keeps the stream of events and another BlockBlob keeps the 
    ///         Snapshot and the position in the append blob from where to start reading remaining events
    ///     CloudTableEventStream : A partition represents a stream and version number 
    /// </summary>
    public interface IEventStream
    {
        /// <summary>
        /// Reads event stream's metadata
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A stream after its content has been written</returns>
        Task<Stream> ReadMetadataAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Writes event stream's metadata
        /// </summary>
        /// <param name="metadata">Metadata serialized as stream</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A task indicating the async operation</returns>
        Task<bool> WriteMetadataAsync(Stream metadata, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Reads all events from the stream. If a snapshot is requested and found then it is returned as the first event, and then all the events after it are returned in sequence.
        /// </summary>
        /// <param name="readSnapshot"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IObservable<StreamEvent> ReadAsync(bool readSnapshot = true, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Writes a new commit event
        /// </summary>
        /// <param name="event"></param>
        /// <param name="snapshot"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteAsync(StreamEvent @event, StreamEvent snapshot = null, CancellationToken cancellationToken = default(CancellationToken));
    }

    public enum StreamEventType
    {
        Commit = 1, 
        Snapshot = 2,
    }

    public class StreamEvent : IDisposable
    {
        /// <summary>
        /// Creates a new instance of StreamEvent
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventVersion"></param>
        /// <param name="streamVersion"></param>
        /// <param name="timestamp"></param>
        /// <param name="payload"></param>
        public StreamEvent(StreamEventType type, 
            ulong eventVersion, ulong streamVersion, DateTime timestamp, 
            Stream payload)
        {
            Type = type;
            EventVersion = eventVersion;
            StreamVersion = streamVersion;
            Timestamp = timestamp;
            Payload = payload;
        }

        public StreamEventType Type { get; }
        public ulong EventVersion { get; }
        public ulong StreamVersion { get; }
        public DateTime Timestamp { get; }
        public Stream Payload { get; }


        public void Dispose()
        {
            ((IDisposable) Payload).Dispose();
        }
    }
}
