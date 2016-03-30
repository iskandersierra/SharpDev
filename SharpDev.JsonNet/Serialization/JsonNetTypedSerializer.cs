using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpDev.Serialization;

namespace SharpDev.JsonNet.Serialization
{
    public class JsonNetTypedSerializer : 
        JsonNetSerializerBase,
        ITypedSerializer
    {
        public Task<SerializerParameters> SerializeAsync<T>(T value, Stream stream, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var textWriter = new StreamWriter(stream, Encoding, BufferSize, true))
            using (var writer = new JsonTextWriter(textWriter))
            {
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, value);
                return Task.FromResult(new SerializerParameters($"application/json;charset={Encoding.WebName}"));
            }
        }

        public Task<T> DeserializeAsync<T>(Stream stream, SerializerParameters parameters, CancellationToken cancellationToken = new CancellationToken())
        {
            Encoding encoding;
            CheckParameters(parameters, out encoding);

            using (var textReader = new StreamReader(stream, encoding, true, BufferSize, true))
            using (var reader = new JsonTextReader(textReader))
            {
                var serializer = JsonSerializer.Create();
                T item = serializer.Deserialize<T>(reader);
                return Task.FromResult(item);
            }
        }
    }
}