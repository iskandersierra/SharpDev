using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using SharpDev.Serialization;

namespace SharpDev.Azure.Storage.Serialization
{
    public class BlockBlobOpenStreamProvider :
        BlockBlobStreamProviderBase
    {
        public BlockBlobOpenStreamProvider(CloudBlockBlob blockBlob) : base(blockBlob)
        {
        }

        public override async Task<AsyncDisposableValue<Stream>> OpenReadAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await BlockBlob.ExistsAsync(cancellationToken))
                throw new ResourceNotFoundException("Metadata not found for this store");

            cancellationToken.ThrowIfCancellationRequested();

            var stream = await BlockBlob.OpenReadAsync(cancellationToken);

            return new AsyncDisposableValue<Stream>(stream, async () => stream.Dispose());
        }

        public override async Task<AsyncDisposableValue<Stream>> OpenWriteAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var stream = await BlockBlob.OpenWriteAsync(cancellationToken);

            return new AsyncDisposableValue<Stream>(stream, async () =>
            {
                await stream.FlushAsync(cancellationToken);

                stream.Dispose();
            });
        }
    }
}
