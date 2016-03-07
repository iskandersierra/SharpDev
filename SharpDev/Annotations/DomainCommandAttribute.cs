using System;

namespace SharpDev.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DomainCommandAttribute : DistributedSystemComponentAttribute
    {
        public DomainCommandAttribute()
        {
        }

        public DomainCommandAttribute(string identifier, string caption, string schemaUri) : base(identifier, caption, schemaUri)
        {
        }

        public DomainCommandAttribute(string identifier, string caption, string schemaUri, string version) : base(identifier, caption, schemaUri, version)
        {
        }

        public bool IsCreation { get; set; }
    }
}
