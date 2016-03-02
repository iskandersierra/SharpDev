using System;

namespace SharpDev.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DomainAggregateAttribute : DistributedSystemComponentAttribute
    {
        public DomainAggregateAttribute()
        {
        }

        public DomainAggregateAttribute(string identifier, string caption, string schemaUri) : base(identifier, caption, schemaUri)
        {
        }

        public DomainAggregateAttribute(string identifier, string caption, string schemaUri, string version) : base(identifier, caption, schemaUri, version)
        {
        }
    }
}