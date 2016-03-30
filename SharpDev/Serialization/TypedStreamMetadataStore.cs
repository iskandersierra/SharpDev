using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public class TypedStreamMetadataStore : 
        ITypedMetadataStore
    {
        public IReadStreamProvider ReadStreamProvider { get; }
        public IWriteStreamProvider WriteStreamProvider { get; }
        public ITypedSerializer Serializer { get; }

        public TypedStreamMetadataStore(IReadStreamProvider readStreamProvider, IWriteStreamProvider writeStreamProvider, ITypedSerializer serializer)
        {
            if (readStreamProvider == null) throw new ArgumentNullException(nameof(readStreamProvider));
            if (writeStreamProvider == null) throw new ArgumentNullException(nameof(writeStreamProvider));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            ReadStreamProvider = readStreamProvider;
            WriteStreamProvider = writeStreamProvider;
            Serializer = serializer;
        }

        public async Task<T> FetchMetadataAsync<T>(CancellationToken cancellationToken = new CancellationToken())
        {
            var stream = await ReadStreamProvider.OpenReadAsync(cancellationToken);
            try
            {
                var parameters = await ReadStreamProvider.ReadParametersAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                var result = await Serializer.DeserializeAsync<T>(stream.Value, parameters, cancellationToken);

                return result;
            }
            finally
            {
                await stream.DisposeAsync(CancellationToken.None);
            }
        }

        public async Task SaveMetadataAsync<T>(T metadata, CancellationToken cancellationToken = new CancellationToken())
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

    public class TypedStreamMetadataStore<T> : 
        ITypedMetadataStore<T>
    {
        public IReadStreamProvider ReadStreamProvider { get; }
        public IWriteStreamProvider WriteStreamProvider { get; }
        public ITypedSerializer Serializer { get; }

        public TypedStreamMetadataStore(IReadStreamProvider readStreamProvider, IWriteStreamProvider writeStreamProvider, ITypedSerializer serializer)
        {
            if (readStreamProvider == null) throw new ArgumentNullException(nameof(readStreamProvider));
            if (writeStreamProvider == null) throw new ArgumentNullException(nameof(writeStreamProvider));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            ReadStreamProvider = readStreamProvider;
            WriteStreamProvider = writeStreamProvider;
            Serializer = serializer;
        }

        public async Task<T> FetchMetadataAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var stream = await ReadStreamProvider.OpenReadAsync(cancellationToken);
            try
            {
                var parameters = await ReadStreamProvider.ReadParametersAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                var result = await Serializer.DeserializeAsync<T>(stream.Value, parameters, cancellationToken);

                return result;
            }
            finally
            {
                await stream.DisposeAsync(CancellationToken.None);
            }
        }

        public async Task SaveMetadataAsync(T metadata, CancellationToken cancellationToken = new CancellationToken())
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