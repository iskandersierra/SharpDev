using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using SharpDev.Serialization;

namespace SharpDev.Azure.Storage.Serialization
{
    public class BlockBlobMemoryStreamProvider :
        BlockBlobStreamProviderBase
    {
        public int InitialCapacity { get; set; } = 4096;

        public BlockBlobMemoryStreamProvider(CloudBlockBlob blockBlob) : base(blockBlob)
        {
        }

        public override async Task<AsyncDisposableValue<Stream>> OpenReadAsync(CancellationToken cancellationToken = new CancellationToken())
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

        public override async Task<AsyncDisposableValue<Stream>> OpenWriteAsync(CancellationToken cancellationToken = new CancellationToken())
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