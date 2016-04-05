using System;

namespace SharpDev.AspNet46
{
    public class MediaTypeInfo
    {
        public MediaTypeInfo(string mediaType, string encoding, string domainModel, string version, string schema, string schemaVersion)
        {
            if (mediaType == null) throw new ArgumentNullException(nameof(mediaType));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            MediaType = mediaType;
            Encoding = encoding;
            DomainModel = domainModel;
            Version = version;
            Schema = schema;
            SchemaVersion = schemaVersion;
        }

        public string MediaType { get; }
        public string Encoding { get; }
        public string DomainModel { get; }
        public string Version { get; }
        public string Schema { get; }
        public string SchemaVersion { get; }
    }
}