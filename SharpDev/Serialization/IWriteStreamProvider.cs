using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public interface IWriteStreamProvider
    {
        Task<AsyncDisposableValue<Stream>> OpenWriteAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task WriteParametersAsync(SerializerParameters parameters, CancellationToken cancellationToken = default(CancellationToken));
    }
}