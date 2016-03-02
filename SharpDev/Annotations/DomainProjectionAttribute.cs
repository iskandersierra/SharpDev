using System;

namespace SharpDev.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DomainProjectionAttribute : DistributedSystemComponentAttribute
    {
        public DomainProjectionAttribute()
        {
        }

        public DomainProjectionAttribute(string identifier, string caption, string schemaUri) : base(identifier, caption, schemaUri)
        {
        }

        public DomainProjectionAttribute(string identifier, string caption, string schemaUri, string version) : base(identifier, caption, schemaUri, version)
        {
        }
    }
}