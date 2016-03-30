using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public class FileStreamProvider :
        IReadStreamProvider,
        IWriteStreamProvider
    {
        public FileInfo FileInfo { get; }

        public FileStreamProvider(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public Task<AsyncDisposableValue<Stream>> OpenReadAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (!FileInfo.Exists)
                throw new ResourceNotFoundException("Metadata not found for this store");

            cancellationToken.ThrowIfCancellationRequested();

            var stream = FileInfo.OpenRead();

            return Task.FromResult(new AsyncDisposableValue<Stream>(stream, async () => stream.Dispose()));
        }

        public Task<SerializerParameters> ReadParametersAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(default(SerializerParameters));
        }

        public Task<AsyncDisposableValue<Stream>> OpenWriteAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (!FileInfo.Exists)
                throw new ResourceNotFoundException("Metadata not found for this store");

            cancellationToken.ThrowIfCancellationRequested();

            var stream = FileInfo.OpenRead();

            return Task.FromResult(new AsyncDisposableValue<Stream>(stream, async () =>
            {
                await stream.FlushAsync(CancellationToken.None);
                stream.Dispose();
            }));
        }

        public Task WriteParametersAsync(SerializerParameters parameters, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }
    }
}
