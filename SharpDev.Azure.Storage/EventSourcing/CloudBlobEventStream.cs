using System;
using System.Buffers;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using SharpDev.EventSourcing;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace SharpDev.Azure.Storage.EventSourcing
{
    /// <summary>
    /// <remarks>This class is suposed to be used single threaded, like from an actor taking care of only this event stream</remarks>
    /// </summary>
    public class CloudBlobEventStream : IEventStream
    {
        private readonly CloudAppendBlob eventStreamBlob;
        private readonly CloudBlockBlob metadataBlob;
        private readonly CloudBlockBlob snapshotBlob;
        private readonly ArrayPool<byte> arrayPool;
        private static readonly byte[] EmptyByteArray = new byte[0];
        private bool alreadyRead;
        private bool readToEnd;
        private ulong eventOffset;
        private ulong eventVersion;
        private ulong streamVersion;
        private DateTime timestamp;

        public CloudBlobEventStream(
            CloudAppendBlob eventStreamBlob,
            CloudBlockBlob metadataBlob,
            CloudBlockBlob snapshotBlob,
            ArrayPool<byte> arrayPool = null)
        {
            if (eventStreamBlob == null) throw new ArgumentNullException(nameof(eventStreamBlob));
            if (metadataBlob == null) throw new ArgumentNullException(nameof(metadataBlob));
            if (snapshotBlob == null) throw new ArgumentNullException(nameof(snapshotBlob));
            this.eventStreamBlob = eventStreamBlob;
            this.metadataBlob = metadataBlob;
            this.snapshotBlob = snapshotBlob;
            this.arrayPool = arrayPool ?? ArrayPool<byte>.Shared;
            ResetState();
        }

        public async Task<Stream> ReadMetadataAsync(CancellationToken cancellationToken)
        {
            var exists = await metadataBlob.ExistsAsync(cancellationToken);
            if (!exists) return Stream.Null;

            int length = (int)metadataBlob.Properties.Length;

            var stream = new ArrayPoolMemoryStream(length, arrayPool);
            //var stream = new MemoryStream(length);

            await metadataBlob.DownloadToStreamAsync(stream, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return stream;
        }

        public async Task<bool> WriteMetadataAsync(Stream metadata, CancellationToken cancellationToken)
        {
            if (metadata == null)
                return await metadataBlob.DeleteIfExistsAsync(cancellationToken);

            await metadataBlob.UploadFromStreamAsync(metadata, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return true;
        }

        public IObservable<StreamEvent> ReadAsync(bool readSnapshot, CancellationToken cancellationToken)
        {
            if (alreadyRead)
                throw new InvalidOperationException("This stream is already read");

            alreadyRead = true;
            bool alreadyUsedObservable = false;
            return Observable.Create<StreamEvent>(async (observer, cancellationToken2) =>
            {
                var disposable = new CompositeDisposable();

                if (alreadyUsedObservable)
                {
                    observer.OnError(new InvalidOperationException("This observable is already observed"));
                    return Task.FromResult(disposable);
                }

                alreadyUsedObservable = true;
                var compositeTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cancellationToken2);
                var compositeToken = compositeTokenSource.Token;

                if (readSnapshot && await snapshotBlob.ExistsAsync(cancellationToken))
                {
                    using (var stream = await snapshotBlob.OpenReadAsync(cancellationToken))
                    {
                        var snapshotEvent = await ReadSnapshotAsync(stream, cancellationToken);

                        compositeToken.ThrowIfCancellationRequested();

                        observer.OnNext(snapshotEvent);
                    }
                }

                if (await eventStreamBlob.ExistsAsync(compositeToken))
                {
                    using (var stream = await eventStreamBlob.OpenReadAsync(compositeToken))
                    {
                        if (eventOffset != 0L)
                            stream.Seek((long) eventOffset, SeekOrigin.Begin);

                        while (stream.Position <= stream.Length)
                        {
                            var commitEvent = await ReadCommitAsync(stream, cancellationToken);

                            compositeToken.ThrowIfCancellationRequested();

                            observer.OnNext(commitEvent);
                        }
                    }
                }

                observer.OnCompleted();
                readToEnd = true;

                return Task.FromResult(disposable);
            });
        }

        public async Task WriteAsync(StreamEvent @event, StreamEvent snapshot, CancellationToken cancellationToken)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            if (!readToEnd)
                throw new InvalidOperationException("Cannot write events without first reading the existing ones");

            if (@event.StreamVersion - 1 != streamVersion)
                throw new OptimisticConcurrencyException($"Expected stream version was {@event.StreamVersion - 1} but found {streamVersion}");
            if (@event.EventVersion <= eventVersion)
                throw new OptimisticConcurrencyException($"Expected event version was at least {@event.EventVersion - 1} but found {eventVersion}");
            if (@event.Timestamp <= timestamp)
                throw new OptimisticConcurrencyException($"Expected timestamp was at least {@event.Timestamp:O} but found {timestamp:O}");

            using (var stream = eventStreamBlob.OpenWriteAsync(false, cancellationToken))
            {
                //await WriteCommitAsync();
            }

            //eventOffset = stream.Length;

        }

        private void ResetState()
        {
            alreadyRead = false;
            readToEnd = false;
            eventOffset = 0;
            eventVersion = 0;
            streamVersion = 0;
            timestamp = new DateTime(0L, DateTimeKind.Utc);
        }

        private async Task<StreamEvent> ReadSnapshotAsync(Stream stream, CancellationToken cancellationToken)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                // Read version
                var version = reader.ReadInt16();

                switch (version)
                {
                    case 1:
                        {
                            var data = await ReadSnapshotV1Async(reader, cancellationToken);
                            eventOffset = data.EventOffset;
                            eventVersion = data.EventVersion;
                            streamVersion = data.StreamVersion;
                            timestamp = data.Timestamp;
                            return new StreamEvent(StreamEventType.Snapshot, data.EventVersion, data.StreamVersion,
                                data.Timestamp, data.Payload);
                        }
                    default: throw new NotSupportedException($"Cannot read snapshot version 0x{version:X4}");
                }
            }
        }
        private async Task<StreamEvent> ReadCommitAsync(Stream stream, CancellationToken cancellationToken)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                // Read version
                var version = reader.ReadInt16();

                switch (version)
                {
                    case 1:
                        {
                            var data = await ReadCommitV1Async(reader, cancellationToken);
                            eventVersion = data.EventVersion;
                            streamVersion = data.StreamVersion;
                            timestamp = data.Timestamp;
                            return new StreamEvent(StreamEventType.Commit, data.EventVersion, data.StreamVersion, data.Timestamp, data.Payload);
                        }
                    default: throw new NotSupportedException($"Cannot read commit version 0x{version:X4}");
                }
            }
        }

        private async Task<SnapshotDataV1> ReadSnapshotV1Async(BinaryReader reader, CancellationToken cancellationToken)
        {
            var data = new SnapshotDataV1();
            data.EventOffset = reader.ReadUInt64();
            data.EventVersion = reader.ReadUInt64();
            data.StreamVersion = reader.ReadUInt64();
            data.Timestamp = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
            var length = reader.ReadInt32();
            var stream = new ArrayPoolMemoryStream(length, arrayPool);
            data.Payload = stream;
            await reader.BaseStream.CopyToAsync(stream, 1024, cancellationToken);
            stream.Seek(0L, SeekOrigin.Begin);
            return data;
        }
        private async Task<CommitDataV1> ReadCommitV1Async(BinaryReader reader, CancellationToken cancellationToken)
        {
            var data = new CommitDataV1();
            data.EventVersion = reader.ReadUInt64();
            data.StreamVersion = reader.ReadUInt64();
            data.Timestamp = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
            var length = reader.ReadInt32();
            var stream = new ArrayPoolMemoryStream(length, arrayPool);
            data.Payload = stream;
            await reader.BaseStream.CopyToAsync(stream, 1024, cancellationToken);
            stream.Seek(0L, SeekOrigin.Begin);
            return data;
        }

        private class SnapshotDataV1
        {
            public ulong EventOffset;
            public ulong EventVersion;
            public ulong StreamVersion;
            public DateTime Timestamp;
            public Stream Payload;
        }
        private class CommitDataV1
        {
            public ulong EventVersion;
            public ulong StreamVersion;
            public DateTime Timestamp;
            public Stream Payload;
        }
    }
}
