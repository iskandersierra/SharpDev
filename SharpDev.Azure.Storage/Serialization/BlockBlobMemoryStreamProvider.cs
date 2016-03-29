using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using SharpDev.Serialization;

namespace SharpDev.Azure.Storage.Serialization
{
    public class BlockBlobMemoryStreamProvider : 
        IReadStreamProvider,
        IWriteStreamProvider
    {
        public CloudBlockBlob BlockBlob { get; }

        public int InitialCapacity { get; set; } = 4096;

        public BlockBlobMemoryStreamProvider(CloudBlockBlob blockBlob)
        {
            if (blockBlob == null) throw new ArgumentNullException(nameof(blockBlob));

            BlockBlob = blockBlob;
        }

        public async Task<AsyncDisposableValue<Stream>> OpenReadAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await BlockBlob.ExistsAsync(cancellationToken))
                throw new ResourceNotFoundException("Metadata not found for this store");

            cancellationToken.ThrowIfCancellationRequested();

            var stream = new MemoryStream(InitialCapacity);

            await BlockBlob.DownloadToStreamAsync(stream, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            stream.Seek(0, SeekOrigin.Begin);

            return new AsyncDisposableValue<Stream>(stream, async () => stream.Dispose());
        }

        public async Task<AsyncDisposableValue<Stream>> OpenWriteAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var stream = new MemoryStream(InitialCapacity);

            return new AsyncDisposableValue<Stream>(stream, async () =>
            {
                stream.Seek(0, SeekOrigin.Begin);
                await BlockBlob.UploadFromStreamAsync(stream, cancellationToken);
            });
        }
    }
}