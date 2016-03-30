using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public interface IReadStreamProvider
    {
        Task<AsyncDisposableValue<Stream>> OpenReadAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<SerializerParameters> ReadParametersAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}