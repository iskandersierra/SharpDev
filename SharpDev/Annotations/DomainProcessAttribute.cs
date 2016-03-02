using System;

namespace SharpDev.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DomainProcessAttribute : DistributedSystemComponentAttribute
    {
        public DomainProcessAttribute()
        {
        }

        public DomainProcessAttribute(string identifier, string caption, string schemaUri) : base(identifier, caption, schemaUri)
        {
        }

        public DomainProcessAttribute(string identifier, string caption, string schemaUri, string version) : base(identifier, caption, schemaUri, version)
        {
        }
    }
}