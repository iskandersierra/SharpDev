using System;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SharpDev.Serialization;

namespace SharpDev.JsonNet.Serialization
{
    public class JsonNetSerializerBase
    {
        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings();

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public int BufferSize { get; set; } = 4096;

        protected virtual void CheckParameters(SerializerParameters parameters, out Encoding encoding)
        {
            if (parameters != null)
            {
                if (parameters.ContentEncoding != null && parameters.ContentEncoding != "identity")
                    throw new NotSupportedException($"Do not support Content-Encoding: {parameters.ContentEncoding}");

                if (!string.IsNullOrEmpty(parameters.ContentType))
                {
                    MediaTypeHeaderValue contentType;
                    if (!MediaTypeHeaderValue.TryParse(parameters.ContentType, out contentType))
                        throw new FormatException($"Cannot parse Content-Type: {parameters.ContentType}");

                    if (contentType.MediaType != "application/json")
                        throw new NotSupportedException($"Invalid media type: {contentType.MediaType}");

                    if (!string.IsNullOrEmpty(contentType.CharSet))
                        encoding = Encoding.GetEncoding(contentType.CharSet, Encoding.EncoderFallback, Encoding.DecoderFallback);
                    else
                        encoding = Encoding;

                    return;
                }
            }
            encoding = Encoding;
        }
    }
}