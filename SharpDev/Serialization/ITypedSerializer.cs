using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public interface ITypedSerializer
    {
        Task<SerializerParameters> SerializeAsync<T>(T value, Stream stream, CancellationToken cancellationToken = default(CancellationToken));

        Task<T> DeserializeAsync<T>(Stream stream, SerializerParameters parameters, CancellationToken cancellationToken = default(CancellationToken));
    }
}