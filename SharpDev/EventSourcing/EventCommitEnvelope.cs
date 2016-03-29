using System;

namespace SharpDev.EventSourcing
{
    /// <summary>
    /// This class represents a serializable event packet
    /// </summary>
    public class EventCommitEnvelope
    {
        /// <summary>
        /// Creates a new instance of <see cref="EventCommitEnvelope"/>
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="commitId"></param>
        /// <param name="commitVersion"></param>
        /// <param name="eventVersion"></param>
        /// <param name="events"></param>
        /// <param name="metadata"></param>
        public EventCommitEnvelope(string timestamp, string commitId, int commitVersion, int eventVersion, TypedObject[] events, TypedObject metadata)
        {
            if (timestamp == null) throw new ArgumentNullException(nameof(timestamp));
            if (commitId == null) throw new ArgumentNullException(nameof(commitId));
            if (events == null) throw new ArgumentNullException(nameof(events));
            if (commitVersion <= 0) throw new ArgumentOutOfRangeException(nameof(commitVersion));
            if (eventVersion <= 0) throw new ArgumentOutOfRangeException(nameof(eventVersion));
            if (events.Length == 0) throw new ArgumentException("Argument is empty collection", nameof(events));

            Timestamp = timestamp;
            CommitId = commitId;
            CommitVersion = commitVersion;
            EventVersion = eventVersion;
            Events = events;
            Metadata = metadata;
        }

        /// <summary>
        /// Creates a new instance of <see cref="EventCommitEnvelope"/>
        /// </summary>
        /// <param name="commitId"></param>
        /// <param name="events"></param>
        /// <param name="metadata"></param>
        public EventCommitEnvelope(string commitId, int commitVersion, TypedObject[] events, TypedObject metadata)
        {
            if (commitId == null) throw new ArgumentNullException(nameof(commitId));
            if (commitVersion <= 0) throw new ArgumentOutOfRangeException(nameof(commitVersion));
            if (events == null) throw new ArgumentNullException(nameof(events));
            if (events.Length == 0) throw new ArgumentException("Argument is empty collection", nameof(events));

            CommitId = commitId;
            CommitVersion = commitVersion;
            Events = events;
            Metadata = metadata;
        }

        /// <summary>
        /// Gets the commit timestamp. A monotonically incremental long integer
        /// </summary>
        public string Timestamp { get; }
        /// <summary>
        /// Gets the commit identifier
        /// </summary>
        public string CommitId { get; }
        /// <summary>
        /// Gets the Commit version
        /// </summary>
        public int CommitVersion { get; }
        /// <summary>
        /// Gets the event version
        /// </summary>
        public int EventVersion { get; }
        /// <summary>
        /// Gets the list of committed events
        /// </summary>
        public TypedObject[] Events { get; }
        /// <summary>
        /// Gets the commit metadata
        /// </summary>
        public TypedObject Metadata { get; }
    }
}