using System;

namespace SharpDev.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DomainEventAttribute : DistributedSystemComponentAttribute
    {
        public DomainEventAttribute()
        {
        }

        public DomainEventAttribute(string identifier, string caption, string schemaUri) : base(identifier, caption, schemaUri)
        {
        }

        public DomainEventAttribute(string identifier, string caption, string schemaUri, string version) : base(identifier, caption, schemaUri, version)
        {
        }

        public bool IsCreation { get; set; }
    }
}