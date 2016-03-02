using System;

namespace SharpDev.Annotations
{
    public abstract class DistributedSystemComponentAttribute : Attribute
    {
        protected DistributedSystemComponentAttribute()
        {
        }

        protected DistributedSystemComponentAttribute(string identifier, string caption, string schemaUri)
        {
            Identifier = identifier;
            Caption = caption;
            SchemaUri = schemaUri;
        }

        protected DistributedSystemComponentAttribute(string identifier, string caption, string schemaUri, string version)
        {
            Identifier = identifier;
            Caption = caption;
            SchemaUri = schemaUri;
            Version = version;
        }

        public string Identifier { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string SchemaUri { get; set; }
        public string Version { get; set; }
    }
}