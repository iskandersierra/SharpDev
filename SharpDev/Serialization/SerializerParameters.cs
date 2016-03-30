namespace SharpDev.Serialization
{
    public class SerializerParameters
    {
        public SerializerParameters()
        {
        }

        public SerializerParameters(string contentType)
        {
            ContentType = contentType;
        }

        public SerializerParameters(string contentType, string contentEncoding)
        {
            ContentType = contentType;
            ContentEncoding = contentEncoding;
        }

        public string ContentType { get; }
        public string ContentEncoding { get; }
    }
}