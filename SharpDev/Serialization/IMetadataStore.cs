using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public interface IMetadataStore
    {
        /// <summary>
        /// Fetches a metadata object
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> FetchMetadataAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Saves a metadata object
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveMetadataAsync(object metadata, CancellationToken cancellationToken = default(CancellationToken));
    }
}