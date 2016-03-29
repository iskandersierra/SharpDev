using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpDev.Serialization;

namespace SharpDev.JsonNet.Serialization
{
    public class JsonNetSerializer : ISerializer
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
    }
}
