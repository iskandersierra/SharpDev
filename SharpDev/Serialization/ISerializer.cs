using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public interface ISerializer
    {
        Task SerializeAsync(object value, Stream stream, CancellationToken cancellationToken = default(CancellationToken));
        Task<object> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface ITypedSerializer
    {
        Task SerializeAsync<T>(T value, Stream stream, CancellationToken cancellationToken = default(CancellationToken));
        Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default(CancellationToken));
    }
}
