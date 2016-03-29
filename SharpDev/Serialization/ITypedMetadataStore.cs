using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public interface ITypedMetadataStore
    {
        Task<T> FetchMetadataAsync<T>(CancellationToken cancellationToken = default(CancellationToken));

        Task SaveMetadataAsync<T>(T metadata, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface ITypedMetadataStore<T>
    {
        Task<T> FetchMetadataAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task SaveMetadataAsync(T metadata, CancellationToken cancellationToken = default(CancellationToken));
    }
}