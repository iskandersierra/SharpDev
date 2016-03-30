using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using SharpDev.Serialization;

namespace SharpDev.Azure.Storage.Serialization
{
    public abstract class BlockBlobStreamProviderBase :
        IReadStreamProvider,
        IWriteStreamProvider
    {
        public CloudBlockBlob BlockBlob { get; }

        protected BlockBlobStreamProviderBase(CloudBlockBlob blockBlob)
        {
            if (blockBlob == null) throw new ArgumentNullException(nameof(blockBlob));
            BlockBlob = blockBlob;
        }

        public abstract Task<AsyncDisposableValue<Stream>> OpenReadAsync(CancellationToken cancellationToken = new CancellationToken());

        public abstract Task<AsyncDisposableValue<Stream>> OpenWriteAsync(CancellationToken cancellationToken = new CancellationToken());

        public async Task<SerializerParameters> ReadParametersAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await BlockBlob.ExistsAsync(cancellationToken))
                return null;

            return new SerializerParameters(BlockBlob.Properties.ContentType, BlockBlob.Properties.ContentEncoding);
        }

        public async Task WriteParametersAsync(SerializerParameters parameters, CancellationToken cancellationToken = new CancellationToken())
        {
            BlockBlob.Properties.ContentType = parameters?.ContentType;
            BlockBlob.Properties.ContentEncoding = parameters?.ContentEncoding;

            await BlockBlob.SetPropertiesAsync();
        }
    }
}