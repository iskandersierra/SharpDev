using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpDev.Serialization;

namespace SharpDev.JsonNet.Serialization
{
    public class JsonNetTypedSerializer : ITypedSerializer
    {
        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings();

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public int BufferSize { get; set; } = 4096;

        public Task SerializeAsync(object value, Stream stream, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var textWriter = new StreamWriter(stream, Encoding, BufferSize, true))
            using (var writer = new JsonTextWriter(textWriter))
            {
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, value);
                return Task.CompletedTask;
            }
        }

        public Task<object> DeserializeAsync(Stream stream, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var textReader = new StreamReader(stream, Encoding, true, BufferSize, true))
            using (var reader = new JsonTextReader(textReader))
            {
                var serializer = JsonSerializer.Create();
                dynamic item = serializer.Deserialize(reader);
                return Task.FromResult(item);
            }
        }

        public Task SerializeAsync<T>(T value, Stream stream, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var textWriter = new StreamWriter(stream, Encoding, BufferSize, true))
            using (var writer = new JsonTextWriter(textWriter))
            {
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, value);
                return Task.CompletedTask;
            }
        }

        public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var textReader = new StreamReader(stream, Encoding, true, BufferSize, true))
            using (var reader = new JsonTextReader(textReader))
            {
                var serializer = JsonSerializer.Create();
                T item = serializer.Deserialize<T>(reader);
                return Task.FromResult(item);
            }
        }
    }
}