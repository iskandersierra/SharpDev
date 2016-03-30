using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public interface ISerializer
    {
        Task<SerializerParameters> SerializeAsync(object value, Stream stream, CancellationToken cancellationToken = default(CancellationToken));

        Task<object> DeserializeAsync(Stream stream, SerializerParameters parameters, CancellationToken cancellationToken = default(CancellationToken));
    }
}
