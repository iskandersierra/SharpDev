using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Serialization
{
    public enum CompressAlgorithm
    {
        Identity,
        Deflate,
        GZip,
    }

    public class CompressSerializer : ISerializer
    {
        public ISerializer InnerSerializer { get; }

        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        public CompressAlgorithm Algorithm { get; set; } = CompressAlgorithm.GZip;

        public CompressSerializer(ISerializer innerSerializer)
        {
            if (innerSerializer == null) throw new ArgumentNullException(nameof(innerSerializer));

            InnerSerializer = innerSerializer;
        }


        public async Task<SerializerParameters> SerializeAsync(object value, Stream stream, CancellationToken cancellationToken = new CancellationToken())
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            Stream innerStream = stream;
            string contentEncoding = null;
            switch (Algorithm)
            {
                case CompressAlgorithm.Identity:
                    break;
                case CompressAlgorithm.Deflate:
                    innerStream = new DeflateStream(stream, CompressionLevel, true);
                    contentEncoding = "deflate";
                    break;
                case CompressAlgorithm.GZip:
                    innerStream = new GZipStream(stream, CompressionLevel, true);
                    contentEncoding = "gzip";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Algorithm));
            }
            try
            {
                var parameters = await InnerSerializer.SerializeAsync(value, innerStream, cancellationToken);

                return new SerializerParameters(parameters.ContentType, contentEncoding);
            }
            finally
            {
                if (innerStream != stream)
                    innerStream.Dispose();
            }
        }

        public async Task<object> DeserializeAsync(Stream stream, SerializerParameters parameters, CancellationToken cancellationToken = new CancellationToken())
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            CompressAlgorithm algorithm;
            CheckParameters(parameters, out algorithm);

            Stream innerStream = stream;
            var innerParameters = new SerializerParameters(parameters.ContentType);
            switch (Algorithm)
            {
                case CompressAlgorithm.Identity:
                    break;
                case CompressAlgorithm.Deflate:
                    innerStream = new DeflateStream(stream, CompressionMode.Decompress, true);
                    break;
                case CompressAlgorithm.GZip:
                    innerStream = new GZipStream(stream, CompressionMode.Decompress, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Algorithm));
            }

            try
            {
                var value = await InnerSerializer.DeserializeAsync(innerStream, innerParameters, cancellationToken);

                return value;
            }
            finally
            {
                if (innerStream != stream)
                    innerStream.Dispose();
            }
        }

        private void CheckParameters(SerializerParameters parameters, out CompressAlgorithm algorithm)
        {
            if (parameters != null)
            {
                if (!string.IsNullOrEmpty(parameters.ContentEncoding))
                {
                    switch (parameters.ContentEncoding)
                    {
                        case "identity":
                            algorithm = CompressAlgorithm.Identity;
                            return;
                        case "deflate":
                            algorithm = CompressAlgorithm.Deflate;
                            return;
                        case "gzip":
                            algorithm = CompressAlgorithm.GZip;
                            return;
                        default:
                            throw new NotSupportedException("Compression algorithm not supported");
                    }
                }
            }

            algorithm = CompressAlgorithm.Identity;
        }
    }
}
