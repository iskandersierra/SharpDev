using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public class StreamMetadataStore : 
        IMetadataStore
    {
        public IReadStreamProvider ReadStreamProvider { get; }
        public IWriteStreamProvider WriteStreamProvider { get; }
        public ISerializer Serializer { get; }

        public StreamMetadataStore(IReadStreamProvider readStreamProvider, IWriteStreamProvider writeStreamProvider, ISerializer serializer)
        {
            if (readStreamProvider == null) throw new ArgumentNullException(nameof(readStreamProvider));
            if (writeStreamProvider == null) throw new ArgumentNullException(nameof(writeStreamProvider));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            ReadStreamProvider = readStreamProvider;
            WriteStreamProvider = writeStreamProvider;
            Serializer = serializer;
        }

        public async Task<object> FetchMetadataAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var stream = await ReadStreamProvider.OpenReadAsync(cancellationToken);
            try
            {
                var parameters = await ReadStreamProvider.ReadParametersAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                var result = await Serializer.DeserializeAsync(stream.Value, parameters, cancellationToken);

                return result;
            }
            finally
            {
                await stream.DisposeAsync(CancellationToken.None);
            }
        }

        public async Task SaveMetadataAsync(object metadata, CancellationToken cancellationToken = new CancellationToken())
        {
            var stream = await WriteStreamProvider.OpenWriteAsync(cancellationToken);
            try
            {
                var parameters = await Serializer.SerializeAsync(metadata, stream.Value, cancellationToken);

                await WriteStreamProvider.WriteParametersAsync(parameters, CancellationToken.None);
            }
            finally
            {
                await stream.DisposeAsync(CancellationToken.None);
            }
        }
    }
}