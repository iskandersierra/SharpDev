using SharpDev.Modeling;

namespace SharpDev.Serialization
{
    public class MediaTypeInfo
    {
        public MediaTypeInfo(string mediaType, string encoding, string schema, DomainVersion schemaVersion, string domainModel, DomainVersion domainModelVersion)
        {
            MediaType = mediaType;
            Encoding = encoding;
            Schema = schema;
            SchemaVersion = schemaVersion;
            DomainModel = domainModel;
            DomainModelVersion = domainModelVersion;
        }

        public string MediaType { get; }
        public string Encoding { get; }
        public string Schema { get; }
        public DomainVersion SchemaVersion { get; }
        public string DomainModel { get; }
        public DomainVersion DomainModelVersion { get; }
    }
}
