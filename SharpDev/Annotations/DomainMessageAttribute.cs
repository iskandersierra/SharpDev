using System;

namespace SharpDev.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DomainMessageAttribute : DistributedSystemComponentAttribute
    {
        public DomainMessageAttribute()
        {
        }

        public DomainMessageAttribute(string identifier, string caption, string schemaUri) : base(identifier, caption, schemaUri)
        {
        }

        public DomainMessageAttribute(string identifier, string caption, string schemaUri, string version) : base(identifier, caption, schemaUri, version)
        {
        }
    }
}